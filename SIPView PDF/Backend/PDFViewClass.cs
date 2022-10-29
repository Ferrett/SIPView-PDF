using ImageGear.ART;
using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Display;

using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Processing;
using ImageGear.Recognition.Forms;
using ImageGear.Recognition;
using ImageGear.Windows.Forms;
using ImageGear.Windows.Forms.Thumbnails;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Security.Policy;

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
        public static ImGearThumbnailCtl ThumbnailController;
        public static ScrollBar ScrollBar;
        public static StatusStrip StatusStrip;
        public static int CurrentPageID = 0;

        private static ImGearPoint StartMousePos;
        private static ImGearPoint CurrentMousePos;

        private static bool CtrlKeyPressed;
        private static bool PageIsZoming = false;

        private static string DocumentPath;

        public static PrintDialog PrintDialog = new PrintDialog();
        public static PageSetupDialog PageSetupDialog = new PageSetupDialog();
        public static PageSettings PageSettings = new PageSettings();
        public static PrintDocument PrintDocument = new PrintDocument();

        public static ImGearCompressOptions CompressOptions = new ImGearCompressOptions();
        public static bool DoCompression = false;
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

            //ImGearEvaluationManager.Initialize();
            //ImGearEvaluationManager.Mode = ImGearEvaluationMode.Watermark;

            //try
            //{
            //    ImGearRecognition recognitionEngine = new ImGearRecognition();
            //}
            //catch (ImGearException ex)
            //{
            //    if (ex.ErrorCode == ImGearErrorCodes.NOT_LICENSED)
            //    {
            //        MessageBox.Show("You are not licensed to use the OCR feature.", "Unlicensed", MessageBoxButtons.OK);
            //        Application.Exit();
            //    }
            //    throw ex;
            //}






            //ThumbnailController.PageAppend(a, "1");
            //ImGearThumbnailItem b = new ImGearThumbnailItem(ThumbnailController, a, "ses", 1, ImGearFormats.PDF);

            //ThumbnailController.DocumentAppend(PDFDocument, "dd");
        }

        private static void ARTForm_MouseMoved(object sender, ImGearARTFormsMouseEventArgs e)
        {
            CurrentMousePos.X = e.EventData.X;
            CurrentMousePos.Y = e.EventData.Y;
            UpdatePageView();
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

        private static bool CursorOnAnyMark(ImGearPoint mousePos, ImGearARTPage page)
        {
            mousePos.X = mousePos.X - (PageView.Width - PageView.Page.DIB.Width) / 2;
            mousePos.Y = mousePos.Y - (PageView.Height - PageView.Page.DIB.Height) / 2;

            foreach (ImGearARTMark mark in page)
            {
                if (mark.Bounds.Contains(mousePos))
                    return true;
            }

            return false;
        }

        private static void ARTForm_MouseRightButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (CursorOnAnyMark(new ImGearPoint(e.EventData.X, e.EventData.Y), ARTPages[CurrentPageID]))
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
            if (PageIsZoming)
            {
                // Create a new pen to draw dotted lines.
                ImGearRectangle igRectangleZoom;
                Pen pen = new Pen(Color.White);
                pen.DashStyle = DashStyle.Dot;
                // Define the currently selected zoom rectangle.

                if (StartMousePos.Y >= CurrentMousePos.Y)
                    igRectangleZoom = new ImGearRectangle(StartMousePos, CurrentMousePos);
                else
                    igRectangleZoom = new ImGearRectangle(CurrentMousePos, StartMousePos);
                // Draw the selection box.
                gr.DrawRectangle(pen, igRectangleZoom.Left, igRectangleZoom.Top,
                    igRectangleZoom.Width, igRectangleZoom.Height);
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
                ScrollBar.Visible = false;
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

        public static void FileSave(string fileName)
        {
            // Save to output file.
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    if (DoCompression)
                        PDFDocument.SaveCompressed(outputStream, CompressOptions);
                    else
                        PDFDocument.Save(outputStream, ImGearSavingFormats.PDF, 0, 0, PDFDocument.Pages.Count, ImGearSavingModes.OVERWRITE);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save file {0}. {1}", fileName, ex.Message));
                    return;
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
                    FileSave(fileDialogSave.FileName);
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
                //if(PrintDialog.PrinterSettings.ToPage > PDFDocument.Pages.Count)
                //{
                //    MessageBox.Show("Test");
                //    return;
                //}
                // Set a name for the print job.
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
            FileSave(DocumentPath);
        }

        public static void FileLoad(string fileName)
        {
            using (FileStream inputStream =
                    new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                try
                {
                    // Load the entire the document.
                    PDFDocument = (ImGearPDFDocument)ImGearFileFormats.LoadDocument(inputStream);

                    ARTPages.Clear();

                    InitializeScrollBar();
                    InitializeArtPages();

                    DocumentPath = fileName;
                    RenderPage(0);

                    OnDocumentChanged(null);
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

;
            OnPageChanged(null);
            UpdateAfterRender();
        }

        public static void ShowPrintMenu()
        {
            PageSetupDialog.PageSettings = PageSettings;
            PageSetupDialog.ShowDialog();
        }
    }
}
