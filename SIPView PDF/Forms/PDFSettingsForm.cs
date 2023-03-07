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

        private void applyBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.PDFDocument.SetInfo("Title", titleTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Author", authorTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Subject", subjectTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Keywords", keywordsTextBox.Text);
            PDFViewClass.PDFDocument.SetInfo("Creator", creatorTextBox.Text);

            PDFViewClass.CompressOptions.IsRemoveMetadataEnabled = dontSaveMetadataCheckBox.Checked;
            PDFViewClass.CompressOptions.IsRemoveImageThumbnailEnabled = dontSaveThumbnailsCheckBox.Checked;

            PDFViewClass.FileSave(PDFViewClass.DocumentPath,PDFViewClass.PDFDocument);

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
        
    }
}
