using ImageGear.ART;
using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Display;

using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Processing;
using ImageGear.Windows.Forms;
using SIPView_PDF.Backend.PDF_Features;
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


        public static int CurrentPageID = 0;

        public static ViewModes ViewMode = ViewModes.DEFAULT;


        public static bool DrawZoomRectangle = false;



        public static string DocumentPath;

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

        private static void DisplayCurrentPageMarks()
        {
            PageView.Display = new ImGearPageDisplay(PDFDocument.Pages[CurrentPageID], ARTPages[CurrentPageID]);
            ARTForm.Page = ARTPages[CurrentPageID];
        }

        public static void InitializeEvents()
        {
            ARTForm.MouseRightButtonDown += PDFViewKeyEvents.ARTForm_MouseRightButtonDown;
            ARTForm.MouseLeftButtonDown += PDFViewKeyEvents.ARTForm_MouseLeftButtonDown;
            ARTForm.MouseLeftButtonUp += PDFViewKeyEvents.ARTForm_MouseLeftButtonUp;
            ARTForm.MouseRightButtonUp += PDFViewKeyEvents.ARTForm_MouseRightButtonUp;
            ARTForm.MouseMoved += PDFViewKeyEvents.ARTForm_MouseMoved;

            ARTForm.MarkCreated += ARTForm_MarkCreated;
        }

        private static void ARTForm_MarkCreated(object sender, ImGearARTFormsMarkCreatedEventArgs e)
        {
            UpdatePageView();
        }

        public static void DrawSelector(System.Drawing.Graphics gr)
        {
            if (DrawZoomRectangle || PDFViewOCR.TextIsSelecting)
            {
                // Create a new pen to draw dotted lines.

                Pen pen = new Pen(Color.DarkMagenta);
                pen.DashStyle = DashStyle.Solid;
                pen.Width = 2;

                ImGearRectangle rect;

                // Define the currently selected zoom rectangle.

                if (PDFViewKeyEvents.StartMousePos.Y >= PDFViewKeyEvents.CurrentMousePos.Y)
                    rect = new ImGearRectangle(PDFViewKeyEvents.StartMousePos, PDFViewKeyEvents.CurrentMousePos);
                else
                    rect = new ImGearRectangle(PDFViewKeyEvents.CurrentMousePos, PDFViewKeyEvents.StartMousePos);
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
            ARTForm = new ImGearARTForms(PageView, ImGearARTToolBarModes.ART30);
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

        private static void UpdateAfterRender()
        {
            DisplayCurrentPageMarks();
            ScrollBar.Value = CurrentPageID;
            PDFViewOCR.FindWordsInPage(); ///////////////// ?? 
            UpdatePageView();
        }

        public static void ToolBarChangeVisibility()
        {
            if (ARTForm.ToolBar.Visible)
                ARTForm.ToolBar.Close();
            else
                ARTForm.ToolBar.Show();
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



        public static void FileLoad(string fileName)
        {
            using (FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                try
                {
                    // Load the entire the document.
                    PDFDocument = (ImGearPDFDocument)ImGearFileFormats.LoadDocument(inputStream);
                    PDFViewOCR.PDFWordFinder = new ImGearPDFWordFinder(PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);

                    DocumentPath = fileName;
                    ARTPages.Clear();

                    InitializeScrollBar();
                    PDFViewAnnotations.InitializeArtPages();
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

            UpdateThumbnailSelection(CurrentPageID);
            OnPageChanged(null);
            UpdateAfterRender();
        }

        public static void UpdatePageView()
        {
            PageView.Invalidate();
            PageView.Update();
        }

        public static void DisposeImGear()
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }
    }
}
