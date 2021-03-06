namespace kwm.ConfigKPPWizard
{
    partial class Credentials
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
            this.UseDefaultServer = new System.Windows.Forms.CheckBox();
            this.txtKpsAddress = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.PasswordLBL = new System.Windows.Forms.Label();
            this.UserNameLBL = new System.Windows.Forms.Label();
            this.ErrorMsgTip = new System.Windows.Forms.ToolTip(this.components);
            this.KPSAdressLBL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UseDefaultServer
            // 
            this.UseDefaultServer.AutoSize = true;
            this.UseDefaultServer.Checked = true;
            this.UseDefaultServer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseDefaultServer.Location = new System.Drawing.Point(6, 3);
            this.UseDefaultServer.Name = "UseDefaultServer";
            this.UseDefaultServer.Size = new System.Drawing.Size(130, 17);
            this.UseDefaultServer.TabIndex = 14;
            this.UseDefaultServer.Text = "Use the default server";
            this.UseDefaultServer.UseVisualStyleBackColor = true;
            this.UseDefaultServer.CheckedChanged += new System.EventHandler(this.HaveEval_CheckedChanged);
            // 
            // txtKpsAddress
            // 
            this.txtKpsAddress.Enabled = false;
            this.txtKpsAddress.Location = new System.Drawing.Point(74, 26);
            this.txtKpsAddress.Name = "txtKpsAddress";
            this.txtKpsAddress.Size = new System.Drawing.Size(165, 20);
            this.txtKpsAddress.TabIndex = 15;
            this.txtKpsAddress.TextChanged += new System.EventHandler(this.txtKpsAddress_TextChanged);
            this.txtKpsAddress.Enter += new System.EventHandler(this.TextBox_Enter);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(74, 78);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(165, 20);
            this.txtPassword.TabIndex = 17;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.Enter += new System.EventHandler(this.TextBox_Enter);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(74, 52);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(165, 20);
            this.txtUsername.TabIndex = 16;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            this.txtUsername.Enter += new System.EventHandler(this.TextBox_Enter);
            // 
            // PasswordLBL
            // 
            this.PasswordLBL.AutoSize = true;
            this.PasswordLBL.Location = new System.Drawing.Point(3, 81);
            this.PasswordLBL.Name = "PasswordLBL";
            this.PasswordLBL.Size = new System.Drawing.Size(53, 13);
            this.PasswordLBL.TabIndex = 13;
            this.PasswordLBL.Text = "Password";
            // 
            // UserNameLBL
            // 
            this.UserNameLBL.AutoSize = true;
            this.UserNameLBL.Location = new System.Drawing.Point(3, 55);
            this.UserNameLBL.Name = "UserNameLBL";
            this.UserNameLBL.Size = new System.Drawing.Size(55, 13);
            this.UserNameLBL.TabIndex = 12;
            this.UserNameLBL.Text = "Username";
            // 
            // ErrorMsgTip
            // 
            this.ErrorMsgTip.IsBalloon = true;
            this.ErrorMsgTip.ToolTipTitle = "Error";
            // 
            // KPSAdressLBL
            // 
            this.KPSAdressLBL.AutoSize = true;
            this.KPSAdressLBL.Location = new System.Drawing.Point(3, 29);
            this.KPSAdressLBL.Name = "KPSAdressLBL";
            this.KPSAdressLBL.Size = new System.Drawing.Size(38, 13);
            this.KPSAdressLBL.TabIndex = 16;
            this.KPSAdressLBL.Text = "Server";
            // 
            // Credentials
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UseDefaultServer);
            this.Controls.Add(this.txtKpsAddress);
            this.Controls.Add(this.KPSAdressLBL);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.PasswordLBL);
            this.Controls.Add(this.UserNameLBL);
            this.Name = "Credentials";
            this.Size = new System.Drawing.Size(245, 104);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox UseDefaultServer;
        private System.Windows.Forms.TextBox txtKpsAddress;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label PasswordLBL;
        private System.Windows.Forms.Label UserNameLBL;
        private System.Windows.Forms.ToolTip ErrorMsgTip;
        private System.Windows.Forms.Label KPSAdressLBL;
    }
}
