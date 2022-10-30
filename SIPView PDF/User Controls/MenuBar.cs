using SIPView_PDF.Forms;
using System;
using System.Windows.Forms;

namespace SIPView_PDF
{
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
            MenuBarClass.InitializeButtons(ToolStrip.Items, MenuStrip.Items);
        }

        private void FileOpenBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.FileLoad();
        }

        private void FileSaveBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.FileSave();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.ToolBarChangeVisibility();
            MenuBarClass.ToolBarChangeCheck();
        }

        private void BakeInBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.AnnotationBakeIn();
        }

        private void SelectAllBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.SelectAllMarks();
        }

        private void RedoBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.Redo();
            MenuBarClass.UpdateHistoryBtns();
        }

        private void UndoBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.Undo();
            MenuBarClass.UpdateHistoryBtns();
        }

        private void PrevPageBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.PrevPage();
            MenuBarClass.UpdatePageBtns();
        }

        private void NextPageBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.NextPage();
            MenuBarClass.UpdatePageBtns();
        }

        private void RotateRightBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.RotateRight();
        }

        private void RotateLeftBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.RotateLeft();
        }

        private void AllFilesToPDFsMenu_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.ALL_FILES_TO_PDFS);
            batchProcessesForm.Show();
        }

        private void AllFilesToSinglePDFMenu_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.ALL_FILES_TO_SINGLE_PDF);
            batchProcessesForm.Show();
        }

        private void SplitMultipagePDFsMenu_Click(object sender, EventArgs e)
        {
            BatchProcessesForm batchProcessesForm = new BatchProcessesForm(BatchProcess.SPLIT_MULTIPAGE_PDFS);
            batchProcessesForm.Show();
        }

        private void FilePrintBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.FilePrint();
        }

        private void FileOpenMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.FileLoad();
        }

        private void FileSaveMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.FileSave();
        }

        private void FilePrintMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.FilePrint();
        }

        private void PrevPageMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.PrevPage();
        }

        private void NextPageMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.NextPage();
        }

        private void RotateRightMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.RotateRight();
        }

        private void RotateLeftMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.RotateLeft();
        }

        private void ShowToolBarMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.ToolBarChangeVisibility();
        }

        private void BakeInMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.AnnotationBakeIn();
        }

        private void SelectAllMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.SelectAllMarks();
        }

        private void UndoMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.Undo();
        }

        private void RedoMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.Redo();
        }

        private void MagnifierBtn_Click(object sender, EventArgs e)
        {
            PDFViewClass.MagnifierChangeVisibility();
            MenuBarClass.MagnifierChangeCheck();
        }

        private void AddImageMenu_Click(object sender, EventArgs e)
        {
            AddImagesForm addImagesForm = new AddImagesForm(PDFViewClass.PDFDocument.Pages.Count);
            addImagesForm.ShowDialog();
        }

        private void PrintSettingsMenu_Click(object sender, EventArgs e)
        {
            PDFViewClass.ShowPrintMenu();
        }

        private void PDFSettingsMenu_Click(object sender, EventArgs e)
        {
            PDFCompressionForm compressioForm = new PDFCompressionForm();
            compressioForm.ShowDialog();
            
        }

        private void pDFSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PDFSettingsForm settingsForm = new PDFSettingsForm();
            settingsForm.ShowDialog();
        }
    }
}
