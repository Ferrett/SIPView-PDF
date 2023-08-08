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

namespace SIPView_PDF.Forms
{
    public partial class PDFSettingsForm : Form
    {
        public PDFSettingsForm()
        {
            InitializeComponent();

            if (PDFManager.DoCompression)
            {
                noneCheckBox.Checked = false;
                downsampleCheckBox.Checked = PDFManager.CompressOptions.IsDownsampleImagesEnabled;
                jpeg2KRadioButton.Checked = PDFManager.CompressOptions.RecompressUsingJpeg2k;
                bitonalComboBox.SelectedIndex = (int)PDFManager.CompressOptions.RecompressImageJbig2CompressionLevel;
                jpegComboBox.SelectedIndex = (int)PDFManager.CompressOptions.RecompressImageJpegCompressionLevel;
                jpeg2KComboBox.SelectedIndex = (int)PDFManager.CompressOptions.RecompressImageJpeg2kCompressionLevel;
                flatteringCheckBox.Checked = PDFManager.CompressOptions.IsFieldFlatteningEnabled;
            }
            else
            {
                bitonalComboBox.SelectedIndex = 0;
                jpegComboBox.SelectedIndex = 0;
                jpeg2KComboBox.SelectedIndex = 0;
            }

            titleTextBox.Text = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.GetInfo("Title");
            subjectTextBox.Text = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.GetInfo("Subject");
            creatorTextBox.Text = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.GetInfo("Creator");
            createdTextBox.Text = File.GetCreationTime(PDFManager.Documents[PDFManager.SelectedTabID].DocumentPath).ToString();
            authorTextBox.Text = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.GetInfo("Author");
            keywordsTextBox.Text = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.GetInfo("Keywords");
            producerTextBox.Text = PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.GetInfo("Producer");
            modifiedTextBox.Text = File.GetLastWriteTime(PDFManager.Documents[PDFManager.SelectedTabID].DocumentPath).ToString();

            dontSaveMetadataCheckBox.Checked = PDFManager.CompressOptions.IsRemoveMetadataEnabled;
            dontSaveThumbnailsCheckBox.Checked = PDFManager.CompressOptions.IsRemoveImageThumbnailEnabled;

            if (PDFManager.CompressOptions.IsRemoveMetadataEnabled)
            {
                dontSaveMetadataCheckBox.Checked = true;
                titleLabel.Enabled = false;
                titleTextBox.Enabled = false;
                authorLabel.Enabled = false;
                authorTextBox.Enabled = false;
                subjectLabel.Enabled = false;
                subjectTextBox.Enabled = false;
                keywordsLabel.Enabled = false;
                keywordsTextBox.Enabled = false;
                creatorLabel.Enabled = false;
                creatorTextBox.Enabled = false;
                producerLabel.Enabled = false;
                producerTextBox.Enabled = false;
                createdLabel.Enabled = false;
                createdTextBox.Enabled = false;
                modifiedLabel.Enabled = false;
                modifiedTextBox.Enabled = false;
            }

            if (PDFManager.DoConversion == false)
                PDFradioButton.Checked = true;
            else if(PDFManager.PreflightProfile==ImGearPDFPreflightProfile.PDFA_1A_2005)
                PDFA1aradioButton.Checked = true;
            else if (PDFManager.PreflightProfile == ImGearPDFPreflightProfile.PDFA_1B_2005)
                PDFA1bradioButton.Checked = true;
        }

       

       

        private void jpeg2KRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (jpeg2KRadioButton.Checked)
            {
                jpeg2KLabel.Enabled = true;
                jpeg2KComboBox.Enabled = true;
            }
            else
            {
                jpeg2KLabel.Enabled = false;
                jpeg2KComboBox.Enabled = false;
            }
        }

