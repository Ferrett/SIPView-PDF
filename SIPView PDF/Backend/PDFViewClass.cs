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
    public class PDFViewClass
    {
        public List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();
        public ImGearPDFDocument PDFDocument = null;



        public ImGearPageView PageView;
        public ScrollBar ScrollBar;

        public Panel OCRPanel;
        public TextBox OCRTextBox;
        public Label OCRLabel;
        public Button OCRSearchBtn;
        public Button OCRPrevBtn;
        public Button OCRNextBtn;
        public Button OCRCloseBtn;

        public int PageID = 0;

        public bool DrawZoomRectangle = false;

        public string DocumentPath;

        public ImGearARTForms ARTForm;
        public PDFViewClass(ImGearPageView PageView, ScrollBar ScrollBar, Panel OCRPanel)
        {
            this.PageView = PageView; 
            this.ScrollBar = ScrollBar; 
            this.OCRPanel = OCRPanel;

            ARTForm = new ImGearARTForms(PageView, ImGearARTToolBarModes.ART30);

            OCRTextBox = OCRPanel.Controls["OCRTextBox"] as TextBox;
            OCRSearchBtn = OCRPanel.Controls["OCRSearchBtn"] as Button;
            OCRPrevBtn = OCRPanel.Controls["OCRPrevBtn"] as Button;
            OCRNextBtn = OCRPanel.Controls["OCRNextBtn"] as Button;
            OCRCloseBtn = OCRPanel.Controls["OCRCloseBtn"] as Button;
            OCRLabel = OCRPanel.Controls["OCRLabel"] as Label;
            


            InitOCREvents();
            InitArtPageEvents();
            InitializeArtFormEvents();
            InitializeToolBar();
        }

       
        private void OCRCloseBtn_GotFocus(object sender, EventArgs e)
        {
            PageView.Focus();
        }

        public void InitOCREvents()
        {
            OCRCloseBtn.GotFocus += OCRCloseBtn_GotFocus;

            OCRSearchBtn.Click += OCRSearchBtn_Click;
            OCRPrevBtn.Click += OCRPrevBtn_Click;
            OCRNextBtn.Click += OCRNextBtn_Click;
            OCRCloseBtn.Click += OCRCloseBtn_Click;
        }

        private void OCRCloseBtn_Click(object sender, EventArgs e)
        {
            PDFViewOCR.CloseOCR();
            OCRPanel.Visible=false;
        }

        private void OCRPrevBtn_Click(object sender, EventArgs e)
        {
            PDFViewOCR.HighlightPrevWord();
        }

        private void OCRNextBtn_Click(object sender, EventArgs e)
        {
            PDFViewOCR.HighlightNextWord();
        }

        private void OCRSearchBtn_Click(object sender, EventArgs e)
        {
            PDFViewOCR.WordSearch();
        }

        public void InitializeToolBar()
        {
            ARTForm.Mode = ImGearARTModes.EDIT;

            ARTForm.ToolBar.Size = new Size(80, 295);
            ARTForm.ToolBar.Text = string.Empty;

            ARTForm.ToolBar.Location = new Point(0, 50);
            ARTForm.ToolBar.ShowInTaskbar = true;
        }

        public void InitializeArtFormEvents()
        {
            ARTForm.MouseRightButtonDown += PDFViewKeyEvents.ARTForm_MouseRightButtonDown;
            ARTForm.MouseLeftButtonDown += PDFViewKeyEvents.ARTForm_MouseLeftButtonDown;
            ARTForm.MouseLeftButtonUp += PDFViewKeyEvents.ARTForm_MouseLeftButtonUp;
            ARTForm.MouseRightButtonUp += PDFViewKeyEvents.ARTForm_MouseRightButtonUp;
            ARTForm.MouseMoved += PDFViewKeyEvents.ARTForm_MouseMoved;

            ARTForm.MarkCreated += ARTForm_MarkCreated;
        }
        private void ARTForm_MarkCreated(object sender, ImGearARTFormsMarkCreatedEventArgs e)
        {
            UpdatePageView();
        }
        public void ToolBarChangeVisibility()
        {
            if (ARTForm.ToolBar.Visible)
                ARTForm.ToolBar.Close();
            else
                ARTForm.ToolBar.Show();
        }
        public void InitArtPageEvents()
        {
            for (int i = 0; i < ARTPages.Count; i++)
            {
                ARTPages[i].MarkAdded += ARTPage_MarkUpdate;
                ARTPages[i].MarkRemoved += ARTPage_MarkUpdate;
                ARTPages[i].MarkSelectionChanged += ARTPage_MarkSelectionChanged;
                ARTPages[i].History.HistoryChanged += ARTPage_HistoryChanged;
            }
        }

        private void ARTPage_HistoryChanged(object sender, ImGearARTHistoryEventArgs e)
        {
            PDFManager.OnARTPage_HistoryChanged(null);
        }

        private void ARTPage_MarkSelectionChanged(object sender, ImGearARTMarkEventArgs e)
        {
            PDFManager.OnARTPage_MarkSelectionChanged(null);
        }

        private void ARTPage_MarkUpdate(object sender, ImGearARTMarkEventArgs e)
        {
            PDFManager.OnARTPage_MarkUpdate(null);
        }

       

       

        public void DrawSelector(System.Drawing.Graphics gr)
        {
            if (DrawZoomRectangle || PDFViewOCR.DrawTextSelecting)
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

        public void InitializeScrollBar()
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

        private void UpdateAfterRender()
        {
            PDFViewAnnotations.DisplayCurrentPageMarks();

            ScrollBar.Value = PageID;
            PDFViewOCR.FindWordsInPage(); ///////////////// ?? 
            UpdatePageView();
        }

       

        public void RotateLeft()
        {
            ImGearProcessing.Rotate(PageView.Page, ImGearRotationValues.VALUE_270);
            UpdatePageView();
        }

        public void RotateRight()
        {
            ImGearProcessing.Rotate(PageView.Page, ImGearRotationValues.VALUE_90);

            UpdatePageView();
        }

        public void Undo()
        {
            ARTPages[PageID].History.Undo();
            UpdatePageView();
        }

        public void Redo()
        {
            ARTPages[PageID].History.Redo();
            UpdatePageView();
        }

        public void PrevPage()
        {
            if (PageID > 0)
                RenderPage(PageID - 1);
        }

        public void NextPage()
        {
            if (PageID < PDFDocument.Pages.Count - 1)
                RenderPage(PageID + 1);
        }



        public void RenderPage(int pageID)
        {
            PageID = pageID;

            try
            {
                PageView.Page = PDFDocument.Pages[PageID];
               
                PageView.Display.Background.Color.Red
                    = PageView.Display.Background.Color.Green
                    = PageView.Display.Background.Color.Blue = 96;

            }
            catch (ImGearException ex)
            {
                MessageBox.Show(ex.Message);
            }

            //UpdateThumbnailSelection(PageID);
            PDFManager.OnPageChanged(null);
            UpdateAfterRender();
        }

        public void UpdatePageView()
        {
            PageView.Invalidate();
            PageView.Update();
        }

       
    }
}
