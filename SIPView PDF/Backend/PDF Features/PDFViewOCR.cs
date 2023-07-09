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
        public static bool DrawTextSelecting = false;
        private static ImGearRectangle[] SelectedWordsBounds;
        private static bool[] WordsIsSelected;
        private static List<string> OCRWords;
        private static Dictionary<int, List<ImGearRectangle>> OCRWordsBounds;

        public static bool TextIsSearched = false;
        public static int HighlightedOCRWord;
        public static void WordSearch()
        {
            HighlightedOCRWord = 0;
            TextIsSearched = true;
            OCRWords = new List<string>();
            OCRWordsBounds = new Dictionary<int, List<ImGearRectangle>>();


            for (int i = 0; i < PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages.Count; i++)
            {
                PDFWordFinder = new ImGearPDFWordFinder(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);
                PDFWordFinder.AcquireWordList(i);

                List<ImGearRectangle> foundTextBounds = new List<ImGearRectangle>();
                for (int j = 0; j < PDFWordFinder.AcquireWordList(i); j++)
                {
                   
                    if (PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String.ToLower().Contains(PDFManager.Documents[PDFManager.SelectedTabID].OCRTextBox.Text.ToLower()))
                    {
                        OCRWords.Add(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String);

                        int firstid, lastid, lenchar;

                        firstid = PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String.ToLower().IndexOf(PDFManager.Documents[PDFManager.SelectedTabID].OCRTextBox.Text.ToLower());
                        lastid = PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String.Length - (firstid + PDFManager.Documents[PDFManager.SelectedTabID].OCRTextBox.Text.ToLower().Length);
                        lenchar = (ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).BottomRight.H) - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).TopLeft.H)) / PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).String.ToLower().Length;

                        foundTextBounds.Add(new ImGearRectangle()
                        {
                            Top = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).BottomRight.V),
                            Bottom = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).TopLeft.V),
                            Left = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).TopLeft.H) + firstid * lenchar,
                            Right = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, j).GetQuad(0).BottomRight.H) - lastid * lenchar
                        });  
                    }
                }
                OCRWordsBounds.Add(i, foundTextBounds);
            }

            foreach (var item in OCRWordsBounds[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
            {
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(item, new ImGearRGBQuad() { Red = 255, Green = 140, Blue = 0 }) { Opacity = 120, UserData = -1 }, ImGearARTCoordinatesType.IMAGE_COORD);
            }

            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }

        public static void CloseOCR()
        {
            TextIsSearched = false;
        }

        public static int WordsInPageCount(int pageID)
        {
            PDFWordFinder = new ImGearPDFWordFinder(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);
            return PDFWordFinder.AcquireWordList(pageID);
        }
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
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(SelectedWordsBounds[i], new ImGearRGBQuad() { Red = 179, Green = 226, Blue = 255 }) { Opacity = 120, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
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
                        PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(SelectedWordsBounds[i], new ImGearRGBQuad() { Red = 179, Green = 226, Blue = 255 }) { Opacity = 120, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
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