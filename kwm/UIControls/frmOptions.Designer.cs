namespace kwm
{
    partial class frmOptions
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.optionTabs = new System.Windows.Forms.TabControl();
            this.tabSupport = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKasPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKasAddr = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnViewKcsLogs = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cboLogLevel = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ktlsDebug = new System.Windows.Forms.RadioButton();
            this.ktlsMin = new System.Windows.Forms.RadioButton();
            this.ktlsNone = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEnableDebugging = new System.Windows.Forms.CheckBox();
            this.chkLogToFile = new System.Windows.Forms.CheckBox();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.helpThinKfs = new System.Windows.Forms.PictureBox();
            this.chkPreserveFileHistory = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lblRestart = new System.Windows.Forms.Label();
            this.btnStorePathBrowse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rbFileStoreCustom = new System.Windows.Forms.RadioButton();
            this.rbFileStoreMyDocs = new System.Windows.Forms.RadioButton();
            this.txtCustomStorePath = new System.Windows.Forms.TextBox();
            this.boxEvent = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNotifDuration = new System.Windows.Forms.TextBox();
            this.chkShowNotification = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.CustomPathBrowseDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.optionTabs.SuspendLayout();
            this.tabSupport.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpThinKfs)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.boxEvent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // optionTabs
            // 
            this.optionTabs.Controls.Add(this.tabSupport);
            this.optionTabs.Controls.Add(this.tabAdvanced);
            resources.ApplyResources(this.optionTabs, "optionTabs");
            this.optionTabs.Name = "optionTabs";
            this.optionTabs.SelectedIndex = 0;
            // 
            // tabSupport
            // 
            this.tabSupport.Controls.Add(this.groupBox3);
            this.tabSupport.Controls.Add(this.groupBox7);
            this.tabSupport.Controls.Add(this.groupBox2);
            this.tabSupport.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabSupport, "tabSupport");
            this.tabSupport.Name = "tabSupport";
            this.tabSupport.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtKasPort);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtKasAddr);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtKasPort
            // 
            resources.ApplyResources(this.txtKasPort, "txtKasPort");
            this.txtKasPort.Name = "txtKasPort";
            this.txtKasPort.Validating += new System.ComponentModel.CancelEventHandler(this.txtKasPort_Validating);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtKasAddr
            // 
            resources.ApplyResources(this.txtKasAddr, "txtKasAddr");
            this.txtKasAddr.Name = "txtKasAddr";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnViewKcsLogs);
            this.groupBox7.Controls.Add(this.btnApply);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.cboLogLevel);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // btnViewKcsLogs
            // 
            resources.ApplyResources(this.btnViewKcsLogs, "btnViewKcsLogs");
            this.btnViewKcsLogs.Name = "btnViewKcsLogs";
            this.btnViewKcsLogs.UseVisualStyleBackColor = true;
            this.btnViewKcsLogs.Click += new System.EventHandler(this.btnViewKcsLogs_Click);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cboLogLevel
            // 
            this.cboLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogLevel.FormattingEnabled = true;
            this.cboLogLevel.Items.AddRange(new object[] {
            resources.GetString("cboLogLevel.Items"),
            resources.GetString("cboLogLevel.Items1"),
            resources.GetString("cboLogLevel.Items2")});
            resources.ApplyResources(this.cboLogLevel, "cboLogLevel");
            this.cboLogLevel.Name = "cboLogLevel";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ktlsDebug);
            this.groupBox2.Controls.Add(this.ktlsMin);
            this.groupBox2.Controls.Add(this.ktlsNone);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // ktlsDebug
            // 
            resources.ApplyResources(this.ktlsDebug, "ktlsDebug");
            this.ktlsDebug.Name = "ktlsDebug";
            this.ktlsDebug.UseVisualStyleBackColor = true;
            // 
            // ktlsMin
            // 
            resources.ApplyResources(this.ktlsMin, "ktlsMin");
            this.ktlsMin.Name = "ktlsMin";
            this.ktlsMin.TabStop = true;
            this.ktlsMin.UseVisualStyleBackColor = true;
            // 
            // ktlsNone
            // 
            resources.ApplyResources(this.ktlsNone, "ktlsNone");
            this.ktlsNone.Checked = true;
            this.ktlsNone.Name = "ktlsNone";
            this.ktlsNone.TabStop = true;
            this.ktlsNone.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkEnableDebugging);
            this.groupBox1.Controls.Add(this.chkLogToFile);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkEnableDebugging
            // 
            resources.ApplyResources(this.chkEnableDebugging, "chkEnableDebugging");
            this.chkEnableDebugging.Name = "chkEnableDebugging";
            this.chkEnableDebugging.UseVisualStyleBackColor = true;
            this.chkEnableDebugging.CheckedChanged += new System.EventHandler(this.chkEnableDebugging_CheckedChanged);
            // 
            // chkLogToFile
            // 
            resources.ApplyResources(this.chkLogToFile, "chkLogToFile");
            this.chkLogToFile.Name = "chkLogToFile";
            this.chkLogToFile.UseVisualStyleBackColor = true;
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.helpThinKfs);
            this.tabAdvanced.Controls.Add(this.chkPreserveFileHistory);
            this.tabAdvanced.Controls.Add(this.groupBox8);
            this.tabAdvanced.Controls.Add(this.boxEvent);
            resources.ApplyResources(this.tabAdvanced, "tabAdvanced");
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // helpThinKfs
            // 
            this.helpThinKfs.Image = global::kwm.Properties.Resources.help_16x16;
            resources.ApplyResources(this.helpThinKfs, "helpThinKfs");
            this.helpThinKfs.Name = "helpThinKfs";
            this.helpThinKfs.TabStop = false;
            this.helpThinKfs.Click += new System.EventHandler(this.helpThinKfs_Click);
            // 
            // chkPreserveFileHistory
            // 
            resources.ApplyResources(this.chkPreserveFileHistory, "chkPreserveFileHistory");
            this.chkPreserveFileHistory.Name = "chkPreserveFileHistory";
            this.chkPreserveFileHistory.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lblRestart);
            this.groupBox8.Controls.Add(this.btnStorePathBrowse);
            this.groupBox8.Controls.Add(this.label4);
            this.groupBox8.Controls.Add(this.rbFileStoreCustom);
            this.groupBox8.Controls.Add(this.rbFileStoreMyDocs);
            this.groupBox8.Controls.Add(this.txtCustomStorePath);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // lblRestart
            // 
            resources.ApplyResources(this.lblRestart, "lblRestart");
            this.lblRestart.ForeColor = System.Drawing.Color.Red;
            this.lblRestart.Name = "lblRestart";
            // 
            // btnStorePathBrowse
            // 
            resources.ApplyResources(this.btnStorePathBrowse, "btnStorePathBrowse");
            this.btnStorePathBrowse.Name = "btnStorePathBrowse";
            this.btnStorePathBrowse.UseVisualStyleBackColor = true;
            this.btnStorePathBrowse.Click += new System.EventHandler(this.btnStorePathBrowse_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // rbFileStoreCustom
            // 
            resources.ApplyResources(this.rbFileStoreCustom, "rbFileStoreCustom");
            this.rbFileStoreCustom.Name = "rbFileStoreCustom";
            this.rbFileStoreCustom.UseVisualStyleBackColor = true;
            this.rbFileStoreCustom.CheckedChanged += new System.EventHandler(this.rbFileStoreCustom_CheckedChanged);
            // 
            // rbFileStoreMyDocs
            // 
            resources.ApplyResources(this.rbFileStoreMyDocs, "rbFileStoreMyDocs");
            this.rbFileStoreMyDocs.Checked = true;
            this.rbFileStoreMyDocs.Name = "rbFileStoreMyDocs";
            this.rbFileStoreMyDocs.TabStop = true;
            this.rbFileStoreMyDocs.UseVisualStyleBackColor = true;
            this.rbFileStoreMyDocs.CheckedChanged += new System.EventHandler(this.rbFileStoreMyDocs_CheckedChanged);
            // 
            // txtCustomStorePath
            // 
            resources.ApplyResources(this.txtCustomStorePath, "txtCustomStorePath");
            this.txtCustomStorePath.Name = "txtCustomStorePath";
            // 
            // boxEvent
            // 
            this.boxEvent.Controls.Add(this.label6);
            this.boxEvent.Controls.Add(this.pictureBox1);
            this.boxEvent.Controls.Add(this.label5);
            this.boxEvent.Controls.Add(this.txtNotifDuration);
            this.boxEvent.Controls.Add(this.chkShowNotification);
            resources.ApplyResources(this.boxEvent, "boxEvent");
            this.boxEvent.Name = "boxEvent";
            this.boxEvent.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::kwm.Properties.Resources.notificationScreenShot;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // txtNotifDuration
            // 
            resources.ApplyResources(this.txtNotifDuration, "txtNotifDuration");
            this.txtNotifDuration.Name = "txtNotifDuration";
            // 
            // chkShowNotification
            // 
            resources.ApplyResources(this.chkShowNotification, "chkShowNotification");
            this.chkShowNotification.Name = "chkShowNotification";
            this.chkShowNotification.UseVisualStyleBackColor = true;
            this.chkShowNotification.CheckedChanged += new System.EventHandler(this.chkShowNotification_CheckedChanged);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CustomPathBrowseDialog
            // 
            resources.ApplyResources(this.CustomPathBrowseDialog, "CustomPathBrowseDialog");
            // 
            // frmOptions
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.optionTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowInTaskbar = false;
            this.optionTabs.ResumeLayout(false);
            this.tabSupport.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.helpThinKfs)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.boxEvent.ResumeLayout(false);
            this.boxEvent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl optionTabs;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabSupport;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton ktlsDebug;
        private System.Windows.Forms.RadioButton ktlsMin;
        private System.Windows.Forms.RadioButton ktlsNone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkEnableDebugging;
        private System.Windows.Forms.CheckBox chkLogToFile;
        private System.Windows.Forms.GroupBox boxEvent;
        private System.Windows.Forms.CheckBox chkShowNotification;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboLogLevel;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnViewKcsLogs;
        private System.Windows.Forms.TabPage tabAdvanced;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton rbFileStoreMyDocs;
        private System.Windows.Forms.TextBox txtCustomStorePath;
        private System.Windows.Forms.Button btnStorePathBrowse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbFileStoreCustom;
        private System.Windows.Forms.FolderBrowserDialog CustomPathBrowseDialog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKasPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKasAddr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNotifDuration;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblRestart;
        private System.Windows.Forms.CheckBox chkPreserveFileHistory;
        private System.Windows.Forms.PictureBox helpThinKfs;
        private System.Windows.Forms.Label label6;
    }
}