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
            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages.Clear();
            for (int i = 0; i < PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages.Count; i++)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages.Add(new ImGearARTPage());
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages.Last().History.IsEnabled = true;
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages.Last().History.Limit = uint.MaxValue;
            }
        }

        public static void SelectAllMarks()
        {
            if (SelectedMarksCount() == PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkCount)
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].SelectMarks(false);
            else
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].SelectMarks(true);
            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }



        public static void AnnotationBakeIn()
        {
            PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID] = ImGearART.BurnIn(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID], PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID], ImGearARTBurnInOptions.SELECTED, null);
            PDFManager.Documents[PDFManager.SelectedTabID].PageView.Page = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID];

            // Get burned marks ID
            List<int> bakedMarkID = new List<int>();
            foreach (ImGearARTMark ARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
            {
                if (PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkIsSelected(ARTMark))
                    bakedMarkID.Add(ARTMark.Id);
            }

            // Delete burned marks by ID
            foreach (int ID in bakedMarkID)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(ID);
            }

            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }

        public static int SelectedMarksCount()
        {
            int selectedMarksCounter = 0;

            foreach (ImGearARTMark ARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
            {
                if (PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkIsSelected(ARTMark))
                    selectedMarksCounter++;
            }

            return selectedMarksCounter;
        }
    }
}