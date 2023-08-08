using ImageGear.ART;
using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Display;

using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Processing;
using ImageGear.Windows.Forms;
using Newtonsoft.Json.Linq;
using SIPView_PDF.Backend.PDF_Features;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;



namespace SIPView_PDF
{
    public class PDFViewClass
    {
        public List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();
        public ImGearPDFDocument PDFDocument = null;
        public ImGearARTForms ARTForm;
       
        public ImGearPageView PageView;
        public SplitContainer SplitContainer;
        public ScrollBar ScrollBar;

        public Panel OCRPanel;
        public TextBox OCRTextBox;
        public Label OCRLabel;
        public Button OCRSearchBtn;
        public Button OCRPrevBtn;
        public Button OCRNextBtn;
        public Button OCRCloseBtn;

        //public Panel ThumbnailPanel;
        public List<Thumbnail> Thumbnails;

        public bool DrawZoomRectangle = false;
        public string DocumentPath;
        public int PageID = 0;

        public PDFViewClass(SplitContainer splitContainer, ImGearPageView PageView, Panel OCRPanel)
        {
            this.PageView = PageView; 
            this.OCRPanel = OCRPanel;
            this.SplitContainer = splitContainer;

            splitContainer.Panel2.Controls.Add(this.OCRPanel);
            splitContainer.Panel2.Controls.Add(this.PageView);

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

        private void UpdateThumbnailSelection()
        {
            if (PDFDocument.Pages.Count <= 1)
                return;

            Thumbnails.Where(x => x.IsSelected == true).First().Deselect();
            Thumbnails[PageID].Select();
        }

        private void InitializeThumbnails()
        {
            Thumbnails = new List<Thumbnail>();

            for (int i = 0; i < PDFDocument.Pages.Count; i++)
            {
                Thumbnails.Add(new Thumbnail
                {
                    IsSelected=false,
                    Image = new ImGearPageView()
                    {
                        Page = PDFDocument.Pages[i],
                        Location = new Point(20, 20 + 140 * i),
                        Size = new Size(100,100),
                        Tag=i
                    },
                    Background = new Panel()
                    {
                        Location = new Point(14, 14 + 140 * i),
                        Size = new Size(112, 112),
                        BackColor = Color.Transparent,
                        Visible = true,
                        Tag = i
                    },
                    Label = new Label
                    {
                        Text = $"Page {i + 1}",
                        Location = new Point(46, 130 + 140 * i),
                        Tag = i
                    }
                });

                Thumbnails.Last().Image.Display.Background.Color.Red = SplitContainer.Panel1.BackColor.R;
                Thumbnails.Last().Image.Display.Background.Color.Green = SplitContainer.Panel1.BackColor.G;
                Thumbnails.Last().Image.Display.Background.Color.Blue = SplitContainer.Panel1.BackColor.B;

                Thumbnails.Last().Image.MouseEnter += PDFViewClass_MouseEnter;
                Thumbnails.Last().Image.MouseLeave += PDFViewClass_MouseLeave;
                Thumbnails.Last().Image.Click += PDFViewClass_Click;

                SplitContainer.Panel1.Controls.Add(Thumbnails.Last().Label);
                SplitContainer.Panel1.Controls.Add(Thumbnails.Last().Background);
                SplitContainer.Panel1.Controls.Add(Thumbnails.Last().Image);
                Thumbnails.Last().Image.BringToFront();
            }
            Thumbnails.First().Select();
        }

        private  void PDFViewClass_Click(object sender, EventArgs e)
        {
            RenderPage((int)(sender as ImGearPageView).Tag);
        }

        private  void PDFViewClass_MouseLeave(object sender, EventArgs e)
        {
            Thumbnails[(int)(sender as ImGearPageView).Tag].Background.BorderStyle = BorderStyle.None;
        }

        private  void PDFViewClass_MouseEnter(object sender, EventArgs e)
        {
            Thumbnails[(int)(sender as ImGearPageView).Tag].Background.BorderStyle = BorderStyle.FixedSingle;
        }

        public void AddMultupageControls(ScrollBar ScrollBar)
        {
            this.ScrollBar = ScrollBar;

            InitializeThumbnails();

            SplitContainer.SplitterMoved += SplitContainer_SplitterMoved;
            SplitContainer.IsSplitterFixed = false;
            
            SplitContainer.Panel2.Controls.Add(ScrollBar);
            SplitContainer.SplitterDistance = 30;
            
        }

        private void SplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // Set the maximum width for the right panel
            int maxRightPanelWidth = SplitContainer.Width/5; // Set your desired maximum width here
            var a = SplitContainer.SplitterDistance;
            // Check if the new position of the splitter exceeds the maximum width
            if (e.SplitX > maxRightPanelWidth)
            {

                SplitContainer.SplitterDistance = maxRightPanelWidth;
                // Cancel the event to prevent the splitter from moving beyond the maximum width
            }
                CenterControlInPanel();
        }

