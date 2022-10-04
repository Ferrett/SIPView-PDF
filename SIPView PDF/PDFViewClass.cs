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
        public static List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();
        public static ImGearDocument PDFDocument = null;
        public static ImGearARTForms ARTForm;
        public static ImGearPageView PageView;
        public static ScrollBar ScrollBar;
        public static StatusStrip StatusStrip;
        public static int currentPageID = 0;

        public static event EventHandler PageChanged;
        public static event EventHandler DocumentChanged;

        private static bool m_bZoomToRectSelect = false;
        // Set of points defining the selected area.
        private static ImGearPoint[] igPointsZoomToRect = new ImGearPoint[2];
        private static Color m_SelectorColor = Color.White;

        private static bool movePage;
        private static bool a;
        public static void TEST()
        {
            ARTForm.MouseRightButtonDown += ARTForm_MouseRightButtonDown;
            ARTForm.MouseRightButtonUp += ARTForm_MouseRightButtonUp;
            ARTForm.MouseMoved += ARTForm_MouseMoved;

            ARTForm.MouseLeftButtonDown += ARTForm_MouseLeftButtonDown;
            ARTForm.MouseLeftButtonUp += ARTForm_MouseLeftButtonUp;
            PageView.AfterDrawEvent += PageView_AfterDrawEvent;
        }

        private static void PageView_AfterDrawEvent(object sender, ImGearAfterDrawEventArgs e)
        {
            if (a)
            {
               // PageView.UseWaitCursor = false;
                a = false;
                MessageBox.Show("Test");
            }
        }

        private static void ARTForm_MouseLeftButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            movePage = false;
        }

        private static void ARTForm_MouseLeftButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            igPointsZoomToRect[0].X = e.EventData.X;
            igPointsZoomToRect[0].Y = e.EventData.Y;
            movePage = true;
        }

        private static void Display_PageScrolled(object sender, EventArgs e)
        {
            if (PageView.Display.GetScrollInfo(PageView).Vertical.Max != 0 || PageView.Display.GetScrollInfo(PageView).Vertical.Max != 0)
            {
                var a = ARTForm.Templates.Arrow;
                
            }
        }

        private static void Display_PageZoomed(object sender, EventArgs e)
        {
            
        }

        public static void KeyPressed(object sender, KeyEventArgs e)
        {
            if (PDFDocument == null)
                return;

            if (e.KeyCode == Keys.Back)
            {
                // запомнить координаты зажатия мыши, и двигать картинку на противоположное значение от разницы запомнитого и текущего
                ImGearScrollInfo igScrollInfo = PageView.Display.GetScrollInfo(PageView);
                //PageView.Display.ScrollTo(PageView, PageView.Display.GetScrollInfo(PageView).Horizontal.Pos,
                 //                                   PageView.Display.GetScrollInfo(PageView).Vertical.Pos+1);

                //PageView.Display.UpdateScrollFrom(PageView, new ImGearScrollInfo());
                PageView.Display.UpdateZoomFrom(new ImGearZoomInfo(1, 1, false, false));
                
                UpdatePageView();
            }
        }

        private static void ARTForm_MouseMoved(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (m_bZoomToRectSelect)
            {
                igPointsZoomToRect[1].X = e.EventData.X;
                igPointsZoomToRect[1].Y = e.EventData.Y;
            }
            else if(movePage)
            {
                var a = PageView.VerticalSmallChange;
                var b = PageView.VerticalLargeChange;

                var c = PageView.Display.GetScrollInfo(PageView).Horizontal.Pos;

                int dX = ((igPointsZoomToRect[0].X - e.EventData.X) * -1)/50;
                int dY = ((igPointsZoomToRect[0].Y - e.EventData.Y) * -1)/50;

                PageView.Display.ScrollTo(PageView, PageView.Display.GetScrollInfo(PageView).Horizontal.Pos - dX,
                                                    PageView.Display.GetScrollInfo(PageView).Vertical.Pos - dY);
            }

            PageView.Invalidate();
        }

        private static void ARTForm_MouseRightButtonUp(object sender, ImGearARTFormsMouseEventArgs e)
        {
            if (m_bZoomToRectSelect)
            {
                a = true;
                PageView.UseWaitCursor = true;

                // Leave Zoom to Rectangle selection mode.
                PageView.RegisterAfterDraw(null);
                m_bZoomToRectSelect = false;
                // Record the second point of the zoom rectangle,
                // normalizing it to the view's aspect ratio.
                igPointsZoomToRect[1].X = e.EventData.X;
                igPointsZoomToRect[1].Y = e.EventData.Y;
                //igPointsZoomToRect[1].Y = igPointsZoomToRect[0].Y +
                //    GetHeight(PageView, Math.Abs(igPointsZoomToRect[1].X - igPointsZoomToRect[0].X) + 1) - 1;
                // Create a rectangle based upon the 2 points.
                ImGearRectangle igRectangle = new ImGearRectangle(igPointsZoomToRect[0], igPointsZoomToRect[1]);
                // Cancel the zoom if it would be for a 0x0 or 1x1 rectangle.
                if (igRectangle.Width <= 1 || igRectangle.Height <= 1)
                    return;
                // Zoom to the selected rectangle
                PageView.Display.ZoomToRectangle(PageView, igRectangle);
                PageView.Invalidate();
            }
        }

        private static void ARTForm_MouseRightButtonDown(object sender, ImGearARTFormsMouseEventArgs e)
        {
            // Enter Zoom to Rectangle selection mode.
            m_bZoomToRectSelect = true;
            // Create a new pair of points to define the rectangle, 
            // setting the first point to the current position.
      
            igPointsZoomToRect[0].X = e.EventData.X;
            igPointsZoomToRect[0].Y = e.EventData.Y;
            // Register method to draw the selection rectangle.
            PageView.RegisterAfterDraw(
                new ImGearPageView.AfterDraw(DrawSelector));
        }

        private static void DrawSelector(System.Drawing.Graphics gr)
        {
            if (m_bZoomToRectSelect)
            {
                // Create a new pen to draw dotted lines.
                Pen pen = new Pen(m_SelectorColor);
                pen.DashStyle = DashStyle.Dot;
                // Define the currently selected zoom rectangle.
                ImGearRectangle igRectangleZoom;

                if (igPointsZoomToRect[0].Y>= igPointsZoomToRect[1].Y)
                 igRectangleZoom = new ImGearRectangle(igPointsZoomToRect[0], igPointsZoomToRect[1]);
                else
                    igRectangleZoom = new ImGearRectangle (igPointsZoomToRect[1], igPointsZoomToRect[0]);
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

            TEST();
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

        public static void RenderPage(int pageID)
        {
            currentPageID = pageID;
            //Load a single page from the loaded document.
            try
            {
                ImGearPage page = PDFDocument.Pages[currentPageID];
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
            ScrollBar.Value = currentPageID;
            UpdatePageView();
        }

        public static void ToolBarChangeVisibility()
        {
            if (ARTForm.ToolBar.Visible)
                ARTForm.ToolBar.Close();
            else
                ARTForm.ToolBar.Show();
        }

        public static void zoomOutToolStripMenuItem_Click()
        {
            if (null != PageView.Display && !PageView.Display.Page.DIB.IsEmpty())
            {
                // Get the current zoom info.
                ImGearZoomInfo igZoomInfo = PageView.Display.GetZoomInfo(PageView);

                igZoomInfo.Horizontal.Value = igZoomInfo.Horizontal.Value * (1 / 1.25);
                igZoomInfo.Vertical.Value = igZoomInfo.Vertical.Value * (1 / 1.25);
                igZoomInfo.Horizontal.Fixed = true;
                igZoomInfo.Vertical.Fixed = true;
                // Set the new zoom values and repaint the view.
                PageView.Display.UpdateZoomFrom(igZoomInfo);
                PageView.Update();
            }
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
            ARTPages[currentPageID].History.Undo();
            UpdatePageView();
        }

        public static void Redo()
        {
            ARTPages[currentPageID].History.Redo();
            UpdatePageView();
        }

        public static void PrevPage()
        {
            if (currentPageID > 0)
                RenderPage(currentPageID - 1);
        }

        public static void NextPage()
        {
            if (currentPageID < PDFDocument.Pages.Count - 1)
                RenderPage(currentPageID + 1);
        }

        public static int SelectedMarksCount()
        {
            int selectedMarksCounter = 0;

            foreach (ImGearARTMark ARTMark in ARTPages[currentPageID])
            {
                if (ARTPages[currentPageID].MarkIsSelected(ARTMark))
                    selectedMarksCounter++;
            }

            return selectedMarksCounter;
        }

        public static void SelectAllMarks()
        {
            if (SelectedMarksCount() == ARTPages[currentPageID].MarkCount)
                ARTPages[currentPageID].SelectMarks(false);
            else
                ARTPages[currentPageID].SelectMarks(true);
            UpdatePageView();
        }

        public static void UpdatePageView()
        {
            PageView.Update();
        }

        public static void AnnotationBakeIn()
        {
            PDFDocument.Pages[currentPageID] = ImGearART.BurnIn(PDFDocument.Pages[currentPageID], ARTPages[currentPageID], ImGearARTBurnInOptions.SELECTED, null);
            PageView.Page = PDFDocument.Pages[currentPageID];

            // Get burned marks ID
            List<int> bakedMarkID = new List<int>();
            foreach (ImGearARTMark ARTMark in ARTPages[currentPageID])
            {
                if (ARTPages[currentPageID].MarkIsSelected(ARTMark))
                    bakedMarkID.Add(ARTMark.Id);
            }

            // Delete burned marks by ID
            foreach (int ID in bakedMarkID)
            {
                ARTPages[currentPageID].MarkRemove(ID);
            }

            UpdatePageView();
        }

        public static void UpdateStatusStrip(StatusStrip statusStrip)
        {
            statusStrip.Items[0].Text = string.Format("{0} of {1}", currentPageID + 1, PDFDocument.Pages.Count);
        }

        public static void ScrollBarScrolled()
        {
            RenderPage(ScrollBar.Value);
            DisplayCurrentPageMarks();
            UpdateStatusStrip(StatusStrip);
        }

        private static void DisplayCurrentPageMarks()
        {
            PageView.Display = new ImGearPageDisplay(PDFDocument.Pages[currentPageID], ARTPages[currentPageID]);
            ARTForm.Page = ARTPages[currentPageID];
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
                    PDFDocument = ImGearFileFormats.LoadDocument(inputStream);
                    ARTPages.Clear();

                    InitializeScrollBar();
                    InitializeArtPages();

                    // Render first page.
                    RenderPage(0);
                    UpdatePageView();
                    OnDocumentChanged(null);
                    PageView.Display.PageZoomed += Display_PageZoomed;
                    PageView.Display.PageScrolled += Display_PageScrolled;

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
            using (ImGearPDFDocument igPDFDocument = (ImGearPDFDocument)PDFDocument)
            {
                ImGearPDFPrintOptions printOptions = new ImGearPDFPrintOptions();
                PrintDocument printDocument = new PrintDocument();

                //Use default Windows printer.
                printOptions.DeviceName = printDocument.PrinterSettings.PrinterName;

                //Print all pages.
                printOptions.StartPage = 0;
                printOptions.EndPage = PDFDocument.Pages.Count;
                igPDFDocument.Print(printOptions);
            }
        }

        public static void DisposeImGear()
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }
    }
}
