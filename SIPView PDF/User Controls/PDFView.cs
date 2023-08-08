using System;
using System.Drawing;
using System.Windows.Forms;
using ImageGear.ART.Forms;
using ImageGear.Evaluation;
using ImageGear.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SIPView_PDF
{
    public partial class PDFView : UserControl
    {
        Point _imageLocation = new Point(20, 4);
        Point _imgHitArea = new Point(20, 4);
        Image closeImage;

        public PDFView()
        {
            InitializeComponent();
            InitializeClassControls();
        }
       
        private void InitializeClassControls()
        {
            PDFManager.TabControl = TabControl;
            PDFManager.Pan = Pan;

            PDFManager._imageLocation = _imageLocation;
            PDFManager._imgHitArea = _imgHitArea;
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PDFManager.SelectedTabChanged();
        }

        private void TabControl_MouseClick(object sender, MouseEventArgs e)
        {
            PDFManager.TabControl_MouseClick(sender, e);
        }

        private void PDFView_Load(object sender, EventArgs e)
        {
            closeImage = Properties.Resources.close_btn;
            TabControl.Padding = new Point(20, 4);
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Image img = new Bitmap(closeImage);
  
            Rectangle r = this.TabControl.GetTabRect(e.Index);
            r.Offset(2, 2);
            Brush TitleBrush = new SolidBrush(Color.Black);
            Font f = this.Font;
            string title = this.TabControl.TabPages[e.Index].Text;
            
            e.Graphics.DrawString(title, f, TitleBrush, new PointF(r.X, r.Y));
            e.Graphics.DrawImage(img, new Point(r.X + (this.TabControl.GetTabRect(e.Index).Width - _imageLocation.X), _imageLocation.Y));
        }
    }
}
