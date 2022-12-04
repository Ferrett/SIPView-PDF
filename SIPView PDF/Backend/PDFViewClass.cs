using ImageGear.ART;
using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Display;

using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Processing;
using ImageGear.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;



namespace SIPView_PDF
{
    public static class PDFViewClass
    {
        public static event EventHandler PageChanged;
        public static event EventHandler DocumentChanged;

        public static List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();
        public static ImGearPDFDocument PDFDocument = null;
        public static ImGearMagnifier Magnifier;
        public static ImGearARTForms ARTForm;
        public static ImGearPageView PageView;
        public static Panel ThumbnailController;
        public static ScrollBar ScrollBar;
        public static StatusStrip StatusStrip;
        public static ImGearPDFWordFinder PDFWordFinder;
        public static int CurrentPageID = 0;

        private static ImGearPoint StartMousePos;
        private static ImGearPoint CurrentMousePos;

        private static bool CtrlKeyPressed;
        private static bool PageIsZoming = false;
        private static bool TextIsSelecting = false;
        public static bool SelectionMode = false;

        public static string DocumentPath;

        public static PrintDialog PrintDialog = new PrintDialog();
        public static PageSetupDialog PageSetupDialog = new PageSetupDialog();
        public static PageSettings PageSettings = new PageSettings() ;
        public static PrintDocument PrintDocument = new PrintDocument();

        public static ImGearCompressOptions CompressOptions = new ImGearCompressOptions() { IsRemoveImageThumbnailEnabled = false };
        public static bool DoCompression = false;

        public static ImGearPDFPreflightProfile PreflightProfile;
        public static bool DoConversion = false;

        public static List<ImGearPageView> Thumbnails = new List<ImGearPageView>();
        public static List<Panel> ThumbnailBackgrounds = new List<Panel>();
        public static List<Label> ThumbnailLabels = new List<Label>();

        private static void UpdateThumbnailSelection(int pageNumber)
        {
            if (PDFDocument.Pages.Count <= 1)
                return;

            for (int i = 0; i < ThumbnailBackgrounds.Count; i++)
            {
                ThumbnailBackgrounds[i].BackColor = Color.Transparent;
            }
            ThumbnailBackgrounds[pageNumber].BackColor = Color.Cyan;
            ThumbnailBackgrounds[pageNumber].Visible = true;
        }

        private static void InitializeThumbnails()
        {
            if (PDFDocument.Pages.Count <= 1)
            {
                PageView.Location = new Point(0, PageView.Location.Y);
                PageView.Width = 1184;
                UpdatePageView();
                return;
            }

            PageView.Location = new Point(ThumbnailController.Width, PageView.Location.Y);
            PageView.Width = 1184 - ThumbnailController.Width - ScrollBar.Width;
            UpdatePageView();

            Thumbnails.Clear();
            ThumbnailBackgrounds.Clear();
            ThumbnailLabels.Clear();
            ThumbnailController.Controls.Clear();

            for (int i = 0; i < PDFDocument.Pages.Count; i++)
            {
                ThumbnailLabels.Add(new Label
                {
                    Text = $"Page {i + 1}",
                    Location = new Point(46, 130 + 140 * i),
                });

                ThumbnailBackgrounds.Add(new Panel()
                {
                    Location = new Point(14, 14 + 140 * i),
                    Width = 112,
                    Height = 112,
                    Tag = i,
                    BackColor = Color.Transparent,

                    Visible = true,

                });

                Thumbnails.Add(new ImGearPageView()
                {
                    Page = PDFDocument.Pages[i],
                    Location = new Point(20, 20 + 140 * i),
                    Width = 100,
                    Height = 100,
                    Tag = i,

                });

                Thumbnails.Last().Display.Background.Color.Red = ThumbnailController.BackColor.R;
                Thumbnails.Last().Display.Background.Color.Green = ThumbnailController.BackColor.G;
                Thumbnails.Last().Display.Background.Color.Blue = ThumbnailController.BackColor.B;

                Thumbnails.Last().MouseEnter += PDFViewClass_MouseEnter;
                Thumbnails.Last().MouseLeave += PDFViewClass_MouseLeave;
                Thumbnails.Last().Click += PDFViewClass_Click;

                ThumbnailController.Controls.Add(ThumbnailLabels.Last());
                ThumbnailController.Controls.Add(ThumbnailBackgrounds.Last());
                ThumbnailController.Controls.Add(Thumbnails.Last());
                Thumbnails.Last().BringToFront();
            }
        }

