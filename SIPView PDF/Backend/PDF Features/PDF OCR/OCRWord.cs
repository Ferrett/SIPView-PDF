using ImageGear.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPView_PDF
{
    public struct OCRWord
    {
        public int ID;
        public int PageID;
        public string Text;
        public ImGearRectangle Bounds;
    }
}
