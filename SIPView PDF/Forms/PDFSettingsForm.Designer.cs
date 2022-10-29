namespace SIPView_PDF.Forms
{
    partial class PDFSettingsForm
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
            this.compressionLabel = new System.Windows.Forms.Label();
            this.compressionPanel = new System.Windows.Forms.Panel();
            this.jpegLabel = new System.Windows.Forms.Label();
            this.jpegComboBox = new System.Windows.Forms.ComboBox();
            this.bitonalLabel = new System.Windows.Forms.Label();
            this.bitonalComboBox = new System.Windows.Forms.ComboBox();
            this.noneCheckBox = new System.Windows.Forms.CheckBox();
            this.downsampleCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.thumbnailsCheckBox = new System.Windows.Forms.CheckBox();
            this.metadataCheckBox = new System.Windows.Forms.CheckBox();
            this.exitBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.jpeg2KLabel = new System.Windows.Forms.Label();
            this.jpeg2KComboBox = new System.Windows.Forms.ComboBox();
            this.flatteringCheckBox = new System.Windows.Forms.CheckBox();
            this.jpegRadioButton = new System.Windows.Forms.RadioButton();
            this.jpeg2KRadioButton = new System.Windows.Forms.RadioButton();
            this.compressionPanel2 = new System.Windows.Forms.Panel();
            this.compressionPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // compressionLabel
            // 
            this.compressionLabel.AutoSize = true;
            this.compressionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.compressionLabel.Location = new System.Drawing.Point(35, 14);
            this.compressionLabel.Name = "compressionLabel";
            this.compressionLabel.Size = new System.Drawing.Size(90, 17);
            this.compressionLabel.TabIndex = 14;
            this.compressionLabel.Text = "Compression";
            // 
            // compressionPanel
            // 
            this.compressionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.compressionPanel.Controls.Add(this.jpeg2KRadioButton);
            this.compressionPanel.Controls.Add(this.jpegRadioButton);
            this.compressionPanel.Controls.Add(this.bitonalComboBox);
            this.compressionPanel.Controls.Add(this.bitonalLabel);
            this.compressionPanel.Controls.Add(this.flatteringCheckBox);
            this.compressionPanel.Controls.Add(this.jpeg2KLabel);
            this.compressionPanel.Controls.Add(this.jpeg2KComboBox);
            this.compressionPanel.Controls.Add(this.jpegLabel);
            this.compressionPanel.Controls.Add(this.jpegComboBox);
            this.compressionPanel.Controls.Add(this.noneCheckBox);
            this.compressionPanel.Controls.Add(this.downsampleCheckBox);
            this.compressionPanel.Controls.Add(this.compressionPanel2);
            this.compressionPanel.Location = new System.Drawing.Point(24, 24);
            this.compressionPanel.Name = "compressionPanel";
            this.compressionPanel.Size = new System.Drawing.Size(475, 274);
            this.compressionPanel.TabIndex = 13;
            // 
            // jpegLabel
            // 
            this.jpegLabel.AutoSize = true;
            this.jpegLabel.Enabled = false;
            this.jpegLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.jpegLabel.Location = new System.Drawing.Point(14, 202);
            this.jpegLabel.Name = "jpegLabel";
            this.jpegLabel.Size = new System.Drawing.Size(122, 16);
            this.jpegLabel.TabIndex = 14;
            this.jpegLabel.Text = "Jpeg Recompress:";
            // 
            // jpegComboBox
            // 
            this.jpegComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jpegComboBox.Enabled = false;
            this.jpegComboBox.FormattingEnabled = true;
            this.jpegComboBox.Items.AddRange(new object[] {
            "None",
            "LossyLow",
            "LossyMedium",
            "LossyHigh",
            "LossyMaximum"});
            this.jpegComboBox.Location = new System.Drawing.Point(17, 223);
            this.jpegComboBox.Name = "jpegComboBox";
            this.jpegComboBox.Size = new System.Drawing.Size(189, 21);
            this.jpegComboBox.TabIndex = 13;
            // 
            // bitonalLabel
            // 
            this.bitonalLabel.AutoSize = true;
            this.bitonalLabel.Enabled = false;
            this.bitonalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.bitonalLabel.Location = new System.Drawing.Point(268, 71);
            this.bitonalLabel.Name = "bitonalLabel";
            this.bitonalLabel.Size = new System.Drawing.Size(132, 16);
            this.bitonalLabel.TabIndex = 11;
            this.bitonalLabel.Text = "Bitonal Recompress:";
            // 
            // bitonalComboBox
            // 
            this.bitonalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bitonalComboBox.Enabled = false;
            this.bitonalComboBox.FormattingEnabled = true;
            this.bitonalComboBox.Items.AddRange(new object[] {
            "Lossless Generic",
            "Lossless Text",
            "Lossy Text"});
            this.bitonalComboBox.Location = new System.Drawing.Point(271, 90);
            this.bitonalComboBox.Name = "bitonalComboBox";
            this.bitonalComboBox.Size = new System.Drawing.Size(189, 21);
            this.bitonalComboBox.TabIndex = 10;
            // 
            // noneCheckBox
            // 
            this.noneCheckBox.AutoSize = true;
            this.noneCheckBox.Checked = true;
            this.noneCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noneCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.noneCheckBox.Location = new System.Drawing.Point(13, 29);
            this.noneCheckBox.Name = "noneCheckBox";
            this.noneCheckBox.Size = new System.Drawing.Size(59, 20);
            this.noneCheckBox.TabIndex = 9;
            this.noneCheckBox.Text = "None";
            this.noneCheckBox.UseVisualStyleBackColor = true;
            this.noneCheckBox.CheckedChanged += new System.EventHandler(this.noneCheckBox_CheckedChanged);
            // 
            // downsampleCheckBox
            // 
            this.downsampleCheckBox.AutoSize = true;
            this.downsampleCheckBox.Enabled = false;
            this.downsampleCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.downsampleCheckBox.Location = new System.Drawing.Point(13, 69);
            this.downsampleCheckBox.Name = "downsampleCheckBox";
            this.downsampleCheckBox.Size = new System.Drawing.Size(153, 20);
            this.downsampleCheckBox.TabIndex = 9;
            this.downsampleCheckBox.Text = "Downsample Images";
            this.downsampleCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label1.Location = new System.Drawing.Point(35, 336);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Metadata";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.thumbnailsCheckBox);
            this.panel1.Controls.Add(this.metadataCheckBox);
            this.panel1.Location = new System.Drawing.Point(24, 346);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 156);
            this.panel1.TabIndex = 15;
            // 
            // thumbnailsCheckBox
            // 
            this.thumbnailsCheckBox.AutoSize = true;
            this.thumbnailsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.thumbnailsCheckBox.Location = new System.Drawing.Point(13, 48);
            this.thumbnailsCheckBox.Name = "thumbnailsCheckBox";
            this.thumbnailsCheckBox.Size = new System.Drawing.Size(145, 20);
            this.thumbnailsCheckBox.TabIndex = 8;
            this.thumbnailsCheckBox.Text = "Remove thumbnails";
            this.thumbnailsCheckBox.UseVisualStyleBackColor = true;
            // 
            // metadataCheckBox
            // 
            this.metadataCheckBox.AutoSize = true;
            this.metadataCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.metadataCheckBox.Location = new System.Drawing.Point(13, 22);
            this.metadataCheckBox.Name = "metadataCheckBox";
            this.metadataCheckBox.Size = new System.Drawing.Size(138, 20);
            this.metadataCheckBox.TabIndex = 7;
            this.metadataCheckBox.Text = "Remove metadata";
            this.metadataCheckBox.UseVisualStyleBackColor = true;
            // 
            // exitBtn
            // 
            this.exitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exitBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.exitBtn.Location = new System.Drawing.Point(137, 526);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(94, 28);
            this.exitBtn.TabIndex = 10;
            this.exitBtn.Text = "Cancel";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.startBtn.Location = new System.Drawing.Point(287, 527);
            this.startBtn.Margin = new System.Windows.Forms.Padding(0);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(94, 27);
            this.startBtn.TabIndex = 9;
            this.startBtn.Text = "Apply";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // jpeg2KLabel
            // 
            this.jpeg2KLabel.AutoSize = true;
            this.jpeg2KLabel.Enabled = false;
            this.jpeg2KLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.jpeg2KLabel.Location = new System.Drawing.Point(268, 202);
            this.jpeg2KLabel.Name = "jpeg2KLabel";
            this.jpeg2KLabel.Size = new System.Drawing.Size(137, 16);
            this.jpeg2KLabel.TabIndex = 17;
            this.jpeg2KLabel.Text = "Jpeg2K Recompress:";
            // 
            // jpeg2KComboBox
            // 
            this.jpeg2KComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jpeg2KComboBox.Enabled = false;
            this.jpeg2KComboBox.FormattingEnabled = true;
            this.jpeg2KComboBox.Items.AddRange(new object[] {
            "Lossless",
            "LossyMinimum",
            "LossyLow",
            "LossyMedium",
            "LossyHigh",
            "LossyMaximum"});
            this.jpeg2KComboBox.Location = new System.Drawing.Point(271, 223);
            this.jpeg2KComboBox.Name = "jpeg2KComboBox";
            this.jpeg2KComboBox.Size = new System.Drawing.Size(189, 21);
            this.jpeg2KComboBox.TabIndex = 16;
            // 
            // flatteringCheckBox
            // 
            this.flatteringCheckBox.AutoSize = true;
            this.flatteringCheckBox.Enabled = false;
            this.flatteringCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.flatteringCheckBox.Location = new System.Drawing.Point(13, 95);
            this.flatteringCheckBox.Name = "flatteringCheckBox";
            this.flatteringCheckBox.Size = new System.Drawing.Size(117, 20);
            this.flatteringCheckBox.TabIndex = 18;
            this.flatteringCheckBox.Text = "Field Flattening";
            this.flatteringCheckBox.UseVisualStyleBackColor = true;
            // 
            // jpegRadioButton
            // 
            this.jpegRadioButton.AutoSize = true;
            this.jpegRadioButton.Checked = true;
            this.jpegRadioButton.Enabled = false;
            this.jpegRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.jpegRadioButton.Location = new System.Drawing.Point(17, 149);
            this.jpegRadioButton.Name = "jpegRadioButton";
            this.jpegRadioButton.Size = new System.Drawing.Size(106, 36);
            this.jpegRadioButton.TabIndex = 19;
            this.jpegRadioButton.TabStop = true;
            this.jpegRadioButton.Text = "Recompress \r\nUsing Jpeg\r\n";
            this.jpegRadioButton.UseVisualStyleBackColor = true;
            this.jpegRadioButton.CheckedChanged += new System.EventHandler(this.jpegRadioButton_CheckedChanged);
            // 
            // jpeg2KRadioButton
            // 
            this.jpeg2KRadioButton.AutoSize = true;
            this.jpeg2KRadioButton.Enabled = false;
            this.jpeg2KRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.jpeg2KRadioButton.Location = new System.Drawing.Point(271, 149);
            this.jpeg2KRadioButton.Name = "jpeg2KRadioButton";
            this.jpeg2KRadioButton.Size = new System.Drawing.Size(109, 36);
            this.jpeg2KRadioButton.TabIndex = 20;
            this.jpeg2KRadioButton.Text = "Recompress \r\nUsing Jpeg2K\r\n";
            this.jpeg2KRadioButton.UseVisualStyleBackColor = true;
            this.jpeg2KRadioButton.CheckedChanged += new System.EventHandler(this.jpeg2KRadioButton_CheckedChanged);
            // 
            // compressionPanel2
            // 
            this.compressionPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.compressionPanel2.Location = new System.Drawing.Point(-1, 134);
            this.compressionPanel2.Name = "compressionPanel2";
            this.compressionPanel2.Size = new System.Drawing.Size(475, 139);
            this.compressionPanel2.TabIndex = 21;
            // 
            // PDFSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 579);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.compressionLabel);
            this.Controls.Add(this.compressionPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PDFSettingsForm";
            this.Text = "PDF Settings";
            this.compressionPanel.ResumeLayout(false);
            this.compressionPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label compressionLabel;
        private System.Windows.Forms.Panel compressionPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.CheckBox thumbnailsCheckBox;
        private System.Windows.Forms.CheckBox metadataCheckBox;
        private System.Windows.Forms.CheckBox noneCheckBox;
        private System.Windows.Forms.CheckBox downsampleCheckBox;
        private System.Windows.Forms.Label bitonalLabel;
        private System.Windows.Forms.ComboBox bitonalComboBox;
        private System.Windows.Forms.Label jpegLabel;
        private System.Windows.Forms.ComboBox jpegComboBox;
        private System.Windows.Forms.CheckBox flatteringCheckBox;
        private System.Windows.Forms.Label jpeg2KLabel;
        private System.Windows.Forms.ComboBox jpeg2KComboBox;
        private System.Windows.Forms.RadioButton jpeg2KRadioButton;
        private System.Windows.Forms.RadioButton jpegRadioButton;
        private System.Windows.Forms.Panel compressionPanel2;
    }
}