namespace kwm
{
    partial class ConfigKPPSignIn
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
            this.label1 = new System.Windows.Forms.Label();
            this.ErrorMsgTip = new System.Windows.Forms.ToolTip(this.components);
            this.creds = new kwm.Credentials();
            this.SuspendLayout();
            // 
            // Banner
            // 
            this.Banner.Size = new System.Drawing.Size(432, 35);
            this.Banner.Subtitle = "";
            this.Banner.Title = "Sign-in with your Teambox account";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 10, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "Sign in using your existing account informations";
            // 
            // ErrorMsgTip
            // 
            this.ErrorMsgTip.IsBalloon = true;
            this.ErrorMsgTip.ToolTipTitle = "Error";
            // 
            // creds
            // 
            this.creds.KpsAddress = "tbsos01.teambox.co";
            this.creds.Location = new System.Drawing.Point(10, 81);
            this.creds.Margin = new System.Windows.Forms.Padding(10, 10, 3, 3);
            this.creds.Name = "creds";
            this.creds.Password = "";
            this.creds.SigninMode = true;
            this.creds.Size = new System.Drawing.Size(279, 94);
            this.creds.TabIndex = 17;
            this.creds.UserName = "";
            this.creds.OnCredFieldChange += new System.EventHandler<System.EventArgs>(this.creds_OnCredFieldChange);
            // 
            // ConfigKPPSignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.creds);
            this.Controls.Add(this.label1);
            this.Name = "ConfigKPPSignIn";
            this.Size = new System.Drawing.Size(432, 303);
            this.WizardNext += new Wizard.UI.WizardPageEventHandler(this.ConfigKPPSignIn_WizardNext);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.ConfigKPPSignIn_SetActive);
            this.Controls.SetChildIndex(this.Banner, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.creds, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip ErrorMsgTip;
        private Credentials creds;

    }
}