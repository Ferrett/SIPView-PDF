using System;
using System.IO;
using System.Windows.Forms;

using ImageGear.Core;
using ImageGear.Display;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Evaluation;
using System.Drawing;
using System.Drawing.Printing;
using ImageGear.ART.Forms;
using ImageGear.ART;
using ImageGear.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using SIPViewPDFlib;

namespace SIPView_PDF
{
    public partial class MainForm : Form
    {
        List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();
        private ImGearDocument PDFDocument = null;
        private int currentPageID = 0;
        ImGearARTForms ARTForm;

        public MainForm()
        {
            // Add support for PDF and PS files.
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();

            InitializeComponent();
            InitToolBar();
        }

        private void InitToolBar()
        {
            ARTForm = new ImGearARTForms(PageView, ImGearARTToolBarModes.ART30);

            ARTForm.Mode = ImGearARTModes.EDIT;

            ARTForm.ToolBar.TopLevel = false;
            ARTForm.ToolBar.Size = new Size(85, 1000);
            ARTForm.ToolBar.Location = new Point(0, 49);

            ARTForm.ToolBar.Show();
            ARTForm.ToolBar.Visible = false;
            ARTForm.ToolBar.FormBorderStyle = FormBorderStyle.None;
            ARTForm.MarkCreated += ARTForm_MarkCreated;

            this.Controls.Add(ARTForm.ToolBar);
        }

        private void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            renderPage(ScrollBar.Value);

            if (currentPageID == 0)
            {
                PrevPageBtn.Enabled = false;
                NextPageBtn.Enabled = true;
            }
            else if (currentPageID == PDFDocument.Pages.Count - 1)
            {
                PrevPageBtn.Enabled = true;
                NextPageBtn.Enabled = false;
            }
            else
            {
                PrevPageBtn.Enabled = true;
                NextPageBtn.Enabled = true;
            }
            DisplayCurrentPageMarks();
        }

        private void ARTForm_MarkCreated(object sender, ImGearARTFormsMarkCreatedEventArgs e)
        {
            PageView.Update();
        }

        private void PageView_MouseDown(object sender, MouseEventArgs e)
        {
            ARTForm.MouseDown(sender, e);
        }

        private void PageView_MouseMove(object sender, MouseEventArgs e)
        {
            ARTForm.MouseMove(sender, e);
        }

        private void PageView_MouseUp(object sender, MouseEventArgs e)
        {
            ARTForm.MouseUp(sender, e);
        }


