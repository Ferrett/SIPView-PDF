﻿using ImageGear.ART.Forms;
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
        public static event EventHandler ARTPage_MarkUpdate;
        public static event EventHandler ARTPage_MarkSelectionChanged;
        public static event EventHandler ARTPage_HistoryChanged;

        public static int SelectedTabID = 0;

        public static List<PDFViewClass> Documents = new List<PDFViewClass>();

        public static ImGearCompressOptions CompressOptions = new ImGearCompressOptions() { IsRemoveImageThumbnailEnabled = false };
        public static bool DoCompression = false;

        public static ImGearPDFPreflightProfile PreflightProfile;
        public static bool DoConversion = false;

        public static ViewModes ViewMode = ViewModes.DEFAULT;

        public static TabControl TabControl;

        public static TabPage tabPage;

        /// <summary>
        /// /////////////  ПЕРЕДАТЬ СЮДА КОНТРОЛЫ ИЗ ОКНА PDFView, разоброаться с мусором в MainForm
        /// </summary>

        public static void AddPageView()
        {
            tabPage = new TabPage();

            ImGearPageView PageView = new ImGearPageView();

            PageView.Dock = DockStyle.Fill;
            PageView.BackColor = SystemColors.Window;
            PageView.BindArrowKeyScrolling = false;

            PageView.Display = null;
            PageView.HorizontalArrowIncerment = 1;

            PageView.Name = "PageView";
            PageView.NotifyPageDown = null;
            PageView.NotifyPageUp = null;
            PageView.Page = null;

            PageView.TabIndex = 1;
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

            ScrollBar.Name = "ScrollBar";


            ScrollBar.TabIndex = 0;
            ScrollBar.Visible = true;
            ScrollBar.ValueChanged += new EventHandler(PDFViewKeyEvents.ScrollBarScrolled);

            tabPage.Controls.Add(PageView);
            tabPage.Controls.Add(ScrollBar);

           
            Documents.Add(new PDFViewClass(PageView, ScrollBar));
        }

        public static void AddTabPage()
        {
            tabPage.Text = Path.GetFileName(Documents[SelectedTabID].DocumentPath);
            TabControl.TabPages.Add(tabPage);
        }

        public static void TabChanged()
        {
            SelectedTabID = TabControl.SelectedIndex;
        }

        public static void InitializeImGear()
        {
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();
        }

        #region EVENTS
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
