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
using System.Windows.Forms;


namespace SIPView_PDF
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

        public static event EventHandler PageChanged;
        public static event EventHandler DocumentChanged;

        public static void OnPageChanged(EventArgs e)
        {
            if (PageChanged != null)
                PageChanged(null, e);
        }

        public static void OnDocumentChanged(EventArgs e)
        {
            if (DocumentChanged != null)
                DocumentChanged(null, e);
        }

        public static void InitializeImGear()
        {
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();
        }

        public static void InitializeToolBar()
        {
            ARTForm.Mode = ImGearARTModes.EDIT;

            ARTForm.ToolBar.TopLevel = false;
            ARTForm.ToolBar.Size = new Size(85, 1000);
            ARTForm.ToolBar.Location = new Point(0, 50);

            ARTForm.ToolBar.Visible = true;
            ARTForm.ToolBar.FormBorderStyle = FormBorderStyle.None;
            ARTForm.ToolBar.Show();

            ToolBarChangeVisibility();
        }

        private static void InitializeScrollBar()
        {
            if (PDFDocument.Pages.Count > 1)
            {
                ScrollBar.Visible = true;
                ScrollBar.Maximum = PDFDocument.Pages.Count - 1;
            }
            else
                ScrollBar.Visible = false;
        }

        private static void InitializeArtPages()
        {
            for (int i = 0; i < PDFDocument.Pages.Count; i++)
            {
                ARTPages.Add(new ImGearARTPage());
                ARTPages.Last().History.IsEnabled = true;
                ARTPages.Last().History.Limit = 99;
            }
            ARTPages.ForEach(x => x.RemoveMarks());
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
            OnPageChanged(null);
            UpdateAfterRender();
        }

        private static void UpdateAfterRender()
        {
            DisplayCurrentPageMarks();
            ScrollBar.Value = currentPageID;
            UpdatePageView();
        }

        public static void ToolBarChangeVisibility()
        {
            if (ARTForm.ToolBar.Visible)
            {
                ARTForm.ToolBar.Visible = false;
                PageView.Location = new Point(PageView.Location.X - ARTForm.ToolBar.Width, PageView.Location.Y);
                PageView.Width = PageView.Width + ARTForm.ToolBar.Width;
            }
            else
            {
                ARTForm.ToolBar.Visible = true;
                PageView.Location = new Point(PageView.Location.X + ARTForm.ToolBar.Width, PageView.Location.Y);
                PageView.Width = PageView.Width - ARTForm.ToolBar.Width;
            }
        }

        public static int PagesInDocumentCount()
        {
            return PDFDocument.Pages.Count;
        }
        public static void RotateLeft()
        {
            PageView.Page.Orientation.Rotate(ImGearRotationValues.VALUE_270);
            UpdatePageView();
        }

        public static void RotateRight()
        {
            PageView.Page.Orientation.Rotate(ImGearRotationValues.VALUE_90);
            UpdatePageView();
        }

        public static void Undo()
        {
            ARTPages[currentPageID].History.Undo();
            UpdatePageView();
        }

        public static void Redo()
        {
            ARTPages[currentPageID].History.Redo();
            UpdatePageView();
        }

        public static void PrevPage()
        {
            if (currentPageID > 0)
                RenderPage(currentPageID - 1);
        }

        public static void NextPage()
        {
            if (currentPageID < PDFDocument.Pages.Count - 1)
                RenderPage(currentPageID + 1);
        }

        public static int SelectedMarksCount()
        {
            int selectedMarksCounter = 0;

            foreach (ImGearARTMark ARTMark in ARTPages[currentPageID])
            {
                if (ARTPages[currentPageID].MarkIsSelected(ARTMark))
                    selectedMarksCounter++;
            }

            return selectedMarksCounter;
        }

        public static void SelectAllMarks()
        {
            if (SelectedMarksCount() == ARTPages[currentPageID].MarkCount)
                ARTPages[currentPageID].SelectMarks(false);
            else
                ARTPages[currentPageID].SelectMarks(true);
            UpdatePageView();
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

        public static void FileLoad()
        {
            OpenFileDialog openFileDialogLoad = new OpenFileDialog();
            // Allow only PDF and PS files.
            openFileDialogLoad.Filter =
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);

            if (DialogResult.OK == openFileDialogLoad.ShowDialog())
            {
                FileLoad(openFileDialogLoad.FileName);
            }
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

                    InitializeScrollBar();
                    InitializeArtPages();

                    // Render first page.
                    RenderPage(0);
                    UpdatePageView();
                    OnDocumentChanged(null);

                }
                catch (ImGearException ex)
                {
                    // Perform error handling.
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static void FileSave(string fileName)
        {
            // Save to output file.
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    // Save the page in the requested format.
                    ImGearFileFormats.SaveDocument(
                        PDFDocument,
                        outputStream,
                        0,
                        ImGearSavingModes.OVERWRITE,
                        ImGearSavingFormats.PDF,
                        null);
                }
                // Perform error handling.
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save file {0}. {1}", fileName, ex.Message));
                    return;
                }
            }
        }

        public static void FileSave()
        {
            using (SaveFileDialog fileDialogSave = new SaveFileDialog())
            {
                fileDialogSave.Filter = ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
                ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);
                fileDialogSave.OverwritePrompt = true;

                if (DialogResult.OK == fileDialogSave.ShowDialog())
                {
                    FileSave(fileDialogSave.FileName);
                }
            }
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

        public static void DisposeImGear()
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }
    }
}
