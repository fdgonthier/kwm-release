namespace kwm
{
    partial class ConfigKPPRegistration
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
            this.label2 = new System.Windows.Forms.Label();
            this.lnkSignIn = new System.Windows.Forms.LinkLabel();
            this.creds = new kwm.Credentials();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Banner
            // 
            this.Banner.Size = new System.Drawing.Size(512, 35);
            this.Banner.Subtitle = "";
            this.Banner.Title = "Step 1 of 2: Account creation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 250);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 17;
            // 
            // lnkSignIn
            // 
            this.lnkSignIn.AutoSize = true;
            this.lnkSignIn.LinkArea = new System.Windows.Forms.LinkArea(25, 8);
            this.lnkSignIn.Location = new System.Drawing.Point(25, 168);
            this.lnkSignIn.Name = "lnkSignIn";
            this.lnkSignIn.Size = new System.Drawing.Size(176, 17);
            this.lnkSignIn.TabIndex = 18;
            this.lnkSignIn.TabStop = true;
            this.lnkSignIn.Text = "Already have an account? Sign in.";
            this.lnkSignIn.UseCompatibleTextRendering = true;
            this.lnkSignIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // creds
            // 
            this.creds.KpsAddress = "tbsos01.teambox.co";
            this.creds.Location = new System.Drawing.Point(14, 74);
            this.creds.Name = "creds";
            this.creds.Password = "";
            this.creds.SigninMode = true;
            this.creds.Size = new System.Drawing.Size(305, 91);
            this.creds.TabIndex = 14;
            this.creds.UserName = "";
            this.creds.OnCredFieldChange += new System.EventHandler<System.EventArgs>(this.creds_OnCredFieldChange);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(10, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 10, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "Create a Teambox account your server";
            // 
            // ConfigKPPRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lnkSignIn);
            this.Controls.Add(this.creds);
            this.Name = "ConfigKPPRegistration";
            this.Size = new System.Drawing.Size(512, 319);
            this.WizardBack += new Wizard.UI.WizardPageEventHandler(this.ConfigKPPCredentials_WizardBack);
            this.WizardNext += new Wizard.UI.WizardPageEventHandler(this.ConfigKPPCredentials_WizardNext);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.ConfigKPPRegistration_SetActive);
            this.Controls.SetChildIndex(this.creds, 0);
            this.Controls.SetChildIndex(this.lnkSignIn, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.Banner, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Credentials creds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkSignIn;
        private System.Windows.Forms.Label label1;
    }
}