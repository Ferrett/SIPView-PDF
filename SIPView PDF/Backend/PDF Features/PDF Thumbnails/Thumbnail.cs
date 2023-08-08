using ImageGear.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPView_PDF
{
    public class Thumbnail
    {
        public bool IsSelected;
        public ImGearPageView Image;
        public Panel Background;
        public Label Label;

        public void Select()
        {
            IsSelected = true;
            Background.BackColor = Color.Cyan;
        }

        public void Deselect()
        {
            IsSelected = false;
            Background.BackColor = Color.Transparent;
        }
    }
}
