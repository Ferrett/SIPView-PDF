using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using ImageGear.ART;
using ImageGear.Core;

namespace SIPView_PDF
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            PDFManager.DocumentChanged += DocumentUpdated;
            PDFManager.PageChanged += PageViewUpdated;

            PDFManager.ARTPage_MarkUpdate += ARTPage_MarkUpdate;
            PDFManager.ARTPage_MarkSelectionChanged += ARTPage_MarkSelectionChanged;
            PDFManager.ARTPage_HistoryChanged += ARTPage_HistoryChanged;

            PDFManager.InitializeImGear();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x007B)
                m.Result = IntPtr.Zero;
            else
                base.WndProc(ref m);
        }

        private void DocumentUpdated(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].InitArtPageEvents();
            MenuBarClass.DocumentOpened();
        }

        private void PageViewUpdated(object sender, EventArgs e)
        {
            MenuBarClass.PageChanged();
        }

        private void ARTPage_HistoryChanged(object sender, EventArgs e)
        {
            MenuBarClass.UpdateHistoryBtns();
        }

        private void ARTPage_MarkSelectionChanged(object sender, EventArgs e)
        {
            MenuBarClass.UpdateBakeInBtn();
        }

        private void ARTPage_MarkUpdate(object sender, EventArgs e)
        {
            MenuBarClass.UpdateSelectionBtn();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            PDFManager.DisposeImGear();
        }
    }
}
