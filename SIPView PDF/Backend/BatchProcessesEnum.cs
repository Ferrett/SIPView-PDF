using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPView_PDF
{
    public enum BatchProcess
    {
        ALL_FILES_TO_PDFS = 0,
        ALL_FILES_TO_SINGLE_PDF = 1,
        SPLIT_MULTIPAGE_PDFS = 2,
    }
}