        private static void PDFViewClass_Click(object sender, EventArgs e)
        {
            RenderPage((int)(sender as ImGearPageView).Tag);
        }

        private static void PDFViewClass_MouseLeave(object sender, EventArgs e)
        {
            ThumbnailBackgrounds[(int)(sender as ImGearPageView).Tag].BorderStyle = BorderStyle.None;
        }

        private static void PDFViewClass_MouseEnter(object sender, EventArgs e)
        {
            ThumbnailBackgrounds[(int)(sender as ImGearPageView).Tag].BorderStyle = BorderStyle.FixedSingle;
        }

        internal static void MagnifierChangeVisibility()
        {
            Magnifier.IsPopUp = !Magnifier.IsPopUp;
            ARTForm.Page = ARTForm.Page == null ? ARTPages[CurrentPageID] : null;
        }

        private static void DisplayCurrentPageMarks()
        {
            PageView.Display = new ImGearPageDisplay(PDFDocument.Pages[CurrentPageID], ARTPages[CurrentPageID]);
            ARTForm.Page = ARTPages[CurrentPageID];

        }

        public static void InitializeEvents()
        {
            ARTForm.MouseRightButtonDown += ARTForm_MouseRightButtonDown;
            ARTForm.MouseLeftButtonDown += ARTForm_MouseLeftButtonDown;
            ARTForm.MouseLeftButtonUp += ARTForm_MouseLeftButtonUp;
            ARTForm.MouseRightButtonUp += ARTForm_MouseRightButtonUp;
            ARTForm.MouseMoved += ARTForm_MouseMoved;

            ARTForm.MarkCreated += ARTForm_MarkCreated;
        }


        public static void PageView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.ControlKey)
                return;

