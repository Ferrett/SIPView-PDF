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

namespace SIPView_PDF
{
    public static class PDFViewOCR
    {

        public static ImGearPDFWordFinder PDFWordFinder;
        public static bool DrawTextSelecting = false;
        private static ImGearRectangle[] WordsBounds;
        private static bool[] WordIsShowed;

        public static int WordsInPageCount(int pageID)
        {
            PDFWordFinder = new ImGearPDFWordFinder(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);
            return PDFWordFinder.AcquireWordList(pageID);
        }
        public static void FindWordsInPage()
        {
            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].SetCoordType(ImGearARTCoordinatesType.DEVICE_COORD);
            WordsBounds = new ImGearRectangle[WordsInPageCount(PDFManager.Documents[PDFManager.SelectedTabID].PageID)];
            WordIsShowed = new bool[WordsInPageCount(PDFManager.Documents[PDFManager.SelectedTabID].PageID)];

            for (int i = 0; i < WordsBounds.Length; i++)
            {
                WordIsShowed[i] = false;
                WordsBounds[i] = new ImGearRectangle()
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
            for (int i = 0; i < WordsBounds.Length; i++)
            {
                if (WordIsShowed[i] == true)
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
            for (int i = 0; i < WordsBounds.Length; i++)
            {
                if (WordIsShowed[i] == true)
                {
                    foreach (ImGearARTMark igARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
                    {
                        if (igARTMark.UserData.Equals(i))
                        {
                            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(igARTMark);
                            WordIsShowed[i] = false;
                        }
                    }
                }
            }
            for (int i = 0; i < WordsBounds.Length; i++)
            {
                WordIsShowed[i] = true;
                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(WordsBounds[i], new ImGearRGBQuad() { Red = 179, Green = 226, Blue = 255 }) { Opacity = 120, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
            }

            PDFManager.Documents[PDFManager.SelectedTabID].UpdatePageView();
        }

        public static void UpdateSelectedWords()
        {
            ImGearPoint[] array = {new ImGearPoint(PDFViewKeyEvents.StartMousePos.X, PDFViewKeyEvents.StartMousePos.Y),
                new ImGearPoint(PDFViewKeyEvents.CurrentMousePos.X,PDFViewKeyEvents.CurrentMousePos.Y) };


            if (array[0].X > array[1].X)
            {
                int tmp = array[0].X;
                array[0].X = array[1].X;
                array[1].X = tmp;
            }

            if (array[0].Y > array[1].Y)
            {
                int tmp = array[0].Y;
                array[0].Y = array[1].Y;
                array[1].Y = tmp;
            }
            PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].ConvertCoordinates(PDFManager.Documents[PDFManager.SelectedTabID].PageView, PDFManager.Documents[PDFManager.SelectedTabID].PageView.Display, ImGearCoordConvModes.DEVICE_TO_IMAGE, array);



            for (int i = 0; i < WordsBounds.Length; i++)
            {
                if (new ImGearRectangle() { Left = array[0].X, Top = array[0].Y, Right = array[1].X, Bottom = array[1].Y }.Contains(new ImGearPoint(WordsBounds[i].Left, WordsBounds[i].Top)) &&
                    new ImGearRectangle() { Left = array[0].X, Top = array[0].Y, Right = array[1].X, Bottom = array[1].Y }.Contains(new ImGearPoint(WordsBounds[i].Right, WordsBounds[i].Bottom)))
                {
                    if (WordIsShowed[i] == false)
                    {
                        WordIsShowed[i] = true;
                        PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].AddMark(new ImGearARTRectangle(WordsBounds[i], new ImGearRGBQuad() { Red = 179, Green = 226, Blue = 255 }) { Opacity = 120, UserData = i }, ImGearARTCoordinatesType.IMAGE_COORD);
                    }
                }
                else
                {
                    if (WordIsShowed[i] == true)
                    {
                        foreach (ImGearARTMark igARTMark in PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID])
                        {
                            if (igARTMark.UserData.Equals(i))
                            {
                                PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkRemove(igARTMark);
                                WordIsShowed[i] = false;
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