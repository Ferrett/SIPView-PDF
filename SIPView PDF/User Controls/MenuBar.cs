using ImageGear.Formats.PDF.Forms;
using SIPView_PDF.Backend.PDF_Features;
using SIPView_PDF.Forms;
using System;
using System.Threading.Tasks;
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
            PDFViewSaveLoad.FileLoad();
        }

        private void FileSaveBtn_Click(object sender, EventArgs e)
        {
            PDFViewSaveLoad.FileSave();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].ToolBarChangeVisibility();
            MenuBarClass.ToolBarChangeCheck();
        }

        private void BakeInBtn_Click(object sender, EventArgs e)
        {
            PDFViewAnnotations.AnnotationBakeIn();
        }

        private void SelectAllBtn_Click(object sender, EventArgs e)
        {
            PDFViewAnnotations.SelectAllMarks();
        }

        private void RedoBtn_Click(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].Redo();
            MenuBarClass.UpdateHistoryBtns();
        }

        private void UndoBtn_Click(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].Undo();
            MenuBarClass.UpdateHistoryBtns();
        }

        private void PrevPageBtn_Click(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].PrevPage();
            MenuBarClass.UpdatePageBtns();
        }

        private void NextPageBtn_Click(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].NextPage();
            MenuBarClass.UpdatePageBtns();
        }

        private void RotateRightBtn_Click(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].RotateRight();
        }

        private void RotateLeftBtn_Click(object sender, EventArgs e)
        {
            PDFManager.Documents[PDFManager.SelectedTabID].RotateLeft();
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
            PDFVeiwPrint.FilePrint();
        }

        private void PrintSettingsMenu_Click(object sender, EventArgs e)
        {
            PDFVeiwPrint.ShowPageSetupMenu();
        }

        private void pDFSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PDFSettingsForm settingsForm = new PDFSettingsForm();
            settingsForm.ShowDialog();
        }

        private void TextSelectionBtn_Click(object sender, EventArgs e)
        {
            PDFViewTextSelecting.TextSelectionModeChange();
            MenuBarClass.TextSelectionChangeCheck();
        }

        private void CloseTabMenu_Click(object sender, EventArgs e)
        {
            PDFManager.CloseTab(PDFManager.SelectedTabID);
        }
    }
}