        private void jpegRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (jpegRadioButton.Checked)
            {
                jpegLabel.Enabled = true;
                jpegComboBox.Enabled = true;
            }
            else
            {
                jpegLabel.Enabled = false;
                jpegComboBox.Enabled = false;
            }
        }

        private void noneCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            downsampleCheckBox.Enabled = !downsampleCheckBox.Enabled;
            flatteringCheckBox.Enabled = !flatteringCheckBox.Enabled;
            bitonalLabel.Enabled = !bitonalLabel.Enabled;
            bitonalComboBox.Enabled = !bitonalComboBox.Enabled;

            jpeg2KRadioButton.Enabled = !jpeg2KRadioButton.Enabled;
            jpegRadioButton.Enabled = !jpegRadioButton.Enabled;

            if (noneCheckBox.Checked == false)
            {
                if (jpeg2KRadioButton.Checked)
                {
                    jpeg2KLabel.Enabled = true;
                    jpeg2KComboBox.Enabled = true;
                    jpegLabel.Enabled = false;
                    jpegComboBox.Enabled = false;
                }
                else
                {
                    jpeg2KLabel.Enabled = false;
                    jpeg2KComboBox.Enabled = false;
                    jpegLabel.Enabled = true;
                    jpegComboBox.Enabled = true;
                }
            }
            else
            {
                jpeg2KLabel.Enabled = false;
                jpeg2KComboBox.Enabled = false;
                jpegLabel.Enabled = false;
                jpegComboBox.Enabled = false;
            }

        }

        private void dontSaveMetadataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            titleLabel.Enabled = !titleLabel.Enabled;
            titleTextBox.Enabled = !titleTextBox.Enabled;
            authorLabel.Enabled = !authorLabel.Enabled;
            authorTextBox.Enabled = !authorTextBox.Enabled;
            subjectLabel.Enabled = !subjectLabel.Enabled;
            subjectTextBox.Enabled = !subjectTextBox.Enabled;
            keywordsLabel.Enabled = !keywordsLabel.Enabled;
            keywordsTextBox.Enabled = !keywordsTextBox.Enabled;
            creatorLabel.Enabled = !creatorLabel.Enabled;
            creatorTextBox.Enabled = !createdTextBox.Enabled;
            producerLabel.Enabled = !producerLabel.Enabled;
            producerTextBox.Enabled = !producerTextBox.Enabled;
            createdLabel.Enabled = !createdLabel.Enabled;
            createdTextBox.Enabled = !createdTextBox.Enabled;
            modifiedLabel.Enabled = !modifiedLabel.Enabled;
            modifiedTextBox.Enabled = !modifiedTextBox.Enabled;
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            PDFManager.CompressOptions.IsDownsampleImagesEnabled = downsampleCheckBox.Checked;
            PDFManager.CompressOptions.RecompressUsingJpeg2k = jpeg2KRadioButton.Checked;
            PDFManager.CompressOptions.RecompressImageJbig2CompressionLevel = (ImGearJbig2CompressionLevel)bitonalComboBox.SelectedIndex;
            PDFManager.CompressOptions.RecompressImageJpegCompressionLevel = (ImGearJpegCompressionLevel)jpegComboBox.SelectedIndex;
            PDFManager.CompressOptions.RecompressImageJpeg2kCompressionLevel = (ImGearJpeg2kCompressionLevel)jpeg2KComboBox.SelectedIndex;
            PDFManager.CompressOptions.IsFieldFlatteningEnabled = flatteringCheckBox.Checked;

            if (noneCheckBox.Checked)
                PDFManager.DoCompression = false;
            else
                PDFManager.DoCompression = true;

            PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.SetInfo("Title", titleTextBox.Text);
            PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.SetInfo("Author", authorTextBox.Text);
            PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.SetInfo("Subject", subjectTextBox.Text);
            PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.SetInfo("Keywords", keywordsTextBox.Text);
            PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.SetInfo("Creator", creatorTextBox.Text);

            PDFManager.CompressOptions.IsRemoveMetadataEnabled = dontSaveMetadataCheckBox.Checked;
            PDFManager.CompressOptions.IsRemoveImageThumbnailEnabled = dontSaveThumbnailsCheckBox.Checked;

            PDFViewSaveLoad.FileSave(PDFManager.Documents[PDFManager.SelectedTabID].DocumentPath, PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument);

            if (PDFradioButton.Checked)
                PDFManager.DoConversion = false;
            if (PDFA1aradioButton.Checked)
            {
                PDFManager.PreflightProfile = ImGearPDFPreflightProfile.PDFA_1A_2005;
                PDFManager.DoConversion = true;
            }
            if (PDFA1bradioButton.Checked)
            {
                PDFManager.PreflightProfile = ImGearPDFPreflightProfile.PDFA_1B_2005;
                PDFManager.DoConversion = true;
            }
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
