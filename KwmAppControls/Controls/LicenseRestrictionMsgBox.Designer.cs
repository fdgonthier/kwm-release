namespace kwm.Utils
{
    partial class LicenseRestrictionMsgBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseRestrictionMsgBox));
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblUpgradeAccnt = new System.Windows.Forms.LinkLabel();
            this.ucGoldRequired1 = new kwm.KwmAppControls.Controls.ucGoldRequired();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.Black;
            this.lblMsg.Location = new System.Drawing.Point(35, 104);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(386, 53);
            this.lblMsg.TabIndex = 21;
            this.lblMsg.Text = "<Dynamic text>";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(186, 200);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 23;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblUpgradeAccnt
            // 
            this.lblUpgradeAccnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUpgradeAccnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpgradeAccnt.LinkArea = new System.Windows.Forms.LinkArea(32, 31);
            this.lblUpgradeAccnt.Location = new System.Drawing.Point(38, 166);
            this.lblUpgradeAccnt.Name = "lblUpgradeAccnt";
            this.lblUpgradeAccnt.Size = new System.Drawing.Size(393, 19);
            this.lblUpgradeAccnt.TabIndex = 25;
            this.lblUpgradeAccnt.TabStop = true;
            this.lblUpgradeAccnt.Text = "To upgrade your account, visit  https://www.teambox.co";
            this.lblUpgradeAccnt.UseCompatibleTextRendering = true;
            this.lblUpgradeAccnt.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblUpgradeAccnt_LinkClicked);
            // 
            // ucGoldRequired1
            // 
            this.ucGoldRequired1.Location = new System.Drawing.Point(-1, -1);
            this.ucGoldRequired1.Name = "ucGoldRequired1";
            this.ucGoldRequired1.Size = new System.Drawing.Size(432, 94);
            this.ucGoldRequired1.TabIndex = 26;
            // 
            // LicenseRestrictionMsgBox
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 235);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.ucGoldRequired1);
            this.Controls.Add(this.lblUpgradeAccnt);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseRestrictionMsgBox";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "License upgrade required";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel lblUpgradeAccnt;
        private kwm.KwmAppControls.Controls.ucGoldRequired ucGoldRequired1;
    }
}