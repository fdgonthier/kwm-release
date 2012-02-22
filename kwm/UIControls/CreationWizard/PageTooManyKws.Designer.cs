namespace kwm
{
    partial class PageTooManyKws
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
            this.lnkUpgrade = new System.Windows.Forms.LinkLabel();
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
            // lnkUpgrade
            // 
            this.lnkUpgrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUpgrade.LinkArea = new System.Windows.Forms.LinkArea(168, 20);
            this.lnkUpgrade.Location = new System.Drawing.Point(18, 104);
            this.lnkUpgrade.Name = "lnkUpgrade";
            this.lnkUpgrade.Size = new System.Drawing.Size(406, 117);
            this.lnkUpgrade.TabIndex = 15;
            this.lnkUpgrade.TabStop = true;
            this.lnkUpgrade.Text = "You reached the maximum number of Teamboxes allowed with your license package.\r\n\r" +
                "\nIn order to create a new one, you can either delete a Teambox you already creat" +
                "ed, or upgrade your account.";
            this.lnkUpgrade.UseCompatibleTextRendering = true;
            this.lnkUpgrade.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpgrade_LinkClicked);
            // 
            // ucGoldRequired1
            // 
            this.ucGoldRequired1.Location = new System.Drawing.Point(0, 0);
            this.ucGoldRequired1.Name = "ucGoldRequired1";
            this.ucGoldRequired1.Size = new System.Drawing.Size(444, 96);
            this.ucGoldRequired1.TabIndex = 16;
            // 
            // PageTooManyKws
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ucGoldRequired1);
            this.Controls.Add(this.lnkUpgrade);
            this.Name = "PageTooManyKws";
            this.Size = new System.Drawing.Size(444, 273);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.PageTooManyKws_SetActive);
            this.Controls.SetChildIndex(this.Banner, 0);
            this.Controls.SetChildIndex(this.lnkUpgrade, 0);
            this.Controls.SetChildIndex(this.ucGoldRequired1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkUpgrade;
        private kwm.KwmAppControls.Controls.ucGoldRequired ucGoldRequired1;


    }
}
