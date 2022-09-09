using System;
using System.IO;
using System.Windows.Forms;

using ImageGear.Core;
using ImageGear.Display;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using ImageGear.Evaluation;

namespace SIPView_PDF
{
    public partial class Form1 : Form
    {
        private ImGearDocument igDocument = null;
        private int currentPageIndex = 0;

        public Form1()
        {
            // Initialize evaluation license.
            ImGearEvaluationManager.Initialize();
            ImGearEvaluationManager.Mode = ImGearEvaluationMode.Watermark;

            // Add support for PDF and PS files.
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();

            InitializeComponent();
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
                        igDocument = ImGearFileFormats.LoadDocument(inputStream);
                    }
                    catch (ImGearException ex)
                    {
                        // Perform error handling.
                        MessageBox.Show(ex.Message);
                    }
                }
                // Render first page.
                renderPage(0);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if document exists.
            if (igDocument == null)
                return;

            string filename = "";
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
                        igDocument,
                        outputStream,
                        0,
                        ImGearSavingModes.OVERWRITE,
                        savingFormat,
                        null);
                    MessageBox.Show("Saved {0}", filename);
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
            ImGearPage igPage = null;
            // Load a single page from the loaded document.
            try
            {
                igPage = igDocument.Pages[pageNumber];
                if (igPage != null)
                {
                    // Create a new page display to prepare the page for being displayed.
                    ImGearPageDisplay igPageDisplay = new ImGearPageDisplay(igPage);
                    // Associate the page display with the page view.
                    imGearPageView1.Display = igPageDisplay;
                    // Cause the page view to repaint.
                    imGearPageView1.Invalidate();
                    currentPageIndex = pageNumber;
                    toolStripStatusLabel1.Text = string.Format("{0} of {1}", pageNumber + 1, igDocument.Pages.Count);

                }
            }
            catch (ImGearException ex)
            {
                // Perform error handling.
                MessageBox.Show(ex.Message);
            }
        }

       

        private void nextPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (igDocument != null)
                if (currentPageIndex < igDocument.Pages.Count - 1)
                    renderPage(currentPageIndex + 1);
        }

        private void previousPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (igDocument != null)
                if (currentPageIndex > 0)
                    renderPage(currentPageIndex - 1);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ImGearPDF.Terminate();
            imGearPageView1.Display = null;
        }

       
    }
}
