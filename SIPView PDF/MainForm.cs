using System;
using System.Windows.Forms;
using ImageGear.ART;


namespace SIPView_PDF
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
           
            PDFViewClass.DocumentChanged += DocumentUpdated;
            PDFViewClass.PageChanged += PageViewUpdated;
        }

        private void DocumentUpdated(object sender, EventArgs e)
        {
            UpdateMenuBtns();
        }

        private void PageViewUpdated(object sender, EventArgs e)
        {
            InitArtPages();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            PDFViewClass.DisposeImGear();
        }

        public void UpdateMenuBtns()
        {
            MenuBarClass.DocumentOpened();
        }

        public void InitArtPages()
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