        private void CenterControlInPanel()
        {
            for (int i = 0; i < Thumbnails.Count; i++)
            {
                Thumbnails[i].Image.Size = new Size((100 * SplitContainer.SplitterDistance) / 180, (100 * SplitContainer.SplitterDistance) / 180);
                Thumbnails[i].Background.Size = new Size((112 * SplitContainer.SplitterDistance) / 180, (112 * SplitContainer.SplitterDistance) / 180);

                Thumbnails[i].Image.Location = new Point((SplitContainer.Panel1.Width / 2) - Thumbnails[i].Image.Width / 2,  ((20+(170*i)) * SplitContainer.SplitterDistance) / 180);
                Thumbnails[i].Background.Location = new Point((SplitContainer.Panel1.Width / 2) - Thumbnails[i].Background.Width / 2, ((14 + (170 * i)) * SplitContainer.SplitterDistance) / 180);
                Thumbnails[i].Label.Location = new Point((SplitContainer.Panel1.Width / 2) - Thumbnails[i].Label.Width / 4, ((140 + (170 * i)) * SplitContainer.SplitterDistance) / 180);
            }
           
        }

       

        private void OCRCloseBtn_GotFocus(object sender, EventArgs e)
        {
            PageView.Focus();
        }

        public void InitOCREvents()
        {
            OCRCloseBtn.GotFocus += OCRCloseBtn_GotFocus;
            OCRTextBox.TextChanged += OCRTextBox_TextChanged;
            OCRSearchBtn.Click += OCRSearchBtn_Click;
            OCRPrevBtn.Click += OCRPrevBtn_Click;
            OCRNextBtn.Click += OCRNextBtn_Click;
            OCRCloseBtn.Click += OCRCloseBtn_Click;
        }

        private void OCRTextBox_TextChanged(object sender, EventArgs e)
        {
            PDFViewOCR.WordSearch();
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
            ImGearARTPage page = sender as ImGearARTPage;

            foreach (ImGearARTMark item in page)
            {
                if (item.UserData != null && (item.UserData.Equals("OCR") || item.UserData.Equals("TXT")))
                {
                    page.MarkSelect(item.Id, false);
                }
            }

            PDFManager.OnARTPage_MarkSelectionChanged(null);
        }

        private void ARTPage_MarkUpdate(object sender, ImGearARTMarkEventArgs e)
        {
            PDFManager.OnARTPage_MarkUpdate(null);
        }

        public void DrawSelector(System.Drawing.Graphics gr)
        {
            if (DrawZoomRectangle || PDFViewTextSelecting.TextIsSelecting)
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
        }

        private void UpdateAfterRender()
        {
            PDFViewAnnotations.DisplayCurrentPageMarks();

            if (PDFDocument.Pages.Count > 1)
            {
                ScrollBar.Value = PageID;
                UpdateThumbnailSelection();
            }

            PDFViewTextSelecting.FindWordsInPage(); 

            if (PDFViewOCR.TextIsSearched)
                PDFViewOCR.DrawHighlightOnCurrentPage();

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
                //PageView.Display.ARTPage = ARTPages[PageID];

                PageView.Display.Background.Color.Red
                    = PageView.Display.Background.Color.Green
                    = PageView.Display.Background.Color.Blue = 96;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           
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
