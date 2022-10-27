using ImageGear.Core;
using ImageGear.Formats;
using ImageGear.Formats.PDF;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        public ImGearPage IGPage = null;
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
            posInDocumentTextBox.Text = (DocumentPagesCount + 1).ToString();
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            if (selectImagesListBox.Items.Count == 0)
            {
                MessageBox.Show("No images selected", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(posInDocumentTextBox.Text, out int i))
            {
                MessageBox.Show("Incorrect page number", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (i < 0)
            {
                MessageBox.Show("Page number too low", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (i > DocumentPagesCount+1)
            {
                MessageBox.Show("Page number too high", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PDFDocument = new ImGearPDFDocument();
            foreach (string file in selectImagesListBox.Items)
            {
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    IGPage = ImGearFileFormats.LoadPage(stream);
                    PDFDocument.Pages.Add(IGPage.Clone());
                }
            }
            PDFViewClass.AddPagesToDocument(int.Parse(posInDocumentTextBox.Text), PDFDocument);
            this.Close();


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
                    clearImagesBtn.Enabled = true;
                }

            }
        }

        private void clearImagesBtn_Click(object sender, EventArgs e)
        {
            selectImagesListBox.Items.Clear();
            clearImagesBtn.Enabled = false;
            removeImagesBtn.Enabled = false;
        }

        private void selectImagesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectImagesListBox.Items.Count > 0)
            {
                removeImagesBtn.Enabled = true;
                clearImagesBtn.Enabled = true;
            }
            else
            {
                removeImagesBtn.Enabled = false;
                clearImagesBtn.Enabled = false;
            }
        }

        private void removeImagesBtn_Click(object sender, EventArgs e)
        {
            selectImagesListBox.Items.RemoveAt(selectImagesListBox.SelectedIndex);
        }
    }
}
