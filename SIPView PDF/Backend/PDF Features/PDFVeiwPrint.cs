using ImageGear.Display;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SIPView_PDF
{
    public static class PDFVeiwPrint
    {
        public static PrintDocument PrintDocument = new PrintDocument();
        public static PrintDialog PrintDialog = new PrintDialog();
        public static PageSetupDialog PageSetupDialog = new PageSetupDialog();
        public static PageSettings PageSettings = new PageSettings();

        public static void FilePrint()
        {
            PrintDocument.DefaultPageSettings.Landscape = PageSettings.Landscape;
            PrintDocument.DefaultPageSettings.Margins = PageSettings.Margins;
            PrintDocument.DefaultPageSettings.PaperSource = PageSettings.PaperSource;
            PrintDocument.DefaultPageSettings.PaperSize = PageSettings.PaperSize;

            // Initialize a PrintDialog for the user to select a printer.

            PrintDialog.ShowNetwork = true;
            PrintDialog.Document = PrintDocument;

            // Set the page range to 1 page.
            PrintDialog.AllowSomePages = true;
            PrintDialog.PrinterSettings.MinimumPage = 1;
            PrintDialog.PrinterSettings.MaximumPage = PDFViewClass.PDFDocument.Pages.Count;
            PrintDialog.PrinterSettings.FromPage = 1;
            PrintDialog.PrinterSettings.ToPage = PDFViewClass.PDFDocument.Pages.Count;

            if (DialogResult.OK == PrintDialog.ShowDialog())
            {

                PrintDocument.DocumentName = PDFViewClass.DocumentPath;


                // Define a PrintPage event handler and start printing.
                PrintDocument.PrintPage += new PrintPageEventHandler(HandlePrinting);
                for (int j = 0; j < PrintDialog.PrinterSettings.Copies; j++)
                {
                    for (int i = 0; i < PrintDialog.PrinterSettings.ToPage; i++)
                    {
                        PDFViewClass.RenderPage(i);
                        PDFViewClass.UpdatePageView();
                        PrintDocument.Print();
                    }
                }
            }
        }

        static void HandlePrinting(object sender, PrintPageEventArgs args)
        {
            // Clone the current Display for use as a printing display.
            ImGearPageDisplay igPageDisplayPrinting = PDFViewClass.PageView.Display.Clone();
            igPageDisplayPrinting.Page = PDFViewClass.PageView.Display.Page;

            // Get the current Zoom settings and disabled fixed zoom.
            ImGearZoomInfo igZoomInfo = igPageDisplayPrinting.GetZoomInfo(PDFViewClass.PageView);
            igZoomInfo.Horizontal.Fixed = igZoomInfo.Vertical.Fixed = false;
            igPageDisplayPrinting.UpdateZoomFrom(igZoomInfo);

            // Disable any background before printing.
            igPageDisplayPrinting.Background.Mode = ImGearBackgroundModes.NONE;

            // Print to the Graphics device chosen from the PrintDialog.
            igPageDisplayPrinting.Print(args.Graphics);

            // Let the PrintDialog know there are no more pages.
            args.HasMorePages = false;
        }

        public static void ShowPrintMenu()
        {
            if (PageSetupDialog.PageSettings == null)
                PageSettings.Margins = new Margins(4, 4, 4, 4);
            PageSetupDialog.EnableMetric = true;
            PageSetupDialog.PageSettings = PageSettings;
            PageSetupDialog.ShowDialog();
        }
    }
}
