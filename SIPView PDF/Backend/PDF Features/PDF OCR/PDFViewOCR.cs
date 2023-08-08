using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using ImageGear.ART;
using ImageGear.Core;
using ImageGear.Display;
using ImageGear.Formats.PDF;
using ImageGear.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SIPView_PDF
{
    public static class PDFViewOCR
    {
        public static ImGearPDFWordFinder PDFWordFinder;

        private static List<OCRWord> OCRWords;
        public static bool TextIsSearched = false;
        public static int HighlightedOCRWordID;

        public static void WordSearch()
        {
            OCRWords = new List<OCRWord>();
            HighlightedOCRWordID = 0;
            TextIsSearched = true;

            if (string.IsNullOrEmpty(PDFManager.Documents[PDFManager.SelectedTabID].OCRTextBox.Text))
            {
                DeleteHighlightOnCurrentPage();
                UpdateOCRLabel();
                PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = false;
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = false;
                return;
            }

            for (int i = 0; i < PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages.Count; i++)
            {
                PDFWordFinder = new ImGearPDFWordFinder(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);
                PDFWordFinder.AcquireWordList(i);


                for (int j = 0; j < PDFWordFinder.AcquireWordList(i); j++)
                {

                    if (PDFWordFinder.GetWord(ImGearPDFContextFlags.XY_ORDER, j).String.ToLower().Contains(PDFManager.Documents[PDFManager.SelectedTabID].OCRTextBox.Text.ToLower()))
                    {

                        int firstid, lastid, lenchar;

                        firstid = PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String.ToLower().IndexOf(PDFManager.Documents[PDFManager.SelectedTabID].OCRTextBox.Text.ToLower());
                        lastid = PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String.Length - (firstid + PDFManager.Documents[PDFManager.SelectedTabID].OCRTextBox.Text.ToLower().Length);
                        lenchar = (ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).BottomRight.H) - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).TopLeft.H)) / PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String.ToLower().Length;

                        ImGearRectangle foundTextBounds = new ImGearRectangle()
                        {
                            Top = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).BottomRight.V),
                            Bottom = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).TopLeft.V),
                            Left = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).TopLeft.H) + firstid * lenchar,
                            Right = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).BottomRight.H) - lastid * lenchar
                        };

                        OCRWords.Add(new OCRWord { ID = OCRWords.Count, PageID = i, Bounds = foundTextBounds, Text = PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String });
                    }
                }

            }



            if (OCRWords.Count > 0)
            {
                if (OCRWords[0].PageID != PDFManager.Documents[PDFManager.SelectedTabID].PageID)
                    PDFManager.Documents[PDFManager.SelectedTabID].RenderPage(OCRWords[0].PageID);

                if (OCRWords.Count > 1)
                {
                    PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = true;
                }
            }
            else
            {
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = false;
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = false;
            }

            DrawHighlightOnCurrentPage();
        }

        public static void DeleteAllHighlight()
        {
            foreach (ImGearARTPage ARTPage in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages)
            {
                List<int> removedMarkID = new List<int>();
                foreach (ImGearARTMark ARTMark in ARTPage)
                {
                    if (ARTMark.UserData != null && ARTMark.UserData.ToString().Equals("OCR"))
                        removedMarkID.Add(ARTMark.Id);
                }

                foreach (int ID in removedMarkID)
                {
                    ARTPage.MarkRemove(ID);
                }
            }
        }

        public static void DeleteHighlightOnCurrentPage()
        {
            List<int> removedMarkID = new List<int>();
            foreach (ImGearARTMark ARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
            {
                if (ARTMark.UserData != null && ARTMark.UserData.ToString().Equals("OCR"))
                    removedMarkID.Add(ARTMark.Id);
            }

            foreach (int ID in removedMarkID)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(ID);
            }
            
        }
        public static void DrawHighlightOnCurrentPage()
        {
            DeleteHighlightOnCurrentPage();
            for (int i = 0; i < OCRWords.Count; i++)
            {
                if (OCRWords[i].PageID == PDFManager.Documents[PDFManager.SelectedTabID].PageID)
                {
                    if (OCRWords[i].ID == HighlightedOCRWordID)
                        PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(OCRWords[i].Bounds, OCRColors.OCRHighlightColor) { Opacity = OCRColors.OCRHighlightOpacity, UserData = $"OCR" }, ImGearARTCoordinatesType.IMAGE_COORD);
                    else
                        PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(OCRWords[i].Bounds, OCRColors.OCRWordColor) { Opacity = OCRColors.OCRWordOpacity, UserData = $"OCR" }, ImGearARTCoordinatesType.IMAGE_COORD);
                }
            }
            UpdateOCRLabel();
            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }
        public static void HighlightPrevWord()
        {
            if (HighlightedOCRWordID > 0)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = true;
                HighlightedOCRWordID--;

                DrawHighlightOnCurrentPage();

                if (OCRWords[HighlightedOCRWordID].PageID !=  PDFManager.Documents[PDFManager.SelectedTabID].PageID)
                    PDFManager.Documents[PDFManager.SelectedTabID].RenderPage(OCRWords[HighlightedOCRWordID].PageID);
            }

            if (HighlightedOCRWordID <= 0)
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = false;
            else
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = true;
        }

        public static void HighlightNextWord()
        {
            if (HighlightedOCRWordID + 1 < OCRWords.Count)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = true;
                HighlightedOCRWordID++;

                DrawHighlightOnCurrentPage();

                if (OCRWords[HighlightedOCRWordID].PageID != PDFManager.Documents[PDFManager.SelectedTabID].PageID)
                    PDFManager.Documents[PDFManager.SelectedTabID].RenderPage(OCRWords[HighlightedOCRWordID].PageID);
            }

            if(HighlightedOCRWordID + 1 >= OCRWords.Count)
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = false;
            else
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = true;
        }

        public static void UpdateOCRLabel()
        {
            PDFManager.Documents[PDFManager.SelectedTabID].OCRLabel.Text = $"{HighlightedOCRWordID + (OCRWords.Count == 0 ? 0 : 1)}/{OCRWords.Count}";
        }

        public static void CloseOCR()
        {
            DeleteAllHighlight();
            TextIsSearched = false;
        }

       

    }
}