using ImageGear.ART;
using ImageGear.ART.Forms;
using System.Reflection.Emit;

namespace SIPView_PDF
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PDFView = new SIPView_PDF.PDFView();
            this.MenuBar = new SIPView_PDF.MenuBar();
            this.SuspendLayout();
            // 
            // PDFView
            // 
            this.PDFView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PDFView.Location = new System.Drawing.Point(0, 0);
            this.PDFView.Name = "PDFView";
            this.PDFView.Size = new System.Drawing.Size(916, 526);
            this.PDFView.TabIndex = 0;
            // 
            // MenuBar
            // 
            this.MenuBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(916, 53);
            this.MenuBar.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 526);
            this.Controls.Add(this.MenuBar);
            this.Controls.Add(this.PDFView);
            this.Name = "MainForm";
            this.Text = "SIPView PDF";
            //this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private PDFView PDFView;
        private MenuBar MenuBar;
    }
}

