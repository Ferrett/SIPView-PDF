﻿using System.Windows.Forms;

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
        private static ToolStripButton SelectAllBtn;
        private static ToolStripButton BakeInBtn;
        private static ToolStripButton ShowToolBarBtn;

        private static ToolStripMenuItem EditMenu;
        private static ToolStripMenuItem FileMenu;
        private static ToolStripMenuItem ToolsMenu;

        private static ToolStripMenuItem RotateLeftMenu;
        private static ToolStripMenuItem RotateRightMenu;
        private static ToolStripMenuItem PrevPageMenu;
        private static ToolStripMenuItem NextPageMenu;
        private static ToolStripMenuItem UndoMenu;
        private static ToolStripMenuItem RedoMenu;
        private static ToolStripMenuItem FilePrintMenu;
        private static ToolStripMenuItem FileSaveMenu;
        private static ToolStripMenuItem SelectAllMenu;
        private static ToolStripMenuItem BakeInMenu;
        private static ToolStripMenuItem ShowToolBarMenu;

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
            SelectAllBtn = (ToolStripButton)toolStrip["SelectAllBtn"];
            BakeInBtn = (ToolStripButton)toolStrip["BakeInBtn"];
            ShowToolBarBtn = (ToolStripButton)toolStrip["ShowToolBarBtn"];

            EditMenu = (ToolStripMenuItem)menuStrip["EditMenu"];
            FileMenu = (ToolStripMenuItem)menuStrip["FileMenu"];
            ToolsMenu = (ToolStripMenuItem)menuStrip["ToolsMenu"];

            FileSaveMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["FileSaveMenu"];
            FilePrintMenu = (ToolStripMenuItem)(menuStrip["FileMenu"] as ToolStripMenuItem).DropDownItems["FilePrintMenu"];
            PrevPageMenu= (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["PrevPageMenu"];
            NextPageMenu = (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["NextPageMenu"];
            RotateLeftMenu = (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["RotateLeftMenu"];
            RotateRightMenu = (ToolStripMenuItem)(menuStrip["ToolsMenu"] as ToolStripMenuItem).DropDownItems["RotateRightMenu"];
            ShowToolBarMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["ShowToolBarMenu"];
            BakeInMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["BakeInMenu"];
            SelectAllMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["SelectAllMenu"];
            UndoMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["UndoMenu"];
            RedoMenu = (ToolStripMenuItem)(menuStrip["EditMenu"] as ToolStripMenuItem).DropDownItems["RedoMenu"];
        }

        public static void DocumentOpened()
        {
            RotateLeftBtn.Enabled = true;
            RotateRightBtn.Enabled = true;
            FileSaveBtn.Enabled = true;
            FilePrintBtn.Enabled = true;
            ShowToolBarBtn.Enabled = true;

            RotateLeftMenu.Enabled = true;
            RotateRightMenu.Enabled = true;
            FileSaveMenu.Enabled = true;
            FilePrintMenu.Enabled = true;
            ShowToolBarMenu.Enabled = true;

            FileSaveMenu.Enabled = true;
            FilePrintMenu.Enabled = true;
            ToolsMenu.Enabled = true;
            EditMenu.Enabled = true;

            if (PDFViewClass.PagesInDocumentCount() > 1)
            {
                NextPageBtn.Enabled = true;
                NextPageMenu.Enabled = true;
            }
        }

        public static void UpdateBakeInBtn()
        {
            BakeInBtn.Enabled = PDFViewClass.SelectedMarksCount() == 0 ? false : true;
        }

        public static void UpdateSelectionBtn()
        {
            SelectAllBtn.Enabled = PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].MarkCount == 0 ? false : true;
        }

        public static void UpdatePageBtns()
        {
            PrevPageBtn.Enabled = PDFViewClass.CurrentPageID == 0 ? false : true;
            NextPageBtn.Enabled = PDFViewClass.CurrentPageID == PDFViewClass.PagesInDocumentCount() ? false : true;
        }

        public static void UpdateHistoryBtns()
        {
            UndoBtn.Enabled = PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].History.UndoCount == 0 ? false : true;
            RedoBtn.Enabled = PDFViewClass.ARTPages[PDFViewClass.CurrentPageID].History.RedoCount == 0 ? false : true;
        }
    }
}