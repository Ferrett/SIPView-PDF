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
        }
       
        private void InitializeClassControls()
        {
            PDFManager.TabControl = TabControl;
            PDFManager.Pan = Pan;
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PDFManager.SelectedTabChanged();
        }

        private void TabControl_MouseClick(object sender, MouseEventArgs e)
        {
            PDFManager.TabControl_MouseClick(sender, e);
        }
    }
}
