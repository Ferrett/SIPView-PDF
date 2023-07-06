using System.Windows.Forms;

namespace SIPView_PDF
{
    partial class PDFView
    {

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
            this.Pan = new ImageGear.Windows.Forms.ImGearPan(this.components);
            this.TabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // Pan
            // 
            this.Pan.AdjustNavigationRectangleByImage = false;
            this.Pan.DestinationView = null;
            this.Pan.DestinationViewCursor = System.Windows.Forms.Cursors.Hand;
            this.Pan.LineColor = System.Drawing.Color.LightGreen;
            this.Pan.LineThickness = 1F;
            this.Pan.PanButton = System.Windows.Forms.MouseButtons.Middle;
            this.Pan.SourceView = null;
            this.Pan.SourceViewCursor = System.Windows.Forms.Cursors.SizeAll;
            // 
            // TabControl
            // 
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Margin = new System.Windows.Forms.Padding(0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1000, 542);
            this.TabControl.TabIndex = 4;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            this.TabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TabControl_MouseClick);
            // 
            // PDFView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabControl);
            this.DoubleBuffered = true;
            this.Name = "PDFView";
            this.Size = new System.Drawing.Size(1000, 542);
            this.ResumeLayout(false);

        }
       
        #endregion
        public ImageGear.Windows.Forms.ImGearPan Pan;
        public TabControl TabControl;
        private System.ComponentModel.IContainer components;
    }
}
