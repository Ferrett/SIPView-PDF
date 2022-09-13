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
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SIPView_PDF
{
    public partial class BatchProcessesForm : Form
    {
        BatchProcess process;
        string[] files;
        public BatchProcessesForm(BatchProcess proc)
        {
            process = proc;
            InitializeComponent();

            switch (process)
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



        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = fbd.SelectedPath;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox2.Text = fbd.SelectedPath;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (process)
            {
                case BatchProcess.ALL_FILES_TO_PDFS:
                    {
                        AllFilesToPDFs();
                        break;
                    }
                case BatchProcess.ALL_FILES_TO_SINGLE_PDF:
                    {
                        AllFilesToSiglePDF();
                        break;
                    }
                case BatchProcess.SPLIT_MULTIPAGE_PDFS:
                    {
                        SplitMultipagePDFs();
                        break;
                    }
                default:
                    break;
            }
        }

        private void SplitMultipagePDFs()
        {
            aboba();

            foreach (string file in files)
            {
                label3.Text = $"Splitting \"{Path.GetFileName(file)}\"";
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
                            String outputFileName = String.Format("{0}_{1}.pdf",
                               Path.GetFileNameWithoutExtension(file), i + 1);
                            String outputPath = System.IO.Path.Combine(textBox2.Text, outputFileName);

                            // Create a new empty PDF document.
                            using (ImGearPDFDocument igTargetDocument = new ImGearPDFDocument())
                            {
                                // Insert page into new PDF document.
                                igTargetDocument.InsertPages((int)ImGearPDFPageNumber.BEFORE_FIRST_PAGE, 
                                    igSourceDocument, i, 1, ImGearPDFInsertFlags.DEFAULT);

                                // Save new PDF document to file.
                                igTargetDocument.Save(outputPath, ImGearSavingFormats.PDF, 0, 0, 1,ImGearSavingModes.OVERWRITE);
                            }
                        }
                    }
                }

                progressBar1.Value++;
            }

            label3.Text = "Done!";
        }
        private void AllFilesToSiglePDF()
        {
            aboba();

            ImGearPDFDocument igResultDocument = new ImGearPDFDocument();

            ImGearPage page = null;
            foreach (var file in files)
            {
                label3.Text = $"Adding {Path.GetFileName(file)}";
                try
                {
                    using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                        page = ImGearFileFormats.LoadPage(stream);


                    igResultDocument.Pages.Add(page);
                }
                catch (Exception)
                {

                }
                progressBar1.Value++;
            }

            string filename = textBox2.Text + "\\" + Path.GetFileName(textBox1.Text) + ".pdf";
            using (FileStream outputStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                try
                {
                    // Save the page in the requested format.
                    ImGearFileFormats.SaveDocument(
                        igResultDocument,
                        outputStream,
                        0,
                        ImGearSavingModes.OVERWRITE,
                        ImGearSavingFormats.PDF,
                        null);
                    MessageBox.Show("Saved");
                }
                // Perform error handling.
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save file"));
                    return;
                }
            }

            label3.Text = "Done!";
        }

        private void aboba()
        {
            files = checkBox1.Checked ? Directory.GetFiles(textBox1.Text, "*", SearchOption.AllDirectories) : Directory.GetFiles(textBox1.Text);
            progressBar1.Visible = true;
            label3.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Maximum = files.Length;
        }


        private void AllFilesToPDFs()
        {
            aboba();

            #region Parallel
            //Parallel.ForEach(files, (file) =>
            //{
            //    // Load required page from a file.
            //        ImGearPage page = null;

            //        using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            //            page = ImGearFileFormats.LoadPage(stream);

            //        // Save page as PDF document to a file.
            //        using (Stream stream = new FileStream($"{textBox2.Text}\\{Path.GetFileNameWithoutExtension(file)}.pdf", FileMode.Create, FileAccess.Write))
            //            ImGearFileFormats.SavePage(page, stream, ImGearSavingFormats.PDF);

            //});
            #endregion

            ImGearPage page = null;
            foreach (var file in files)
            {
                try
                {
                    label3.Text = $"Converting \"{Path.GetFileName(file)}\"...";

                    // Load required page from a file.

                    using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                        page = ImGearFileFormats.LoadPage(stream);

                    // Save page as PDF document to a file.
                    using (Stream stream = new FileStream($"{textBox2.Text}\\{Path.GetFileNameWithoutExtension(file)}.pdf", FileMode.Create, FileAccess.Write))
                        ImGearFileFormats.SavePage(page, stream, ImGearSavingFormats.PDF);
                }
                catch (Exception)
                {

                }
                progressBar1.Value++;
            }

            label3.Text = "Done!";
        }


    }
}
