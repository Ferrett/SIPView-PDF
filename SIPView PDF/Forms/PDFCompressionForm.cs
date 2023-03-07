using ImageGear.Formats.PDF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPView_PDF.Forms
{
    public partial class PDFCompressionForm : Form
    {
        public PDFCompressionForm()
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

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
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
            this.Close();
        }
        

    }
}
