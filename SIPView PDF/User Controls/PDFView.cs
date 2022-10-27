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
            //ImGearEvaluationManager.Initialize();

            PDFViewClass.ARTForm = new ImGearARTForms(PageView, ImGearARTToolBarModes.ART30);
            PDFViewClass.PageView = PageView;
            PDFViewClass.ScrollBar = ScrollBar;
            PDFViewClass.StatusStrip = StatusStrip;
            PDFViewClass.Magnifier = Magnifier;
            PDFViewClass.ThumbnailController = ThumbnailController;
        }

        private void PDFView_MouseWheel(object sender, MouseEventArgs e)
        {
            PDFViewClass.WheelScrolled(sender,e);
        }

        private void PageView_KeyDown(object sender, KeyEventArgs e)
        {
            PDFViewClass.KeyDown(sender,e);
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
           PDFViewClass.ScrollBarScrolled();
        }

        private void PageView_MouseDown(object sender, MouseEventArgs e)
        {
            PDFViewClass.ARTForm.MouseDown(sender, e);
            PDFViewClass.PageView.Cursor = Cursors.IBeam;
        }

        private void PageView_MouseUp(object sender, MouseEventArgs e)
        {
            PDFViewClass.ARTForm.MouseUp(sender, e);
            PDFViewClass.PageView.Cursor = Cursors.Default;
        }

        private void PageView_MouseMove(object sender, MouseEventArgs e)
        {
            PDFViewClass.ARTForm.MouseMove(sender, e);
        }

        private void PageView_KeyUp(object sender, KeyEventArgs e)
        {
            PDFViewClass.PageView_KeyUp(sender, e);
        }
    }
}
