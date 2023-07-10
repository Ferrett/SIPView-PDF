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
        public static bool TextIsSelecting = false;
        private static ImGearRectangle[] SelectedWordsBounds;
        private static bool[] WordsIsSelected;


        private static List<OCRWord> OCRWords;

        public static bool TextIsSearched = false;
        public static int HighlightedOCRWord;
        public static void WordSearch()
        {
            OCRWords = new List<OCRWord>();
            HighlightedOCRWord = 0;
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
                    if (ARTMark.UserData.ToString().Equals("OCR"))
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
                if (ARTMark.UserData.ToString().Equals("OCR"))
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
                    if (OCRWords[i].ID == HighlightedOCRWord)
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
            if (HighlightedOCRWord > 0)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = true;
                HighlightedOCRWord--;

                DrawHighlightOnCurrentPage();

                if (OCRWords[HighlightedOCRWord].PageID !=  PDFManager.Documents[PDFManager.SelectedTabID].PageID)
                    PDFManager.Documents[PDFManager.SelectedTabID].RenderPage(OCRWords[HighlightedOCRWord].PageID);
            }

            if (HighlightedOCRWord <= 0)
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = false;
            else
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = true;
        }

        public static void HighlightNextWord()
        {
            if (HighlightedOCRWord + 1 < OCRWords.Count)
            {
                PDFManager.Documents[PDFManager.SelectedTabID].OCRPrevBtn.Enabled = true;
                HighlightedOCRWord++;

                DrawHighlightOnCurrentPage();

                if (OCRWords[HighlightedOCRWord].PageID != PDFManager.Documents[PDFManager.SelectedTabID].PageID)
                    PDFManager.Documents[PDFManager.SelectedTabID].RenderPage(OCRWords[HighlightedOCRWord].PageID);
            }

            if(HighlightedOCRWord + 1 >= OCRWords.Count)
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = false;
            else
                PDFManager.Documents[PDFManager.SelectedTabID].OCRNextBtn.Enabled = true;
        }

        public static void UpdateOCRLabel()
        {
            PDFManager.Documents[PDFManager.SelectedTabID].OCRLabel.Text = $"{HighlightedOCRWord + (OCRWords.Count == 0 ? 0 : 1)}/{OCRWords.Count}";
        }

        public static void CloseOCR()
        {
            DeleteAllHighlight();
            TextIsSearched = false;
        }

        public static int WordsInPageCount(int pageID)
        {
            PDFWordFinder = new ImGearPDFWordFinder(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);
            return PDFWordFinder.AcquireWordList(pageID);
        }

        //   ПЕРЕНЕСТИ ВЫДЕЛЕННОЕ СЛОВО В ОТДЕЛЬНЫЙ КЛАСС
        public static void FindWordsInPage()
        {
            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].SetCoordType(ImGearARTCoordinatesType.DEVICE_COORD);
            SelectedWordsBounds = new ImGearRectangle[WordsInPageCount(PDFManager.Documents[PDFManager.SelectedTabID].PageID)];
            WordsIsSelected = new bool[WordsInPageCount(PDFManager.Documents[PDFManager.SelectedTabID].PageID)];

            for (int i = 0; i < SelectedWordsBounds.Length; i++)
            {
                WordsIsSelected[i] = false;
                SelectedWordsBounds[i] = new ImGearRectangle()
                {
                    Top = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomRight.V),
                    Left = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).TopLeft.H),
                    Bottom = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).TopLeft.V),
                    Right = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomRight.H)
                };
            }
        }

        public static void CopySelectedText()
        {
            string text = String.Empty;
            bool s = false;
            for (int i = 0; i < SelectedWordsBounds.Length; i++)
            {
                if (WordsIsSelected[i] == true)
                {
                    if (s == true && PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomLeft.V < PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i - 1).GetQuad(0).BottomLeft.V)
                    {
                        text += '\n';
                    }

                    s = true;
                    text += PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).String + " ";
                }
            }
            text.Remove(text.Length - 1);
            Clipboard.SetText(text);
        }

        public static void SelectAllText()
        {
            for (int i = 0; i < SelectedWordsBounds.Length; i++)
            {
                if (WordsIsSelected[i] == true)
                {
                    foreach (ImGearARTMark igARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
                    {
                        if (igARTMark.UserData.Equals(i))
                        {
                            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(igARTMark);
                            WordsIsSelected[i] = false;
                        }
                    }
                }
            }
            for (int i = 0; i < SelectedWordsBounds.Length; i++)
            {
                WordsIsSelected[i] = true;
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(SelectedWordsBounds[i], OCRColors.TextSelectionColor) { Opacity = OCRColors.TextSelectionOpacity, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
            }

            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }

        public static void UpdateSelectedWords()
        {
            ImGearPoint[] MousePos = {new ImGearPoint(PDFViewKeyEvents.StartMousePos.X, PDFViewKeyEvents.StartMousePos.Y),
                new ImGearPoint(PDFViewKeyEvents.CurrentMousePos.X,PDFViewKeyEvents.CurrentMousePos.Y) };


            if (MousePos[0].X > MousePos[1].X)
            {
                int tmp = MousePos[0].X;
                MousePos[0].X = MousePos[1].X;
                MousePos[1].X = tmp;
            }

            if (MousePos[0].Y > MousePos[1].Y)
            {
                int tmp = MousePos[0].Y;
                MousePos[0].Y = MousePos[1].Y;
                MousePos[1].Y = tmp;
            }
            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].ConvertCoordinates(PDFManager.Documents[PDFManager.SelectedTabID].PageView, PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display, ImGearCoordConvModes.DEVICE_TO_IMAGE, MousePos);



            for (int i = 0; i < SelectedWordsBounds.Length; i++)
            {
                if (new ImGearRectangle() { Left = MousePos[0].X, Top = MousePos[0].Y, Right = MousePos[1].X, Bottom = MousePos[1].Y }.Contains(new ImGearPoint(SelectedWordsBounds[i].Left, SelectedWordsBounds[i].Top)) &&
                    new ImGearRectangle() { Left = MousePos[0].X, Top = MousePos[0].Y, Right = MousePos[1].X, Bottom = MousePos[1].Y }.Contains(new ImGearPoint(SelectedWordsBounds[i].Right, SelectedWordsBounds[i].Bottom)))
                {
                    if (WordsIsSelected[i] == false)
                    {
                        WordsIsSelected[i] = true;
                        PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(SelectedWordsBounds[i], OCRColors.TextSelectionColor) { Opacity = OCRColors.TextSelectionOpacity, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
                    }
                }
                else
                {
                    if (WordsIsSelected[i] == true)
                    {
                        foreach (ImGearARTMark igARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
                        {
                            if (igARTMark.UserData.Equals(i))
                            {
                                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(igARTMark);
                                WordsIsSelected[i] = false;
                            }
                        }
                    }

                }
            }
        }
        public static void TextSelectionModeChange()
        {
            if (PDFManager.ViewMode != ViewModes.TEXT_SELECTION)
            {
                PDFManager.ViewMode = ViewModes.TEXT_SELECTION;
                PDFManager.Documents[PDFManager.SelectedTabID].PageView.Cursor = Cursors.IBeam;
            }
            else
            {
                PDFManager.ViewMode = ViewModes.DEFAULT;
            }

        }
        public static void StartTextSelecting(object sender, MouseEventArgs e)
        {
            if (PDFManager.ViewMode == ViewModes.TEXT_SELECTION)
            {
                PDFViewKeyEvents.StartMousePos.X = e.X;
                PDFViewKeyEvents.StartMousePos.Y = e.Y;

                PDFManager.Documents[PDFManager.SelectedTabID].PageView.RegisterAfterDraw(
               new ImGearPageView.AfterDraw(PDFManager.Documents[PDFManager.SelectedTabID].DrawSelector));
            }
        }

    }
}