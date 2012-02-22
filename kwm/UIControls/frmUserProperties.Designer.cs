namespace kwm
{
    partial class frmUserProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserProperties));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserEmail = new System.Windows.Forms.Label();
            this.etchedLine1 = new Wizard.Controls.EtchedLine();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkLockedAccount = new System.Windows.Forms.CheckBox();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.helpLock = new System.Windows.Forms.PictureBox();
            this.helpRole = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.helpUserName = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpUserName)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::kwm.Properties.Resources.person;
            this.pictureBox1.InitialImage = global::kwm.Properties.Resources.person;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(85, 29);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(261, 21);
            this.lblUserName.TabIndex = 6;
            this.lblUserName.Text = "Regular User";
            // 
            // lblUserEmail
            // 
            this.lblUserEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserEmail.Location = new System.Drawing.Point(107, 50);
            this.lblUserEmail.Name = "lblUserEmail";
            this.lblUserEmail.Size = new System.Drawing.Size(239, 19);
            this.lblUserEmail.TabIndex = 7;
            this.lblUserEmail.Text = "regular@user.com";
            // 
            // etchedLine1
            // 
            this.etchedLine1.Edge = Wizard.Controls.EtchEdge.Top;
            this.etchedLine1.Location = new System.Drawing.Point(13, 84);
            this.etchedLine1.Name = "etchedLine1";
            this.etchedLine1.Size = new System.Drawing.Size(333, 1);
            this.etchedLine1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Role:";
            // 
            // chkLockedAccount
            // 
            this.chkLockedAccount.AutoSize = true;
            this.chkLockedAccount.Location = new System.Drawing.Point(15, 159);
            this.chkLockedAccount.Name = "chkLockedAccount";
            this.chkLockedAccount.Size = new System.Drawing.Size(111, 17);
            this.chkLockedAccount.TabIndex = 3;
            this.chkLockedAccount.Text = "Account is locked";
            this.chkLockedAccount.UseVisualStyleBackColor = true;
            // 
            // cboRole
            // 
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.FormattingEnabled = true;
            this.cboRole.ItemHeight = 13;
            this.cboRole.Items.AddRange(new object[] {
            "Administrator",
            "Manager",
            "User"});
            this.cboRole.Location = new System.Drawing.Point(66, 126);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(121, 21);
            this.cboRole.TabIndex = 2;
            this.cboRole.SelectedIndexChanged += new System.EventHandler(this.cboRole_SelectedIndexChanged);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(66, 100);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(251, 20);
            this.txtUserName.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(174, 245);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(255, 245);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // helpLock
            // 
            this.helpLock.Image = ((System.Drawing.Image)(resources.GetObject("helpLock.Image")));
            this.helpLock.Location = new System.Drawing.Point(139, 159);
            this.helpLock.Name = "helpLock";
            this.helpLock.Size = new System.Drawing.Size(16, 16);
            this.helpLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpLock.TabIndex = 11;
            this.helpLock.TabStop = false;
            this.helpLock.Click += new System.EventHandler(this.helpLock_Click);
            // 
            // helpRole
            // 
            this.helpRole.Image = ((System.Drawing.Image)(resources.GetObject("helpRole.Image")));
            this.helpRole.Location = new System.Drawing.Point(196, 130);
            this.helpRole.Name = "helpRole";
            this.helpRole.Size = new System.Drawing.Size(16, 16);
            this.helpRole.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpRole.TabIndex = 12;
            this.helpRole.TabStop = false;
            this.helpRole.Click += new System.EventHandler(this.helpRole_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(15, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(324, 31);
            this.label7.TabIndex = 8;
            this.label7.Text = "User properties are disabled if you do not have sufficient permissions to edit th" +
                "em.";
            // 
            // helpUserName
            // 
            this.helpUserName.Image = ((System.Drawing.Image)(resources.GetObject("helpUserName.Image")));
            this.helpUserName.Location = new System.Drawing.Point(323, 103);
            this.helpUserName.Name = "helpUserName";
            this.helpUserName.Size = new System.Drawing.Size(16, 16);
            this.helpUserName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpUserName.TabIndex = 40;
            this.helpUserName.TabStop = false;
            this.helpUserName.Click += new System.EventHandler(this.helpName_Click);
            // 
            // frmUserProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(358, 282);
            this.Controls.Add(this.helpUserName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.helpRole);
            this.Controls.Add(this.helpLock);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.chkLockedAccount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.etchedLine1);
            this.Controls.Add(this.lblUserEmail);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cboRole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserProperties";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Regular User Properties";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpUserName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserEmail;
        private Wizard.Controls.EtchedLine etchedLine1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkLockedAccount;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox helpLock;
        private System.Windows.Forms.PictureBox helpRole;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox helpUserName;
    }
}