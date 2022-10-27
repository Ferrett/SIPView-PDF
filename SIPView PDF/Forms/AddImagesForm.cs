using ImageGear.Core;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SIPView_PDF
{
    public partial class AddImagesForm : Form
    {
        public int DocumentPagesCount;
        public ImGearPDFDocument PDFDocument = null;
        public ImGearPage page = null;
        public AddImagesForm(int documentPagesCount)
        {
            DocumentPagesCount = documentPagesCount;
            InitializeComponent();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startPos_Click(object sender, EventArgs e)
        {
            posInDocumentTextBox.Text = "1";
        }

        private void endPos_Click(object sender, EventArgs e)
        {
            posInDocumentTextBox.Text = (DocumentPagesCount+1).ToString();
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(posInDocumentTextBox.Text, out int i))
            {
                PDFDocument = new ImGearPDFDocument();
                foreach (string file in selectImagesListBox.Items)
                {
                    using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        ImGearPage page = ImGearFileFormats.LoadPage(stream);
                        PDFDocument.Pages.Add(page.Clone());
                    }
                }
                PDFViewClass.AddPagesToDocument(int.Parse(posInDocumentTextBox.Text), PDFDocument);
                this.Close();
            }
            else
                MessageBox.Show("Test");
        }

        private void selectImagesBtn_Click(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                fbd.Multiselect = true;
                fbd.Filter = "Images|*.jpg;*.jpeg;*.png";
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && fbd.FileNames != null)
                {
                    selectImagesListBox.Items.AddRange(fbd.FileNames);
                }

            }
        }
    }
}
