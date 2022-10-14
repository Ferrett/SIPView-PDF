using System;
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

            PDFViewClass.DocumentChanged += DocumentUpdated;
            PDFViewClass.PageChanged += PageViewUpdated;

            Cursor.Current = Cursors.WaitCursor;
        }


        private void DocumentUpdated(object sender, EventArgs e)
        {
            InitArtPageEvents();
            MenuBarClass.DocumentOpened();
        }

        private void PageViewUpdated(object sender, EventArgs e)
        {
            MenuBarClass.PageChanged();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            PDFViewClass.DisposeImGear();
        }

        public void InitArtPageEvents()
        {
            for (int i = 0; i < PDFViewClass.ARTPages.Count; i++)
            {
                PDFViewClass.ARTPages[i].MarkAdded += ARTPage_MarkUpdate;
                PDFViewClass.ARTPages[i].MarkRemoved += ARTPage_MarkUpdate;
                PDFViewClass.ARTPages[i].MarkSelectionChanged += ARTPage_MarkSelectionChanged;
                PDFViewClass.ARTPages[i].History.HistoryChanged += ARTPage_HistoryChanged;
            }
        }

        private void ARTPage_HistoryChanged(object sender, ImGearARTHistoryEventArgs e)
        {
            MenuBarClass.UpdateHistoryBtns();
        }

        private void ARTPage_MarkSelectionChanged(object sender, ImGearARTMarkEventArgs e)
        {
            MenuBarClass.UpdateBakeInBtn();
        }

        private void ARTPage_MarkUpdate(object sender, ImGearARTMarkEventArgs e)
        {
            MenuBarClass.UpdateSelectionBtn();
        }


    }
}
