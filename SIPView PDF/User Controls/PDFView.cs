using System;
using System.Windows.Forms;
using ImageGear.ART.Forms;
using ImageGear.Evaluation;
using ImageGear.Windows.Forms;

namespace SIPView_PDF
{
    public partial class PDFView : UserControl
    {
        public PDFView()
        {
            InitializeComponent();
            InitializeClassControls();

            PDFViewClass.InitializeImGear();
            PDFViewClass.InitializeToolBar();
        }

        private void InitializeClassControls()
        {
            //PDFViewClass.ARTForm = new ImGearARTForms(PageView, ImGearARTToolBarModes.ART30);
            PDFViewClass.PageView = PageView;
            PDFViewClass.ScrollBar = ScrollBar;

            PDFViewClass.ThumbnailController = ThumbnailController;
        }

        private void PDFView_MouseWheel(object sender, MouseEventArgs e)
        {
            PDFViewKeyEvents.WheelScrolled(sender,e);
        }

        private void PageView_KeyDown(object sender, KeyEventArgs e)
        {
            PDFViewKeyEvents.KeyDown(sender,e);
        }

        private void PageView_KeyUp(object sender, KeyEventArgs e)
        {
            PDFViewKeyEvents.PageView_KeyUp(sender, e);
        }

        public  void PageView_MouseDown(object sender, MouseEventArgs e)
        {
            PDFViewKeyEvents.PageView_MouseDown(sender, e);
        }

        public  void PageView_MouseUp(object sender, MouseEventArgs e)
        {
            PDFViewKeyEvents.PageView_MouseUp(sender, e);
        }

        public  void PageView_MouseMove(object sender, MouseEventArgs e)
        {
            PDFViewKeyEvents.PageView_MouseMove(sender, e);
        }

        public void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            PDFViewKeyEvents.ScrollBarScrolled();
        }
    }
}
