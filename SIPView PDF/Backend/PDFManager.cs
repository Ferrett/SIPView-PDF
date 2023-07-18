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

        public static void CreatePDFView()
        {
            if (TabControl.SelectedIndex == -1)
            {
                OnTabChanged(null);
            }

            SelectedTabID++;

            NewTabPage = new TabPage();

            Documents.Add(new PDFViewClass(CreatePageView(), CreateOCRPanel()));
            NewTabPage.Controls.Add(Documents.Last().OCRPanel);
            NewTabPage.Controls.Add(Documents.Last().PageView);
        }
        public static Panel CreateThumbnailPanel()
        {
            Panel ThumbnailPanel = new Panel()
            { 
                AutoScroll = true,
                Dock = DockStyle.Left,
                Location = new Point(0, 0),
                Name = "ThumbnailController",
                Size = new Size(155, 678),
                TabIndex = 3
            };

            return ThumbnailPanel;
        }
        public static Panel CreateOCRPanel()
        {
            Panel OCRPanel = new Panel()
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.WhiteSmoke,
                Name = "OCRPanel",
                Size = new Size(325, 54),
                TabIndex = 0,
                Visible = false
            };
            OCRPanel.Location = new Point(NewTabPage.Width - OCRPanel.Width - 17, 0);

            TextBox OCRTextBox = new TextBox()
            {
                Location = new Point(15, 17),
                Name = "OCRTextBox",
                Size = new Size(131, 20),
                TabIndex = 0
            };

            Label OCRLabel = new Label()
            {
                AutoSize = true,
                Location = new Point(157, 20),
                Name = "OCRLabel",
                Size = new Size(24, 13),
                TabIndex = 1,
                Text = "0/0"
            };

            Button OCRSearchBtn = new Button()
            {
                Location = new Point(197, 14),
                Margin = new Padding(0),
                Name = "OCRSearchBtn",
                Size = new Size(24, 24),
                TabIndex = 2,
                Text = "S",
                UseVisualStyleBackColor = true
            };

            Button OCRPrevBtn = new Button()
            {
                Location = new Point(234, 14),
                Margin = new Padding(0),
                Name = "OCRPrevBtn",
                Size = new Size(24, 24),
                TabIndex = 2,
                Text = "<",
                Enabled = false,
                UseVisualStyleBackColor = true
            };

            Button OCRNextBtn = new Button()
            {
                Location = new Point(260, 14),
                Margin = new Padding(0),
                Name = "OCRNextBtn",
                Size = new Size(24, 24),
                TabIndex = 3,
                Text = ">",
                Enabled = false,
                UseVisualStyleBackColor = true
            };
           
            Button OCRCloseBtn = new Button()
            {
                Location = new Point(295, 14),
                Name = "OCRCloseBtn",
                Size = new Size(24, 24),
                TabIndex = 4,
                Text = "X",
                UseVisualStyleBackColor = true
            };
            
            OCRPanel.Controls.Add(OCRCloseBtn);
            OCRPanel.Controls.Add(OCRNextBtn);
            OCRPanel.Controls.Add(OCRPrevBtn);
            OCRPanel.Controls.Add(OCRLabel);
            OCRPanel.Controls.Add(OCRTextBox);
            OCRPanel.Controls.Add(OCRSearchBtn);

            return OCRPanel;
        }
        public static ScrollBar CreateScrollBar()
        {
            ScrollBar ScrollBar = new VScrollBar() {
                Dock = DockStyle.Right,
                LargeChange = 1,
                TabIndex = 0,
                Visible = true,
            };

            ScrollBar.ValueChanged += new EventHandler(PDFViewKeyEvents.ScrollBarScrolled);

            return ScrollBar;
        }
        public static ImGearPageView CreatePageView()
        {
            ImGearPageView PageView = new ImGearPageView()
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Window,
                BindArrowKeyScrolling = false,
                Display = null,
                HorizontalArrowIncerment = 1,
                NotifyPageDown = null,
                NotifyPageUp = null,
                Page = null,
                TabIndex = 2,
                UseConfiguredScrollbarIncrements = false,
                VerticalArrowIncerment = 1
            };

            PageView.KeyDown += new KeyEventHandler(PDFViewKeyEvents.KeyDown);
            PageView.KeyUp += new KeyEventHandler(PDFViewKeyEvents.KeyUp);
            PageView.MouseDown += new MouseEventHandler(PDFViewKeyEvents.PageView_MouseDown);
            PageView.MouseUp += new MouseEventHandler(PDFViewKeyEvents.PageView_MouseUp);
            PageView.MouseMove += new MouseEventHandler(PDFViewKeyEvents.PageView_MouseMove);
            PageView.MouseWheel += new MouseEventHandler(PDFViewKeyEvents.WheelScrolled);

            return PageView;
        }

        public static void AddTab()
        {
            if (Documents[SelectedTabID].PDFDocument.Pages.Count > 1) {
                Documents[SelectedTabID].AddMultupageControls(CreateScrollBar(), CreateThumbnailPanel());
                NewTabPage.Controls.Add(Documents.Last().ScrollBar);
                NewTabPage.Controls.Add(Documents.Last().ThumbnailPanel);
            }

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

        public static void TabControl_MouseClick(object sender, MouseEventArgs e)
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
