namespace SIPView_PDF
{
    partial class BatchProcessesForm
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
            this.targetFolderLabel = new System.Windows.Forms.Label();
            this.sourseFolderLabel = new System.Windows.Forms.Label();
            this.sourseFolderTextBox = new System.Windows.Forms.TextBox();
            this.targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.sourseFolderBtn = new System.Windows.Forms.Button();
            this.targetFolderBtn = new System.Windows.Forms.Button();
            this.includeSubfoldersCheckBox = new System.Windows.Forms.CheckBox();
            this.startBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.sourseFolderPanel = new System.Windows.Forms.Panel();
            this.targetFolderPanel = new System.Windows.Forms.Panel();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.currentProcessLabel = new System.Windows.Forms.Label();
            this.sourseFolderPanel.SuspendLayout();
            this.targetFolderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // targetFolderLabel
            // 
            this.targetFolderLabel.AutoSize = true;
            this.targetFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.targetFolderLabel.Location = new System.Drawing.Point(29, 119);
            this.targetFolderLabel.Name = "targetFolderLabel";
            this.targetFolderLabel.Size = new System.Drawing.Size(90, 17);
            this.targetFolderLabel.TabIndex = 0;
            this.targetFolderLabel.Text = "Target folder";
            // 
            // sourseFolderLabel
            // 
            this.sourseFolderLabel.AutoSize = true;
            this.sourseFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.sourseFolderLabel.Location = new System.Drawing.Point(29, 15);
            this.sourseFolderLabel.Name = "sourseFolderLabel";
            this.sourseFolderLabel.Size = new System.Drawing.Size(93, 17);
            this.sourseFolderLabel.TabIndex = 12;
            this.sourseFolderLabel.Text = "Source folder";
            // 
            // sourseFolderTextBox
            // 
            this.sourseFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourseFolderTextBox.CausesValidation = false;
            this.sourseFolderTextBox.Location = new System.Drawing.Point(14, 21);
            this.sourseFolderTextBox.Name = "sourseFolderTextBox";
            this.sourseFolderTextBox.Size = new System.Drawing.Size(350, 20);
            this.sourseFolderTextBox.TabIndex = 2;
            // 
            // targetFolderTextBox
            // 
            this.targetFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFolderTextBox.Location = new System.Drawing.Point(16, 21);
            this.targetFolderTextBox.Name = "targetFolderTextBox";
            this.targetFolderTextBox.Size = new System.Drawing.Size(350, 20);
            this.targetFolderTextBox.TabIndex = 2;
            // 
            // sourseFolderBtn
            // 
            this.sourseFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sourseFolderBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.sourseFolderBtn.FlatAppearance.BorderSize = 4;
            this.sourseFolderBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sourseFolderBtn.Location = new System.Drawing.Point(392, 21);
            this.sourseFolderBtn.Name = "sourseFolderBtn";
            this.sourseFolderBtn.Size = new System.Drawing.Size(55, 23);
            this.sourseFolderBtn.TabIndex = 4;
            this.sourseFolderBtn.Text = "...";
            this.sourseFolderBtn.UseVisualStyleBackColor = true;
            this.sourseFolderBtn.Click += new System.EventHandler(this.sourseFolderBtn_Click);
            // 
            // targetFolderBtn
            // 
            this.targetFolderBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFolderBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.targetFolderBtn.Location = new System.Drawing.Point(392, 19);
            this.targetFolderBtn.Name = "targetFolderBtn";
            this.targetFolderBtn.Size = new System.Drawing.Size(55, 23);
            this.targetFolderBtn.TabIndex = 5;
            this.targetFolderBtn.Text = "...";
            this.targetFolderBtn.UseVisualStyleBackColor = true;
            this.targetFolderBtn.Click += new System.EventHandler(this.targetFolderBtn_Click);
            // 
            // includeSubfoldersCheckBox
            // 
            this.includeSubfoldersCheckBox.AutoSize = true;
            this.includeSubfoldersCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.includeSubfoldersCheckBox.Location = new System.Drawing.Point(13, 47);
            this.includeSubfoldersCheckBox.Name = "includeSubfoldersCheckBox";
            this.includeSubfoldersCheckBox.Size = new System.Drawing.Size(152, 20);
            this.includeSubfoldersCheckBox.TabIndex = 6;
            this.includeSubfoldersCheckBox.Text = "Include all subfolders";
            this.includeSubfoldersCheckBox.UseVisualStyleBackColor = true;
            // 
            // startBtn
            // 
            this.startBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.startBtn.Location = new System.Drawing.Point(312, 291);
            this.startBtn.Margin = new System.Windows.Forms.Padding(0);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(94, 27);
            this.startBtn.TabIndex = 7;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exitBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.exitBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.exitBtn.Location = new System.Drawing.Point(90, 291);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(94, 28);
            this.exitBtn.TabIndex = 8;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // sourseFolderPanel
            // 
            this.sourseFolderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourseFolderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sourseFolderPanel.Controls.Add(this.includeSubfoldersCheckBox);
            this.sourseFolderPanel.Controls.Add(this.sourseFolderBtn);
            this.sourseFolderPanel.Controls.Add(this.sourseFolderTextBox);
            this.sourseFolderPanel.Location = new System.Drawing.Point(18, 25);
            this.sourseFolderPanel.Name = "sourseFolderPanel";
            this.sourseFolderPanel.Size = new System.Drawing.Size(464, 79);
            this.sourseFolderPanel.TabIndex = 11;
            // 
            // targetFolderPanel
            // 
            this.targetFolderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFolderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetFolderPanel.Controls.Add(this.targetFolderBtn);
            this.targetFolderPanel.Controls.Add(this.targetFolderTextBox);
            this.targetFolderPanel.Location = new System.Drawing.Point(18, 128);
            this.targetFolderPanel.Name = "targetFolderPanel";
            this.targetFolderPanel.Size = new System.Drawing.Size(464, 63);
            this.targetFolderPanel.TabIndex = 12;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(32, 238);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(434, 23);
            this.ProgressBar.TabIndex = 13;
            this.ProgressBar.Visible = false;
            // 
            // currentProcessLabel
            // 
            this.currentProcessLabel.AutoSize = true;
            this.currentProcessLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.currentProcessLabel.Location = new System.Drawing.Point(32, 208);
            this.currentProcessLabel.Name = "currentProcessLabel";
            this.currentProcessLabel.Size = new System.Drawing.Size(67, 17);
            this.currentProcessLabel.TabIndex = 15;
            this.currentProcessLabel.Text = "FileName";
            this.currentProcessLabel.Visible = false;
            // 
            // BatchProcessesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 333);
            this.Controls.Add(this.currentProcessLabel);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.sourseFolderLabel);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.targetFolderLabel);
            this.Controls.Add(this.sourseFolderPanel);
            this.Controls.Add(this.targetFolderPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchProcessesForm";
            this.Text = "Batch_Processes_Form";
            this.sourseFolderPanel.ResumeLayout(false);
            this.sourseFolderPanel.PerformLayout();
            this.targetFolderPanel.ResumeLayout(false);
            this.targetFolderPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label targetFolderLabel;
        private System.Windows.Forms.Label sourseFolderLabel;
        private System.Windows.Forms.TextBox sourseFolderTextBox;
        private System.Windows.Forms.TextBox targetFolderTextBox;
        private System.Windows.Forms.Button sourseFolderBtn;
        private System.Windows.Forms.Button targetFolderBtn;
        private System.Windows.Forms.CheckBox includeSubfoldersCheckBox;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Panel sourseFolderPanel;
        private System.Windows.Forms.Panel targetFolderPanel;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label currentProcessLabel;
    }
}