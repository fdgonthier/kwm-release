namespace kwm.KwmAppControls.Controls
{
    partial class ucGoldRequired
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
            this.etchedLine1 = new Wizard.Controls.EtchedLine();
            this.lblOp = new System.Windows.Forms.Label();
            this.lnkLearnMore = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // etchedLine1
            // 
            this.etchedLine1.Edge = Wizard.Controls.EtchEdge.Top;
            this.etchedLine1.Location = new System.Drawing.Point(20, 68);
            this.etchedLine1.Name = "etchedLine1";
            this.etchedLine1.Size = new System.Drawing.Size(405, 10);
            this.etchedLine1.TabIndex = 25;
            // 
            // lblOp
            // 
            this.lblOp.AutoSize = true;
            this.lblOp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOp.ForeColor = System.Drawing.Color.Black;
            this.lblOp.Location = new System.Drawing.Point(119, 16);
            this.lblOp.Name = "lblOp";
            this.lblOp.Size = new System.Drawing.Size(257, 25);
            this.lblOp.TabIndex = 23;
            this.lblOp.Text = "License upgrade required";
            // 
            // lnkLearnMore
            // 
            this.lnkLearnMore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLearnMore.LinkArea = new System.Windows.Forms.LinkArea(0, 10);
            this.lnkLearnMore.Location = new System.Drawing.Point(349, 75);
            this.lnkLearnMore.Name = "lnkLearnMore";
            this.lnkLearnMore.Size = new System.Drawing.Size(78, 19);
            this.lnkLearnMore.TabIndex = 26;
            this.lnkLearnMore.TabStop = true;
            this.lnkLearnMore.Text = "Learn more";
            this.lnkLearnMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLearnMore_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::kwm.KwmAppControls.Properties.Resources.DollarSign;
            this.pictureBox1.Location = new System.Drawing.Point(20, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // ucGoldRequired
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkLearnMore);
            this.Controls.Add(this.etchedLine1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblOp);
            this.Name = "ucGoldRequired";
            this.Size = new System.Drawing.Size(451, 101);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wizard.Controls.EtchedLine etchedLine1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblOp;
        private System.Windows.Forms.LinkLabel lnkLearnMore;
    }
}
