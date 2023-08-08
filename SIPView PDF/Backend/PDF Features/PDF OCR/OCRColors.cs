using ImageGear.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPView_PDF
{
    public static class OCRColors
    {
        public static ImGearRGBQuad TextSelectionColor = new ImGearRGBQuad() { Red = 179, Green = 226, Blue = 255 };
        public static int TextSelectionOpacity = 120;

        public static ImGearRGBQuad OCRWordColor = new ImGearRGBQuad() { Red = 255, Green = 140, Blue = 0 };
        public static int OCRWordOpacity = 120;

        public static ImGearRGBQuad OCRHighlightColor = new ImGearRGBQuad() { Red = 255, Green = 255, Blue = 0 };
        public static int OCRHighlightOpacity = 120;
    }
}
