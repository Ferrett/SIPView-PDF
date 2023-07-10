using ImageGear.ART;
using ImageGear.Core;
using ImageGear.Display;
using ImageGear.Formats.PDF;
using ImageGear.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPView_PDF
{
    public static class PDFViewTextSelecting
    {
        public static ImGearPDFWordFinder PDFWordFinder;
        public static bool TextIsSelecting = false;

        private static List<TextSelectionWord> TextSelectionWords;


        public static int WordsInPageCount(int pageID)
        {
            PDFWordFinder = new ImGearPDFWordFinder(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);
            return PDFWordFinder.AcquireWordList(pageID);
        }

        public static void FindWordsInPage()
        {
            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].SetCoordType(ImGearARTCoordinatesType.DEVICE_COORD);
            TextSelectionWords = new List<TextSelectionWord>();

            for (int i = 0; i < WordsInPageCount(PDFManager.Documents[PDFManager.SelectedTabID].PageID); i++)
            {
                ImGearRectangle bounds = new ImGearRectangle()
                {
                    Top = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomRight.V),
                    Left = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).TopLeft.H),
                    Bottom = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].DIB.Height - ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).TopLeft.V),
                    Right = ImGearPDF.FixedRoundToInt(PDFWordFinder.GetWord(ImGearPDFContextFlags.PDF_ORDER, i).GetQuad(0).BottomRight.H)
                };

                TextSelectionWords.Add(new TextSelectionWord { ID = i, IsSelected = false, Bounds = bounds });
            }
        }

        public static void CopySelectedText()
        {
            string text = string.Empty;
            bool s = false;
            for (int i = 0; i < TextSelectionWords.Count; i++)
            {
                if (TextSelectionWords[i].IsSelected == true)
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
            for (int i = 0; i < TextSelectionWords.Count; i++)
            {
                if (TextSelectionWords[i].IsSelected == true)
                {
                    foreach (ImGearARTMark igARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
                    {
                        if (igARTMark.UserData != null &&igARTMark.UserData.ToString().Equals("TXT"))
                        {
                            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(igARTMark);
                            TextSelectionWords[i].IsSelected = false;
                        }
                    }
                }
            }
            for (int i = 0; i < TextSelectionWords.Count; i++)
            {
                TextSelectionWords[i].IsSelected = true;
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(TextSelectionWords[i].Bounds, OCRColors.TextSelectionColor) { Opacity = OCRColors.TextSelectionOpacity, UserData = "TXT" }, ImGearARTCoordinatesType.IMAGE_COORD);
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

            for (int i = 0; i < TextSelectionWords.Count; i++)
            {
                if (new ImGearRectangle() { Left = MousePos[0].X, Top = MousePos[0].Y, Right = MousePos[1].X, Bottom = MousePos[1].Y }.Contains(new ImGearPoint(TextSelectionWords[i].Bounds.Left, TextSelectionWords[i].Bounds.Top)) &&
                    new ImGearRectangle() { Left = MousePos[0].X, Top = MousePos[0].Y, Right = MousePos[1].X, Bottom = MousePos[1].Y }.Contains(new ImGearPoint(TextSelectionWords[i].Bounds.Right, TextSelectionWords[i].Bounds.Bottom)))
                {
                    if (TextSelectionWords[i].IsSelected == false)
                    {
                        TextSelectionWords[i].IsSelected = true;
                        PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(TextSelectionWords[i].Bounds, OCRColors.TextSelectionColor) { Opacity = OCRColors.TextSelectionOpacity, UserData = "TXT" }, ImGearARTCoordinatesType.IMAGE_COORD);
                    }
                }
                else
                {
                    if (TextSelectionWords[i].IsSelected == true)
                    {
                        foreach (ImGearARTMark igARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
                        {
                            if (igARTMark.UserData != null && igARTMark.UserData.ToString().Equals("TXT"))
                            {
                                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(igARTMark);
                                TextSelectionWords[i].IsSelected = false;
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
                RemoveAllSelection();
            }

        }

        public static void RemoveAllSelection()
        {
            foreach (var Word in TextSelectionWords)
            {
                Word.IsSelected = false;
            }
            foreach (ImGearARTPage ARTPage in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages)
            {
                List<int> removedMarkID = new List<int>();
                foreach (ImGearARTMark ARTMark in ARTPage)
                {
                    if (ARTMark.UserData != null && ARTMark.UserData.ToString().Equals("TXT"))
                        removedMarkID.Add(ARTMark.Id);
                }

                foreach (int ID in removedMarkID)
                {
                    ARTPage.MarkRemove(ID);
                }
            }
        }

        public static void StartTextSelecting(object sender, MouseEventArgs e)
        {
            if (PDFManager.ViewMode == ViewModes.TEXT_SELECTION)
            {
                PDFViewKeyEvents.StartMousePos.X = e.X;
                PDFViewKeyEvents.StartMousePos.Y = e.Y;

                RemoveAllSelection();

                PDFManager.Documents[PDFManager.SelectedTabID].PageView.RegisterAfterDraw(
               new ImGearPageView.AfterDraw(PDFManager.Documents[PDFManager.SelectedTabID].DrawSelector));
            }
        }
    }
}
