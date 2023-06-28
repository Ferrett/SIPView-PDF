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

            if (PDFViewClass.DoCompression)
            {
                noneCheckBox.Checked = false;
                downsampleCheckBox.Checked = PDFViewClass.CompressOptions.IsDownsampleImagesEnabled;
                jpeg2KRadioButton.Checked = PDFViewClass.CompressOptions.RecompressUsingJpeg2k;
                bitonalComboBox.SelectedIndex = (int)PDFViewClass.CompressOptions.RecompressImageJbig2CompressionLevel;
                jpegComboBox.SelectedIndex = (int)PDFViewClass.CompressOptions.RecompressImageJpegCompressionLevel;
                jpeg2KComboBox.SelectedIndex = (int)PDFViewClass.CompressOptions.RecompressImageJpeg2kCompressionLevel;
                flatteringCheckBox.Checked = PDFViewClass.CompressOptions.IsFieldFlatteningEnabled;
            }
            else
            {
                bitonalComboBox.SelectedIndex = 0;
                jpegComboBox.SelectedIndex = 0;
                jpeg2KComboBox.SelectedIndex = 0;
            }

            titleTextBox.Text = PDFViewClass.PDFDocument.GetInfo("Title");
            subjectTextBox.Text = PDFViewClass.PDFDocument.GetInfo("Subject");
            creatorTextBox.Text = PDFViewClass.PDFDocument.GetInfo("Creator");
            createdTextBox.Text = File.GetCreationTime(PDFViewClass.DocumentPath).ToString();
            authorTextBox.Text = PDFViewClass.PDFDocument.GetInfo("Author");
            keywordsTextBox.Text = PDFViewClass.PDFDocument.GetInfo("Keywords");
            producerTextBox.Text = PDFViewClass.PDFDocument.GetInfo("Producer");
            modifiedTextBox.Text = File.GetLastWriteTime(PDFViewClass.DocumentPath).ToString();

            dontSaveMetadataCheckBox.Checked = PDFViewClass.CompressOptions.IsRemoveMetadataEnabled;
            dontSaveThumbnailsCheckBox.Checked = PDFViewClass.CompressOptions.IsRemoveImageThumbnailEnabled;

            if (PDFViewClass.CompressOptions.IsRemoveMetadataEnabled)
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

            if (PDFViewClass.DoConversion == false)
                PDFradioButton.Checked = true;
            else if(PDFViewClass.PreflightProfile==ImGearPDFPreflightProfile.PDFA_1A_2005)
                PDFA1aradioButton.Checked = true;
            else if (PDFViewClass.PreflightProfile == ImGearPDFPreflightProfile.PDFA_1B_2005)
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
            PDFViewClass.CompressOptions.IsDownsampleImagesEnabled = downsampleCheckBox.Checked;
            PDFViewClass.CompressOptions.RecompressUsingJpeg2k = jpeg2KRadioButton.Checked;
            PDFViewClass.CompressOptions.RecompressImageJbig2CompressionLevel = (ImGearJbig2CompressionLevel)bitonalComboBox.SelectedIndex;
            PDFViewClass.CompressOptions.RecompressImageJpegCompressionLevel = (ImGearJpegCompressionLevel)jpegComboBox.SelectedIndex;
            PDFViewClass.CompressOptions.RecompressImageJpeg2kCompressionLevel = (ImGearJpeg2kCompressionLevel)jpeg2KComboBox.SelectedIndex;
            PDFViewClass.CompressOptions.IsFieldFlatteningEnabled = flatteringCheckBox.Checked;

            if (noneCheckBox.Checked)
                PDFViewClass.DoCompression = false;
            else
                PDFViewClass.DoCompression = true;

            PDFViewClass.PDFDocument.SetInfo("Title", titleTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Author", authorTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Subject", subjectTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Keywords", keywordsTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Creator", creatorTextBox.Text);

            PDFViewClass.CompressOptions.IsRemoveMetadataEnabled = dontSaveMetadataCheckBox.Checked;
            PDFViewClass.CompressOptions.IsRemoveImageThumbnailEnabled = dontSaveThumbnailsCheckBox.Checked;

            PDFViewClass.FileSave(PDFViewClass.DocumentPath, PDFViewClass.PDFDocument);

            if (PDFradioButton.Checked)
                PDFViewClass.DoConversion = false;
            if (PDFA1aradioButton.Checked)
            {
                PDFViewClass.PreflightProfile = ImGearPDFPreflightProfile.PDFA_1A_2005;
                PDFViewClass.DoConversion = true;
            }
            if (PDFA1bradioButton.Checked)
            {
                PDFViewClass.PreflightProfile = ImGearPDFPreflightProfile.PDFA_1B_2005;
                PDFViewClass.DoConversion = true;
            }
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
