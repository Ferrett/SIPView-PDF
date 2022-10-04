using System.Windows.Forms;

namespace SIPView_PDF
{
    partial class PDFView
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
            this.PageView = new ImageGear.Windows.Forms.ImGearPageView();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ScrollBar = new System.Windows.Forms.VScrollBar();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PageView
            // 
            this.PageView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PageView.BindArrowKeyScrolling = false;
            this.PageView.Display = null;
            this.PageView.HorizontalArrowIncerment = 1;
            this.PageView.Location = new System.Drawing.Point(3, 51);
            this.PageView.Name = "PageView";
            this.PageView.NotifyPageDown = null;
            this.PageView.NotifyPageUp = null;
            this.PageView.Page = null;
            this.PageView.Size = new System.Drawing.Size(792, 444);
            this.PageView.TabIndex = 0;
            this.PageView.UseConfiguredScrollbarIncrements = false;
            this.PageView.VerticalArrowIncerment = 1;
            this.PageView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageView_KeyDown);
            this.PageView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PageView_MouseDown);
            this.PageView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageView_MouseMove);
            this.PageView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PageView_MouseUp);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 495);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(813, 22);
            this.StatusStrip.TabIndex = 1;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // StatusStripLabel
            // 
            this.StatusStripLabel.Name = "StatusStripLabel";
            this.StatusStripLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ScrollBar
            // 
            this.ScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollBar.LargeChange = 1;
            this.ScrollBar.Location = new System.Drawing.Point(796, 51);
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.Size = new System.Drawing.Size(17, 444);
            this.ScrollBar.TabIndex = 2;
            this.ScrollBar.Visible = false;
            this.ScrollBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // PDFView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ScrollBar);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.PageView);
            this.Name = "PDFView";
            this.Size = new System.Drawing.Size(813, 517);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
       
        #endregion

        private ImageGear.Windows.Forms.ImGearPageView PageView;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.VScrollBar ScrollBar;
        private System.Windows.Forms.ToolStripStatusLabel StatusStripLabel;
    }
}
