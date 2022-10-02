using ImageGear.ART;
using ImageGear.ART.Forms;
using ImageGear.Core;
using ImageGear.Display;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SIPViewPDFlib
{
    public static class PDFViewClass
    {
        public static List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();
        public static ImGearDocument PDFDocument = null;
        public static int currentPageID = 0;
        public static ImGearARTForms ARTForm;
        public static ImGearPageView PageView;
        public static ScrollBar ScrollBar;
        public static StatusStrip StatusStrip;

        public static void DisposeImGear()
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }



        public static void FilePrint()
        {
            using (ImGearPDFDocument igPDFDocument = (ImGearPDFDocument)PDFDocument)
            {

                ImGearPDFPrintOptions printOptions = new ImGearPDFPrintOptions();
                PrintDocument printDocument = new PrintDocument();

                //Use default Windows printer.
                printOptions.DeviceName = printDocument.PrinterSettings.PrinterName;

                //Print all pages.
                printOptions.StartPage = 0;
                printOptions.EndPage = PDFDocument.Pages.Count;
                igPDFDocument.Print(printOptions);
            }
        }

        public static void ToolBarChangeVisibility()
        {
            if (ARTForm.ToolBar.Visible)
            {
                ARTForm.ToolBar.Visible = false;
                PageView.Location = new Point(PageView.Location.X - ARTForm.ToolBar.Width, PageView.Location.Y);
            }
            else
            {
                ARTForm.ToolBar.Visible = true;
                PageView.Location = new Point(PageView.Location.X + ARTForm.ToolBar.Width, PageView.Location.Y);
            }
        }

        public static void RenderPage(int pageID)
        {
            currentPageID = pageID;
            //Load a single page from the loaded document.
            try
            {
                ImGearPage page = PDFDocument.Pages[currentPageID];
                if (page != null)
                {
                    //Create a new page display to prepare the page for being displayed.

                   ImGearPageDisplay igPageDisplay = new ImGearPageDisplay(page);
                   //Associate the page display with the page view.
                    PageView.Display = igPageDisplay;
                    //Cause the page view to repaint.
                    PageView.Invalidate();
                   
                    
                }

            }
            catch (ImGearException ex)
            {
                //Perform error handling.
               MessageBox.Show(ex.Message);
            }

            UpdateAfterRender();
        }

        public static void UpdateAfterRender()
        {
            DisplayCurrentPageMarks();
            ScrollBar.Value = currentPageID;
            PageView.Update();
        }

        public static int CountSelectedMarks()
        {
            int selectedMarksCounter = 0;

            foreach (ImGearARTMark ARTMark in ARTPages[currentPageID])
            {
                if (ARTPages[currentPageID].MarkIsSelected(ARTMark))
                    selectedMarksCounter++;
            }

            return selectedMarksCounter;
        }

       


        public static void PageViewMouseDown(object sender, MouseEventArgs e)
        {
            ARTForm.MouseDown(sender, e);
        }

        public static void PageViewMouseUp(object sender, MouseEventArgs e)
        {
            ARTForm.MouseDown(sender, e);
        }

        public static void PageViewMouseMove(object sender, MouseEventArgs e)
        {
            ARTForm.MouseDown(sender, e);
        }


       
        public static void InitImGear()
        {
           
        }



        public static void SelectAllMarks(ImGearPageView pageView)
        {
            if (CountSelectedMarks() == ARTPages[currentPageID].MarkCount)
                ARTPages[currentPageID].SelectMarks(false);
            else
                ARTPages[currentPageID].SelectMarks(true);
            pageView.Update();
        }

        public static void InitScrollBar()
        {
            if (PDFDocument.Pages.Count > 1)
            {
                ScrollBar.Visible = true;
                ScrollBar.Maximum = PDFDocument.Pages.Count - 1;
            }
            else
                ScrollBar.Visible = false;
        }

        public static void UpdatePageView()
        {
            PageView.Update();
        }

        public static void AnnotationBakeIn()
        {
            PDFDocument.Pages[currentPageID] = ImGearART.BurnIn(PDFDocument.Pages[currentPageID], ARTPages[currentPageID], ImGearARTBurnInOptions.SELECTED, null);
            PageView.Page = PDFDocument.Pages[currentPageID];

            // Get burned marks ID
            List<int> bakedMarkID = new List<int>();
            foreach (ImGearARTMark ARTMark in ARTPages[currentPageID])
            {
                if (ARTPages[currentPageID].MarkIsSelected(ARTMark))
                    bakedMarkID.Add(ARTMark.Id);
            }

            // Delete burned marks by ID
            foreach (int ID in bakedMarkID)
            {
                ARTPages[currentPageID].MarkRemove(ID);
            }

            UpdatePageView();
        }


        public static void UpdateStatusStrip(StatusStrip statusStrip)
        {
            statusStrip.Items[0].Text = string.Format("{0} of {1}", currentPageID + 1, PDFDocument.Pages.Count);
        }

        public static void ScrollBarScrolled()
        {
            RenderPage(ScrollBar.Value);
            DisplayCurrentPageMarks();
            UpdateStatusStrip(StatusStrip);
        }

        private static void DisplayCurrentPageMarks()
        {
            PageView.Display = new ImGearPageDisplay(PDFDocument.Pages[currentPageID], ARTPages[currentPageID]);
            ARTForm.Page = ARTPages[currentPageID];
        }

        public static void InitArtPages()
        {
            for (int i = 0; i < PDFDocument.Pages.Count; i++)
            {
                ARTPages.Add(new ImGearARTPage());
                ARTPages.Last().History.IsEnabled = true;
                ARTPages.Last().History.Limit = 99;
            }
            ARTPages.ForEach(x => x.RemoveMarks());
        }

        public static void FileLoad(string fileName)
        {

            using (FileStream inputStream =
                new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                try
                {
                    // Load the entire the document.
                    PDFDocument = ImGearFileFormats.LoadDocument(inputStream);
                    ARTPages.Clear();

                }
                catch (ImGearException ex)
                {
                    // Perform error handling.
                    MessageBox.Show(ex.Message);
                }
            }
            // Render first page.



            InitScrollBar();
            InitArtPages();


            RenderPage(0);
            UpdatePageView();
        }

        //public void InitArtPages()
        //{
        //    for (int i = 0; i < PDFDocument.Pages.Count; i++)
        //    {
        //        ARTPages.Add(new ImGearARTPage());
        //        ARTPages.Last().History.IsEnabled = true;
        //        ARTPages.Last().MarkAdded += ARTPage_MarkAdded;
        //        ARTPages.Last().MarkRemoved += ARTPage_MarkRemoved;
        //        ARTPages.Last().MarkSelectionChanged += ARTPage_MarkSelectionChanged;
        //        ARTPages.Last().History.Limit = 99;
        //        ARTPages.Last().History.HistoryChanged += ARTPageHistory_HistoryChanged;
        //    }
        //    ARTPages.ForEach(x => x.RemoveMarks());
        //}

    }
}
