namespace CARInstaller
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
            this.btnInstall = new System.Windows.Forms.Button();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.rtxtlogs = new System.Windows.Forms.RichTextBox();
            this.pLogsContainer = new System.Windows.Forms.Panel();
            this.lProgressStatus = new System.Windows.Forms.Label();
            this.pLogsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(12, 12);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(12, 41);
            this.pbProgress.MarqueeAnimationSpeed = 10;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(549, 23);
            this.pbProgress.TabIndex = 1;
            // 
            // rtxtlogs
            // 
            this.rtxtlogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtlogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtlogs.Location = new System.Drawing.Point(0, 0);
            this.rtxtlogs.Name = "rtxtlogs";
            this.rtxtlogs.Size = new System.Drawing.Size(547, 312);
            this.rtxtlogs.TabIndex = 2;
            this.rtxtlogs.Text = "";
            // 
            // pLogsContainer
            // 
            this.pLogsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pLogsContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pLogsContainer.Controls.Add(this.rtxtlogs);
            this.pLogsContainer.Location = new System.Drawing.Point(12, 91);
            this.pLogsContainer.Name = "pLogsContainer";
            this.pLogsContainer.Size = new System.Drawing.Size(549, 314);
            this.pLogsContainer.TabIndex = 3;
            // 
            // lProgressStatus
            // 
            this.lProgressStatus.AutoSize = true;
            this.lProgressStatus.Location = new System.Drawing.Point(9, 70);
            this.lProgressStatus.Name = "lProgressStatus";
            this.lProgressStatus.Size = new System.Drawing.Size(99, 13);
            this.lProgressStatus.TabIndex = 4;
            this.lProgressStatus.Text = "Not doing anything!";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 417);
            this.Controls.Add(this.lProgressStatus);
            this.Controls.Add(this.pLogsContainer);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.btnInstall);
            this.MinimumSize = new System.Drawing.Size(388, 183);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Classic Applet Revived - Prerequisites Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.pLogsContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.RichTextBox rtxtlogs;
        private System.Windows.Forms.Panel pLogsContainer;
        private System.Windows.Forms.Label lProgressStatus;
    }
}

