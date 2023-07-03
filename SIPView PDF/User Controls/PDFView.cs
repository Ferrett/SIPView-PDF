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
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PDFManager.TabChanged();
        }
    }
}
