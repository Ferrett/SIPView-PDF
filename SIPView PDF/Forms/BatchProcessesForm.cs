using ImageGear.Core;
using ImageGear.Evaluation;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using System;

using System.Collections.Generic;
using System.IO;

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


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



            ProgressBar.Style = ProgressBarStyle.Continuous;
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
            this.Cursor = Cursors.WaitCursor;
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

            ProgressBar.Maximum = filesInSelectedDir.Length;
            ProgressBar.Visible = true;
            currentProcessLabel.Visible = true;
            ProgressBar.Value = 0;


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
            this.Cursor = Cursors.Default;
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


            Invoke(new Action(() => this.FinishProgressBar()));
            Invoke(new Action(() => currentProcessLabel.Text = "Done!"));
            //MessageBox.Show("All files are converted to PDFs", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


            Invoke(new Action(() => this.FinishProgressBar()));
            Invoke(new Action(() => currentProcessLabel.Text = "Done!"));
            //MessageBox.Show("PDF file was splitted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                    if (igSourceDocument == null)
                        return;

                    if (igSourceDocument.Pages.Count <= 1)
                        return;

                    Invoke(new Action(() => currentProcessLabel.Text = $"Splitting: {file}                                         "));
                    Invoke(new Action(() => this.Update()));
                    Invoke(new Action(() => UpdProgressBar()));


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

                        PDFViewSaveLoad.FileSave(outputPath, igTargetDocument);
                    }
                    Invoke(new Action(() => ProgressBar.Value++));


                }
            }

            ImGearPDF.Terminate();
        }

        private void AllFilesToSiglePDF()
        {
            ImGearPDFDocument igResultDocument = new ImGearPDFDocument();

            foreach (var file in filesInSelectedDir)
            {

                currentProcessLabel.Text = $"Adding: {file}                                         ";
                this.Update();
                UpdProgressBar();


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

                }
                catch (Exception) { }
                ProgressBar.Value++;
            }

            string filename = targetFolderTextBox.Text + "\\" + Path.GetFileName(sourseFolderTextBox.Text) + ".pdf";

            PDFViewSaveLoad.FileSave(filename, igResultDocument);

            FinishProgressBar();
            currentProcessLabel.Text = "Done!";

        }


        private void AllFilesToPDFs(string file)
        {
            ImGearPDF.Initialize();


            Invoke(new Action(() => currentProcessLabel.Text = $"Converting: {file}                                         "));
            Invoke(new Action(() => this.Update()));
            Invoke(new Action(() => UpdProgressBar()));

            try
            {
                // Load required page from a file.
                using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    page = ImGearFileFormats.LoadPage(stream);


                string filename = $"{targetFolderTextBox.Text}\\{Path.GetFileNameWithoutExtension(file)}.pdf";
                // Save page as PDF document to a file.

                ImGearPDFDocument tmp = new ImGearPDFDocument();
                tmp.Pages.Add(page);
                PDFViewSaveLoad.FileSave(filename, tmp);
            }
            // If file can't be converted to PDF, it triggers exeption.
            catch (Exception) { }
            Invoke(new Action(() => ProgressBar.Value++));


            ImGearPDF.Terminate();
        }


        private void FinishProgressBar()
        {
            ProgressBar.Maximum = 101;
            ProgressBar.Value = 101;
            ProgressBar.Maximum = 100;
        }

        private void UpdProgressBar()
        {
            var a = ProgressBar.Value;
            ProgressBar.Value = ProgressBar.Maximum;
            ProgressBar.Value = a;
        }


        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
