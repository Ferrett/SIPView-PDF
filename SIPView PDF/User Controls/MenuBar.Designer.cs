namespace SIPView_PDF
{
    partial class MenuBar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOpenMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSaveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FilePrintMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PrevPageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.NextPageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RotateRightMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RotateLeftMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectAllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.BakeInMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowToolBarMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.BatchProcessesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AllFilesToPDFsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AllFilesToSinglePDFMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitMultipagePDFsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.RotateLeftBtn = new System.Windows.Forms.ToolStripButton();
            this.RotateRightBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PrevPageBtn = new System.Windows.Forms.ToolStripButton();
            this.NextPageBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.UndoBtn = new System.Windows.Forms.ToolStripButton();
            this.RedoBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.FilePrintBtn = new System.Windows.Forms.ToolStripButton();
            this.FileSaveBtn = new System.Windows.Forms.ToolStripButton();
            this.FileOpenBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAllBtn = new System.Windows.Forms.ToolStripButton();
            this.BakeInBtn = new System.Windows.Forms.ToolStripButton();
            this.ShowToolBarBtn = new System.Windows.Forms.ToolStripButton();
            this.MagnifierBtn = new System.Windows.Forms.ToolStripButton();
            this.AddImageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.ToolsMenu,
            this.EditMenu,
            this.BatchProcessesMenu});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(418, 24);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileOpenMenu,
            this.FileSaveMenu,
            this.FilePrintMenu,
            this.AddImageMenu});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // FileOpenMenu
            // 
            this.FileOpenMenu.Name = "FileOpenMenu";
            this.FileOpenMenu.Size = new System.Drawing.Size(180, 22);
            this.FileOpenMenu.Text = "Open";
            this.FileOpenMenu.Click += new System.EventHandler(this.FileOpenMenu_Click);
            // 
            // FileSaveMenu
            // 
            this.FileSaveMenu.Enabled = false;
            this.FileSaveMenu.Name = "FileSaveMenu";
            this.FileSaveMenu.Size = new System.Drawing.Size(180, 22);
            this.FileSaveMenu.Text = "Save";
            this.FileSaveMenu.Click += new System.EventHandler(this.FileSaveMenu_Click);
            // 
            // FilePrintMenu
            // 
            this.FilePrintMenu.Enabled = false;
            this.FilePrintMenu.Name = "FilePrintMenu";
            this.FilePrintMenu.Size = new System.Drawing.Size(180, 22);
            this.FilePrintMenu.Text = "Print";
            this.FilePrintMenu.Click += new System.EventHandler(this.FilePrintMenu_Click);
            // 
            // ToolsMenu
            // 
            this.ToolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrevPageMenu,
            this.NextPageMenu,
            this.RotateRightMenu,
            this.RotateLeftMenu});
            this.ToolsMenu.Enabled = false;
            this.ToolsMenu.Name = "ToolsMenu";
            this.ToolsMenu.Size = new System.Drawing.Size(46, 20);
            this.ToolsMenu.Text = "Tools";
            // 
            // PrevPageMenu
            // 
            this.PrevPageMenu.Enabled = false;
            this.PrevPageMenu.Name = "PrevPageMenu";
            this.PrevPageMenu.Size = new System.Drawing.Size(180, 22);
            this.PrevPageMenu.Text = "Previous page";
            this.PrevPageMenu.Click += new System.EventHandler(this.PrevPageMenu_Click);
            // 
            // NextPageMenu
            // 
            this.NextPageMenu.Enabled = false;
            this.NextPageMenu.Name = "NextPageMenu";
            this.NextPageMenu.Size = new System.Drawing.Size(180, 22);
            this.NextPageMenu.Text = "Next page";
            this.NextPageMenu.Click += new System.EventHandler(this.NextPageMenu_Click);
            // 
            // RotateRightMenu
            // 
            this.RotateRightMenu.Enabled = false;
            this.RotateRightMenu.Name = "RotateRightMenu";
            this.RotateRightMenu.Size = new System.Drawing.Size(180, 22);
            this.RotateRightMenu.Text = "Rotate right";
            this.RotateRightMenu.Click += new System.EventHandler(this.RotateRightMenu_Click);
            // 
            // RotateLeftMenu
            // 
            this.RotateLeftMenu.Enabled = false;
            this.RotateLeftMenu.Name = "RotateLeftMenu";
            this.RotateLeftMenu.Size = new System.Drawing.Size(180, 22);
            this.RotateLeftMenu.Text = "Rotate left";
            this.RotateLeftMenu.Click += new System.EventHandler(this.RotateLeftMenu_Click);
            // 
            // EditMenu
            // 
            this.EditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoMenu,
            this.RedoMenu,
            this.SelectAllMenu,
            this.BakeInMenu,
            this.ShowToolBarMenu});
            this.EditMenu.Enabled = false;
            this.EditMenu.Name = "EditMenu";
            this.EditMenu.Size = new System.Drawing.Size(39, 20);
            this.EditMenu.Text = "Edit";
            // 
            // UndoMenu
            // 
            this.UndoMenu.Enabled = false;
            this.UndoMenu.Name = "UndoMenu";
            this.UndoMenu.Size = new System.Drawing.Size(180, 22);
            this.UndoMenu.Text = "Undo";
            this.UndoMenu.Click += new System.EventHandler(this.UndoMenu_Click);
            // 
            // RedoMenu
            // 
            this.RedoMenu.Enabled = false;
            this.RedoMenu.Name = "RedoMenu";
            this.RedoMenu.Size = new System.Drawing.Size(180, 22);
            this.RedoMenu.Text = "Redo";
            this.RedoMenu.Click += new System.EventHandler(this.RedoMenu_Click);
            // 
            // SelectAllMenu
            // 
            this.SelectAllMenu.Enabled = false;
            this.SelectAllMenu.Name = "SelectAllMenu";
            this.SelectAllMenu.Size = new System.Drawing.Size(180, 22);
            this.SelectAllMenu.Text = "Select all";
            this.SelectAllMenu.Click += new System.EventHandler(this.SelectAllMenu_Click);
            // 
            // BakeInMenu
            // 
            this.BakeInMenu.Enabled = false;
            this.BakeInMenu.Name = "BakeInMenu";
            this.BakeInMenu.Size = new System.Drawing.Size(180, 22);
            this.BakeInMenu.Text = "Bake in";
            this.BakeInMenu.Click += new System.EventHandler(this.BakeInMenu_Click);
            // 
            // ShowToolBarMenu
            // 
            this.ShowToolBarMenu.Enabled = false;
            this.ShowToolBarMenu.Name = "ShowToolBarMenu";
            this.ShowToolBarMenu.Size = new System.Drawing.Size(180, 22);
            this.ShowToolBarMenu.Text = "Show toolbar";
            this.ShowToolBarMenu.Click += new System.EventHandler(this.ShowToolBarMenu_Click);
            // 
            // BatchProcessesMenu
            // 
            this.BatchProcessesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllFilesToPDFsMenu,
            this.AllFilesToSinglePDFMenu,
            this.SplitMultipagePDFsMenu});
            this.BatchProcessesMenu.Name = "BatchProcessesMenu";
            this.BatchProcessesMenu.Size = new System.Drawing.Size(103, 20);
            this.BatchProcessesMenu.Text = "Batch Processes";
            // 
            // AllFilesToPDFsMenu
            // 
            this.AllFilesToPDFsMenu.Name = "AllFilesToPDFsMenu";
            this.AllFilesToPDFsMenu.Size = new System.Drawing.Size(184, 22);
            this.AllFilesToPDFsMenu.Text = "All files to PDFs";
            this.AllFilesToPDFsMenu.Click += new System.EventHandler(this.AllFilesToPDFsMenu_Click);
            // 
            // AllFilesToSinglePDFMenu
            // 
            this.AllFilesToSinglePDFMenu.Name = "AllFilesToSinglePDFMenu";
            this.AllFilesToSinglePDFMenu.Size = new System.Drawing.Size(184, 22);
            this.AllFilesToSinglePDFMenu.Text = "All files to single PDF";
            this.AllFilesToSinglePDFMenu.Click += new System.EventHandler(this.AllFilesToSinglePDFMenu_Click);
            // 
            // SplitMultipagePDFsMenu
            // 
            this.SplitMultipagePDFsMenu.Name = "SplitMultipagePDFsMenu";
            this.SplitMultipagePDFsMenu.Size = new System.Drawing.Size(184, 22);
            this.SplitMultipagePDFsMenu.Text = "Split multipage PDFs";
            this.SplitMultipagePDFsMenu.Click += new System.EventHandler(this.SplitMultipagePDFsMenu_Click);
            // 
            // ToolStrip
            // 
            this.ToolStrip.AllowMerge = false;
            this.ToolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.ToolStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RotateLeftBtn,
            this.RotateRightBtn,
            this.toolStripSeparator1,
            this.PrevPageBtn,
            this.NextPageBtn,
            this.toolStripSeparator2,
            this.UndoBtn,
            this.RedoBtn,
            this.toolStripSeparator3,
            this.FilePrintBtn,
            this.FileSaveBtn,
            this.FileOpenBtn,
            this.toolStripSeparator4,
            this.SelectAllBtn,
            this.BakeInBtn,
            this.ShowToolBarBtn,
            this.MagnifierBtn});
            this.ToolStrip.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip.Margin = new System.Windows.Forms.Padding(20);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.ToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ToolStrip.Size = new System.Drawing.Size(418, 25);
            this.ToolStrip.TabIndex = 9;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // RotateLeftBtn
            // 
            this.RotateLeftBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RotateLeftBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RotateLeftBtn.Enabled = false;
            this.RotateLeftBtn.Image = global::SIPView_PDF.Properties.Resources.rotate_left;
            this.RotateLeftBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RotateLeftBtn.Name = "RotateLeftBtn";
            this.RotateLeftBtn.Size = new System.Drawing.Size(23, 22);
            this.RotateLeftBtn.Text = "toolStripButton1";
            this.RotateLeftBtn.ToolTipText = "Rotate left";
            this.RotateLeftBtn.Click += new System.EventHandler(this.RotateLeftBtn_Click);
            // 
            // RotateRightBtn
            // 
            this.RotateRightBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RotateRightBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RotateRightBtn.Enabled = false;
            this.RotateRightBtn.Image = global::SIPView_PDF.Properties.Resources.rotate_right;
            this.RotateRightBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RotateRightBtn.Name = "RotateRightBtn";
            this.RotateRightBtn.Size = new System.Drawing.Size(23, 22);
            this.RotateRightBtn.Text = "toolStripButton2";
            this.RotateRightBtn.ToolTipText = "Rotate right";
            this.RotateRightBtn.Click += new System.EventHandler(this.RotateRightBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // PrevPageBtn
            // 
            this.PrevPageBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PrevPageBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PrevPageBtn.Enabled = false;
            this.PrevPageBtn.Image = global::SIPView_PDF.Properties.Resources.prev_page;
            this.PrevPageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrevPageBtn.Name = "PrevPageBtn";
            this.PrevPageBtn.Size = new System.Drawing.Size(23, 22);
            this.PrevPageBtn.Text = "toolStripButton3";
            this.PrevPageBtn.ToolTipText = "Previous page";
            this.PrevPageBtn.Click += new System.EventHandler(this.PrevPageBtn_Click);
            // 
            // NextPageBtn
            // 
            this.NextPageBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.NextPageBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NextPageBtn.Enabled = false;
            this.NextPageBtn.Image = global::SIPView_PDF.Properties.Resources.next_page;
            this.NextPageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextPageBtn.Name = "NextPageBtn";
            this.NextPageBtn.Size = new System.Drawing.Size(23, 22);
            this.NextPageBtn.Text = "toolStripButton4";
            this.NextPageBtn.ToolTipText = "Next page";
            this.NextPageBtn.Click += new System.EventHandler(this.NextPageBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // UndoBtn
            // 
            this.UndoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UndoBtn.Enabled = false;
            this.UndoBtn.Image = global::SIPView_PDF.Properties.Resources.undo;
            this.UndoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UndoBtn.Name = "UndoBtn";
            this.UndoBtn.Size = new System.Drawing.Size(23, 22);
            this.UndoBtn.Text = "toolStripButton5";
            this.UndoBtn.ToolTipText = "Undo";
            this.UndoBtn.Click += new System.EventHandler(this.UndoBtn_Click);
            // 
            // RedoBtn
            // 
            this.RedoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RedoBtn.Enabled = false;
            this.RedoBtn.Image = global::SIPView_PDF.Properties.Resources.redo;
            this.RedoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RedoBtn.Name = "RedoBtn";
            this.RedoBtn.Size = new System.Drawing.Size(23, 22);
            this.RedoBtn.Text = "toolStripButton6";
            this.RedoBtn.ToolTipText = "Redo";
            this.RedoBtn.Click += new System.EventHandler(this.RedoBtn_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // FilePrintBtn
            // 
            this.FilePrintBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FilePrintBtn.Enabled = false;
            this.FilePrintBtn.Image = global::SIPView_PDF.Properties.Resources.print;
            this.FilePrintBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FilePrintBtn.Name = "FilePrintBtn";
            this.FilePrintBtn.Size = new System.Drawing.Size(23, 22);
            this.FilePrintBtn.Text = "toolStripButton1";
            this.FilePrintBtn.ToolTipText = "Print";
            this.FilePrintBtn.Click += new System.EventHandler(this.FilePrintBtn_Click);
            // 
            // FileSaveBtn
            // 
            this.FileSaveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FileSaveBtn.Enabled = false;
            this.FileSaveBtn.Image = global::SIPView_PDF.Properties.Resources.save;
            this.FileSaveBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileSaveBtn.Name = "FileSaveBtn";
            this.FileSaveBtn.Size = new System.Drawing.Size(23, 22);
            this.FileSaveBtn.Text = "toolStripButton9";
            this.FileSaveBtn.ToolTipText = "Save";
            this.FileSaveBtn.Click += new System.EventHandler(this.FileSaveBtn_Click);
            // 
            // FileOpenBtn
            // 
            this.FileOpenBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FileOpenBtn.Image = global::SIPView_PDF.Properties.Resources.open;
            this.FileOpenBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileOpenBtn.Name = "FileOpenBtn";
            this.FileOpenBtn.Size = new System.Drawing.Size(23, 22);
            this.FileOpenBtn.ToolTipText = "Open";
            this.FileOpenBtn.Click += new System.EventHandler(this.FileOpenBtn_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // SelectAllBtn
            // 
            this.SelectAllBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectAllBtn.Enabled = false;
            this.SelectAllBtn.Image = global::SIPView_PDF.Properties.Resources.select_all;
            this.SelectAllBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectAllBtn.Name = "SelectAllBtn";
            this.SelectAllBtn.Size = new System.Drawing.Size(23, 22);
            this.SelectAllBtn.Text = "toolStripButton7";
            this.SelectAllBtn.ToolTipText = "Select all";
            this.SelectAllBtn.Click += new System.EventHandler(this.SelectAllBtn_Click);
            // 
            // BakeInBtn
            // 
            this.BakeInBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BakeInBtn.Enabled = false;
            this.BakeInBtn.Image = global::SIPView_PDF.Properties.Resources.bake;
            this.BakeInBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BakeInBtn.Name = "BakeInBtn";
            this.BakeInBtn.Size = new System.Drawing.Size(23, 22);
            this.BakeInBtn.Text = "toolStripButton8";
            this.BakeInBtn.ToolTipText = "Bake in";
            this.BakeInBtn.Click += new System.EventHandler(this.BakeInBtn_Click);
            // 
            // ShowToolBarBtn
            // 
            this.ShowToolBarBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowToolBarBtn.Enabled = false;
            this.ShowToolBarBtn.Image = global::SIPView_PDF.Properties.Resources.edit_mode;
            this.ShowToolBarBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShowToolBarBtn.Name = "ShowToolBarBtn";
            this.ShowToolBarBtn.Size = new System.Drawing.Size(23, 22);
            this.ShowToolBarBtn.Text = "toolStripButton11";
            this.ShowToolBarBtn.ToolTipText = "Edit";
            this.ShowToolBarBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // MagnifierBtn
            // 
            this.MagnifierBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MagnifierBtn.Enabled = false;
            this.MagnifierBtn.Image = global::SIPView_PDF.Properties.Resources.glass;
            this.MagnifierBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MagnifierBtn.Name = "MagnifierBtn";
            this.MagnifierBtn.Size = new System.Drawing.Size(23, 22);
            this.MagnifierBtn.Text = "toolStripButton1";
            this.MagnifierBtn.ToolTipText = "Magnifying glass";
            this.MagnifierBtn.Click += new System.EventHandler(this.MagnifierBtn_Click);
            // 
            // AddImageMenu
            // 
            this.AddImageMenu.Enabled = false;
            this.AddImageMenu.Name = "AddImageMenu";
            this.AddImageMenu.Size = new System.Drawing.Size(180, 22);
            this.AddImageMenu.Text = "Add image";
            this.AddImageMenu.Click += new System.EventHandler(this.AddImageMenu_Click);
            // 
            // MenuBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.MenuStrip);
            this.Name = "MenuBar";
            this.Size = new System.Drawing.Size(418, 60);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem FileOpenMenu;
        private System.Windows.Forms.ToolStripMenuItem FileSaveMenu;
        private System.Windows.Forms.ToolStripMenuItem FilePrintMenu;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenu;
        private System.Windows.Forms.ToolStripMenuItem PrevPageMenu;
        private System.Windows.Forms.ToolStripMenuItem NextPageMenu;
        private System.Windows.Forms.ToolStripMenuItem RotateRightMenu;
        private System.Windows.Forms.ToolStripMenuItem RotateLeftMenu;
        private System.Windows.Forms.ToolStripMenuItem EditMenu;
        private System.Windows.Forms.ToolStripMenuItem ShowToolBarMenu;
        private System.Windows.Forms.ToolStripMenuItem BakeInMenu;
        private System.Windows.Forms.ToolStripMenuItem SelectAllMenu;
        private System.Windows.Forms.ToolStripMenuItem UndoMenu;
        private System.Windows.Forms.ToolStripMenuItem RedoMenu;
        private System.Windows.Forms.ToolStripMenuItem BatchProcessesMenu;
        private System.Windows.Forms.ToolStripMenuItem AllFilesToPDFsMenu;
        private System.Windows.Forms.ToolStripMenuItem AllFilesToSinglePDFMenu;
        private System.Windows.Forms.ToolStripMenuItem SplitMultipagePDFsMenu;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripButton RotateLeftBtn;
        private System.Windows.Forms.ToolStripButton RotateRightBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton PrevPageBtn;
        private System.Windows.Forms.ToolStripButton NextPageBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton UndoBtn;
        private System.Windows.Forms.ToolStripButton RedoBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton SelectAllBtn;
        private System.Windows.Forms.ToolStripButton BakeInBtn;
        private System.Windows.Forms.ToolStripButton FileSaveBtn;
        private System.Windows.Forms.ToolStripButton FileOpenBtn;
        private System.Windows.Forms.ToolStripButton ShowToolBarBtn;
        private System.Windows.Forms.ToolStripButton FilePrintBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton MagnifierBtn;
        private System.Windows.Forms.ToolStripMenuItem AddImageMenu;
    }
}
