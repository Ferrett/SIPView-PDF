using ImageGear.ART;
using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Display;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace SIPViewPDFlib
{
    public static class MenuBarClass
    {
        public static void ScrollBarScrolled(Button prevPageBtn, Button nextPageBtn)
        {
            if (PDFViewClass.currentPageID == 0)
            {
                DisableButton(prevPageBtn);
                EnableButton(nextPageBtn);
            }
            else if (PDFViewClass.currentPageID == PDFViewClass.PDFDocument.Pages.Count-1)
            {
                EnableButton(prevPageBtn);
                DisableButton(nextPageBtn);
            }
            else
            {
                EnableButton(prevPageBtn);
                EnableButton(nextPageBtn);
            }
        }
        public static void EnableButton(Button button)
        {
            button.Enabled = true;
        }
        public static void DisableButton(Button button)
        {
            button.Enabled = true;
        }
        
    }

   
}
