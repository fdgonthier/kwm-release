namespace kwm
{
    partial class ucKwsHeaderToolbar
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
            this.btnKwsTask = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.imgKwsStatus = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblKwsName = new System.Windows.Forms.Label();
            this.lblCreator = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblKasError = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.etchedLine1 = new Wizard.Controls.EtchedLine();
            this.picSecure = new System.Windows.Forms.PictureBox();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgKwsStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSecure)).BeginInit();
            this.SuspendLayout();
            // 
            // btnKwsTask
            // 
            this.btnKwsTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKwsTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKwsTask.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnKwsTask.Location = new System.Drawing.Point(474, 9);
            this.btnKwsTask.Name = "btnKwsTask";
            this.btnKwsTask.Size = new System.Drawing.Size(74, 21);
            this.btnKwsTask.TabIndex = 5;
            this.btnKwsTask.Text = "Disconnected";
            this.btnKwsTask.UseVisualStyleBackColor = true;
            this.btnKwsTask.Click += new System.EventHandler(this.btnKwsTask_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.imgKwsStatus);
            this.panelMain.Controls.Add(this.lblStatus);
            this.panelMain.Controls.Add(this.lblKwsName);
            this.panelMain.Controls.Add(this.lblCreator);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.label3);
            this.panelMain.Controls.Add(this.lblKasError);
            this.panelMain.Controls.Add(this.lblServer);
            this.panelMain.Controls.Add(this.etchedLine1);
            this.panelMain.Controls.Add(this.btnKwsTask);
            this.panelMain.Controls.Add(this.picSecure);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(554, 87);
            this.panelMain.TabIndex = 0;
            // 
            // imgKwsStatus
            // 
            this.imgKwsStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgKwsStatus.Image = global::kwm.Properties.Resources.connecting_3;
            this.imgKwsStatus.Location = new System.Drawing.Point(452, 11);
            this.imgKwsStatus.Name = "imgKwsStatus";
            this.imgKwsStatus.Size = new System.Drawing.Size(16, 16);
            this.imgKwsStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgKwsStatus.TabIndex = 23;
            this.imgKwsStatus.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Location = new System.Drawing.Point(352, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(97, 14);
            this.lblStatus.TabIndex = 22;
            this.lblStatus.Text = "Connecting";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblKwsName
            // 
            this.lblKwsName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKwsName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKwsName.Location = new System.Drawing.Point(36, 10);
            this.lblKwsName.Name = "lblKwsName";
            this.lblKwsName.Size = new System.Drawing.Size(242, 18);
            this.lblKwsName.TabIndex = 21;
            this.lblKwsName.Text = "My New Teambox";
            // 
            // lblCreator
            // 
            this.lblCreator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreator.AutoEllipsis = true;
            this.lblCreator.Location = new System.Drawing.Point(72, 48);
            this.lblCreator.Name = "lblCreator";
            this.lblCreator.Size = new System.Drawing.Size(248, 13);
            this.lblCreator.TabIndex = 20;
            this.lblCreator.Text = "";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = global::kwm.Properties.Resources.user;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(3, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 18);
            this.label4.TabIndex = 19;
            this.label4.Text = " Creator:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = global::kwm.Properties.Resources.server;
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 18);
            this.label3.TabIndex = 18;
            this.label3.Text = "Server:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblKasError
            // 
            this.lblKasError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKasError.AutoEllipsis = true;
            this.lblKasError.ForeColor = System.Drawing.Color.Red;
            this.lblKasError.Location = new System.Drawing.Point(330, 48);
            this.lblKasError.Name = "lblKasError";
            this.lblKasError.Size = new System.Drawing.Size(218, 27);
            this.lblKasError.TabIndex = 16;
            this.lblKasError.Text = "Login error: the Teambox has been deleted from the server.";
            // 
            // lblServer
            // 
            this.lblServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServer.Location = new System.Drawing.Point(72, 66);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(248, 13);
            this.lblServer.TabIndex = 15;
            this.lblServer.Text = "kcd02.teambox.co (ID: 1100)";
            // 
            // etchedLine1
            // 
            this.etchedLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.etchedLine1.Edge = Wizard.Controls.EtchEdge.Top;
            this.etchedLine1.Location = new System.Drawing.Point(4, 40);
            this.etchedLine1.Name = "etchedLine1";
            this.etchedLine1.Size = new System.Drawing.Size(544, 10);
            this.etchedLine1.TabIndex = 14;
            // 
            // picSecure
            // 
            this.picSecure.Image = global::kwm.Properties.Resources.NewLock_OK_24x24;
            this.picSecure.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picSecure.Location = new System.Drawing.Point(4, 6);
            this.picSecure.Name = "picSecure";
            this.picSecure.Size = new System.Drawing.Size(24, 24);
            this.picSecure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSecure.TabIndex = 3;
            this.picSecure.TabStop = false;
            // 
            // ucKwsHeaderToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "ucKwsHeaderToolbar";
            this.Size = new System.Drawing.Size(554, 87);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgKwsStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSecure)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSecure;
        private System.Windows.Forms.Button btnKwsTask;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblKasError;
        private System.Windows.Forms.Label lblServer;
        private Wizard.Controls.EtchedLine etchedLine1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCreator;
        private System.Windows.Forms.Label lblKwsName;
        private System.Windows.Forms.PictureBox imgKwsStatus;
        private System.Windows.Forms.Label lblStatus;

    }
}
