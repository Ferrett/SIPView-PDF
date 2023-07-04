
using SIPView_PDF.Backend.PDF_Features;
using System;
using System.Windows.Forms;


namespace SIPView_PDF
{
    public static class MenuBarClass
    {
        private static ToolStripButton RotateLeftBtn;
        private static ToolStripButton RotateRightBtn;
        private static ToolStripButton PrevPageBtn;
        private static ToolStripButton NextPageBtn;
        private static ToolStripButton UndoBtn;
        private static ToolStripButton RedoBtn;
        private static ToolStripButton FilePrintBtn;
        private static ToolStripButton FileSaveBtn;
        private static ToolStripButton FileOpenBtn;
        private static ToolStripButton SelectAllBtn;
        private static ToolStripButton BakeInBtn;
        private static ToolStripButton ShowToolBarBtn;
        private static ToolStripButton TextSelectionBtn;

        private static ToolStripMenuItem FileMenu;
        private static ToolStripMenuItem ToolsMenu;
        private static ToolStripMenuItem EditMenu;
        private static ToolStripMenuItem BatchProcessesMenu;

        private static ToolStripMenuItem FileOpenMenu;
        private static ToolStripMenuItem FileSaveMenu;
        private static ToolStripMenuItem FilePrintMenu;
        private static ToolStripMenuItem PrintSettingsMenu;
        private static ToolStripMenuItem PDFSettingsMenu;
        private static ToolStripMenuItem CloseTabMenu;

        private static ToolStripMenuItem PrevPageMenu;
        private static ToolStripMenuItem NextPageMenu;
        private static ToolStripMenuItem RotateLeftMenu;
        private static ToolStripMenuItem RotateRightMenu;

        private static ToolStripMenuItem UndoMenu;
        private static ToolStripMenuItem RedoMenu;
        private static ToolStripMenuItem SelectAllMenu;
        private static ToolStripMenuItem BakeInMenu;
        private static ToolStripMenuItem ShowToolBarMenu;
        private static ToolStripMenuItem TextSelectionMenu;

        public static void InitializeButtons(ToolStripItemCollection toolStrip, ToolStripItemCollection menuStrip)
        {
            RotateLeftBtn = (ToolStripButton)toolStrip["RotateLeftBtn"];
            RotateRightBtn = (ToolStripButton)toolStrip["RotateRightBtn"];
            PrevPageBtn = (ToolStripButton)toolStrip["PrevPageBtn"];
            NextPageBtn = (ToolStripButton)toolStrip["NextPageBtn"];
            UndoBtn = (ToolStripButton)toolStrip["UndoBtn"];
            RedoBtn = (ToolStripButton)toolStrip["RedoBtn"];
            FilePrintBtn = (ToolStripButton)toolStrip["FilePrintBtn"];
            FileSaveBtn = (ToolStripButton)toolStrip["FileSaveBtn"];
            FileOpenBtn = (ToolStripButton)toolStrip["FileOpenBtn"];
            SelectAllBtn = (ToolStripButton)toolStrip["SelectAllBtn"];
            BakeInBtn = (ToolStripButton)toolStrip["BakeInBtn"];
            ShowToolBarBtn = (ToolStripButton)toolStrip["ShowToolBarBtn"];
            TextSelectionBtn = (ToolStripButton)toolStrip["TextSelectionBtn"];

            FileMenu = (ToolStripMenuItem)menuStrip["FileMenu"];
            EditMenu = (ToolStripMenuItem)menuStrip["EditMenu"];
            ToolsMenu = (ToolStripMenuItem)menuStrip["ToolsMenu"];
            BatchProcessesMenu = (ToolStripMenuItem)menuStrip["BatchProcessesMenu"];

            FileOpenMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["FileOpenMenu"];
            FileSaveMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["FileSaveMenu"];
            FilePrintMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["FilePrintMenu"];
            PrintSettingsMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["PrintSettingsMenu"];
            PDFSettingsMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["PDFSettingsMenu"];
            CloseTabMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["CloseTabMenu"];

            PrevPageMenu = (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["PrevPageMenu"];
            NextPageMenu = (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["NextPageMenu"];
            RotateLeftMenu = (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["RotateLeftMenu"];
            RotateRightMenu = (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["RotateRightMenu"];

            UndoMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["UndoMenu"];
            RedoMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["RedoMenu"];
            BakeInMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["BakeInMenu"];
            SelectAllMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["SelectAllMenu"];
            ShowToolBarMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["ShowToolBarMenu"];
            TextSelectionMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["TextSelectionMenu"];
        }

        public static void PageChanged()
        {
            UpdateBakeInBtn();
            UpdateHistoryBtns();
            UpdateSelectionBtn();
            UpdatePageBtns();

            UpdateTextSelectionBtn();

        }

        public static void TabChanged()
        {
            if(PDFManager.Documents.Count  > 0)
            {

            }
            
        }

        public static void DocumentOpened()
        {
            RotateLeftBtn.Enabled = true;
            RotateRightBtn.Enabled = true;
            FilePrintBtn.Enabled = true;
            ShowToolBarBtn.Enabled = true;

            RotateLeftMenu.Enabled = true;
            RotateRightMenu.Enabled = true;
            FileSaveMenu.Enabled = true;
            FilePrintMenu.Enabled = true;

            ShowToolBarMenu.Enabled = true;

            FileSaveMenu.Enabled = true;
            FilePrintMenu.Enabled = true;

            PDFSettingsMenu.Enabled = true;
            ToolsMenu.Enabled = true;
            EditMenu.Enabled = true;
        }

        public static void UpdateTextSelectionBtn()
        {
            TextSelectionBtn.Enabled = PDFViewOCR.WordsInPageCount(PDFManager.Documents[PDFManager.SelectedTabID].PageID) > 0 ? true : false;
        }

        public static void UpdateBakeInBtn()
        {
            BakeInBtn.Enabled = PDFViewAnnotations.SelectedMarksCount() == 0 ? false : true;
        }

        public static void UpdateSelectionBtn()
        {
            SelectAllBtn.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].MarkCount == 0 ? false : true;
        }

        public static void UpdatePageBtns()
        {
            PrevPageBtn.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].PageID == 0 ? false : true;
            PrevPageMenu.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].PageID == 0 ? false : true;

            NextPageBtn.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].PageID == PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages.Count-1 ? false : true;
            NextPageMenu.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].PageID == PDFManager.Documents[PDFManager.SelectedTabID].PDFDocument.Pages.Count-1 ? false : true;
        }

        public static void UpdateHistoryBtns()
        {
            UndoBtn.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].History.UndoCount == 0 ? false : true;
            RedoBtn.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].History.RedoCount == 0 ? false : true;

            UndoMenu.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].History.UndoCount == 0 ? false : true;
            RedoMenu.Enabled = PDFManager.Documents[PDFManager.SelectedTabID].ARTPages[PDFManager.Documents[PDFManager.SelectedTabID].PageID].History.RedoCount == 0 ? false : true;
        }


        public static void ToolBarChangeCheck()
        {
            ShowToolBarBtn.Checked = !ShowToolBarBtn.Checked;

            if(ShowToolBarBtn.Checked ==true)
            {
                
                TextSelectionBtn.Checked = false;
            }
            else
            {

            }
        }

        public static void TextSelectionChangeCheck()
        {
            TextSelectionBtn.Checked = !TextSelectionBtn.Checked;

            if (TextSelectionBtn.Checked == true)
            {
                
                ShowToolBarBtn.Checked = false;
            }
        }

    }
}
