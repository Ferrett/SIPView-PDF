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

namespace SIPView_PDF
{
    public partial class MainForm : Form
    {
        private ImGearDocument PDFDocument = null;
        private int currentPageIndex = 0;
        ImGearARTForms ARTForms;
        List<ImGearARTPage> ARTPages = new List<ImGearARTPage>();


        #region Legend
        //  ImGearDocument - opened pdf file
        //  ImGearPage - one page of pdf file
        //  ImGearARTForms - provides properties and methods for using ART forms functionality.
        //  ImGearARTPage - represents a collection of annotation groups.
        #endregion
        public MainForm()
        {
            // Add support for PDF and PS files.
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();
            
            InitializeComponent(); 

            ARTForms = new ImGearARTForms(PageView, ImGearARTToolBarModes.ART20);

            
            //ARTPage = new ImGearARTPage();


            ARTForms.Mode = ImGearARTModes.EDIT;


            ARTForms.ToolBar.TopLevel = false;
            ARTForms.ToolBar.Size = new Size(1085, 1000);
            ARTForms.ToolBar.Location = new Point(0, 49);

           

            ARTForms.ToolBar.Show();
            ARTForms.ToolBar.Visible = false;
            ARTForms.ToolBar.FormBorderStyle = FormBorderStyle.None;
            ARTForms.MarkCreated += ImGearARTForms_MarkCreated;

            //ImGearART.SavePage(imGearARTPage, imGearPageView1.Page);
            

            Button test = new Button();
            test.Click += loadToolStripMenuItem_Click;

            this.Controls.Add(ARTForms.ToolBar);
        }

        private void SaveAnnotations()  
        {
            ImGearPage igPageNew = ImGearART.BurnIn(PageView.Page, ARTPages[currentPageIndex], ImGearARTBurnInOptions.SELECTED, null);
            PageView.Page = igPageNew;
        }

        private void ImGearARTForms_MarkCreated(object sender, ImGearARTFormsMarkCreatedEventArgs e)
        {
            //imGearARTPage.SelectMarks(true);
            
            
            PageView.Update();
        }

        private void imGearPageView1_MouseDown(object sender, MouseEventArgs e)
        {
            ARTForms.MouseDown(sender, e);
        }

        private void imGearPageView1_MouseMove(object sender, MouseEventArgs e)
        {
            ARTForms.MouseMove(sender, e);
        }

        private void imGearPageView1_MouseUp(object sender, MouseEventArgs e)
        {
            ARTForms.MouseUp(sender, e);
        }


        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialogLoad = new OpenFileDialog();
            // Allow only PDF and PS files.
            openFileDialogLoad.Filter =
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);

            if (DialogResult.OK == openFileDialogLoad.ShowDialog())
            {
                using (FileStream inputStream =
                    new FileStream(openFileDialogLoad.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        // Load the entire the document.
                        PDFDocument = ImGearFileFormats.LoadDocument(inputStream);
                    }
                    catch (ImGearException ex)
                    {
                        // Perform error handling.
                        MessageBox.Show(ex.Message);
                    }
                }
                // Render first page.
                renderPage(0);
                for (int i = 0; i < PDFDocument.Pages.Count; i++)
                {
                    ARTPages.Add(new ImGearARTPage());
                }
                UpdateBtns();
                ARTPages.ForEach(x => x.RemoveMarks());//.RemoveMarks();
                DisplayCurrentPageMarks();


            }
        }

        private void DisplayCurrentPageMarks()
        {
            PageView.Display = new ImGearPageDisplay(PageView.Page, ARTPages[currentPageIndex]);
            ARTForms.Page = ARTPages[currentPageIndex];
           
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAnnotations();

            // Check if document exists.
            if (PDFDocument == null)
                return;

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
                    currentPageIndex = pageNumber;
                    toolStripStatusLabel1.Text = string.Format("{0} of {1}", pageNumber + 1, PDFDocument.Pages.Count);
                }
                ARTForms.ToolBar.Visible = true;
            }
            catch (ImGearException ex)
            {
                // Perform error handling.
                MessageBox.Show(ex.Message);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void allFilesToPDFsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.ALL_FILES_TO_PDFS);
            batchProcessesForm.ShowDialog();
        }

        private void allFilesToSinglePDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.ALL_FILES_TO_SINGLE_PDF);
            batchProcessesForm.ShowDialog();
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.SPLIT_MULTIPAGE_PDFS);
            batchProcessesForm.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            PageView.Page.Orientation.Rotate(ImGearRotationValues.VALUE_270);
            PageView.Update();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            PageView.Page.Orientation.Rotate(ImGearRotationValues.VALUE_90);
            PageView.Update();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (currentPageIndex > 0)
                renderPage(currentPageIndex - 1);

            if (currentPageIndex == 0)
                toolStripButton3.Enabled = false;

            toolStripButton4.Enabled = true;
            DisplayCurrentPageMarks();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (currentPageIndex < PDFDocument.Pages.Count - 1)
                renderPage(currentPageIndex + 1);

            if (currentPageIndex == PDFDocument.Pages.Count - 1)
                toolStripButton4.Enabled = false;

            toolStripButton3.Enabled = true;
            DisplayCurrentPageMarks();
        }

        private void UpdateBtns()
        {
            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = true;

            if (PDFDocument.Pages.Count > 1)
                toolStripButton4.Enabled = true;
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ImGearPDF.Terminate();
            PageView.Display = null;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ARTPages[0].History.Undo();
            PageView.Update();
        }
        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            ARTPages[0].History.Redo();
            PageView.Update();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {

        }

    }
}
