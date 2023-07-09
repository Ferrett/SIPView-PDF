using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Windows.Forms;
using SIPView_PDF.Backend.PDF_Features;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SIPView_PDF
{
    public static class PDFManager
    {
        public static event EventHandler PageChanged;
        public static event EventHandler DocumentChanged;
        public static event EventHandler TabChanged;

        public static event EventHandler ARTPage_MarkUpdate;
        public static event EventHandler ARTPage_MarkSelectionChanged;
        public static event EventHandler ARTPage_HistoryChanged;

        public static ImGearCompressOptions CompressOptions = new ImGearCompressOptions() { IsRemoveImageThumbnailEnabled = false };
        public static bool DoCompression = false;

        public static ImGearPDFPreflightProfile PreflightProfile;
        public static bool DoConversion = false;

        public static int SelectedTabID = -1;
        public static ViewModes ViewMode = ViewModes.DEFAULT;

        public static List<PDFViewClass> Documents = new List<PDFViewClass>();
        public static TabControl TabControl;
        public static ImGearPan Pan;
        public static TabPage NewTabPage;

        public static void InitializeImGear()
        {
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();
        }

        public static void AddPageView()
        {
            if (TabControl.SelectedIndex == -1)
            {
               OnTabChanged(null);
            }

            SelectedTabID++;

            #region Controls
            NewTabPage = new TabPage();

            ImGearPageView PageView = new ImGearPageView();

            PageView.Dock = DockStyle.Fill;
            PageView.BackColor = SystemColors.Window;
            PageView.BindArrowKeyScrolling = false;

            PageView.Display = null;
            PageView.HorizontalArrowIncerment = 1;

            PageView.NotifyPageDown = null;
            PageView.NotifyPageUp = null;
            PageView.Page = null;

            PageView.TabIndex = 2;
            PageView.UseConfiguredScrollbarIncrements = false;
            PageView.VerticalArrowIncerment = 1;

            PageView.KeyDown += new KeyEventHandler(PDFViewKeyEvents.KeyDown);
            PageView.KeyUp += new KeyEventHandler(PDFViewKeyEvents.KeyUp);

            PageView.MouseDown += new MouseEventHandler(PDFViewKeyEvents.PageView_MouseDown);
            PageView.MouseUp += new MouseEventHandler(PDFViewKeyEvents.PageView_MouseUp);
            PageView.MouseMove += new MouseEventHandler(PDFViewKeyEvents.PageView_MouseMove);
            PageView.MouseWheel += new MouseEventHandler(PDFViewKeyEvents.WheelScrolled);

            ScrollBar ScrollBar = new VScrollBar();
            ScrollBar.Dock = DockStyle.Right;
            ScrollBar.LargeChange = 1;

            ScrollBar.TabIndex = 0;
            ScrollBar.Visible = true;
            ScrollBar.ValueChanged += new EventHandler(PDFViewKeyEvents.ScrollBarScrolled);

            Panel OCRPanel = new Panel();
            OCRPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            OCRPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            OCRPanel.Name = "OCRPanel";
            OCRPanel.Size = new System.Drawing.Size(325, 54);
            OCRPanel.Location = new System.Drawing.Point(NewTabPage.Width-OCRPanel.Width - ScrollBar.Width, 0);
            OCRPanel.TabIndex = 0;
            OCRPanel.Visible = false;

            TextBox OCRTextBox = new TextBox();
            OCRTextBox.Location = new System.Drawing.Point(15, 17);
            OCRTextBox.Name = "OCRTextBox";
            OCRTextBox.Size = new System.Drawing.Size(131, 20);
            OCRTextBox.TabIndex = 0;
            
            Label OCRLabel = new Label();
            OCRLabel.AutoSize = true;
            OCRLabel.Location = new System.Drawing.Point(157, 20);
            OCRLabel.Name = "OCRLabel";
            OCRLabel.Size = new System.Drawing.Size(24, 13);
            OCRLabel.TabIndex = 1;
            OCRLabel.Text = "0/0";

            Button OCRSearchBtn = new Button();
            OCRSearchBtn.Location = new System.Drawing.Point(197, 14);
            OCRSearchBtn.Margin = new System.Windows.Forms.Padding(0);
            OCRSearchBtn.Name = "OCRSearchBtn";
            OCRSearchBtn.Size = new System.Drawing.Size(24, 24);
            OCRSearchBtn.TabIndex = 2;
            OCRSearchBtn.Text = "S";
            OCRSearchBtn.UseVisualStyleBackColor = true;

            Button OCRPrevBtn = new Button();
            OCRPrevBtn.Location = new System.Drawing.Point(234, 14);
            OCRPrevBtn.Margin = new System.Windows.Forms.Padding(0);
            OCRPrevBtn.Name = "OCRPrevBtn";
            OCRPrevBtn.Size = new System.Drawing.Size(24, 24);
            OCRPrevBtn.TabIndex = 2;
            OCRPrevBtn.Text = "<";
            OCRPrevBtn.UseVisualStyleBackColor = true;

            Button OCRNextBtn = new Button();
            OCRNextBtn.Location = new System.Drawing.Point(260, 14);
            OCRNextBtn.Margin = new System.Windows.Forms.Padding(0);
            OCRNextBtn.Name = "OCRNextBtn";
            OCRNextBtn.Size = new System.Drawing.Size(24, 24);
            OCRNextBtn.TabIndex = 3;
            OCRNextBtn.Text = ">";
            OCRNextBtn.UseVisualStyleBackColor = true;

            Button OCRCloseBtn = new Button();
            OCRCloseBtn.Location = new System.Drawing.Point(295, 14);
            OCRCloseBtn.Name = "OCRCloseBtn";
            OCRCloseBtn.Size = new System.Drawing.Size(24, 24);
            OCRCloseBtn.TabIndex = 4;
            OCRCloseBtn.Text = "X";
            OCRCloseBtn.UseVisualStyleBackColor = true;

            OCRPanel.Controls.Add(OCRCloseBtn);
            OCRPanel.Controls.Add(OCRNextBtn);
            OCRPanel.Controls.Add(OCRPrevBtn);
            OCRPanel.Controls.Add(OCRLabel);
            OCRPanel.Controls.Add(OCRTextBox);
            OCRPanel.Controls.Add(OCRSearchBtn);

            NewTabPage.Controls.Add(OCRPanel);
            NewTabPage.Controls.Add(PageView);
            NewTabPage.Controls.Add(ScrollBar);
            #endregion

            Documents.Add(new PDFViewClass(PageView, ScrollBar, OCRPanel));
            
        }

        public static void AddTab()
        {
            NewTabPage.Text = Path.GetFileName(Documents[SelectedTabID].DocumentPath);
            TabControl.TabPages.Add(NewTabPage);
            TabControl.SelectedTab = TabControl.TabPages[TabControl.TabPages.Count - 1];

            if (TabControl.TabPages.Count == 1)
            {
                Documents[SelectedTabID].PageView.Focus();
                Pan.SourceView = Documents[SelectedTabID].PageView;
            }
        }

        public static void CloseTab(int tabID)
        {
            Documents.RemoveAt(tabID);
            TabControl.TabPages.RemoveAt(tabID);
            SelectedTabID = TabControl.SelectedIndex;

            if (TabControl.TabPages.Count != 0)
            {
                TabControl.SelectedTab = TabControl.TabPages[TabControl.TabPages.Count - 1];
            }
        }

        public static void SelectedTabChanged()
        {
            SelectedTabID = TabControl.SelectedIndex;

            if (TabControl.TabPages.Count != 0)
            {
                Documents[SelectedTabID].PageView.Focus();
                Pan.SourceView = Documents[SelectedTabID].PageView;
            }
            OnTabChanged(null);
        }

        public static void TabControl_MouseClick(object sender,MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                TabControl tabControl = (TabControl)sender;

                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    Rectangle tabRect = tabControl.GetTabRect(i);
                    if (tabRect.Contains(e.Location))
                    {
                        CloseTab(i);
                    }
                }
            }
        }

        #region EVENT_HANDLERS

        public static void OnTabChanged(EventArgs e)
        {
            if (TabChanged != null)
                TabChanged(null, e);
        }
        public static void OnARTPage_MarkUpdate(EventArgs e)
        {
            if (ARTPage_MarkUpdate != null)
                ARTPage_MarkUpdate(null, e);
        }

        public static void OnARTPage_MarkSelectionChanged(EventArgs e)
        {
            if (ARTPage_MarkSelectionChanged != null)
                ARTPage_MarkSelectionChanged(null, e);
        }

        public static void OnPageChanged(EventArgs e)
        {
            if (PageChanged != null)
                PageChanged(null, e);
        }

        public static void OnARTPage_HistoryChanged(EventArgs e)
        {
            if (ARTPage_HistoryChanged != null)
                ARTPage_HistoryChanged(null, e);
        }

        public static void OnDocumentChanged(EventArgs e)
        {
            if (DocumentChanged != null)
                DocumentChanged(null, e);
        }
        #endregion

        public static void DisposeImGear()
        {
            ImGearPDF.Terminate();
            foreach (PDFViewClass document in Documents)
            {
                document.PageView.Display = null;
            }
        }
    }
}
