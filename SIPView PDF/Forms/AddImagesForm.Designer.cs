namespace SIPView_PDF
{
    partial class AddImagesForm
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
            this.selectImagesLabel = new System.Windows.Forms.Label();
            this.selectImagesPanel = new System.Windows.Forms.Panel();
            this.addImagesBtn = new System.Windows.Forms.Button();
            this.selectImagesListBox = new System.Windows.Forms.ListBox();
            this.exitBtn = new System.Windows.Forms.Button();
            this.applyBtn = new System.Windows.Forms.Button();
            this.posInDocumentLabel = new System.Windows.Forms.Label();
            this.posInDocumentTextBox = new System.Windows.Forms.TextBox();
            this.posInDocumentPanel = new System.Windows.Forms.Panel();
            this.endPos = new System.Windows.Forms.Button();
            this.startPos = new System.Windows.Forms.Button();
            this.clearImagesBtn = new System.Windows.Forms.Button();
            this.removeImagesBtn = new System.Windows.Forms.Button();
            this.selectImagesPanel.SuspendLayout();
            this.posInDocumentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectImagesLabel
            // 
            this.selectImagesLabel.AutoSize = true;
            this.selectImagesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.selectImagesLabel.Location = new System.Drawing.Point(38, 14);
            this.selectImagesLabel.Name = "selectImagesLabel";
            this.selectImagesLabel.Size = new System.Drawing.Size(96, 17);
            this.selectImagesLabel.TabIndex = 13;
            this.selectImagesLabel.Text = "Select Images";
            // 
            // selectImagesPanel
            // 
            this.selectImagesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectImagesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectImagesPanel.Controls.Add(this.removeImagesBtn);
            this.selectImagesPanel.Controls.Add(this.clearImagesBtn);
            this.selectImagesPanel.Controls.Add(this.addImagesBtn);
            this.selectImagesPanel.Controls.Add(this.selectImagesListBox);
            this.selectImagesPanel.Location = new System.Drawing.Point(25, 23);
            this.selectImagesPanel.Name = "selectImagesPanel";
            this.selectImagesPanel.Size = new System.Drawing.Size(341, 216);
            this.selectImagesPanel.TabIndex = 14;
            // 
            // addImagesBtn
            // 
            this.addImagesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addImagesBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.addImagesBtn.Location = new System.Drawing.Point(246, 172);
            this.addImagesBtn.Name = "addImagesBtn";
            this.addImagesBtn.Size = new System.Drawing.Size(79, 27);
            this.addImagesBtn.TabIndex = 5;
            this.addImagesBtn.Text = "Add";
            this.addImagesBtn.UseVisualStyleBackColor = true;
            this.addImagesBtn.Click += new System.EventHandler(this.selectImagesBtn_Click);
            // 
            // selectImagesListBox
            // 
            this.selectImagesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectImagesListBox.FormattingEnabled = true;
            this.selectImagesListBox.HorizontalScrollbar = true;
            this.selectImagesListBox.Location = new System.Drawing.Point(15, 21);
            this.selectImagesListBox.Name = "selectImagesListBox";
            this.selectImagesListBox.Size = new System.Drawing.Size(310, 134);
            this.selectImagesListBox.TabIndex = 15;
            this.selectImagesListBox.SelectedIndexChanged += new System.EventHandler(this.selectImagesListBox_SelectedIndexChanged);
            // 
            // exitBtn
            // 
            this.exitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exitBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.exitBtn.Location = new System.Drawing.Point(58, 392);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(94, 28);
            this.exitBtn.TabIndex = 16;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // applyBtn
            // 
            this.applyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.applyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.applyBtn.Location = new System.Drawing.Point(238, 392);
            this.applyBtn.Margin = new System.Windows.Forms.Padding(0);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(94, 28);
            this.applyBtn.TabIndex = 15;
            this.applyBtn.Text = "Apply";
            this.applyBtn.UseVisualStyleBackColor = true;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // posInDocumentLabel
            // 
            this.posInDocumentLabel.AutoSize = true;
            this.posInDocumentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.posInDocumentLabel.Location = new System.Drawing.Point(38, 258);
            this.posInDocumentLabel.Name = "posInDocumentLabel";
            this.posInDocumentLabel.Size = new System.Drawing.Size(139, 17);
            this.posInDocumentLabel.TabIndex = 0;
            this.posInDocumentLabel.Text = "Position in document";
            // 
            // posInDocumentTextBox
            // 
            this.posInDocumentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.posInDocumentTextBox.Location = new System.Drawing.Point(15, 23);
            this.posInDocumentTextBox.Name = "posInDocumentTextBox";
            this.posInDocumentTextBox.Size = new System.Drawing.Size(310, 20);
            this.posInDocumentTextBox.TabIndex = 18;
            // 
            // posInDocumentPanel
            // 
            this.posInDocumentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.posInDocumentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.posInDocumentPanel.Controls.Add(this.endPos);
            this.posInDocumentPanel.Controls.Add(this.startPos);
            this.posInDocumentPanel.Controls.Add(this.posInDocumentTextBox);
            this.posInDocumentPanel.Location = new System.Drawing.Point(25, 268);
            this.posInDocumentPanel.Name = "posInDocumentPanel";
            this.posInDocumentPanel.Size = new System.Drawing.Size(341, 100);
            this.posInDocumentPanel.TabIndex = 2;
            // 
            // endPos
            // 
            this.endPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.endPos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.endPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.endPos.Location = new System.Drawing.Point(246, 58);
            this.endPos.Name = "endPos";
            this.endPos.Size = new System.Drawing.Size(79, 28);
            this.endPos.TabIndex = 19;
            this.endPos.Text = "End";
            this.endPos.UseVisualStyleBackColor = true;
            this.endPos.Click += new System.EventHandler(this.endPos_Click);
            // 
            // startPos
            // 
            this.startPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startPos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.startPos.Location = new System.Drawing.Point(15, 58);
            this.startPos.Name = "startPos";
            this.startPos.Size = new System.Drawing.Size(79, 28);
            this.startPos.TabIndex = 17;
            this.startPos.Text = "Start";
            this.startPos.UseVisualStyleBackColor = true;
            this.startPos.Click += new System.EventHandler(this.startPos_Click);
            // 
            // clearImagesBtn
            // 
            this.clearImagesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clearImagesBtn.Enabled = false;
            this.clearImagesBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clearImagesBtn.Location = new System.Drawing.Point(15, 172);
            this.clearImagesBtn.Name = "clearImagesBtn";
            this.clearImagesBtn.Size = new System.Drawing.Size(79, 27);
            this.clearImagesBtn.TabIndex = 16;
            this.clearImagesBtn.Text = "Clear";
            this.clearImagesBtn.UseVisualStyleBackColor = true;
            this.clearImagesBtn.Click += new System.EventHandler(this.clearImagesBtn_Click);
            // 
            // removeImagesBtn
            // 
            this.removeImagesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.removeImagesBtn.Enabled = false;
            this.removeImagesBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.removeImagesBtn.Location = new System.Drawing.Point(132, 172);
            this.removeImagesBtn.Name = "removeImagesBtn";
            this.removeImagesBtn.Size = new System.Drawing.Size(79, 27);
            this.removeImagesBtn.TabIndex = 17;
            this.removeImagesBtn.Text = "Remove";
            this.removeImagesBtn.UseVisualStyleBackColor = true;
            this.removeImagesBtn.Click += new System.EventHandler(this.removeImagesBtn_Click);
            // 
            // AddImagesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 432);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.applyBtn);
            this.Controls.Add(this.posInDocumentLabel);
            this.Controls.Add(this.selectImagesLabel);
            this.Controls.Add(this.selectImagesPanel);
            this.Controls.Add(this.posInDocumentPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddImagesForm";
            this.Text = "Add Images";
            this.selectImagesPanel.ResumeLayout(false);
            this.posInDocumentPanel.ResumeLayout(false);
            this.posInDocumentPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selectImagesLabel;
        private System.Windows.Forms.Panel selectImagesPanel;
        private System.Windows.Forms.Button addImagesBtn;
        private System.Windows.Forms.ListBox selectImagesListBox;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Label posInDocumentLabel;
        private System.Windows.Forms.TextBox posInDocumentTextBox;
        private System.Windows.Forms.Panel posInDocumentPanel;
        private System.Windows.Forms.Button endPos;
        private System.Windows.Forms.Button startPos;
        private System.Windows.Forms.Button removeImagesBtn;
        private System.Windows.Forms.Button clearImagesBtn;
    }
}