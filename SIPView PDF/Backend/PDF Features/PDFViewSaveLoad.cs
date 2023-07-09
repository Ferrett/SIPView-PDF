using ImageGear.Core;
using ImageGear.Formats.PDF;
using ImageGear.Formats;
using SIPView_PDF.Backend.PDF_Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPView_PDF
{
    public static class PDFViewSaveLoad
    {
        public static void FileSave(string fileName, ImGearPDFDocument document)
        {
            // Save to output file.
            using (FileStream outputStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                try
                {
                    if (PDFManager.DoConversion)
                        SavePdfDocumentAsPDFA(document, PDFManager.PreflightProfile, outputStream);

                    if (PDFManager.DoCompression)
                        document.SaveCompressed(outputStream, PDFManager.CompressOptions);
                    else
                        document.Save(outputStream, ImGearSavingFormats.PDF, 0, 0, document.Pages.Count, ImGearSavingModes.OVERWRITE);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Could not save file {0}. {1}", fileName, ex.Message));
                    return;
                }
            }
        }

        private static void SavePdfDocumentAsPDFA(ImGearPDFDocument document, ImGearPDFPreflightProfile profile, FileStream outputStream)
        {
            // Create object-converter. 
            using (ImGearPDFPreflight preflight = new ImGearPDFPreflight(document))
            {
                ImGearPDFPreflightReport report = preflight.VerifyCompliance(profile, 0, document.Pages.Count);

                if (report.Code != ImGearPDFPreflightReportCodes.SUCCESS)
                {
                    if (report.Status == ImGearPDFPreflightStatusCode.Fixable)
                    {
                        // Create conversion options. We need to be sure the "ig_rgb_profile.icm" color profile
                        // is placed in executable file directory of this application.
                        ImGearPDFPreflightConvertOptions conversionOptions = new ImGearPDFPreflightConvertOptions(profile, 0, document.Pages.Count);
                        // Perform the conversion.
                        report = preflight.Convert(conversionOptions);
                        if (report.Code == ImGearPDFPreflightReportCodes.SUCCESS)
                        {
                            return;
                        }
                        else
                            MessageBox.Show("PDF document cannot be converted to APDF standard.");
                    }
                    else
                        MessageBox.Show("PDF document cannot be converted to APDF standard.");
                }
            }
        }

        public static void FileSave()
        {
            using (SaveFileDialog fileDialogSave = new SaveFileDialog())
            {
                fileDialogSave.Filter = ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
                ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);
                fileDialogSave.OverwritePrompt = true;

                if (DialogResult.OK == fileDialogSave.ShowDialog())
                {
                    FileSave(fileDialogSave.FileName, PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument);
                }
            }
        }



        public static void FileLoad()
        {
            OpenFileDialog openFileDialogLoad = new OpenFileDialog();
            // Allow only PDF and PS files.
            openFileDialogLoad.Filter =
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PDF) + "|" +
            ImGearFileFormats.GetSavingFilter(ImGearSavingFormats.PS);

            if (DialogResult.OK == openFileDialogLoad.ShowDialog())
            {
                FileLoad(openFileDialogLoad.FileName);
            }
        }

        public static void FileLoad(string fileName)
        {
            try
            {
                using (FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    PDFManager.AddPageView();

                    // Load the entire the document.
                    PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument = (ImGearPDFDocument)ImGearFileFormats.LoadDocument(inputStream);
                    PDFViewOCR.PDFWordFinder = new ImGearPDFWordFinder(PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument, ImGearPDFWordFinderVersion.LATEST_VERSION, ImGearPDFContextFlags.XY_ORDER);

                    PDFManager.Documents[PDFManager.SelectedTabID].DocumentPath = fileName;
                    PDFManager.Documents[PDFManager.SelectedTabID].ARTPages.Clear();

                    PDFManager.Documents[PDFManager.SelectedTabID].InitializeScrollBar();
                    PDFViewAnnotations.InitializeArtPages();
                    //InitializeThumbnails();

                    PDFManager.OnDocumentChanged(null);
                    PDFManager.Documents[PDFManager.SelectedTabID].RenderPage(0);

                    PDFManager.AddTab();
                }
            }
            catch (ImGearException ex)
            {
                // Perform error handling.
                MessageBox.Show(ex.Message);
            }
        }
    }
}
