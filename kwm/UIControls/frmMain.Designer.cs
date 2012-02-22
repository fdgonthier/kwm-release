namespace kwm
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.splitContainerWsMembers = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvWorkspaces = new kwm.Utils.KTreeView();
            this.tvWorkspacesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CmWorkOnline = new System.Windows.Forms.ToolStripMenuItem();
            this.CmWorkOffline = new System.Windows.Forms.ToolStripMenuItem();
            this.CmCollapse = new System.Windows.Forms.ToolStripMenuItem();
            this.CmExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CmCreateNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CmRenameFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.CmRenameKws = new System.Windows.Forms.ToolStripMenuItem();
            this.CmDeleteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.CmDeleteKws = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.CmAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.CmExport = new System.Windows.Forms.ToolStripMenuItem();
            this.CmRebuild = new System.Windows.Forms.ToolStripMenuItem();
            this.CmDisableKws = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.CmRemoveKwsFromList = new System.Windows.Forms.ToolStripMenuItem();
            this.CmDeleteFromServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.CmProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblLinkInviteUsers = new System.Windows.Forms.LinkLabel();
            this.lstUsers = new System.Windows.Forms.ListView();
            this.lstUsersContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResendInvitation = new System.Windows.Forms.ToolStripMenuItem();
            this.ResetPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ShowProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.SplitHeaderAndFiles = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.picExpand = new System.Windows.Forms.PictureBox();
            this.lnkShowProperties = new System.Windows.Forms.LinkLabel();
            this.lnkAdvancedView = new System.Windows.Forms.LinkLabel();
            this.kwsHeader = new kwm.ucKwsHeaderToolbar();
            this.createKwsHeader = new kwm.ucCreateKwsHeader();
            this.ApplicationsTabControl = new VSControls.XPTabControl();
            this.tabFiles = new System.Windows.Forms.TabPage();
            this.appKfsControl = new kwm.KwmAppControls.AppKfs.AppKfsControl(this.components);
            this.tabSS = new System.Windows.Forms.TabPage();
            this.appAppSharing = new kwm.KwmAppControls.AppScreenSharingControl();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.appChatbox = new kwm.KwmAppControls.AppChatboxControl(this.components);
            this.toolStripCommands = new System.Windows.Forms.ToolStrip();
            this.btnGenNew = new System.Windows.Forms.ToolStripSplitButton();
            this.btnNewKws = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNewKwsFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDeleteKws = new System.Windows.Forms.ToolStripButton();
            this.myMainMenu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.FileContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MmNewKwsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MmNewFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MmImportTeamboxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MmExportTeamboxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MmExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MmOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quickStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MmConfigurationWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upgradeAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MmDebuggingConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MmAboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripInvite = new System.Windows.Forms.ToolStrip();
            this.btnCustomInvite = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.btnInvite = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripKfs = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.ToolStripSplitButton();
            this.addFileToolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderToolstrip = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSync = new System.Windows.Forms.ToolStripButton();
            this.btnViewOffline = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.ViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.trayExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.kasStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ProgressText = new System.Windows.Forms.ToolStripStatusLabel();
            this.Progress = new System.Windows.Forms.ToolStripProgressBar();
            this.AppStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblKwsName = new System.Windows.Forms.ToolStripStatusLabel();
            this.importFilesToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.importFolderToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.splitContainerWsMembers.Panel1.SuspendLayout();
            this.splitContainerWsMembers.Panel2.SuspendLayout();
            this.splitContainerWsMembers.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tvWorkspacesContextMenu.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.lstUsersContextMenu.SuspendLayout();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.SplitHeaderAndFiles.Panel1.SuspendLayout();
            this.SplitHeaderAndFiles.Panel2.SuspendLayout();
            this.SplitHeaderAndFiles.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExpand)).BeginInit();
            this.ApplicationsTabControl.SuspendLayout();
            this.tabFiles.SuspendLayout();
            this.tabSS.SuspendLayout();
            this.toolStripCommands.SuspendLayout();
            this.myMainMenu.SuspendLayout();
            this.FileContextMenuStrip.SuspendLayout();
            this.ToolsContextMenuStrip.SuspendLayout();
            this.AboutContextMenuStrip.SuspendLayout();
            this.toolStripInvite.SuspendLayout();
            this.toolStripKfs.SuspendLayout();
            this.trayIconMenu.SuspendLayout();
            this.AppStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripInvite);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripCommands);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripKfs);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.myMainMenu);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainerLeft);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // splitContainerLeft
            // 
            resources.ApplyResources(this.splitContainerLeft, "splitContainerLeft");
            this.splitContainerLeft.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerLeft.Name = "splitContainerLeft";
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.splitContainerWsMembers);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerLeft.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // splitContainerWsMembers
            // 
            resources.ApplyResources(this.splitContainerWsMembers, "splitContainerWsMembers");
            this.splitContainerWsMembers.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerWsMembers.MinimumSize = new System.Drawing.Size(100, 0);
            this.splitContainerWsMembers.Name = "splitContainerWsMembers";
            // 
            // splitContainerWsMembers.Panel1
            // 
            this.splitContainerWsMembers.Panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitContainerWsMembers.Panel1.Controls.Add(this.groupBox1);
            this.splitContainerWsMembers.Panel1.Controls.Add(this.label2);
            // 
            // splitContainerWsMembers.Panel2
            // 
            this.splitContainerWsMembers.Panel2.Controls.Add(this.toolStripContainer2);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tvWorkspaces);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tvWorkspaces
            // 
            this.tvWorkspaces.AllowContextMenuOnNothing = true;
            this.tvWorkspaces.AllowDrop = true;
            this.tvWorkspaces.ContextMenuStrip = this.tvWorkspacesContextMenu;
            resources.ApplyResources(this.tvWorkspaces, "tvWorkspaces");
            this.tvWorkspaces.FullRowSelect = true;
            this.tvWorkspaces.HideSelection = false;
            this.tvWorkspaces.LabelEdit = true;
            this.tvWorkspaces.Name = "tvWorkspaces";
            this.tvWorkspaces.ShowNodeToolTips = true;
            this.tvWorkspaces.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvWorkspaces_AfterCollapse);
            this.tvWorkspaces.DragLeave += new System.EventHandler(this.tvWorkspaces_DragLeave);
            this.tvWorkspaces.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvWorkspaces_AfterLabelEdit);
            this.tvWorkspaces.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvWorkspaces_DragDrop);
            this.tvWorkspaces.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvWorkspaces_AfterSelect);
            this.tvWorkspaces.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvWorkspaces_DragEnter);
            this.tvWorkspaces.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvWorkspaces_BeforeLabelEdit);
            this.tvWorkspaces.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvWorkspaces_KeyDown);
            this.tvWorkspaces.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvWorkspaces_AfterExpand);
            this.tvWorkspaces.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvWorkspaces_ItemDrag);
            this.tvWorkspaces.DragOver += new System.Windows.Forms.DragEventHandler(this.tvWorkspaces_DragOver);
            // 
            // tvWorkspacesContextMenu
            // 
            this.tvWorkspacesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmWorkOnline,
            this.CmWorkOffline,
            this.CmCollapse,
            this.CmExpand,
            this.separator1,
            this.CmCreateNewFolder,
            this.separator2,
            this.CmRenameFolder,
            this.CmRenameKws,
            this.CmDeleteFolder,
            this.CmDeleteKws,
            this.toolStripSeparator8,
            this.CmAdvanced,
            this.toolStripSeparator7,
            this.CmProperties});
            this.tvWorkspacesContextMenu.Name = "lstWorkspacesContextMenu";
            this.tvWorkspacesContextMenu.ShowImageMargin = false;
            resources.ApplyResources(this.tvWorkspacesContextMenu, "tvWorkspacesContextMenu");
            this.tvWorkspacesContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tvWorkspacesContextMenu_ItemClicked);
            this.tvWorkspacesContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tvWorkspacesContextMenu_Opening);
            this.tvWorkspacesContextMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.tvWorkspacesContextMenu_Closing);
            // 
            // CmWorkOnline
            // 
            this.CmWorkOnline.Name = "CmWorkOnline";
            resources.ApplyResources(this.CmWorkOnline, "CmWorkOnline");
            this.CmWorkOnline.Click += new System.EventHandler(this.CmWorkOnline_Click);
            // 
            // CmWorkOffline
            // 
            this.CmWorkOffline.Name = "CmWorkOffline";
            resources.ApplyResources(this.CmWorkOffline, "CmWorkOffline");
            this.CmWorkOffline.Click += new System.EventHandler(this.CmWorkOffline_Click);
            // 
            // CmCollapse
            // 
            resources.ApplyResources(this.CmCollapse, "CmCollapse");
            this.CmCollapse.Name = "CmCollapse";
            this.CmCollapse.Click += new System.EventHandler(this.CmCollapse_Click);
            // 
            // CmExpand
            // 
            resources.ApplyResources(this.CmExpand, "CmExpand");
            this.CmExpand.Name = "CmExpand";
            this.CmExpand.Click += new System.EventHandler(this.CmExpand_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            resources.ApplyResources(this.separator1, "separator1");
            // 
            // CmCreateNewFolder
            // 
            this.CmCreateNewFolder.Name = "CmCreateNewFolder";
            resources.ApplyResources(this.CmCreateNewFolder, "CmCreateNewFolder");
            this.CmCreateNewFolder.Click += new System.EventHandler(this.CmCreateNewFolder_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            resources.ApplyResources(this.separator2, "separator2");
            // 
            // CmRenameFolder
            // 
            this.CmRenameFolder.Name = "CmRenameFolder";
            resources.ApplyResources(this.CmRenameFolder, "CmRenameFolder");
            this.CmRenameFolder.Click += new System.EventHandler(this.CmRenameFolder_Click);
            // 
            // CmRenameKws
            // 
            this.CmRenameKws.Name = "CmRenameKws";
            resources.ApplyResources(this.CmRenameKws, "CmRenameKws");
            this.CmRenameKws.Click += new System.EventHandler(this.CmRenameKws_Click);
            // 
            // CmDeleteFolder
            // 
            this.CmDeleteFolder.Name = "CmDeleteFolder";
            resources.ApplyResources(this.CmDeleteFolder, "CmDeleteFolder");
            this.CmDeleteFolder.Click += new System.EventHandler(this.CmDeleteFolder_Click);
            // 
            // CmDeleteKws
            // 
            this.CmDeleteKws.Name = "CmDeleteKws";
            resources.ApplyResources(this.CmDeleteKws, "CmDeleteKws");
            this.CmDeleteKws.Click += new System.EventHandler(this.CmDeleteKws_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // CmAdvanced
            // 
            this.CmAdvanced.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmExport,
            this.CmRebuild,
            this.CmDisableKws,
            this.toolStripSeparator9,
            this.CmRemoveKwsFromList,
            this.CmDeleteFromServer});
            this.CmAdvanced.Name = "CmAdvanced";
            resources.ApplyResources(this.CmAdvanced, "CmAdvanced");
            this.CmAdvanced.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CmAdvanced_DropDownItemClicked);
            // 
            // CmExport
            // 
            this.CmExport.Name = "CmExport";
            resources.ApplyResources(this.CmExport, "CmExport");
            this.CmExport.Click += new System.EventHandler(this.CmExport_Click);
            // 
            // CmRebuild
            // 
            this.CmRebuild.Name = "CmRebuild";
            resources.ApplyResources(this.CmRebuild, "CmRebuild");
            this.CmRebuild.Click += new System.EventHandler(this.CmRebuild_Click);
            // 
            // CmDisableKws
            // 
            this.CmDisableKws.Name = "CmDisableKws";
            resources.ApplyResources(this.CmDisableKws, "CmDisableKws");
            this.CmDisableKws.Click += new System.EventHandler(this.CmDisableKws_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // CmRemoveKwsFromList
            // 
            this.CmRemoveKwsFromList.Name = "CmRemoveKwsFromList";
            resources.ApplyResources(this.CmRemoveKwsFromList, "CmRemoveKwsFromList");
            this.CmRemoveKwsFromList.Click += new System.EventHandler(this.CmRemoveKwsFromList_Click);
            // 
            // CmDeleteFromServer
            // 
            this.CmDeleteFromServer.Name = "CmDeleteFromServer";
            resources.ApplyResources(this.CmDeleteFromServer, "CmDeleteFromServer");
            this.CmDeleteFromServer.Click += new System.EventHandler(this.CmDeleteKwsOnServer_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // CmProperties
            // 
            this.CmProperties.Name = "CmProperties";
            resources.ApplyResources(this.CmProperties, "CmProperties");
            this.CmProperties.Click += new System.EventHandler(this.CmKwsProperties_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Name = "label2";
            // 
            // toolStripContainer2
            // 
            resources.ApplyResources(this.toolStripContainer2, "toolStripContainer2");
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.groupBox4);
            resources.ApplyResources(this.toolStripContainer2.ContentPanel, "toolStripContainer2.ContentPanel");
            this.toolStripContainer2.LeftToolStripPanelVisible = false;
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.RightToolStripPanelVisible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblLinkInviteUsers);
            this.groupBox4.Controls.Add(this.lstUsers);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // lblLinkInviteUsers
            // 
            resources.ApplyResources(this.lblLinkInviteUsers, "lblLinkInviteUsers");
            this.lblLinkInviteUsers.BackColor = System.Drawing.SystemColors.Window;
            this.lblLinkInviteUsers.Name = "lblLinkInviteUsers";
            this.lblLinkInviteUsers.TabStop = true;
            this.lblLinkInviteUsers.Click += new System.EventHandler(this.btnCustomInvite_Click);
            // 
            // lstUsers
            // 
            this.lstUsers.AllowColumnReorder = true;
            this.lstUsers.ContextMenuStrip = this.lstUsersContextMenu;
            resources.ApplyResources(this.lstUsers, "lstUsers");
            this.lstUsers.FullRowSelect = true;
            this.lstUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstUsers.HideSelection = false;
            this.lstUsers.LabelEdit = true;
            this.lstUsers.MultiSelect = false;
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.ShowItemToolTips = true;
            this.lstUsers.UseCompatibleStateImageBehavior = false;
            this.lstUsers.View = System.Windows.Forms.View.Details;
            this.lstUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstUsers_MouseDoubleClick);
            this.lstUsers.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstusers_AfterLabelEdit);
            this.lstUsers.SelectedIndexChanged += new System.EventHandler(this.lstUsers_SelectedIndexChanged);
            this.lstUsers.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstUsers_BeforeLabelEdit);
            this.lstUsers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstUsers_KeyDown);
            // 
            // lstUsersContextMenu
            // 
            this.lstUsersContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResendInvitation,
            this.ResetPassword,
            this.Copy,
            this.toolStripSeparator5,
            this.Delete,
            this.toolStripSeparator6,
            this.ShowProperties});
            this.lstUsersContextMenu.Name = "lstUsersContextMenu";
            resources.ApplyResources(this.lstUsersContextMenu, "lstUsersContextMenu");
            this.lstUsersContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.lstUsersContextMenu_ItemClicked);
            this.lstUsersContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.lstUsersContextMenu_Opening);
            this.lstUsersContextMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.lstUsersContextMenu_Closing);
            // 
            // ResendInvitation
            // 
            this.ResendInvitation.Name = "ResendInvitation";
            resources.ApplyResources(this.ResendInvitation, "ResendInvitation");
            this.ResendInvitation.Tag = "true";
            this.ResendInvitation.Click += new System.EventHandler(this.ResendInvitation_Click);
            // 
            // ResetPassword
            // 
            this.ResetPassword.Name = "ResetPassword";
            resources.ApplyResources(this.ResetPassword, "ResetPassword");
            this.ResetPassword.Tag = "true";
            this.ResetPassword.Click += new System.EventHandler(this.resetPasswordToolStripMenuItem_Click);
            // 
            // Copy
            // 
            this.Copy.Name = "Copy";
            resources.ApplyResources(this.Copy, "Copy");
            this.Copy.Tag = "true";
            this.Copy.Click += new System.EventHandler(this.CopyEmailAddr_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            resources.ApplyResources(this.Delete, "Delete");
            this.Delete.Tag = "true";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // ShowProperties
            // 
            this.ShowProperties.Name = "ShowProperties";
            resources.ApplyResources(this.ShowProperties, "ShowProperties");
            this.ShowProperties.Tag = "true";
            this.ShowProperties.Click += new System.EventHandler(this.UserProperties_Click);
            // 
            // splitContainerRight
            // 
            resources.ApplyResources(this.splitContainerRight, "splitContainerRight");
            this.splitContainerRight.Name = "splitContainerRight";
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.SplitHeaderAndFiles);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.appChatbox);
            // 
            // SplitHeaderAndFiles
            // 
            resources.ApplyResources(this.SplitHeaderAndFiles, "SplitHeaderAndFiles");
            this.SplitHeaderAndFiles.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitHeaderAndFiles.Name = "SplitHeaderAndFiles";
            // 
            // SplitHeaderAndFiles.Panel1
            // 
            this.SplitHeaderAndFiles.Panel1.Controls.Add(this.groupBox3);
            // 
            // SplitHeaderAndFiles.Panel2
            // 
            this.SplitHeaderAndFiles.Panel2.Controls.Add(this.ApplicationsTabControl);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.picExpand);
            this.groupBox3.Controls.Add(this.lnkShowProperties);
            this.groupBox3.Controls.Add(this.lnkAdvancedView);
            this.groupBox3.Controls.Add(this.kwsHeader);
            this.groupBox3.Controls.Add(this.createKwsHeader);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // picExpand
            // 
            resources.ApplyResources(this.picExpand, "picExpand");
            this.picExpand.Name = "picExpand";
            this.picExpand.TabStop = false;
            this.picExpand.MouseLeave += new System.EventHandler(this.picExpand_MouseLeave);
            this.picExpand.Click += new System.EventHandler(this.picExpand_Click);
            this.picExpand.MouseEnter += new System.EventHandler(this.picExpand_MouseEnter);
            // 
            // lnkShowProperties
            // 
            resources.ApplyResources(this.lnkShowProperties, "lnkShowProperties");
            this.lnkShowProperties.Name = "lnkShowProperties";
            this.lnkShowProperties.TabStop = true;
            this.lnkShowProperties.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // lnkAdvancedView
            // 
            resources.ApplyResources(this.lnkAdvancedView, "lnkAdvancedView");
            this.lnkAdvancedView.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkAdvancedView.LinkColor = System.Drawing.Color.Black;
            this.lnkAdvancedView.Name = "lnkAdvancedView";
            this.lnkAdvancedView.TabStop = true;
            this.lnkAdvancedView.VisitedLinkColor = System.Drawing.Color.Black;
            this.lnkAdvancedView.MouseLeave += new System.EventHandler(this.picExpand_MouseLeave);
            this.lnkAdvancedView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.lnkAdvancedView.MouseEnter += new System.EventHandler(this.picExpand_MouseEnter);
            // 
            // kwsHeader
            // 
            resources.ApplyResources(this.kwsHeader, "kwsHeader");
            this.kwsHeader.Name = "kwsHeader";
            // 
            // createKwsHeader
            // 
            resources.ApplyResources(this.createKwsHeader, "createKwsHeader");
            this.createKwsHeader.Name = "createKwsHeader";
            // 
            // ApplicationsTabControl
            // 
            this.ApplicationsTabControl.Controls.Add(this.tabFiles);
            this.ApplicationsTabControl.Controls.Add(this.tabSS);
            resources.ApplyResources(this.ApplicationsTabControl, "ApplicationsTabControl");
            this.ApplicationsTabControl.ImageList = this.imgList;
            this.ApplicationsTabControl.Name = "ApplicationsTabControl";
            this.ApplicationsTabControl.SelectedIndex = 0;
            // 
            // tabFiles
            // 
            this.tabFiles.Controls.Add(this.appKfsControl);
            resources.ApplyResources(this.tabFiles, "tabFiles");
            this.tabFiles.Name = "tabFiles";
            this.tabFiles.UseVisualStyleBackColor = true;
            // 
            // appKfsControl
            // 
            resources.ApplyResources(this.appKfsControl, "appKfsControl");
            this.appKfsControl.Name = "appKfsControl";
            // 
            // tabSS
            // 
            this.tabSS.Controls.Add(this.appAppSharing);
            resources.ApplyResources(this.tabSS, "tabSS");
            this.tabSS.Name = "tabSS";
            this.tabSS.UseVisualStyleBackColor = true;
            // 
            // appAppSharing
            // 
            resources.ApplyResources(this.appAppSharing, "appAppSharing");
            this.appAppSharing.Name = "appAppSharing";
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "folder_page_white.png");
            this.imgList.Images.SetKeyName(1, "television-16x16.png");
            // 
            // appChatbox
            // 
            resources.ApplyResources(this.appChatbox, "appChatbox");
            this.appChatbox.Name = "appChatbox";
            // 
            // toolStripCommands
            // 
            resources.ApplyResources(this.toolStripCommands, "toolStripCommands");
            this.toolStripCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGenNew,
            this.btnDeleteKws});
            this.toolStripCommands.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripCommands.Name = "toolStripCommands";
            // 
            // btnGenNew
            // 
            this.btnGenNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewKws,
            this.btnNewKwsFolder});
            resources.ApplyResources(this.btnGenNew, "btnGenNew");
            this.btnGenNew.Name = "btnGenNew";
            this.btnGenNew.ButtonClick += new System.EventHandler(this.btnGenNew_ButtonClick);
            // 
            // btnNewKws
            // 
            resources.ApplyResources(this.btnNewKws, "btnNewKws");
            this.btnNewKws.Name = "btnNewKws";
            this.btnNewKws.Click += new System.EventHandler(this.btnNewKws_Click);
            // 
            // btnNewKwsFolder
            // 
            resources.ApplyResources(this.btnNewKwsFolder, "btnNewKwsFolder");
            this.btnNewKwsFolder.Name = "btnNewKwsFolder";
            this.btnNewKwsFolder.Click += new System.EventHandler(this.btnNewKwsFolder_Click);
            // 
            // btnDeleteKws
            // 
            this.btnDeleteKws.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.btnDeleteKws, "btnDeleteKws");
            this.btnDeleteKws.Name = "btnDeleteKws";
            this.btnDeleteKws.Click += new System.EventHandler(this.btnDeleteKws_Click);
            // 
            // myMainMenu
            // 
            resources.ApplyResources(this.myMainMenu, "myMainMenu");
            this.myMainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.myMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.myMainMenu.Name = "myMainMenu";
            this.myMainMenu.ShowItemToolTips = true;
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItem5.DropDown = this.FileContextMenuStrip;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            // 
            // FileContextMenuStrip
            // 
            this.FileContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.toolStripSeparator2,
            this.MmImportTeamboxesToolStripMenuItem,
            this.MmExportTeamboxesToolStripMenuItem,
            this.toolStripSeparator1,
            this.MmExitToolStripMenuItem});
            this.FileContextMenuStrip.Name = "FileContextMenuStrip";
            this.FileContextMenuStrip.OwnerItem = this.fileToolStripMenuItem;
            resources.ApplyResources(this.FileContextMenuStrip, "FileContextMenuStrip");
            this.FileContextMenuStrip.ShowImageMargin = false;
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MmNewKwsToolStripMenuItem,
            this.MmNewFolderToolStripMenuItem});
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            resources.ApplyResources(this.newToolStripMenuItem1, "newToolStripMenuItem1");
            // 
            // MmNewKwsToolStripMenuItem
            // 
            resources.ApplyResources(this.MmNewKwsToolStripMenuItem, "MmNewKwsToolStripMenuItem");
            this.MmNewKwsToolStripMenuItem.Name = "MmNewKwsToolStripMenuItem";
            this.MmNewKwsToolStripMenuItem.Click += new System.EventHandler(this.MmNewKwsToolStripMenuItem_Click);
            // 
            // MmNewFolderToolStripMenuItem
            // 
            resources.ApplyResources(this.MmNewFolderToolStripMenuItem, "MmNewFolderToolStripMenuItem");
            this.MmNewFolderToolStripMenuItem.Name = "MmNewFolderToolStripMenuItem";
            this.MmNewFolderToolStripMenuItem.Click += new System.EventHandler(this.MmNewFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // MmImportTeamboxesToolStripMenuItem
            // 
            this.MmImportTeamboxesToolStripMenuItem.Name = "MmImportTeamboxesToolStripMenuItem";
            resources.ApplyResources(this.MmImportTeamboxesToolStripMenuItem, "MmImportTeamboxesToolStripMenuItem");
            this.MmImportTeamboxesToolStripMenuItem.Click += new System.EventHandler(this.Import_Click);
            // 
            // MmExportTeamboxesToolStripMenuItem
            // 
            this.MmExportTeamboxesToolStripMenuItem.Name = "MmExportTeamboxesToolStripMenuItem";
            resources.ApplyResources(this.MmExportTeamboxesToolStripMenuItem, "MmExportTeamboxesToolStripMenuItem");
            this.MmExportTeamboxesToolStripMenuItem.Click += new System.EventHandler(this.Export_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // MmExitToolStripMenuItem
            // 
            this.MmExitToolStripMenuItem.Name = "MmExitToolStripMenuItem";
            resources.ApplyResources(this.MmExitToolStripMenuItem, "MmExitToolStripMenuItem");
            this.MmExitToolStripMenuItem.Click += new System.EventHandler(this.OnFileExitClick);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.DropDown = this.ToolsContextMenuStrip;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
            // 
            // ToolsContextMenuStrip
            // 
            this.ToolsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MmOptionsToolStripMenuItem});
            this.ToolsContextMenuStrip.Name = "ToolsContextMenuStrip";
            this.ToolsContextMenuStrip.OwnerItem = this.toolsToolStripMenuItem;
            resources.ApplyResources(this.ToolsContextMenuStrip, "ToolsContextMenuStrip");
            this.ToolsContextMenuStrip.ShowImageMargin = false;
            // 
            // MmOptionsToolStripMenuItem
            // 
            this.MmOptionsToolStripMenuItem.Name = "MmOptionsToolStripMenuItem";
            resources.ApplyResources(this.MmOptionsToolStripMenuItem, "MmOptionsToolStripMenuItem");
            this.MmOptionsToolStripMenuItem.Click += new System.EventHandler(this.OnToolsOptionsFormClick);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.DropDown = this.AboutContextMenuStrip;
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            resources.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
            // 
            // AboutContextMenuStrip
            // 
            this.AboutContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickStartToolStripMenuItem,
            this.userManualToolStripMenuItem,
            this.toolStripSeparator10,
            this.licenseToolStripMenuItem,
            this.toolStripSeparator3,
            this.MmDebuggingConsoleToolStripMenuItem,
            this.MmAboutToolStripMenuItem});
            this.AboutContextMenuStrip.Name = "AboutContextMenuStrip";
            this.AboutContextMenuStrip.OwnerItem = this.helpToolStripMenuItem;
            resources.ApplyResources(this.AboutContextMenuStrip, "AboutContextMenuStrip");
            this.AboutContextMenuStrip.ShowImageMargin = false;
            // 
            // quickStartToolStripMenuItem
            // 
            this.quickStartToolStripMenuItem.Name = "quickStartToolStripMenuItem";
            resources.ApplyResources(this.quickStartToolStripMenuItem, "quickStartToolStripMenuItem");
            this.quickStartToolStripMenuItem.Click += new System.EventHandler(this.quickStartToolStripMenuItem_Click);
            // 
            // userManualToolStripMenuItem
            // 
            this.userManualToolStripMenuItem.Name = "userManualToolStripMenuItem";
            resources.ApplyResources(this.userManualToolStripMenuItem, "userManualToolStripMenuItem");
            this.userManualToolStripMenuItem.Click += new System.EventHandler(this.userManualToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MmConfigurationWizardToolStripMenuItem,
            this.upgradeAccountToolStripMenuItem});
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            resources.ApplyResources(this.licenseToolStripMenuItem, "licenseToolStripMenuItem");
            // 
            // MmConfigurationWizardToolStripMenuItem
            // 
            this.MmConfigurationWizardToolStripMenuItem.Name = "MmConfigurationWizardToolStripMenuItem";
            resources.ApplyResources(this.MmConfigurationWizardToolStripMenuItem, "MmConfigurationWizardToolStripMenuItem");
            this.MmConfigurationWizardToolStripMenuItem.Click += new System.EventHandler(this.MmConfigurationWizardToolStripMenuItem_Click);
            // 
            // upgradeAccountToolStripMenuItem
            // 
            this.upgradeAccountToolStripMenuItem.Name = "upgradeAccountToolStripMenuItem";
            resources.ApplyResources(this.upgradeAccountToolStripMenuItem, "upgradeAccountToolStripMenuItem");
            this.upgradeAccountToolStripMenuItem.Click += new System.EventHandler(this.upgradeAccountToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // MmDebuggingConsoleToolStripMenuItem
            // 
            this.MmDebuggingConsoleToolStripMenuItem.Name = "MmDebuggingConsoleToolStripMenuItem";
            resources.ApplyResources(this.MmDebuggingConsoleToolStripMenuItem, "MmDebuggingConsoleToolStripMenuItem");
            this.MmDebuggingConsoleToolStripMenuItem.Click += new System.EventHandler(this.OnShowConsoleFormClick);
            // 
            // MmAboutToolStripMenuItem
            // 
            this.MmAboutToolStripMenuItem.Name = "MmAboutToolStripMenuItem";
            resources.ApplyResources(this.MmAboutToolStripMenuItem, "MmAboutToolStripMenuItem");
            this.MmAboutToolStripMenuItem.Click += new System.EventHandler(this.OnShowAboutFormClick);
            // 
            // toolStripInvite
            // 
            resources.ApplyResources(this.toolStripInvite, "toolStripInvite");
            this.toolStripInvite.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCustomInvite,
            this.toolStripSeparator13,
            this.btnInvite});
            this.toolStripInvite.Name = "toolStripInvite";
            // 
            // btnCustomInvite
            // 
            this.btnCustomInvite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCustomInvite.Image = global::kwm.Properties.Resources.user_add_many_16x16;
            resources.ApplyResources(this.btnCustomInvite, "btnCustomInvite");
            this.btnCustomInvite.Name = "btnCustomInvite";
            this.btnCustomInvite.Click += new System.EventHandler(this.btnCustomInvite_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // btnInvite
            // 
            this.btnInvite.Image = global::kwm.Properties.Resources.user_add_16x16;
            resources.ApplyResources(this.btnInvite, "btnInvite");
            this.btnInvite.Name = "btnInvite";
            this.btnInvite.ButtonClick += new System.EventHandler(this.btnInvite_Click);
            this.btnInvite.DropDownOpening += new System.EventHandler(this.btnInvite_DropDownOpening);
            // 
            // toolStripKfs
            // 
            resources.ApplyResources(this.toolStripKfs, "toolStripKfs");
            this.toolStripKfs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnAdd,
            this.btnSync,
            this.btnViewOffline,
            this.toolStripSeparator12});
            this.toolStripKfs.Name = "toolStripKfs";
            // 
            // btnNew
            // 
            this.btnNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripSeparator14,
            this.toolStripMenuItem3});
            resources.ApplyResources(this.btnNew, "btnNew");
            this.btnNew.Name = "btnNew";
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            // 
            // btnAdd
            // 
            this.btnAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileToolstrip,
            this.addFolderToolstrip});
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            // 
            // addFileToolstrip
            // 
            resources.ApplyResources(this.addFileToolstrip, "addFileToolstrip");
            this.addFileToolstrip.Name = "addFileToolstrip";
            // 
            // addFolderToolstrip
            // 
            resources.ApplyResources(this.addFolderToolstrip, "addFolderToolstrip");
            this.addFolderToolstrip.Name = "addFolderToolstrip";
            // 
            // btnSync
            // 
            resources.ApplyResources(this.btnSync, "btnSync");
            this.btnSync.Name = "btnSync";
            // 
            // btnViewOffline
            // 
            this.btnViewOffline.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.btnViewOffline, "btnViewOffline");
            this.btnViewOffline.Name = "btnViewOffline";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolStripMenuItem.DropDown = this.FileContextMenuStrip;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDown = this.ToolsContextMenuStrip;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDown = this.AboutContextMenuStrip;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.DropDownOpening += new System.EventHandler(this.helpToolStripMenuItem_DropDownOpening);
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
            this.toolStripButton5.Name = "toolStripButton5";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
            this.toolStripButton6.Name = "toolStripButton6";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // toolStripButton2
            // 
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Name = "toolStripButton2";
            // 
            // toolStripButton4
            // 
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.Name = "toolStripButton4";
            // 
            // ViewContextMenuStrip
            // 
            this.ViewContextMenuStrip.Name = "ViewContextMenuStrip";
            this.ViewContextMenuStrip.ShowCheckMargin = true;
            this.ViewContextMenuStrip.ShowImageMargin = false;
            resources.ApplyResources(this.ViewContextMenuStrip, "ViewContextMenuStrip");
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayIconMenu;
            resources.ApplyResources(this.trayIcon, "trayIcon");
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayIconMenu
            // 
            this.trayIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayOpenToolStripMenuItem,
            this.toolStripSeparator4,
            this.trayExitToolStripMenuItem});
            this.trayIconMenu.Name = "trayIconMenu";
            this.trayIconMenu.ShowImageMargin = false;
            resources.ApplyResources(this.trayIconMenu, "trayIconMenu");
            // 
            // trayOpenToolStripMenuItem
            // 
            this.trayOpenToolStripMenuItem.Name = "trayOpenToolStripMenuItem";
            resources.ApplyResources(this.trayOpenToolStripMenuItem, "trayOpenToolStripMenuItem");
            this.trayOpenToolStripMenuItem.Click += new System.EventHandler(this.trayOpenToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // trayExitToolStripMenuItem
            // 
            this.trayExitToolStripMenuItem.Name = "trayExitToolStripMenuItem";
            resources.ApplyResources(this.trayExitToolStripMenuItem, "trayExitToolStripMenuItem");
            this.trayExitToolStripMenuItem.Click += new System.EventHandler(this.trayExitToolStripMenuItem_Click);
            // 
            // BottomToolStripPanel
            // 
            resources.ApplyResources(this.BottomToolStripPanel, "BottomToolStripPanel");
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // TopToolStripPanel
            // 
            resources.ApplyResources(this.TopToolStripPanel, "TopToolStripPanel");
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // RightToolStripPanel
            // 
            resources.ApplyResources(this.RightToolStripPanel, "RightToolStripPanel");
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // LeftToolStripPanel
            // 
            resources.ApplyResources(this.LeftToolStripPanel, "LeftToolStripPanel");
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // ContentPanel
            // 
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            // 
            // kasStatusBar
            // 
            this.kasStatusBar.Name = "kasStatusBar";
            resources.ApplyResources(this.kasStatusBar, "kasStatusBar");
            // 
            // toolStripDropDownButton1
            // 
            resources.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // ProgressText
            // 
            resources.ApplyResources(this.ProgressText, "ProgressText");
            this.ProgressText.Name = "ProgressText";
            // 
            // Progress
            // 
            this.Progress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Progress.Name = "Progress";
            resources.ApplyResources(this.Progress, "Progress");
            // 
            // AppStatusStrip
            // 
            this.AppStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            resources.ApplyResources(this.AppStatusStrip, "AppStatusStrip");
            this.AppStatusStrip.Name = "AppStatusStrip";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripStatusLabel3
            // 
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
            // 
            // lblSpacer
            // 
            this.lblSpacer.Name = "lblSpacer";
            resources.ApplyResources(this.lblSpacer, "lblSpacer");
            this.lblSpacer.Spring = true;
            // 
            // lblKwsName
            // 
            resources.ApplyResources(this.lblKwsName, "lblKwsName");
            this.lblKwsName.Name = "lblKwsName";
            // 
            // importFilesToolStrip
            // 
            resources.ApplyResources(this.importFilesToolStrip, "importFilesToolStrip");
            this.importFilesToolStrip.Name = "importFilesToolStrip";
            // 
            // importFolderToolStrip
            // 
            resources.ApplyResources(this.importFolderToolStrip, "importFolderToolStrip");
            this.importFolderToolStrip.Name = "importFolderToolStrip";
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.AppStatusStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.myMainMenu;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Deactivate += new System.EventHandler(this.frmMain_Deactivate);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.VisibleChanged += new System.EventHandler(this.frmMain_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.LocationChanged += new System.EventHandler(this.frmMain_LocationChanged);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            this.splitContainerLeft.ResumeLayout(false);
            this.splitContainerWsMembers.Panel1.ResumeLayout(false);
            this.splitContainerWsMembers.Panel1.PerformLayout();
            this.splitContainerWsMembers.Panel2.ResumeLayout(false);
            this.splitContainerWsMembers.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tvWorkspacesContextMenu.ResumeLayout(false);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.lstUsersContextMenu.ResumeLayout(false);
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            this.splitContainerRight.ResumeLayout(false);
            this.SplitHeaderAndFiles.Panel1.ResumeLayout(false);
            this.SplitHeaderAndFiles.Panel2.ResumeLayout(false);
            this.SplitHeaderAndFiles.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExpand)).EndInit();
            this.ApplicationsTabControl.ResumeLayout(false);
            this.tabFiles.ResumeLayout(false);
            this.tabFiles.PerformLayout();
            this.tabSS.ResumeLayout(false);
            this.toolStripCommands.ResumeLayout(false);
            this.toolStripCommands.PerformLayout();
            this.myMainMenu.ResumeLayout(false);
            this.myMainMenu.PerformLayout();
            this.FileContextMenuStrip.ResumeLayout(false);
            this.ToolsContextMenuStrip.ResumeLayout(false);
            this.AboutContextMenuStrip.ResumeLayout(false);
            this.toolStripInvite.ResumeLayout(false);
            this.toolStripInvite.PerformLayout();
            this.toolStripKfs.ResumeLayout(false);
            this.toolStripKfs.PerformLayout();
            this.trayIconMenu.ResumeLayout(false);
            this.AppStatusStrip.ResumeLayout(false);
            this.AppStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayIconMenu;
        private System.Windows.Forms.ToolStripMenuItem trayOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem trayExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel kasStatusBar;
        private System.Windows.Forms.ContextMenuStrip tvWorkspacesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CmWorkOnline;
        private System.Windows.Forms.ToolStripMenuItem CmWorkOffline;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripMenuItem CmDeleteKws;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.SplitContainer splitContainerWsMembers;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private kwm.KwmAppControls.AppChatboxControl appChatbox;
        private kwm.KwmAppControls.AppKfs.AppKfsControl appKfsControl;
        private System.Windows.Forms.ToolStripStatusLabel ProgressText;
        private System.Windows.Forms.ToolStripProgressBar Progress;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private kwm.Utils.KTreeView tvWorkspaces;
        private System.Windows.Forms.ToolStripMenuItem CmExpand;
        private System.Windows.Forms.ToolStripMenuItem CmCollapse;
        private System.Windows.Forms.ToolStripMenuItem CmRenameKws;
        private System.Windows.Forms.ToolStripMenuItem CmCreateNewFolder;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.MenuStrip myMainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip FileContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MmImportTeamboxesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MmExportTeamboxesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MmExitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ToolsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MmOptionsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip AboutContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MmAboutToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ViewContextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip lstUsersContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ResetPassword;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MmNewKwsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MmNewFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem MmDebuggingConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResendInvitation;
        private System.Windows.Forms.ToolStripMenuItem Copy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem ShowProperties;
        private System.Windows.Forms.ToolStripMenuItem CmAdvanced;
        private System.Windows.Forms.ToolStripMenuItem CmRebuild;
        private System.Windows.Forms.ToolStripMenuItem CmExport;
        private System.Windows.Forms.ToolStripMenuItem CmDisableKws;
        private System.Windows.Forms.ToolStripMenuItem CmProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem CmRenameFolder;
        private System.Windows.Forms.ToolStripMenuItem CmRemoveKwsFromList;
        private System.Windows.Forms.ToolStripMenuItem CmDeleteFromServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem CmDeleteFolder;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.StatusStrip AppStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblKwsName;
        private System.Windows.Forms.ToolStripStatusLabel lblSpacer;
        private System.Windows.Forms.ToolStripMenuItem quickStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MmConfigurationWizardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upgradeAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFilesToolStrip;
        private System.Windows.Forms.ToolStripMenuItem importFolderToolStrip;
        private System.Windows.Forms.ToolStrip toolStripCommands;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private ucKwsHeaderToolbar kwsHeader;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.SplitContainer SplitHeaderAndFiles;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripButton btnDeleteKws;
        private System.Windows.Forms.ToolStrip toolStripKfs;
        private System.Windows.Forms.ToolStripSplitButton btnNew;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSplitButton btnAdd;
        private System.Windows.Forms.ToolStripMenuItem addFileToolstrip;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolstrip;
        private System.Windows.Forms.ToolStripButton btnSync;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripSplitButton btnGenNew;
        private System.Windows.Forms.ToolStripMenuItem btnNewKws;
        private System.Windows.Forms.ToolStripMenuItem btnNewKwsFolder;
        private System.Windows.Forms.ToolStripButton btnViewOffline;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.LinkLabel lnkAdvancedView;
        private System.Windows.Forms.LinkLabel lnkShowProperties;
        private System.Windows.Forms.PictureBox picExpand;
        private ucCreateKwsHeader createKwsHeader;
        private System.Windows.Forms.ImageList imgList;
        private VSControls.XPTabControl ApplicationsTabControl;
        private System.Windows.Forms.TabPage tabFiles;
        private System.Windows.Forms.TabPage tabSS;
        private kwm.KwmAppControls.AppScreenSharingControl appAppSharing;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.ListView lstUsers;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStrip toolStripInvite;
        private System.Windows.Forms.ToolStripButton btnCustomInvite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.LinkLabel lblLinkInviteUsers;
        private System.Windows.Forms.ToolStripSplitButton btnInvite;
    }
}

