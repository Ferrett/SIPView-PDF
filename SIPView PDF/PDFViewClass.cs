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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace SIPView_PDF
{
    public static class PDFViewClass
    {
        public static event EventHandler PageChanged;
        public static event EventHandler DocumentChanged;

        public static List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();
        public static ImGearPDFDocument PDFDocument = null;
        public static ImGearARTForms ARTForm;
        public static ImGearPageView PageView;
        public static ScrollBar ScrollBar;
        public static StatusStrip StatusStrip;
        public static int CurrentPageID = 0;

        private static ImGearPoint[] MousePosition = new ImGearPoint[2];

        private static bool CtrlKeyPressed;
        private static bool PageIsZoming = false;

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
            for (int i = 0; i < PDFDocument.Pages.Count; i++)
            {
                ARTPages.Add(new ImGearARTPage());
                ARTPages.Last().History.IsEnabled = true;
                ARTPages.Last().History.Limit = 99;
            }
            ARTPages.ForEach(x => x.RemoveMarks());
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
            if (!CtrlKeyPressed)
                return;

            if (e.Delta > 0)
            {
                PageView.Display.ZoomToRectangle(PageView, new ImGearRectangle(
                    new ImGearPoint((int)(MousePosition[1].X - PageView.Width / 2.4), (int)(MousePosition[1].Y - PageView.Height / 2.4)),
                    new ImGearPoint((int)(MousePosition[1].X + PageView.Width / 2.4), (int)(MousePosition[1].Y + PageView.Height / 2.4))));
            }
            else
            {
                PageView.Display.ZoomToRectangle(PageView, new ImGearRectangle(
                    new ImGearPoint((int)(MousePosition[1].X - PageView.Width * 0.6), (int)(MousePosition[1].Y - PageView.Height * 0.6)),
                    new ImGearPoint((int)(MousePosition[1].X + PageView.Width * 0.6), (int)(MousePosition[1].Y + PageView.Height * 0.6))));
            }

            PageView.Invalidate();
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
        }

        private static void ARTForm_MouseMoved(object sender, ImGearARTFormsMouseEventArgs e)
        {
            MousePosition[1].X = e.EventData.X;
            MousePosition[1].Y = e.EventData.Y;

            UpdatePageView();
        }

        private static void ARTForm_MouseRightButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (PageIsZoming)
            {
                PageView.RegisterAfterDraw(null);
                PageIsZoming = false;

                MousePosition[1].X = e.EventData.X;
                MousePosition[1].Y = e.EventData.Y;

                ImGearRectangle igRectangle = new ImGearRectangle(MousePosition[0], MousePosition[1]);
                // Cancel the zoom if it would be for a 0x0 or 1x1 rectangle.
                if (igRectangle.Width <= 1 || igRectangle.Height <= 1)
                    return;
                // Zoom to the selected rectangle
                PageView.Display.ZoomToRectangle(PageView, igRectangle);

                PageView.Invalidate();
            }
        }

        private static bool CursorOnAnyMark(ImGearPoint mousePos, ImGearARTPage page)
        {
            mousePos.X = mousePos.X - (PageView.Width- PageView.Page.DIB.Width)/2;
            mousePos.Y = mousePos.Y - (PageView.Height- PageView.Page.DIB.Height)/2;
            
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

            MousePosition[0].X = e.EventData.X;
            MousePosition[0].Y = e.EventData.Y;
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
                Pen pen = new Pen(Color.MediumVioletRed);
                pen.DashStyle = DashStyle.Dot;
                // Define the currently selected zoom rectangle.

                if (MousePosition[0].Y >= MousePosition[1].Y)
                    igRectangleZoom = new ImGearRectangle(MousePosition[0], MousePosition[1]);
                else
                    igRectangleZoom = new ImGearRectangle(MousePosition[1], MousePosition[0]);
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

        public static void RenderPage(int pageID)
        {
            CurrentPageID = pageID;
            //Load a single page from the loaded document.
            try
            {
                ImGearPage page = PDFDocument.Pages[CurrentPageID];
                if (page != null)
                {
                    //Create a new page display to prepare the page for being displayed.

                    ImGearPageDisplay igPageDisplay = new ImGearPageDisplay(page);
                    //Associate the page display with the page view.
                    PageView.Display = igPageDisplay;

                    //Cause the page view to repaint.
                    PageView.Invalidate();
                }
            }
            catch (ImGearException ex)
            {
                //Perform error handling.
                MessageBox.Show(ex.Message);
            }
            OnPageChanged(null);
            UpdateAfterRender();
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

        public static void UpdateStatusStrip(StatusStrip statusStrip)
        {
            statusStrip.Items[0].Text = $"{CurrentPageID + 1} of {PDFDocument.Pages.Count}";
        }

        public static void ScrollBarScrolled()
        {
            RenderPage(ScrollBar.Value);
            DisplayCurrentPageMarks();
            UpdateStatusStrip(StatusStrip);
        }

        private static void DisplayCurrentPageMarks()
        {
            PageView.Display = new ImGearPageDisplay(PDFDocument.Pages[CurrentPageID], ARTPages[CurrentPageID]);
            ARTForm.Page = ARTPages[CurrentPageID];
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

                    // Render first page.
                    RenderPage(0);
                    UpdatePageView();
                    OnDocumentChanged(null);
                }
                catch (ImGearException ex)
                {
                    // Perform error handling.
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static void FileSave(string fileName)
        {
            // Save to output file.
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    // Save the page in the requested format.
                    ImGearFileFormats.SaveDocument(
                        PDFDocument,
                        outputStream,
                        0,
                        ImGearSavingModes.OVERWRITE,
                        ImGearSavingFormats.PDF,
                        null);
                }
                // Perform error handling.
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
            ImGearPDFPrintOptions printOptions = new ImGearPDFPrintOptions();
            PrintDocument printDocument = new PrintDocument();

            //Use default Windows printer.

            printOptions.DeviceName = printDocument.PrinterSettings.PrinterName;

            //Print all pages.
            printOptions.StartPage = 0;
            printOptions.EndPage = PDFDocument.Pages.Count;
            PDFDocument.Print(printOptions);
        }

        public static void DisposeImGear()
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }
    }
}
