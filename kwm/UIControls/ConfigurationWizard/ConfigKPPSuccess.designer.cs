namespace kwm
{
    partial class ConfigKPPSuccess
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkShowQuickstart = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Banner
            // 
            this.Banner.Size = new System.Drawing.Size(432, 10);
            this.Banner.Subtitle = "";
            this.Banner.Title = "";
            this.Banner.UseWaitCursor = true;
            this.Banner.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 75);
            this.label1.TabIndex = 3;
            this.label1.Text = "Your Teambox Manager is now ready for action.\r\n\r\nPress Finish and start enjoying " +
                "Teambox!";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(163, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ready to start";
            // 
            // chkShowQuickstart
            // 
            this.chkShowQuickstart.AutoSize = true;
            this.chkShowQuickstart.Checked = true;
            this.chkShowQuickstart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowQuickstart.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowQuickstart.Image = global::kwm.Properties.Resources.pdfIcon;
            this.chkShowQuickstart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowQuickstart.Location = new System.Drawing.Point(23, 223);
            this.chkShowQuickstart.Name = "chkShowQuickstart";
            this.chkShowQuickstart.Size = new System.Drawing.Size(195, 19);
            this.chkShowQuickstart.TabIndex = 5;
            this.chkShowQuickstart.Text = "Open the Quick Start tutorial";
            this.chkShowQuickstart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkShowQuickstart.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::kwm.Properties.Resources.GreenCheck2;
            this.pictureBox1.Location = new System.Drawing.Point(23, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ConfigKPPSuccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkShowQuickstart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ConfigKPPSuccess";
            this.Size = new System.Drawing.Size(432, 256);
            this.WizardFinish += new System.ComponentModel.CancelEventHandler(this.ConfigKPPSuccess_WizardFinish);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.ConfigKPPSuccess_SetActive);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.chkShowQuickstart, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.Banner, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkShowQuickstart;
    }
}
