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
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SIPView_PDF
{
    public partial class BatchProcessesForm : Form
    {
        BatchProcess process;
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

                        break;
                    }
                case BatchProcess.SPLIT_MULTIPAGE_PDFS:
                    {

                        break;
                    }
                default:
                    break;
            }
        }


        private void AllFilesToPDFs()
        {
            string[] files = Directory.GetFiles(textBox1.Text);

            progressBar1.Visible = true;
            label3.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Maximum = files.Length;



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

            foreach (var file in files)
            {
                try
                {
                    label3.Text = $"Converting \"{Path.GetFileName(file)}\"...";

                    // Load required page from a file.
                    Thread.Sleep(1000);
                    ImGearPage page = null;
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
