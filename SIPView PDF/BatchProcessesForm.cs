using ImageGear.Core;
using ImageGear.Evaluation;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SIPView_PDF
{
    public partial class BatchProcessesForm : Form
    {
        BatchProcess batchProcess;
        string[] filesInSelectedDir;
        ImGearPage page;
        public BatchProcessesForm(BatchProcess passingProc)
        {
            batchProcess = passingProc;

            InitializeComponent();

            switch (batchProcess)
            {
                case BatchProcess.ALL_FILES_TO_PDFS:
                    {
                        this.Text = "All files to PDFs";
                        break;
                    }
                case BatchProcess.ALL_FILES_TO_SINGLE_PDF:
                    {
                        this.Text = "All files to single PDF";
                        break;
                    }
                case BatchProcess.SPLIT_MULTIPAGE_PDFS:
                    {
                        this.Text = "Split multipage PDFs";
                        break;
                    }
                default:
                    break;
            }
        }


        private void sourseFolderBtn_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    sourseFolderTextBox.Text = fbd.SelectedPath;
                }
            }
        }


        private void targetFolderBtn_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    targetFolderTextBox.Text = fbd.SelectedPath;
                }
            }
        }


        private async void startBtn_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(sourseFolderTextBox.Text))
            {
                MessageBox.Show("Incorrect Sourse Folder", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(targetFolderTextBox.Text))
            {
                MessageBox.Show("Incorrect Target Folder", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GetFilesFromSoureDir();

            if (filesInSelectedDir == null)
            {
                MessageBox.Show("Sourse Folder Is Empty", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (batchProcess)
            {
                case BatchProcess.ALL_FILES_TO_PDFS:
                    {
                        await Task.Run(() => AllFilesToPDFsThreadManager());
                        break;
                    }
                case BatchProcess.ALL_FILES_TO_SINGLE_PDF:
                    {
                        AllFilesToSiglePDF();
                        break;
                    }
                case BatchProcess.SPLIT_MULTIPAGE_PDFS:
                    {
                        await Task.Run(() => SplitMultipagePDFsThreadManager());
                        break;
                    }
                default:
                    break;
            }
        }

        private void GetFilesFromSoureDir()
        {
            // Get all file paths in sourse folder
            if (includeSubfoldersCheckBox.Checked)
                filesInSelectedDir = Directory.GetFiles(sourseFolderTextBox.Text, "*", SearchOption.AllDirectories);
            else
                filesInSelectedDir = Directory.GetFiles(sourseFolderTextBox.Text);
        }

        private void AllFilesToPDFsThreadManager()
        {
            // Allocate thread pool.

            List<Thread> threadList = new List<Thread>();
            for (int i = 0; i < filesInSelectedDir.Length; i++)
            {
                Thread thread = new Thread(new ThreadStart(() => AllFilesToPDFs(filesInSelectedDir[i])));
                thread.Name = i.ToString();
                threadList.Add(thread);
                thread.Start();
                thread.Join();
            }

            MessageBox.Show("Done");
        }

        private void SplitMultipagePDFsThreadManager()
        {
            // Allocate thread pool.

            List<Thread> threadList = new List<Thread>();
            for (int i = 0; i < filesInSelectedDir.Length; i++)
            {
                Thread thread = new Thread(new ThreadStart(() => SplitMultipagePDFs(filesInSelectedDir[i])));
                thread.Name = i.ToString();
                threadList.Add(thread);
                thread.Start();
                thread.Join();
            }

            MessageBox.Show("Done");
        }


        private void SplitMultipagePDFs(string file)
        {
            ImGearPDF.Initialize();

            // Open file for reading.
            using (FileStream pdfData = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                // Read PDF document to memory.
                using (ImGearPDFDocument igSourceDocument = ImGearFileFormats.LoadDocument(
                    pdfData, 0, (int)ImGearPDFPageRange.ALL_PAGES) as ImGearPDFDocument)
                {

                    // For each page in document.
                    for (int i = 0; i < igSourceDocument.Pages.Count; i++)
                    {
                        // Construct the output filepath.
                        string outputFileName = String.Format("{0}_{1}.pdf",
                           Path.GetFileNameWithoutExtension(file), i + 1);
                        string outputPath = Path.Combine(targetFolderTextBox.Text, outputFileName);

                        // Create a new empty PDF document.
                        ImGearPDFDocument igTargetDocument = new ImGearPDFDocument();

                        // Insert page into new PDF document.
                        igTargetDocument.InsertPages((int)ImGearPDFPageNumber.BEFORE_FIRST_PAGE,
                            igSourceDocument, i, 1, ImGearPDFInsertFlags.DEFAULT);

                        // Save new PDF document to file.
                        igTargetDocument.Save(outputPath, ImGearSavingFormats.PDF, 0, 0, 1, ImGearSavingModes.OVERWRITE);
                    }

                }
            }

            ImGearPDF.Terminate();
        }

        private void AllFilesToSiglePDF()
        {
            ImGearPDFDocument igResultDocument = new ImGearPDFDocument();

            foreach (var file in filesInSelectedDir)
            {
                try
                {
                    // Convert file to PDF page
                    using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        if (Path.GetExtension(file) == ".pdf")
                        {
                            ImGearDocument igDocument = ImGearFileFormats.LoadDocument(stream);
                            for (int pageIndex = 0; pageIndex < igDocument.Pages.Count; pageIndex++)
                                igResultDocument.Pages.Add(igDocument.Pages[pageIndex].Clone());
                        }
                        else
                        {
                            page = ImGearFileFormats.LoadPage(stream);
                            igResultDocument.Pages.Add(page.Clone());
                        }
                    }
                    // Add page to new document


                }
                catch (Exception) { }
            }

            string filename = targetFolderTextBox.Text + "\\" + Path.GetFileName(sourseFolderTextBox.Text) + ".pdf";
            using (FileStream outputStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    // Save the document.
                    ImGearFileFormats.SaveDocument(
                        igResultDocument,
                        outputStream,
                        0,
                        ImGearSavingModes.OVERWRITE,
                        ImGearSavingFormats.PDF,
                        null);
                }
                // Perform error handling.
                catch (Exception)
                {
                    MessageBox.Show("Could not save file", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("Done");
        }


        private void AllFilesToPDFs(string file)
        {
            ImGearPDF.Initialize();

            try
            {
                // Load required page from a file.
                using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    page = ImGearFileFormats.LoadPage(stream);

                // Save page as PDF document to a file.
                using (Stream stream = new FileStream($"{targetFolderTextBox.Text}\\{Path.GetFileNameWithoutExtension(file)}.pdf", FileMode.Create, FileAccess.Write))
                    ImGearFileFormats.SavePage(page, stream, ImGearSavingFormats.PDF);
            }
            // If file can't be converted to PDF, it triggers exeption.
            catch (Exception) { }

            ImGearPDF.Terminate();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
