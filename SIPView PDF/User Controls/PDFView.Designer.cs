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
            this.components = new System.ComponentModel.Container();
            this.PageView = new ImageGear.Windows.Forms.ImGearPageView();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ScrollBar = new System.Windows.Forms.VScrollBar();
            this.Pan = new ImageGear.Windows.Forms.ImGearPan(this.components);
            this.Magnifier = new ImageGear.Windows.Forms.ImGearMagnifier(this.components);
            this.ThumbnailController = new System.Windows.Forms.Panel();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PageView
            // 
            this.PageView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PageView.BackColor = System.Drawing.SystemColors.Window;
            this.PageView.BindArrowKeyScrolling = false;
            this.PageView.Cursor = System.Windows.Forms.Cursors.Default;
            this.PageView.Display = null;
            this.PageView.HorizontalArrowIncerment = 1;
            this.PageView.Location = new System.Drawing.Point(156, 0);
            this.PageView.Name = "PageView";
            this.PageView.NotifyPageDown = null;
            this.PageView.NotifyPageUp = null;
            this.PageView.Page = null;
            this.PageView.Size = new System.Drawing.Size(637, 520);
            this.PageView.TabIndex = 0;
            this.PageView.UseConfiguredScrollbarIncrements = false;
            this.PageView.VerticalArrowIncerment = 1;
            this.PageView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageView_KeyDown);
            this.PageView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PageView_KeyUp);
            this.PageView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PageView_MouseDown);
            this.PageView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageView_MouseMove);
            this.PageView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PageView_MouseUp);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusStripLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 523);
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
            this.ScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.ScrollBar.LargeChange = 1;
            this.ScrollBar.Location = new System.Drawing.Point(796, 0);
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.Size = new System.Drawing.Size(17, 523);
            this.ScrollBar.TabIndex = 2;
            this.ScrollBar.Visible = false;
            this.ScrollBar.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // Pan
            // 
            this.Pan.AdjustNavigationRectangleByImage = false;
            this.Pan.DestinationView = null;
            this.Pan.DestinationViewCursor = System.Windows.Forms.Cursors.Hand;
            this.Pan.LineColor = System.Drawing.Color.LightGreen;
            this.Pan.LineThickness = 1F;
            this.Pan.PanButton = System.Windows.Forms.MouseButtons.Middle;
            this.Pan.SourceView = this.PageView;
            this.Pan.SourceViewCursor = System.Windows.Forms.Cursors.SizeAll;
            // 
            // Magnifier
            // 
            this.Magnifier.Cursor = System.Windows.Forms.Cursors.Default;
            this.Magnifier.DestinationView = null;
            this.Magnifier.IsPopUp = false;
            this.Magnifier.PopupHeight = 150;
            this.Magnifier.PopupWidth = 150;
            this.Magnifier.ShapeType = ImageGear.Windows.Forms.ImGearMagnifierShapeType.Rectangle;
            this.Magnifier.SourceView = this.PageView;
            this.Magnifier.Zoom = 2F;
            // 
            // ThumbnailController
            // 
            this.ThumbnailController.AutoScroll = true;
            this.ThumbnailController.Dock = System.Windows.Forms.DockStyle.Left;
            this.ThumbnailController.Location = new System.Drawing.Point(0, 0);
            this.ThumbnailController.Name = "ThumbnailController";
            this.ThumbnailController.Size = new System.Drawing.Size(157, 523);
            this.ThumbnailController.TabIndex = 3;
            // 
            // PDFView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ThumbnailController);
            this.Controls.Add(this.ScrollBar);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.PageView);
            this.Name = "PDFView";
            this.Size = new System.Drawing.Size(813, 545);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.PDFView_MouseWheel);
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
        private ImageGear.Windows.Forms.ImGearPan Pan;
        private ImageGear.Windows.Forms.ImGearMagnifier Magnifier;
        private Panel ThumbnailController;
    }
}