            CtrlKeyPressed = false;
        }

        public static void WheelScrolled(object sender, MouseEventArgs e)
        {
            if (PDFDocument == null)
                return;

            if (CtrlKeyPressed)
            {
                if (e.Delta > 0)
                {
                    PageView.Display.ZoomToRectangle(PageView, new ImGearRectangle(
                        new ImGearPoint((int)(CurrentMousePos.X - PageView.Width / 2.4), (int)(CurrentMousePos.Y - PageView.Height / 2.4)),
                        new ImGearPoint((int)(CurrentMousePos.X + PageView.Width / 2.4), (int)(CurrentMousePos.Y + PageView.Height / 2.4))));
                }
                else
                {
                    PageView.Display.ZoomToRectangle(PageView, new ImGearRectangle(
                        new ImGearPoint((int)(CurrentMousePos.X - PageView.Width * 0.6), (int)(CurrentMousePos.Y - PageView.Height * 0.6)),
                        new ImGearPoint((int)(CurrentMousePos.X + PageView.Width * 0.6), (int)(CurrentMousePos.Y + PageView.Height * 0.6))));
                }

                UpdatePageView();
                return;
            }

            if (PageView.Display.GetScrollInfo(PageView).Vertical.Max == 0 && PageView.Display.GetScrollInfo(PageView).Horizontal.Max == 0)
            {
                if (e.Delta > 0)
                {
                    PrevPage();
                }
                else
                {
                    NextPage();
                }
                return;
            }

            if (PageView.Display.GetScrollInfo(PageView).Vertical.Pos == PageView.Display.GetScrollInfo(PageView).Vertical.Min)
            {
                PrevPage();
            }
            else if (PageView.Display.GetScrollInfo(PageView).Vertical.Pos == PageView.Display.GetScrollInfo(PageView).Vertical.Max)
            {
                NextPage();
            }
        }

        private static void ARTForm_MarkCreated(object sender, ImGearARTFormsMarkCreatedEventArgs e)
        {
            UpdatePageView();
        }
        public static void KeyDown(object sender, KeyEventArgs e)
        {


            if (PDFDocument == null)
                return;

            if (e.KeyCode == Keys.Space)
            {
                PageView.Display.UpdateZoomFrom(new ImGearZoomInfo(1, 1, false, false));
                UpdatePageView();
            }
            else if (e.KeyCode == Keys.ControlKey)
            {
                CtrlKeyPressed = true;
            }

            if (CtrlKeyPressed == true && SelectionMode == true && e.KeyCode == Keys.A)
            {
                SelectAllText();
            }

            if (CtrlKeyPressed == true && SelectionMode == true && e.KeyCode == Keys.C)
            {
                CopySelectedText();
            }
        }


        private static void ARTForm_MouseMoved(object sender, ImGearARTFormsMouseEventArgs e)
        {
            Test3(sender, e.EventData);
        }

        private static void ARTForm_MouseLeftButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            Test2(sender, e.EventData);
        }
        private static void Test1(object sender, MouseEventArgs e)
        {
            if (SelectionMode)
            {
                StartMousePos.X = e.X;
                StartMousePos.Y = e.Y;

                PageView.RegisterAfterDraw(
               new ImGearPageView.AfterDraw(DrawSelector));
            }
        }

        private static void Test2(object sender, MouseEventArgs e)
        {
            UpdatePageView();
        }
        private static void Test3(object sender, MouseEventArgs e)
        {
            CurrentMousePos.X = e.X;
            CurrentMousePos.Y = e.Y;
            UpdatePageView();
        }
        private static void ARTForm_MouseLeftButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            Test1(sender, e.EventData);
        }

        private static void ARTForm_MouseRightButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (PageIsZoming)
            {
                PageView.RegisterAfterDraw(null);
                PageIsZoming = false;

                CurrentMousePos.X = e.EventData.X;
                CurrentMousePos.Y = e.EventData.Y;

                ImGearRectangle igRectangle = new ImGearRectangle(StartMousePos, CurrentMousePos);
                // Cancel the zoom if it would be for a 0x0 or 1x1 rectangle.
                if (igRectangle.Width <= 1 || igRectangle.Height <= 1)
                    return;
                // Zoom to the selected rectangle
                PageView.Display.ZoomToRectangle(PageView, igRectangle);

                UpdatePageView();
            }
        }

        private static void ARTForm_MouseRightButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (e.Mark != null)
                return;

            PageIsZoming = true;

            StartMousePos.X = e.EventData.X;
            StartMousePos.Y = e.EventData.Y;
            // Register method to draw the selection rectangle.

            PageView.RegisterAfterDraw(
                new ImGearPageView.AfterDraw(DrawSelector));
        }

        private static void DrawSelector(System.Drawing.Graphics gr)
        {
            if (PageIsZoming || TextIsSelecting)
            {
                // Create a new pen to draw dotted lines.

                Pen pen = new Pen(Color.DarkMagenta);
                pen.DashStyle = DashStyle.Solid;
                pen.Width = 2;

                ImGearRectangle rect;

                // Define the currently selected zoom rectangle.

                if (StartMousePos.Y >= CurrentMousePos.Y)
                    rect = new ImGearRectangle(StartMousePos, CurrentMousePos);
                else
                    rect = new ImGearRectangle(CurrentMousePos, StartMousePos);
                // Draw the selection box.
                gr.DrawRectangle(pen, rect.Left, rect.Top,
                    rect.Width, rect.Height);

            }
        }

        public static void OnPageChanged(EventArgs e)
        {
            if (PageChanged != null)
                PageChanged(null, e);
        }

        public static void OnDocumentChanged(EventArgs e)
        {
            if (DocumentChanged != null)
                DocumentChanged(null, e);
        }

        public static void InitializeImGear()
        {
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();

            InitializeEvents();
        }

        public static void InitializeToolBar()
        {
            ARTForm.Mode = ImGearARTModes.EDIT;

            ARTForm.ToolBar.Size = new Size(80, 295);
            ARTForm.ToolBar.Text = string.Empty;

            ARTForm.ToolBar.Location = new Point(0, 50);
            ARTForm.ToolBar.ShowInTaskbar = true;
        }

        private static void InitializeScrollBar()
        {
            if (PDFDocument.Pages.Count > 1)
            {
                ScrollBar.Visible = true;
                ScrollBar.Maximum = PDFDocument.Pages.Count - 1;
            }
            else
            {
                ScrollBar.Visible = false;
            }
        }

        private static void InitializeArtPages()
        {
            ARTPages.Clear();
            for (int i = 0; i < PDFDocument.Pages.Count; i++)
            {
                ARTPages.Add(new ImGearARTPage());
                ARTPages.Last().History.IsEnabled = true;
                ARTPages.Last().History.Limit = uint.MaxValue;
            }
        }

        private static void UpdateAfterRender()
        {
            DisplayCurrentPageMarks();
            ScrollBar.Value = CurrentPageID;
            FindWordsInPage();
            UpdatePageView();
        }

        public static void ToolBarChangeVisibility()
        {
            if (ARTForm.ToolBar.Visible)
                ARTForm.ToolBar.Close();
            else
                ARTForm.ToolBar.Show();
        }

        public static int PagesInDocumentCount()
        {
            return PDFDocument.Pages.Count;
        }
        public static void RotateLeft()
        {
            ImGearProcessing.Rotate(PageView.Page, ImGearRotationValues.VALUE_270);
            UpdatePageView();
        }

        public static void RotateRight()
        {
            ImGearProcessing.Rotate(PageView.Page, ImGearRotationValues.VALUE_90);

            UpdatePageView();
        }

        public static void Undo()
        {
            ARTPages[CurrentPageID].History.Undo();
            UpdatePageView();
        }

        public static void Redo()
        {
            ARTPages[CurrentPageID].History.Redo();
            UpdatePageView();
        }

        public static void PrevPage()
        {
            if (CurrentPageID > 0)
                RenderPage(CurrentPageID - 1);
        }

        public static void NextPage()
        {
            if (CurrentPageID < PDFDocument.Pages.Count - 1)
                RenderPage(CurrentPageID + 1);
        }

        public static int SelectedMarksCount()
        {
            int selectedMarksCounter = 0;

            foreach (ImGearARTMark ARTMark in ARTPages[CurrentPageID])
            {
                if (ARTPages[CurrentPageID].MarkIsSelected(ARTMark))
                    selectedMarksCounter++;
            }

            return selectedMarksCounter;
        }

        public static void SelectAllMarks()
        {
            if (SelectedMarksCount() == ARTPages[CurrentPageID].MarkCount)
                ARTPages[CurrentPageID].SelectMarks(false);
            else
                ARTPages[CurrentPageID].SelectMarks(true);
            UpdatePageView();
        }

        public static void UpdatePageView()
        {
            PageView.Invalidate();
            PageView.Update();
        }

        public static void AnnotationBakeIn()
        {
            PDFDocument.Pages[CurrentPageID] = ImGearART.BurnIn(PDFDocument.Pages[CurrentPageID], ARTPages[CurrentPageID], ImGearARTBurnInOptions.SELECTED, null);
            PageView.Page = PDFDocument.Pages[CurrentPageID];

            // Get burned marks ID
            List<int> bakedMarkID = new List<int>();
            foreach (ImGearARTMark ARTMark in ARTPages[CurrentPageID])
            {
                if (ARTPages[CurrentPageID].MarkIsSelected(ARTMark))
                    bakedMarkID.Add(ARTMark.Id);
            }

            // Delete burned marks by ID
            foreach (int ID in bakedMarkID)
            {
                ARTPages[CurrentPageID].MarkRemove(ID);
            }

            UpdatePageView();
        }

        public static void ScrollBarScrolled()
        {
            RenderPage(ScrollBar.Value);
        }

        public static void FileSave(string fileName, ImGearPDFDocument document)
        {
            // Save to output file.
            using (FileStream outputStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                try
                {
                    if (DoConversion)
                        SavePdfDocumentAsPDFA(document, PreflightProfile, outputStream);

                    if (DoCompression)
                        document.SaveCompressed(outputStream, CompressOptions);
                    else
                        document.Save(outputStream, ImGearSavingFormats.PDF, 0, 0, document.Pages.Count, ImGearSavingModes.OVERWRITE);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save file {0}. {1}", fileName, ex.Message));
                    return;
                }
            }
        }

        private static void SavePdfDocumentAsPDFA(ImGearPDFDocument document, ImGearPDFPreflightProfile profile, FileStream outputStream)
        {
            // Create object-converter. 
            using (ImGearPDFPreflight preflight = new ImGearPDFPreflight(document))
            {
                ImGearPDFPreflightReport report = preflight.VerifyCompliance(profile, 0, document.Pages.Count);

                if (report.Code != ImGearPDFPreflightReportCodes.SUCCESS)
                {
                    if (report.Status == ImGearPDFPreflightStatusCode.Fixable)
                    {
                        // Create conversion options. We need to be sure the "ig_rgb_profile.icm" color profile
                        // is placed in executable file directory of this application.
                        ImGearPDFPreflightConvertOptions conversionOptions = new ImGearPDFPreflightConvertOptions(profile, 0, document.Pages.Count);
                        // Perform the conversion.
                        report = preflight.Convert(conversionOptions);
                        if (report.Code == ImGearPDFPreflightReportCodes.SUCCESS)
                        {
                            return;
                        }
                        else
                            MessageBox.Show("PDF document cannot be converted to APDF standard.");
                    }
                    else
                        MessageBox.Show("PDF document cannot be converted to APDF standard.");
                }
            }
        }

        public static void FileSave()
        {
            using (SaveFileDialog fileDialogSave = new SaveFileDialog())
            {
                fileDialogSave.Filter = ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
                ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);
                fileDialogSave.OverwritePrompt = true;

                if (DialogResult.OK == fileDialogSave.ShowDialog())
                {
                    FileSave(fileDialogSave.FileName, PDFDocument);
                }
            }
        }

        public static void FilePrint()
        {
            PrintDocument.DefaultPageSettings.Landscape = PageSettings.Landscape;
            PrintDocument.DefaultPageSettings.Margins = PageSettings.Margins;
            PrintDocument.DefaultPageSettings.PaperSource = PageSettings.PaperSource;
            PrintDocument.DefaultPageSettings.PaperSize = PageSettings.PaperSize;

            // Initialize a PrintDialog for the user to select a printer.

            PrintDialog.ShowNetwork = true;
            PrintDialog.Document = PrintDocument;

            // Set the page range to 1 page.
            PrintDialog.AllowSomePages = true;
            PrintDialog.PrinterSettings.MinimumPage = 1;
            PrintDialog.PrinterSettings.MaximumPage = PDFDocument.Pages.Count;
            PrintDialog.PrinterSettings.FromPage = 1;
            PrintDialog.PrinterSettings.ToPage = PDFDocument.Pages.Count;

            if (DialogResult.OK == PrintDialog.ShowDialog())
            {

                PrintDocument.DocumentName = DocumentPath;


                // Define a PrintPage event handler and start printing.
                PrintDocument.PrintPage += new PrintPageEventHandler(HandlePrinting);
                for (int j = 0; j < PrintDialog.PrinterSettings.Copies; j++)
                {
                    for (int i = 0; i < PrintDialog.PrinterSettings.ToPage; i++)
                    {
                        RenderPage(i);
                        UpdatePageView();
                        PrintDocument.Print();
                    }
                }
            }
        }

        static void HandlePrinting(object sender, PrintPageEventArgs args)
        {
            // Clone the current Display for use as a printing display.
            ImGearPageDisplay igPageDisplayPrinting = PageView.Display.Clone();
            igPageDisplayPrinting.Page = PageView.Display.Page;

            // Get the current Zoom settings and disabled fixed zoom.
            ImGearZoomInfo igZoomInfo = igPageDisplayPrinting.GetZoomInfo(PageView);
            igZoomInfo.Horizontal.Fixed = igZoomInfo.Vertical.Fixed = false;
            igPageDisplayPrinting.UpdateZoomFrom(igZoomInfo);

            // Disable any background before printing.
            igPageDisplayPrinting.Background.Mode = ImGearBackgroundModes.NONE;

            // Print to the Graphics device chosen from the PrintDialog.
            igPageDisplayPrinting.Print(args.Graphics);

            // Let the PrintDialog know there are no more pages.
            args.HasMorePages = false;
        }

        public static void DisposeImGear()
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }



        public static void FileLoad()
        {
            OpenFileDialog openFileDialogLoad = new OpenFileDialog();
            // Allow only PDF and PS files.
            openFileDialogLoad.Filter =
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);

            if (DialogResult.OK == openFileDialogLoad.ShowDialog())
            {
                FileLoad(openFileDialogLoad.FileName);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //PageView.Display.Layout.FitMode = ImGearFitModes.ACTUAL_SIZE;
            //PageView.Display.Layout.Alignment.Horizontal = ImGearAlignmentHModes.LEFT;
            //Clipboard.SetText(openFileDialogLoad.FileName);
        }

        public static int WordsInPageCount(int page)
        {
            PDFWordFinder = new ImGearPDFWordFinder(PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);
            return PDFWordFinder.AcquireWordList(page);
        }
        private static ImGearRectangle[] WordsBounds;
        private static bool[] WordIsShowed;
        public static void FindWordsInPage()
        {
            ARTPages[CurrentPageID].SetCoordType(ImGearARTCoordinatesType.DEVICE_COORD);
            WordsBounds = new ImGearRectangle[WordsInPageCount(CurrentPageID)];
            WordIsShowed = new bool[WordsInPageCount(CurrentPageID)];

            for (int i = 0; i < WordsBounds.Length; i++)
            {
                WordIsShowed[i] = false;
                WordsBounds[i] = new ImGearRectangle()
                {
                    Top = PDFDocument.Pages[CurrentPageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomRight.V),
                    Left = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).TopLeft.H),
                    Bottom = PDFDocument.Pages[CurrentPageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).TopLeft.V),
                    Right = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomRight.H)
                };
            }
        }

        public static void CopySelectedText()
        {
            string text = String.Empty;
            bool s = false;
            for (int i = 0; i < WordsBounds.Length; i++)
            {
                if (WordIsShowed[i] == true)
                {
                    if (s==true && PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomLeft.V < PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i - 1).GetQuad(0).BottomLeft.V)
                    {
                        text += '\n';
                    }

                    s = true;
                    text += PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).String + " ";
                }
            }
            text.Remove(text.Length - 1);
            Clipboard.SetText(text);
        }

        public static void SelectAllText()
        {
            for (int i = 0; i < WordsBounds.Length; i++)
            {
                if (WordIsShowed[i] == true)
                {
                    foreach (ImGearARTMark igARTMark in ARTPages[CurrentPageID])
                    {
                        if (igARTMark.UserData.Equals(i))
                        {
                            ARTPages[CurrentPageID].MarkRemove(igARTMark);
                            WordIsShowed[i] = false;
                        }
                    }
                }
            }
            for (int i = 0; i < WordsBounds.Length; i++)
            {
                WordIsShowed[i] = true;
                ARTPages[CurrentPageID].AddMark(new ImGearARTRectangle(WordsBounds[i], new ImGearRGBQuad() { Red= 179 ,Green=226,Blue=255}) { Opacity = 120, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
            }

            UpdatePageView();
        }

        public static void Test()
        {
            ImGearPoint[] array = {new ImGearPoint(StartMousePos.X,StartMousePos.Y), new ImGearPoint(CurrentMousePos.X,CurrentMousePos.Y) };


            if (array[0].X > array[1].X)
            {
                int tmp = array[0].X;
                array[0].X = array[1].X;
                array[1].X = tmp;
            }

            if (array[0].Y > array[1].Y)
            {
                int tmp = array[0].Y;
                array[0].Y = array[1].Y;
                array[1].Y = tmp;
            }
            ARTPages[CurrentPageID].ConvertCoordinates(PageView, PageView.Display, ImGearCoordConvModes.DEVICE_TO_IMAGE, array);



            for (int i = 0; i < WordsBounds.Length; i++)
            {
                if (new ImGearRectangle() { Left = array[0].X, Top = array[0].Y, Right = array[1].X, Bottom = array[1].Y }.Contains(new ImGearPoint(WordsBounds[i].Left, WordsBounds[i].Top)) &&
                    new ImGearRectangle() { Left = array[0].X, Top = array[0].Y, Right = array[1].X, Bottom = array[1].Y }.Contains(new ImGearPoint(WordsBounds[i].Right, WordsBounds[i].Bottom)))
                {
                    if (WordIsShowed[i] == false)
                    {
                        WordIsShowed[i] = true;
                        ARTPages[CurrentPageID].AddMark(new ImGearARTRectangle(WordsBounds[i], new ImGearRGBQuad() { Red = 179, Green = 226, Blue = 255 }) { Opacity = 120, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
                    }
                }
                else
                {
                    if (WordIsShowed[i] == true)
                    {
                        foreach (ImGearARTMark igARTMark in ARTPages[CurrentPageID])
                        {
                            if (igARTMark.UserData.Equals(i))
                            {
                                ARTPages[CurrentPageID].MarkRemove(igARTMark);
                                WordIsShowed[i] = false;
                            }
                        }
                    }

                }
            }
        }

        public static void AddPagesToDocument(int startIndex, ImGearPDFDocument pDFDocument)
        {
            PDFDocument.InsertPages(startIndex - 2, pDFDocument, 0, pDFDocument.Pages.Count, ImGearPDFInsertFlags.ALL);
            for (int i = 0; i < pDFDocument.Pages.Count; i++)
            {
                ARTPages.Insert((startIndex - 1) + i, new ImGearARTPage());
            }

            InitializeScrollBar();
            RenderPage(ScrollBar.Value);
            UpdatePageView();
            FileSave(DocumentPath, PDFDocument);
        }

        public static void FileLoad(string fileName)
        {
            using (FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                try
                {
                    // Load the entire the document.
                    PDFDocument = (ImGearPDFDocument)ImGearFileFormats.LoadDocument(inputStream);
                    PDFWordFinder = new ImGearPDFWordFinder(PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);

                    DocumentPath = fileName;
                    ARTPages.Clear();

                    InitializeScrollBar();
                    InitializeArtPages();
                    InitializeThumbnails();

                    OnDocumentChanged(null);
                    RenderPage(0);
                }
                catch (ImGearException ex)
                {
                    // Perform error handling.
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static void RenderPage(int pageID)
        {
            CurrentPageID = pageID;
            //Load a single page from the loaded document.
            try
            {
                PageView.Page = PDFDocument.Pages[CurrentPageID];
                PageView.Display.Background.Color.Red
                    = PageView.Display.Background.Color.Green
                    = PageView.Display.Background.Color.Blue = 96;

            }
            catch (ImGearException ex)
            {
                MessageBox.Show(ex.Message);
            }

            UpdateThumbnailSelection(pageID);
            OnPageChanged(null);
            UpdateAfterRender();
        }

        public static void ShowPrintMenu()
        {
            if (PageSetupDialog.PageSettings == null)
                PageSettings.Margins = new Margins(4, 4, 4, 4);
            PageSetupDialog.EnableMetric = true;
            PageSetupDialog.PageSettings = PageSettings;
            PageSetupDialog.ShowDialog();

        }

        public static void TextSelectionModeChange()
        {
            SelectionMode = !SelectionMode;

            if (SelectionMode)
            {
                PageView.Cursor = Cursors.IBeam;
            }
        }

        public static void PageView_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelectionMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Test1(sender, e);
                    TextIsSelecting = true;
                    PageView.Cursor = Cursors.IBeam;
                }
            }
            else
                ARTForm.MouseDown(sender, e);
        }

        public static void PageView_MouseUp(object sender, MouseEventArgs e)
        {
            if (SelectionMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Test2(sender, e);
                    TextIsSelecting = false;
                }
            }
            else
                ARTForm.MouseUp(sender, e);
        }

        public static void PageView_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectionMode)
            {
                if (TextIsSelecting)
                {
                    Test3(sender, e);
                    PageView.Cursor = Cursors.IBeam;
                    Test();
                }
            }
            else
                ARTForm.MouseMove(sender, e);
        }
    }
}
