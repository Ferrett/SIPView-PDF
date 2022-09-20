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


namespace SIPView_PDF
{
    public partial class MainForm : Form
    {
        private ImGearDocument igDocument = null;
        private int currentPageIndex = 0;
        ImGearARTForms imGearARTForms;
        public MainForm()
        {
            // Add support for PDF and PS files.
            ImGearCommonFormats.Initialize();
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePDFFormat());
            ImGearFileFormats.Filters.Insert(0, ImGearPDF.CreatePSFormat());
            ImGearPDF.Initialize();


            InitializeComponent();
            

            imGearARTForms = new ImGearARTForms(imGearPageView1,ImGearARTToolBarModes.ART20);
            imGearARTForms.Mode = ImGearARTModes.EDIT;


           

            //imGearARTForms.ToolBar.Owner = this;
            imGearARTForms.ToolBar.TopLevel = false;
            imGearARTForms.ToolBar.Size = new Size(80, 1000);
            imGearARTForms.ToolBar.Location =new Point(0, 49);



            imGearARTForms.ToolBar.Show();
            imGearARTForms.ToolBar.FormBorderStyle = FormBorderStyle.None;
            this.Controls.Add(imGearARTForms.ToolBar);
            
        }



        private void imGearPageView1_MouseDown(object sender, MouseEventArgs e)
        {
            imGearARTForms.MouseDown(sender, e);
        }

        private void imGearPageView1_MouseMove(object sender, MouseEventArgs e)
        {
            imGearARTForms.MouseMove(sender, e);
        }

        private void imGearPageView1_MouseUp(object sender, MouseEventArgs e)
        {
            imGearARTForms.MouseUp(sender, e);
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

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (igDocument == null)
                return;

            using (ImGearPDFDocument igPDFDocument = (ImGearPDFDocument)igDocument)
            {
                ImGearPDFPrintOptions printOptions = new ImGearPDFPrintOptions();
                PrintDocument printDocument = new PrintDocument();

                // Use default Windows printer.
                printOptions.DeviceName = printDocument.PrinterSettings.PrinterName;

                // Print all pages.
                printOptions.StartPage = 0;
                printOptions.EndPage = igDocument.Pages.Count;
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ImGearPDF.Terminate();
            imGearPageView1.Display = null;

           
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
          
            if (imGearPageView1.Display.Orientation.Value == ImGearOrientationModes.TOP_LEFT)

                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.LEFT_BOTTOM;

            else if (imGearPageView1.Display.Orientation.Value == ImGearOrientationModes.LEFT_BOTTOM)

                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.BOTTOM_RIGHT;

            else if (imGearPageView1.Display.Orientation.Value == ImGearOrientationModes.BOTTOM_RIGHT)
                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.RIGHT_TOP;
            else
                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.TOP_LEFT;

            imGearPageView1.Refresh();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
            if (imGearPageView1.Display.Orientation.Value == ImGearOrientationModes.TOP_LEFT)
                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.RIGHT_TOP;
            else if (imGearPageView1.Display.Orientation.Value == ImGearOrientationModes.RIGHT_TOP)
                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.BOTTOM_RIGHT;
            else if (imGearPageView1.Display.Orientation.Value == ImGearOrientationModes.BOTTOM_RIGHT)
                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.LEFT_BOTTOM;
            else
                imGearPageView1.Display.Orientation.Value = ImGearOrientationModes.TOP_LEFT;

            imGearPageView1.Refresh();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (currentPageIndex > 0)
                renderPage(currentPageIndex - 1);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (currentPageIndex < igDocument.Pages.Count - 1)
                renderPage(currentPageIndex + 1);


           
        }

       
    }
}
