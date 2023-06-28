using ImageGear.ART;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SIPView_PDF.Backend.PDF_Features
{
    public static class PDFViewAnnotations
    {
        public static void InitializeArtPages()
        {
            PDFViewClass.ARTPages.Clear();
            for (int i = 0; i < PDFViewClass.PDFDocument.Pages.Count; i++)
            {
                PDFViewClass.ARTPages.Add(new ImGearARTPage());
                PDFViewClass.ARTPages.Last().History.IsEnabled = true;
                PDFViewClass.ARTPages.Last().History.Limit = uint.MaxValue;
            }
        }

        public static void SelectAllMarks()
        {
            if (SelectedMarksCount() == PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].MarkCount)
                PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].SelectMarks(false);
            else
                PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].SelectMarks(true);
            PDFViewClass.UpdatePageView();
        }



        public static void AnnotationBakeIn()
        {
            PDFViewClass.PDFDocument.Pages[PDFViewClass.CurrentPageID] = ImGearART.BurnIn(PDFViewClass.PDFDocument.Pages[PDFViewClass.CurrentPageID], PDFViewClass.ARTPages[PDFViewClass.CurrentPageID], ImGearARTBurnInOptions.SELECTED, null);
            PDFViewClass.PageView.Page = PDFViewClass.PDFDocument.Pages[PDFViewClass.CurrentPageID];

            // Get burned marks ID
            List<int> bakedMarkID = new List<int>();
            foreach (ImGearARTMark ARTMark in PDFViewClass.ARTPages[PDFViewClass.CurrentPageID])
            {
                if (PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].MarkIsSelected(ARTMark))
                    bakedMarkID.Add(ARTMark.Id);
            }

            // Delete burned marks by ID
            foreach (int ID in bakedMarkID)
            {
                PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].MarkRemove(ID);
            }

            PDFViewClass.UpdatePageView();
        }

        public static int SelectedMarksCount()
        {
            int selectedMarksCounter = 0;

            foreach (ImGearARTMark ARTMark in PDFViewClass.ARTPages[PDFViewClass.CurrentPageID])
            {
                if (PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].MarkIsSelected(ARTMark))
                    selectedMarksCounter++;
            }

            return selectedMarksCounter;
        }
    }
}
