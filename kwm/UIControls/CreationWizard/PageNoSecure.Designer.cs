namespace kwm
{
    partial class PageNoSecure
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
            this.btnRetry = new System.Windows.Forms.Button();
            this.lnkUpgrade = new System.Windows.Forms.LinkLabel();
            this.chkSwitchToStd = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.ucGoldRequired1 = new kwm.KwmAppControls.Controls.ucGoldRequired();
            this.SuspendLayout();
            // 
            // Banner
            // 
            this.Banner.Size = new System.Drawing.Size(444, 0);
            this.Banner.Subtitle = "";
            this.Banner.Title = "Teambox creation failed";
            this.Banner.Visible = false;
            // 
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRetry.BackColor = System.Drawing.SystemColors.Control;
            this.btnRetry.Location = new System.Drawing.Point(359, 240);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(75, 23);
            this.btnRetry.TabIndex = 14;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = false;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // lnkUpgrade
            // 
            this.lnkUpgrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUpgrade.LinkArea = new System.Windows.Forms.LinkArea(111, 31);
            this.lnkUpgrade.Location = new System.Drawing.Point(16, 97);
            this.lnkUpgrade.Name = "lnkUpgrade";
            this.lnkUpgrade.Size = new System.Drawing.Size(406, 45);
            this.lnkUpgrade.TabIndex = 17;
            this.lnkUpgrade.Text = "Secure Teamboxes are not available with your license package.";
            this.lnkUpgrade.UseCompatibleTextRendering = true;
            // 
            // chkSwitchToStd
            // 
            this.chkSwitchToStd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSwitchToStd.AutoSize = true;
            this.chkSwitchToStd.Checked = true;
            this.chkSwitchToStd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSwitchToStd.Location = new System.Drawing.Point(178, 244);
            this.chkSwitchToStd.Name = "chkSwitchToStd";
            this.chkSwitchToStd.Size = new System.Drawing.Size(175, 17);
            this.chkSwitchToStd.TabIndex = 18;
            this.chkSwitchToStd.Text = "Retry with a Standard Teambox";
            this.chkSwitchToStd.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(42, 31);
            this.linkLabel1.Location = new System.Drawing.Point(16, 156);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(406, 62);
            this.linkLabel1.TabIndex = 20;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "You can upgrade your account by visiting\r\nhttps://www.teambox.co";
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ucGoldRequired1
            // 
            this.ucGoldRequired1.Location = new System.Drawing.Point(0, 0);
            this.ucGoldRequired1.Name = "ucGoldRequired1";
            this.ucGoldRequired1.Size = new System.Drawing.Size(451, 91);
            this.ucGoldRequired1.TabIndex = 23;
            // 
            // PageNoSecure
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ucGoldRequired1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.chkSwitchToStd);
            this.Controls.Add(this.lnkUpgrade);
            this.Controls.Add(this.btnRetry);
            this.Name = "PageNoSecure";
            this.Size = new System.Drawing.Size(444, 273);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.PageFailure_SetActive);
            this.Controls.SetChildIndex(this.btnRetry, 0);
            this.Controls.SetChildIndex(this.lnkUpgrade, 0);
            this.Controls.SetChildIndex(this.chkSwitchToStd, 0);
            this.Controls.SetChildIndex(this.linkLabel1, 0);
            this.Controls.SetChildIndex(this.ucGoldRequired1, 0);
            this.Controls.SetChildIndex(this.Banner, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.LinkLabel lnkUpgrade;
        private System.Windows.Forms.CheckBox chkSwitchToStd;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private kwm.KwmAppControls.Controls.ucGoldRequired ucGoldRequired1;


    }
}