        private void FileOpenMenu_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialogLoad = new OpenFileDialog();
            // Allow only PDF and PS files.
            openFileDialogLoad.Filter =
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);

            if (DialogResult.OK == openFileDialogLoad.ShowDialog())
            {
                using (FileStream inputStream =
                    new FileStream(openFileDialogLoad.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                UpdateToolBarBtns();

                renderPage(0);
                PageView.Refresh();
            }
        }

        private void InitArtPages()
        {
            for (int i = 0; i < PDFDocument.Pages.Count; i++)
            {
                ARTPages.Add(new ImGearARTPage());
                ARTPages.Last().History.IsEnabled = true;
                ARTPages.Last().MarkAdded += ARTPage_MarkAdded;
                ARTPages.Last().MarkRemoved += ARTPage_MarkRemoved;
                ARTPages.Last().MarkSelectionChanged += ARTPage_MarkSelectionChanged;
                ARTPages.Last().History.Limit = 99;
                ARTPages.Last().History.HistoryChanged += ARTPageHistory_HistoryChanged;
            }
            ARTPages.ForEach(x => x.RemoveMarks());//.RemoveMarks();
        }

        private void ARTPageHistory_HistoryChanged(object sender, ImGearARTHistoryEventArgs e)
        {
            if (ARTPages[currentPageID].History.UndoCount == 0)
                UndoBtn.Enabled = false;
            else
                UndoBtn.Enabled = true;

            if (ARTPages[currentPageID].History.RedoCount== 0)
                RedoBtn.Enabled = false;
            else
                RedoBtn.Enabled = true;
        }

        private void InitScrollBar()
        {
            if (PDFDocument.Pages.Count > 1)
            {
                ScrollBar.Visible = true;
                ScrollBar.Maximum = PDFDocument.Pages.Count - 1;
            }
            else
                ScrollBar.Visible = false;
        }

        private void ARTPage_MarkSelectionChanged(object sender, ImGearARTMarkEventArgs e)
        {
            if (CountSelectedMarks() == 0)
                BakeInBtn.Enabled = false;
            else
                BakeInBtn.Enabled = true;
        }

        private void ARTPage_MarkRemoved(object sender, ImGearARTMarkEventArgs e)
        {
            if (ARTPages[currentPageID].MarkCount == 0)
                SelectAllBtn.Enabled = false;
        }

        private void ARTPage_MarkAdded(object sender, ImGearARTMarkEventArgs e)
        {
            SelectAllBtn.Enabled = true;
        }

        private void DisplayCurrentPageMarks()
        {
            PageView.Display = new ImGearPageDisplay(PDFDocument.Pages[currentPageID], ARTPages[currentPageID]);
            ARTForm.Page = ARTPages[currentPageID];

        }

        private void FileSaveMenu_Click(object sender, EventArgs e)
        {
            string filename = String.Empty;
            // Open File dialog. For this sample, just allow PDF or PS.
            ImGearSavingFormats savingFormat = ImGearSavingFormats.PDF;

            using (SaveFileDialog fileDialogSave = new SaveFileDialog())
            {
                fileDialogSave.Filter = ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
                ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);
                fileDialogSave.OverwritePrompt = true;
                if (fileDialogSave.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                filename = fileDialogSave.FileName;
            }

            // Save to output file.
            using (FileStream outputStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    // Save the page in the requested format.
                    ImGearFileFormats.SaveDocument(
                        PDFDocument,
                        outputStream,
                        0,
                        ImGearSavingModes.OVERWRITE,
                        savingFormat,
                        null);
                }
                // Perform error handling.
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save file {0}. {1}", filename, ex.Message));
                    return;
                }
            }
        }

        private void renderPage(int pageNumber)
        {
            // Create page to hold the content.
            ImGearPage page = null;
            // Load a single page from the loaded document.
            try
            {
                page = PDFDocument.Pages[pageNumber];
                if (page != null)
                {
                    // Create a new page display to prepare the page for being displayed.
                    ImGearPageDisplay igPageDisplay = new ImGearPageDisplay(page);
                    // Associate the page display with the page view.
                    PageView.Display = igPageDisplay;
                    // Cause the page view to repaint.
                    PageView.Invalidate();
                    currentPageID = pageNumber;
                    toolStripStatusLabel1.Text = string.Format("{0} of {1}", pageNumber + 1, PDFDocument.Pages.Count);
                }
                
            }
            catch (ImGearException ex)
            {
                // Perform error handling.
                MessageBox.Show(ex.Message);
            }
            DisplayCurrentPageMarks();
            ScrollBar.Value = currentPageID;
            PageView.Update();
        }

        private void FilePrintMenu_Click(object sender, EventArgs e)
        {
            if (PDFDocument == null)
                return;

            using (ImGearPDFDocument igPDFDocument = (ImGearPDFDocument)PDFDocument)
            {

                ImGearPDFPrintOptions printOptions = new ImGearPDFPrintOptions();
                PrintDocument printDocument = new PrintDocument();

                // Use default Windows printer.
                printOptions.DeviceName = printDocument.PrinterSettings.PrinterName;

                // Print all pages.
                printOptions.StartPage = 0;
                printOptions.EndPage = PDFDocument.Pages.Count;
                igPDFDocument.Print(printOptions);
            }
        }

        private void AllFilesToPDFsMenu_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.ALL_FILES_TO_PDFS);
            batchProcessesForm.Show();
        }

        private void AllFilesToSinglePDFMenu_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.ALL_FILES_TO_SINGLE_PDF);
            batchProcessesForm.Show();
        }

        private void SplitMultipagePDFsMenu_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.SPLIT_MULTIPAGE_PDFS);
            batchProcessesForm.Show();
        }

        private void RotateLeftBtn_Click(object sender, EventArgs e)
        {
            PageView.Page.Orientation.Rotate(ImGearRotationValues.VALUE_270);
            PageView.Update();
        }

        private void RotateRightBtn_Click(object sender, EventArgs e)
        {
            PageView.Page.Orientation.Rotate(ImGearRotationValues.VALUE_90);
            PageView.Update();
        }

        private int CountSelectedMarks()
        {
            int selectedMarksCounter = 0;

            foreach (ImGearARTMark ARTMark in ARTPages[currentPageID])
            {
                if (ARTPages[currentPageID].MarkIsSelected(ARTMark))
                    selectedMarksCounter++;
            }

            return selectedMarksCounter;
        }
        private void PrevPageBtn_Click(object sender, EventArgs e)
        {
            if (currentPageID > 0)
                renderPage(currentPageID - 1);

            if (currentPageID == 0)
                PrevPageBtn.Enabled = false;

            NextPageBtn.Enabled = true;
        }

        private void NextPageBtn_Click(object sender, EventArgs e)
        {
            if (currentPageID < PDFDocument.Pages.Count - 1)
                renderPage(currentPageID + 1);

            if (currentPageID == PDFDocument.Pages.Count - 1)
                NextPageBtn.Enabled = false;

            PrevPageBtn.Enabled = true;

        }

        private void UpdateToolBarBtns()
        {
            RotateLeftBtn.Enabled = true;
            RotateRightBtn.Enabled = true;
            FileSaveBtn.Enabled = true;
            EditBtn.Enabled = true;

            FileSaveMenu.Enabled = true;
            FilePrintMenu.Enabled = true;
            ToolsMenu.Enabled = true;
            EditMenu.Enabled = true;

            if (PDFDocument.Pages.Count > 1)
                NextPageBtn.Enabled = true;
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }

        private void UndoBtn_Click(object sender, EventArgs e)
        {
            ARTPages[currentPageID].History.Undo();

            PageView.Update();
        }
        private void RedoBtn_Click(object sender, EventArgs e)
        {
            ARTPages[currentPageID].History.Redo();
            PageView.Update();
        }

        private void SelectAllBtn_Click(object sender, EventArgs e)
        {
            if (CountSelectedMarks() == ARTPages[currentPageID].MarkCount)
                ARTPages[currentPageID].SelectMarks(false);
            else
                ARTPages[currentPageID].SelectMarks(true);
            PageView.Update();
        }

        private void BakeInBtn_Click(object sender, EventArgs e)
        {
            PDFDocument.Pages[currentPageID] = ImGearART.BurnIn(PDFDocument.Pages[currentPageID], ARTPages[currentPageID], ImGearARTBurnInOptions.SELECTED, null);
            PageView.Page = PDFDocument.Pages[currentPageID];


            List<int> bakedMarkID = new List<int>();
            foreach (ImGearARTMark ARTMark in ARTPages[currentPageID])
            {
                if (ARTPages[currentPageID].MarkIsSelected(ARTMark))
                    bakedMarkID.Add(ARTMark.Id);
            }

            foreach (int ID in bakedMarkID)
            {
                ARTPages[currentPageID].MarkRemove(ID);
            }

            PageView.Update();
        }

        private void FileSaveBtn_Click(object sender, EventArgs e)
        {
            FileSaveMenu_Click(sender, e);
        }
        private void FileOpenBtn_Click(object sender, EventArgs e)
        {
            FileOpenMenu_Click(sender, e);
        }

        private void EditBtn_Click(object sender, EventArgs e)
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

       
    }
}
