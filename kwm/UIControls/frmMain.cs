using System;
using System.Drawing;
using System.Windows.Forms;
using kwm.Utils;
using System.Collections;
using kwm.KwmAppControls;
using kwm.KwmAppControls.AppKfs;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Win32;
using System.IO;
using System.Media;
using Tbx.Utils;
using System.ComponentModel;
using Iesi.Collections;

/* Methods used to modify the UI elements.
 * NOTICE : these methods assume the caller is running
 * in the UI thread. This is a convention that is not enforced
 * by this code. It is forbidden to access UI elements from another
 * thread than the one that created the elements.
 */

namespace kwm
{
    public partial class frmMain : frmKBaseForm
    {
        /// <summary>
        /// Menu item from the workspace or user context menu that
        /// was clicked by the user. We handle the disabled items ourself
        /// instead of setting the Enabled property to false to be able to
        /// display tooltips (which is impossible when Enabled is set to false).
        /// The Tag property of the item is used to store wether or not
        /// the item should be enabled or not. When a "disabled" item is clicked,
        /// the on_click handler detects it and does nothing. The context menu
        /// on_closing handler is then called, this is where we detect that a
        /// disabled item was clicked and we cancel the close.
        /// </summary>
        private ToolStripItem m_contextMenuItemClicked = null;

        /// <summary>
        /// Reference to the UI broker.
        /// </summary>
        private WmUiBroker m_uiBroker;

        /// <summary>
        /// Log messages to a file.
        /// </summary>
        private CFileLogger m_fileLogger;

        /// <summary>
        /// Reference to the console window which display logged messages.
        /// </summary>
        private ConsoleWindow m_consoleWindow;

        /// <summary>
        /// Handle the generation of balloon notifications.
        /// </summary>
        private TrayIconNotifier m_trayNotifier;

        /// <summary>
        /// Helper object to manage the status bar. Only used by the UI Broker.
        /// </summary>
        public StatusBarHelper StatusBarManager;

        /// <summary>
        /// Used to change icon at each second for notification when the UI is 
        /// not visible.
        /// </summary>
        private Timer m_blinkingTimer;

        /// <summary>
        /// Track the current icon for the flashing sequence.
        /// </summary>
        private int m_currentTrayIcon = 0;

        /// <summary>
        /// Tree mapping applications IDs to application controls.
        /// </summary>
        private SortedDictionary<UInt32, BaseAppControl> m_appControlTree;

        /// <summary>
        /// Set to true if /minimized is passed on command line, 
        /// false otherwise. Check frmMain_VisibleChanged() event handler.
        /// </summary>
        private bool m_startMinimized = false;

        /// <summary>
        /// Set this flag when we explicitly want to shutdown the application.
        /// As long as it is set to false, form_closing will cancel the close.
        /// </summary>
        private bool m_exitFlag = false;

        /// <summary>
        /// Used to track what was the previous window state when handling the 
        /// SizeChanged event.
        /// </summary>
        private FormWindowState m_lastWindowState;

        /// <summary>
        /// Used to track whether we should save the various main form
        /// window settings on LocationChanged and SizeChanged events.
        /// These events are fired while we are still in the constructor and we
        /// must ignore them until the constructor is done.
        /// </summary>
        private bool m_saveWindowSettingsFlag = false;

        /// <summary>
        /// When we're updating a selection in the UI, or when we're dragging
        /// and dropping workspaces in the TreeView, we do not want to update
        /// the UI whenever the selection changes. This flag is set to true 
        /// when selection event must be ignored.   
        /// </summary>
        private bool m_ignoreSelectFlag = false;

        /// <summary>
        /// Information about the drag and drop operation in tvWorkspaces, if any.
        /// </summary>
        private TvWorkspacesDragInfo m_tvWorkspacesDragInfo = new TvWorkspacesDragInfo();

        /// <summary>
        /// Prevent reentrance in the SizeChanged event handler of the main form.
        /// </summary>
        private bool m_SizeChangedHandlerFlag = false;

        private bool m_expandedKwsHeaderFlag = false;

        /// <summary>
        /// Reference to the workspace browser.
        /// </summary>
        public KwsBrowser Browser { get { return m_uiBroker.Browser; } }

        /// <summary>
        /// True if the browser tree does not match tvWorkspaces.
        /// </summary>
        public bool StaleBrowser { get { return m_uiBroker.RebuildBrowserFlag; } }

        /// <summary>
        /// Set the image displayed in the application's status bar. Private to the
        /// StatusbarHelper.
        /// </summary>
        public Image StatusBarImage
        {
            set 
            {
                ToolStripLabel l = AppStatusStrip.Items[0] as ToolStripLabel;
                if (l == null) return;
                l.Image = value;
            }
        }

        /// <summary>
        /// Set the text displayed in the application's status bar. Private to the
        /// StatusbarHelper.
        /// </summary>
        public String StatusBarText
        {
            set
            {
                ToolStripLabel l = AppStatusStrip.Items[0] as ToolStripLabel;
                if (l == null) return;
                l.Text = value;
            }
        }

        /// <summary>
        /// Array containing all the required icons to create the Tray Icon 
        /// blinking animation.
        /// </summary>
        private Icon[] m_blinkingIcons = new Icon[16];

        public SortedDictionary<UInt32, BaseAppControl> AppControlTree
        {
            get { return m_appControlTree; }
        }

        /// <summary>
        /// Initialize the main form.
        /// </summary>
        public void Initialize(WmUiBroker broker)
        {
            InitializeComponent();
            panel1.Size = panel1.Parent.ClientSize;
            m_uiBroker = broker;
            kwsHeader.UiBroker = broker;
            createKwsHeader.UiBroker = broker;
            // Show the advanced view if so desired. Otherwise, stay with
            // the default value, which is hidden.
            if (Misc.ApplicationSettings.ShowMainFormAdvancedView)
                ShowAdvancedView();

            // Initialize the resource manager. This cannot be done before
            // this form is loaded.
            Misc.InitResourceMngr(typeof(frmMain).Assembly);

            // Correctly order the toolstrips.
            toolStripContainer1.TopToolStripPanel.Controls.Clear();
            toolStripContainer1.TopToolStripPanel.Join(myMainMenu, 0);
            toolStripContainer1.TopToolStripPanel.Join(toolStripKfs, 1);
            toolStripContainer1.TopToolStripPanel.Join(toolStripInvite, 1);
            toolStripContainer1.TopToolStripPanel.Join(toolStripCommands, 1);            

            // Save the main form handle so that another instance of KWM can
            // send messages to it. There is a race condition here that is
            // difficult to fix.
            RegistryKey key = Base.GetKwmCURegKey();
            key.SetValue("kwmWindowHandle", Handle, RegistryValueKind.String);

            this.AllowDrop = true;
            ApplicationsTabControl.AllowDrop = true;

            InitializeImages();

            m_trayNotifier = new TrayIconNotifier(trayIcon);

            ApplyOptions();

            // Set some visual controls properties.
            lstUsers.Columns.Add(Misc.GetString("Members"), -2);

            InitializeApplications();

            Logging.Log("KWM started.");

            // Handle minimization.
            this.MinimumSize = this.Size;

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg == "/minimized")
                {
                    m_startMinimized = true;
                    break;
                }
            }

            // Initialize our blinking array.
            for (int i = 0; i < 16; i++)
            {
                String obj = "anim" + (i < 10 ? "0" : "") + i;
                Bitmap b = (Bitmap)kwm.Properties.Resources.ResourceManager.GetObject(obj);
                m_blinkingIcons[i] = Misc.MakeIcon(b, 16, true);
            }

            trayIcon.Icon = m_blinkingIcons[0];

            // Handle blinking icons.
            m_blinkingTimer = new Timer();
            m_blinkingTimer.Interval = 100;
            m_blinkingTimer.Tick += new EventHandler(TrayIconFlashTimer_Tick);
            this.Activated += new EventHandler(frmMain_Activated);

            // Restore the window state.
            m_lastWindowState = this.WindowState;
            RestoreMainFormSettings();

            StatusBarManager = new StatusBarHelper(this);
            // SaveWindowSettingsFlag will be set to true in the frmMain_Shown event handler.
        }

        /// <summary>
        /// Initialize the list of images used by the KWM.
        /// </summary>
        private void InitializeImages()
        {
            tvWorkspaces.ImageList = new ImageList();
            
            tvWorkspaces.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            tvWorkspaces.ImageList.Images.Add(Color.Green.Name, KwmMisc.GetKwmStateImageFromKey(Color.Green));
            tvWorkspaces.ImageList.Images.Add(Color.Red.Name, KwmMisc.GetKwmStateImageFromKey(Color.Red));
            tvWorkspaces.ImageList.Images.Add(Color.Gray.Name, KwmMisc.GetKwmStateImageFromKey(Color.Gray));

            Bitmap img = kwm.Properties.Resources.folderClosed;
            img.MakeTransparent();
            tvWorkspaces.ImageList.Images.Add("FolderClosed", img);

            img = kwm.Properties.Resources.folderOpened;
            img.MakeTransparent();
            tvWorkspaces.ImageList.Images.Add("FolderOpened", img);
        }

        /// <summary>
        /// Initialize the workspace application controls.
        /// </summary>
        private void InitializeApplications()
        {
            m_appControlTree = new SortedDictionary<UInt32, BaseAppControl>();
            m_appControlTree.Add(KAnpType.KANP_NS_CHAT, appChatbox);
            m_appControlTree.Add(KAnpType.KANP_NS_VNC, appAppSharing);
            m_appControlTree.Add(KAnpType.KANP_NS_KFS, appKfsControl);
            appKfsControl.InitializeApp(toolStripKfs);
            m_uiBroker.UpdateAppControlDataSource(null);
        }

        /// <summary>
        /// Save all the relevant window settings in order to restore the window
        /// just like it was when the program was shut down.
        /// </summary>
        private void SaveMainFormSettings()
        {
            Logging.Log(1, "SaveMainFormSettings called. WindowState : " + this.WindowState);
            Misc.ApplicationSettings.MainWindowState = this.WindowState;

            // MainWindowPosition and MainWindowSize are updated in real time
            // from the various event handlers.
            Misc.ApplicationSettings.MainWindow_SplitPanelLeftSplitSize = splitContainerLeft.SplitterDistance;
            Misc.ApplicationSettings.MainWindow_SplitPanelWsMembersSplitSize = splitContainerWsMembers.SplitterDistance;
            Misc.ApplicationSettings.Save();
        }

        /// <summary>
        /// Restore the window state using the saved settings.
        /// </summary>
        private void RestoreMainFormSettings()
        {
            // If the location is set to an invalid value, force it to a valid one.
            if (Misc.ApplicationSettings.MainWindowPosition.X < 0 || Misc.ApplicationSettings.MainWindowPosition.Y < 0)
                this.Location = new Point(0, 0);
            else
                this.Location = Misc.ApplicationSettings.MainWindowPosition;

            if (m_startMinimized)
                this.WindowState = FormWindowState.Minimized;
            else
                this.WindowState = Misc.ApplicationSettings.MainWindowState;

            this.Size = Misc.ApplicationSettings.MainWindowSize;

            this.splitContainerLeft.SplitterDistance = Misc.ApplicationSettings.MainWindow_SplitPanelLeftSplitSize;
            this.splitContainerWsMembers.SplitterDistance = Misc.ApplicationSettings.MainWindow_SplitPanelWsMembersSplitSize;
        }

        /// <summary>
        /// Called on startup and when the user changes its options.
        /// </summary>
        private void ApplyOptions()
        {
            bool debugFlag = Misc.ApplicationSettings.KwmEnableDebugging;
            bool fileFlag = Misc.ApplicationSettings.KwmLogToFile && debugFlag;

            // Adjust the logging stuff.
            if (fileFlag && m_fileLogger == null)
            {
                m_fileLogger = new CFileLogger(Misc.GetKwmLogFilePath(), "kwm-" + Base.GetLogFileName());
                Logging.RegisterLogHandler(m_fileLogger);
            }

            else if (!fileFlag && m_fileLogger != null)
            {
                m_fileLogger.CloseAndUnregister();
                m_fileLogger = null;
            }

            Logging.SetLoggingLevel(debugFlag ? LoggingLevel.Debug : LoggingLevel.Normal);
        }

        /// <summary>
        /// Make the KWM visible if it is minimized or not in the foreground.
        /// </summary>
        public void ShowMainForm()
        {
            Logging.Log("ShowMainForm() called.");

            HandleOnMainFormShown();

            if (Syscalls.IsIconic(this.Handle))
                Syscalls.ShowWindowAsync(this.Handle, (int)Syscalls.WindowStatus.SW_RESTORE);

            this.Visible = true;
            Syscalls.SetForegroundWindow(this.Handle);
            this.Activate();
        }

        /// <summary>
        /// Updates the form's UI (Home tab info, title bar, workspace users).
        /// </summary>
        public void UpdateUI(Workspace kws)
        {
            ToolStripLabel statusKwsName = AppStatusStrip.Items[2] as ToolStripLabel;
            Debug.Assert(statusKwsName != null);
            
            UpdateDeleteItemToolbarButton();

            // Set the title and status bar.
            if (kws == null)
            {
                HideAdvancedView(); 
                this.Text = Base.GetKwmString();
                statusKwsName.Visible = false;
                kwsHeader.Visible = false;
                createKwsHeader.Visible = true;
            }

            else
            {
                createKwsHeader.Visible = false;
                kwsHeader.Visible = true;
                kwsHeader.UpdateUI(kws);
                if (kwsHeader.HasError || Misc.ApplicationSettings.ShowMainFormAdvancedView)
                    ShowAdvancedView();
                else
                    HideAdvancedView();

                this.Text = Base.GetKwmString() + " - " + kws.GetKwsName();
                statusKwsName.Visible = true;
                statusKwsName.Text = kws.GetKwsName();
                lnkShowProperties.Enabled = !kws.IsPublicKws();
            }

            // Update the invitation controls status.
            UpdateInviteStatus(kws);

            // Set the user list.
            UpdateKwsUserList(kws);
        }

        /// <summary>
        /// Update the invitation UI (button, textbox and linklabel) so that
        /// the controls are enabled at the right moment.
        /// </summary>
        private void UpdateInviteStatus(Workspace kws)
        {
            if (kws == null)
            {
                btnInvite.Enabled = false;
                btnCustomInvite.Enabled = false;
                return;
            }

            bool enable = (kws.LoginStatus == KwsLoginStatus.LoggedIn &&
                           kws.IsOnlineCapable() &&
                          !kws.IsPublicKws());

            btnInvite.Enabled = enable;
            btnCustomInvite.Enabled = enable;
        }

        /// <summary>
        /// Select the appropriate user in lstUsers.
        /// </summary>
        public void UpdateKwsUserSelection()
        {
            m_ignoreSelectFlag = true;

            // Reselect the previously selected user, if any.
            KwsBrowserKwsNode bNode = Browser.GetSelectedKwsNode();
            if (bNode != null && bNode.SelectedUser != null)
            {
                String uid = bNode.SelectedUser.UserID.ToString();
                if (lstUsers.Items.ContainsKey(uid))
                    lstUsers.Items[uid].Selected = true;
            }

            m_ignoreSelectFlag = false;
        }

        /// <summary>
        /// Update the workspace user list with the information from the
        /// workspace specified, if any.
        /// </summary>
        private void UpdateKwsUserList(Workspace kws)
        {
            lstUsers.BeginUpdate();

            lstUsers.Items.Clear();
            lstUsers.Enabled = (kws != null);

            if (kws != null)
            {
                // We want to display the user display name preferably.
                // If a dupe exists, show name and email address, and if no name
                // is present, just show the email address.
                foreach (KwsUser user in kws.CoreData.UserInfo.UserTree.Values)
                {
                    // Ignore banned users.
                    if (user.BanFlag) continue;

                    String userName = user.UiSimpleName;

                    UInt32 dupeID = IsNamePresent(userName);
                    if (dupeID != 0)
                    {
                        // If this username already exists in the UI, use the FullName instead.
                        userName = user.UiFullName;

                        // Go back to the initial dupe and change its display name to the FullName.
                        lstUsers.Items[dupeID.ToString()].Text = kws.CoreData.UserInfo.UserTree[dupeID].UiFullName;
                    }

                    ListViewItem item = new ListViewItem();
                    item.Name = user.UserID.ToString();
                    item.Text = userName;
                    item.ToolTipText = user.UiTooltipText;

                    // Mark ourself in a different way.
                    if (user.UserID == kws.CoreData.Credentials.UserID)
                    {
                        item.Font = new Font(item.Font, FontStyle.Bold);
                    }

                    lstUsers.Items.Add(item);
                }


                // Sort the list in alphabetical order.
                lstUsers.Sorting = SortOrder.Ascending;
                lstUsers.Sort();

                // Put the logged on user on top of the list.
                lstUsers.Sorting = SortOrder.None;
                ListViewItem i = lstUsers.Items[kws.CoreData.Credentials.UserID.ToString()];
                if (i != null)
                {
                    i.Remove();
                    lstUsers.Items.Insert(0, i);
                }

                UpdateKwsUserSelection();

                // Display a link inviting users to add more people to
                // empty non-public workspaces.
                if (lstUsers.Items.Count <= 1 && !m_uiBroker.Browser.SelectedKws.IsPublicKws())
                {
                    lblLinkInviteUsers.Visible = true;
                }
                else
                {
                    lblLinkInviteUsers.Visible = false;
                }
            }

            lstUsers.EndUpdate();
        }

        /// <summary>
        /// Return the userID of a KwsUser if the username is already displayed 
        /// in the member list, 0 otherwise.
        /// </summary>
        public UInt32 IsNamePresent(String userName)
        {
            foreach (ListViewItem i in lstUsers.Items)
                if (i.Text == userName) return UInt32.Parse(i.Name);

            return 0;
        }

        /// <summary>
        /// Select the appropriate tree node in tvWorkspaces.
        /// </summary>
        public void UpdateTvWorkspacesSelection()
        {
            m_ignoreSelectFlag = true;

            TreeNode nodeToSelect = null;

            // Reselect the previously selected folder.
            if (Browser.SelectedFolder != null)
                nodeToSelect = GetTvWorkspacesTreeNodeByPath(Browser.SelectedFolder.FullPath);

            // Reselect the selected workspace.
            if (nodeToSelect == null && Browser.SelectedKws != null)
                nodeToSelect = GetTvWorkspacesTreeNodeByPath(Browser.GetSelectedKwsNode().FullPath);

            // Select the primary folder.
            if (nodeToSelect == null)
            {
                Browser.SelectedFolder = Browser.PrimaryFolder;
                nodeToSelect = GetTvWorkspacesTreeNodeByPath(Browser.SelectedFolder.FullPath);
            }

            // Update the selection in the UI.
            Debug.Assert(nodeToSelect != null);
            tvWorkspaces.SelectedNode = nodeToSelect;

            m_ignoreSelectFlag = false;
        }

        /// <summary>
        /// Update tvWorkspaces with the information contained in the browser.
        /// If rebuildFlag is true, then tvWorkspaces is stale and it will be 
        /// repopulated. Rebuilding should be avoided as much as possible 
        /// because the UI is so slow that selection errors occur. If
        /// reselectFlag is true, the previously selected folder / workspace
        /// will be reselected as much as possible on rebuild.
        /// </summary>
        public void UpdateTvWorkspacesList(bool rebuildFlag, bool reselectFlag)
        {
            tvWorkspaces.BeginUpdate();

            // Get the current scroll position.
            Point scrollPos = tvWorkspaces.GetScrollPos();

            if (rebuildFlag)
            {
                // Rebuild the treeview.
                tvWorkspaces.Nodes.Clear();
                AddTvWorkspacesChildNodes(Browser.RootNode, tvWorkspaces.Nodes);
            }

            else
            {
                ArrayList nodes = new ArrayList();
                Browser.RootNode.GetSubtreeNodes(nodes);
                foreach (KwsBrowserNode bNode in nodes)
                {
                    TreeNode tNode = GetTvWorkspacesTreeNodeByPath(bNode.FullPath);
                    if (tNode != null) UpdateTvWorkspacesTreeNode(tNode, bNode);
                }
            }

            tvWorkspaces.EndUpdate();

            // Reselect a tree node if possible. Do NOT call this code before
            // EndUpdate() has been called, it won't scroll properly.
            if (rebuildFlag && reselectFlag)
            {
                tvWorkspaces.BeginUpdate();
                UpdateTvWorkspacesSelection();
                tvWorkspaces.SetScrollPos(scrollPos);
                tvWorkspaces.EndUpdate();
            }
        }

        /// <summary>
        /// Insert the children of the parent folder specified recursively.
        /// </summary>
        private void AddTvWorkspacesChildNodes(KwsBrowserFolderNode bParent, TreeNodeCollection tnc)
        {
            foreach (KwsBrowserNode bNode in bParent.GetNodes())
            {
                KwsBrowserKwsNode bKwsNode = bNode as KwsBrowserKwsNode;
                KwsBrowserFolderNode bFolderNode = bNode as KwsBrowserFolderNode;
                if (bKwsNode != null && !bKwsNode.Kws.IsDisplayable()) continue;

                TreeNode tNode = CreateTvWorkspacesTreeNode(bNode);
                tnc.Add(tNode);
                if (bFolderNode != null)
                {
                    AddTvWorkspacesChildNodes(bFolderNode, tNode.Nodes);
                    if (bFolderNode.ExpandedFlag) tNode.Expand();
                }
            }
        }

        /// <summary>
        /// Return a TreeNode matching the specified browser node.
        /// </summary>
        private TreeNode CreateTvWorkspacesTreeNode(KwsBrowserNode bNode)
        {
            TreeNode tNode = new TreeNode();
            tNode.Name = bNode.ID;
            tNode.Tag = bNode.FullPath;
            UpdateTvWorkspacesTreeNode(tNode, bNode);
            return tNode;
        }

        /// <summary>
        /// Update the tree node information with the browser node specified.
        /// </summary>
        private void UpdateTvWorkspacesTreeNode(TreeNode tNode, KwsBrowserNode bNode)
        {
            String text = "";
            String imageKey = "";

            tNode.NodeFont = new Font(tvWorkspaces.Font, bNode.NotifyFlag ? FontStyle.Bold : FontStyle.Regular);

            if (bNode is KwsBrowserFolderNode)
            {
                KwsBrowserFolderNode folderNode = bNode as KwsBrowserFolderNode;
                text = folderNode.FolderName;
                imageKey = folderNode.ExpandedFlag ? "FolderOpened" : "FolderClosed";
            }

            else
            {
                KwsBrowserKwsNode kwsNode = bNode as KwsBrowserKwsNode;
                text = kwsNode.Kws.GetKwsName();
                imageKey = kwsNode.GetKwsIconImageKey().Name;
            }

            // Not sure if this is really necessary for performance; not taking
            // chances.
            if (tNode.Text != text) tNode.Text = text;
            if (tNode.ImageKey != imageKey) tNode.ImageKey = tNode.SelectedImageKey = imageKey;
        }

        /// <summary>
        /// Return the path to the currently selected node in tvWorkspaces, if
        /// any.
        /// </summary>
        private String GetTvWorkspacesSelectedPath()
        {
            if (tvWorkspaces.SelectedNode == null) return "";
            return (String)tvWorkspaces.SelectedNode.Tag;
        }

        /// <summary>
        /// Return the browser node corresponding to the currently selected 
        /// node in tvWorkspaces, if any.
        /// </summary>
        private KwsBrowserNode GetTvWorkspacesSelectedBrowserNode()
        {
            return Browser.GetNodeByPath(GetTvWorkspacesSelectedPath());
        }

        /// <summary>
        /// Return the tvWorkspaces node having the path specified, if any.
        /// </summary>
        private TreeNode GetTvWorkspacesTreeNodeByPath(String path)
        {
            TreeNodeCollection tnc = tvWorkspaces.Nodes;
            TreeNode curTreeNode = null;

            foreach (String ID in path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!tnc.ContainsKey(ID)) return null;
                curTreeNode = tnc[ID];
                tnc = curTreeNode.Nodes;
            }

            return curTreeNode;
        }

        /// <summary>
        /// Update the tabs for displaying the selected workspace, if any.
        /// </summary>
        public void UpdateKwsTabs()
        {
            bool publicFlag = (Browser.SelectedKws != null && Browser.SelectedKws.IsPublicKws());
            UpdateOneKwsTab(tabSS, !publicFlag);
        }

        /// <summary>
        /// Hide or show one workspace application tab.
        /// </summary>
        private void UpdateOneKwsTab(TabPage tab, bool visibleFlag)
        {
            if (!visibleFlag && ApplicationsTabControl.TabPages.Contains(tab))
                ApplicationsTabControl.TabPages.Remove(tab);
            else if (visibleFlag && !ApplicationsTabControl.TabPages.Contains(tab))
                ApplicationsTabControl.TabPages.Add(tab);
        }

        /// <summary>
        /// Select the specified application tab.
        /// </summary>
        public void SetTabApplication(uint appNumber)
        {
            switch (appNumber)
            {
                case KAnpType.KANP_NS_CHAT: break;
                case KAnpType.KANP_NS_KFS: ApplicationsTabControl.SelectedTab = tabFiles; break;
                case KAnpType.KANP_NS_VNC: ApplicationsTabControl.SelectedTab = tabFiles; break;
                default: break;
            }
        }

        /// <summary>
        /// Initiate tray icon blinking animation.
        /// </summary>
        public void StartBlinking()
        {
            m_blinkingTimer.Start();
            trayIcon.Text = "New activity in your " + Base.GetKwmString();
        }
        
        /// <summary>
        /// Stop tray icon blinking animation.
        /// </summary>
        private void HandleOnMainFormShown()
        {
            if (m_exitFlag) return;

            StopBlinking();
            NotifyFocusChange(true);
        }

        private void StopBlinking()
        {
            m_blinkingTimer.Stop();
            trayIcon.Icon = m_blinkingIcons[0];
            m_currentTrayIcon = 0;
            trayIcon.Text = Base.GetKwmString();
        }

        private void NotifyFocusChange(bool isFocused)
        {
            Logging.Log("NotifyFocusChange(" + isFocused + ") called.");
            foreach (BaseAppControl c in m_appControlTree.Values)
                c.HandleOnKwmFocusChanged(isFocused);
        }

        // frmMain event handlers.

        /// <summary>
        /// Called when the tray icon flash timer triggers.
        /// </summary>
        private void TrayIconFlashTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                m_blinkingTimer.Stop();

                trayIcon.Icon = m_blinkingIcons[m_currentTrayIcon];

                if (m_currentTrayIcon == 0)
                    System.Threading.Thread.Sleep(1000);

                m_currentTrayIcon = (m_currentTrayIcon + 1) % m_blinkingIcons.Length;

                m_blinkingTimer.Start();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void frmMain_LocationChanged(object sender, EventArgs e)
        {
            try
            {
                if (!m_saveWindowSettingsFlag || m_exitFlag) return;

                if (WindowState != FormWindowState.Minimized)
                {
                    // Only store sane values.
                    if (Location.X > 0 && Location.Y > 0)
                        Misc.ApplicationSettings.MainWindowPosition = this.Location;
                }

            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            if (m_SizeChangedHandlerFlag || m_exitFlag) return;

            try
            {
                m_SizeChangedHandlerFlag = true;

                if (this.WindowState == FormWindowState.Minimized)
                    Hide();

                if (!m_saveWindowSettingsFlag)
                    return;

                // WindowState has not changed and is Normal: this is just a Resize.
                if (m_lastWindowState == this.WindowState &&
                    m_lastWindowState == FormWindowState.Normal)
                {
                    Misc.ApplicationSettings.MainWindowSize = this.Size;
                }

                // The WindowState has changed.
                else if (m_lastWindowState != this.WindowState)
                {
                    // If going Minimized, remember in which state we
                    // must restore when we change from Minimized to anything else.
                    if (this.WindowState == FormWindowState.Minimized)
                        Misc.ApplicationSettings.MainWindowStateAfterMinimize = m_lastWindowState;

                    // In any case, remember in which state we must be when the 
                    // application starts again.
                    Misc.ApplicationSettings.MainWindowState = this.WindowState;
                }

                // Remember the new state.
                m_lastWindowState = this.WindowState;
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
            finally
            {
                m_SizeChangedHandlerFlag = false;
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                Logging.Log("frmMain_Shown called. startMinimized = " + m_startMinimized);

                if (m_startMinimized)
                {
                    this.WindowState = FormWindowState.Minimized;
                    m_startMinimized = false;
                }
                HandleOnMainFormShown();

                m_saveWindowSettingsFlag = true;
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        void frmMain_Activated(object sender, EventArgs e)
        {
            try
            {
                HandleOnMainFormShown();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        /// <summary>
        /// If the console window exists, hide or make it visible.
        /// </summary>
        private void frmMain_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_exitFlag) return;

                if (this.Visible || Focused)
                {
                    HandleOnMainFormShown();
                }

                if (m_consoleWindow != null)
                {
                    if (this.Visible)
                    {
                        m_consoleWindow.Visible = MmDebuggingConsoleToolStripMenuItem.Checked;
                    }
                    else
                    {
                        m_consoleWindow.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Logging.Log("frmMain_FormClosing");
                if (!m_exitFlag &&
                   e.CloseReason == CloseReason.UserClosing)
                {
                    Logging.Log("Setting WindowState to Minimized. Original one: " + WindowState);
                    e.Cancel = true;
                    Syscalls.ShowWindowAsync(this.Handle, (int)Syscalls.WindowStatus.SW_SHOWMINIMIZED);
                    return;
                }

                Logging.Log("CloseReason: " + e.CloseReason);

                // We're closing because the application is exiting.
                SaveMainFormSettings();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex, true);
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ShowMainForm();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            try
            {
                lstUsers.Columns[0].Width = lstUsers.Width - 4;
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void Export_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = GetExportFilePath("Export " + Base.GetKwsString() + " list as", "");

                if (filename != "")
                    m_uiBroker.RequestExportBrowser(filename, 0);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dial = new System.Windows.Forms.OpenFileDialog();

                if (Misc.ApplicationSettings.ImportWsPath != "" && Directory.Exists(Misc.ApplicationSettings.ImportWsPath))
                    dial.InitialDirectory = Misc.ApplicationSettings.ImportWsPath;
                else
                    dial.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                dial.SupportMultiDottedExtensions = true;
                dial.Title = "Import " + Misc.GetString("Kwses");
                dial.Filter = Base.GetKwsString() + " list (*.wsl)|*.wsl|All files (*.*)|*.*";
                Misc.OnUiEntry();
                dial.ShowDialog(this);
                Misc.OnUiExit();

                if (dial.FileName != "")
                {
                    Misc.ApplicationSettings.ImportWsPath = Path.GetDirectoryName(dial.FileName);
                    Misc.ApplicationSettings.Save();
                    m_uiBroker.RequestImportBrowser(dial.FileName);
                }
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        /// <summary>
        /// Called when the selection changes in tvWorkspaces. This is called
        /// both when the user genuinely selects a node and when we
        /// programmatically select a node. Normally this method should have
        /// no effect if we are the source of the selection.
        /// </summary>
        private void tvWorkspaces_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (m_ignoreSelectFlag) return;

            KwsBrowserNode bNode = m_uiBroker.Browser.GetNodeByPath((String)e.Node.Tag);
            if (bNode == null) return;

            if (bNode is KwsBrowserKwsNode)
                m_uiBroker.RequestSelectKws(((KwsBrowserKwsNode)bNode).Kws, false);
            else
                Browser.SelectedFolder = bNode as KwsBrowserFolderNode;

            UpdateDeleteItemToolbarButton();
        }

        /// <summary>
        /// Updates the toolbar buttons statuses, according to the selected
        /// item.
        /// </summary>
        private void UpdateDeleteItemToolbarButton()
        {
            // Disable the button strictly when the selected folder is the root KWS 
            // folder, or if nothing at all is selected. For any other case, leave 
            // the button enabled and let the poutine do its work.
            bool en = true;
            KwsBrowserNode bNode = GetTvWorkspacesSelectedBrowserNode();
            
            if (bNode is KwsBrowserFolderNode)
            {
                en = !((KwsBrowserFolderNode)bNode).PermanentFlag;
            }

            else if (bNode is KwsBrowserKwsNode)
            {
                KwsBrowserKwsNode kwsNode = bNode as KwsBrowserKwsNode;
                if (m_uiBroker.GetGenDeleteAction(kwsNode.Kws) == KwsAction.DeleteFromServer)
                {
                    SetKwsToolsStripItemStatus(btnDeleteKws, KwsAction.DeleteFromServer, kwsNode.Kws);
                }
                else
                {
                    SetKwsToolsStripItemStatus(btnDeleteKws, KwsAction.RemoveFromList, kwsNode.Kws);
                }

                if (IsItemEnabled(btnDeleteKws))
                    btnDeleteKws.ToolTipText = "Delete the selected item";
            }
            else
            {
                en = false;
            }

            btnDeleteKws.Enabled = en; 
        }

        /// <summary>
        /// Helper for drag & drop. Return the treenode at the mouse position
        /// specified, if any.
        /// </summary>
        private TreeNode TvWorkspacesGetNodeByMousePos(int x, int y)
        {
            Point pos = tvWorkspaces.PointToClient(new Point(x, y));
            return tvWorkspaces.GetNodeAt(pos);
        }

        /// <summary>
        /// Helper for drag & drop. Update SrcNode from SrcPath. Return true if
        /// the source can be moved.
        /// </summary>
        private bool TvWorkspacesDragValidateSource()
        {
            TvWorkspacesDragInfo di = m_tvWorkspacesDragInfo;
            di.SrcNode = Browser.GetNodeByPath(di.SrcPath);
            if (di.SrcNode == null) return false;

            KwsBrowserFolderNode folder = di.SrcNode as KwsBrowserFolderNode;
            if (folder != null && folder.PermanentFlag) return false;

            return true;
        }

        /// <summary>
        /// Helper for drag & drop. Update the SrcNode, DstFolder and DstIndex 
        /// fields based on the destination tree node specified. Return true if
        /// the move is currently valid.
        /// </summary>
        private bool TvWorkspacesDragValidateDst(TreeNode tDstNode)
        {
            TvWorkspacesDragInfo di = m_tvWorkspacesDragInfo;

            // Revalidate the source.
            if (!TvWorkspacesDragValidateSource()) return false;

            // Get the destination node in the treeview and the browser, if any.
            KwsBrowserNode bDstNode = null;
            if (tDstNode != null) bDstNode = Browser.GetNodeByPath((String)tDstNode.Tag);

            // Cast the destination node to the appropriate type. If it is a
            // folder, the destination node is the destination folder.
            KwsBrowserKwsNode bDstKwsNode = bDstNode as KwsBrowserKwsNode;
            di.DstFolder = bDstNode as KwsBrowserFolderNode;

            // By default we insert at the beginning of the folder selected.
            di.DstIndex = 0;

            // We're moving a workspace.
            if (di.SrcNode is KwsBrowserKwsNode)
            {
                // Cannot move a workspace at the top level.
                if (bDstNode == null) return false;

                // A workspace is selected. The destination folder is the
                // parent folder and the insert position is one more than
                // the index of the selected workspace.
                if (bDstKwsNode != null)
                {
                    di.DstFolder = bDstKwsNode.Parent;
                    di.DstIndex = bDstKwsNode.Index;
                }
            }

            // We're moving a folder.
            else
            {
                // Cannot move a folder in the workspace area.
                if (bDstKwsNode != null) return false;

                // If there is no destination node, the destination node is the
                // root folder. We always insert at the end.
                if (di.DstFolder == null)
                {
                    di.DstFolder = Browser.RootNode;
                    di.DstIndex = Browser.RootNode.FolderNodes.Count;
                }
            }

            return (Browser.MoveCheck(di.SrcNode, di.DstFolder, di.SrcNode.Name, di.DstIndex) == null);
        }

        /// <summary>
        /// Helper for drag & drop. Return the effect Move is the move is 
        /// allowed, None otherwise.
        /// </summary>
        private DragDropEffects TvWorkspacesDragEffect(bool allowFlag)
        {
            return allowFlag ? DragDropEffects.Move : DragDropEffects.None;
        }

        /// <summary>
        /// Start the dragging of a node in tvWorkspaces.
        /// </summary>
        private void tvWorkspaces_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Debug.Assert(m_ignoreSelectFlag == false);
            TvWorkspacesDragInfo di = m_tvWorkspacesDragInfo;
            TreeNode sourceNode = (TreeNode)e.Item;
            di.SrcPath = (String)sourceNode.Tag;
            bool allowFlag = !StaleBrowser && TvWorkspacesDragValidateSource();
            DoDragDrop(sourceNode, TvWorkspacesDragEffect(allowFlag));
        }

        private void tvWorkspaces_DragEnter(object sender, DragEventArgs e)
        {
            m_ignoreSelectFlag = true;
            e.Effect = TvWorkspacesDragEffect(!StaleBrowser);
        }

        private void tvWorkspaces_DragLeave(object sender, EventArgs e)
        {
            tvWorkspaces.SelectedNode = null;
            m_ignoreSelectFlag = false;
        }

        private void tvWorkspaces_DragOver(object sender, DragEventArgs e)
        {
            TvWorkspacesDragInfo di = m_tvWorkspacesDragInfo;
            TreeNode node = TvWorkspacesGetNodeByMousePos(e.X, e.Y);
            if (node == null) return;
            tvWorkspaces.SelectedNode = node;
            e.Effect = TvWorkspacesDragEffect(!StaleBrowser && TvWorkspacesDragValidateDst(node));

            Point pt = PointToClient(new Point(e.X, e.Y));
            double diff = (DateTime.Now - di.ScrollTime).TotalMilliseconds;

            // Scroll up quickly.
            if (pt.Y < tvWorkspaces.ItemHeight)
            {
                if (node.PrevVisibleNode != null)
                    node = node.PrevVisibleNode;
                node.EnsureVisible();
                di.ScrollTime = DateTime.Now;
            }

            // Scroll up slowly.
            else if (pt.Y < (tvWorkspaces.ItemHeight * 2))
            {
                if (diff > 250)
                {
                    node = node.PrevVisibleNode;
                    if (node != null) node = node.PrevVisibleNode;
                    if (node != null)
                    {
                        node.EnsureVisible();
                        di.ScrollTime = DateTime.Now;
                    }
                }
            }
        }

        private void tvWorkspaces_DragDrop(object sender, DragEventArgs e)
        {
            TvWorkspacesDragInfo di = m_tvWorkspacesDragInfo;
            TreeNode node = TvWorkspacesGetNodeByMousePos(e.X, e.Y);
            bool allowFlag = !StaleBrowser && TvWorkspacesDragValidateDst(node);
            if (allowFlag)
            {
                Browser.Move(di.SrcNode, di.DstFolder, di.SrcNode.Name, di.DstIndex);
                KwsBrowserKwsNode bNode = di.SrcNode as KwsBrowserKwsNode;

                // Do not change the order of these calls.
                m_uiBroker.RequestBrowserUiUpdate(true);
                if (bNode != null) m_uiBroker.RequestSelectKws(bNode.Kws, true);
            }

            m_ignoreSelectFlag = false;
        }

        private void tvWorkspaces_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            KwsBrowserNode bNode = Browser.GetNodeByPath((String)e.Node.Tag);
            
            String reason = "";
            if (!CanEditTreeNode(bNode, ref reason))
            {
                e.CancelEdit = true;
            }
        }

        private void tvWorkspaces_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = true;

            if (e.Label == null || StaleBrowser) return;
            
            String label = e.Label.Trim();
            KwsBrowserNode node = Browser.GetNodeByPath((String)e.Node.Tag);
            if (node is KwsBrowserFolderNode)
            {
                KwsBrowserFolderNode f = node as KwsBrowserFolderNode;
                String reason = Browser.MoveCheck(f, f.Parent, label, f.Index);
                if (reason != null)
                {
                    ReportRenameError(reason, true, sender as Control);   
                    return;
                }

                Browser.Move(f, f.Parent, label, f.Index);

                // Don't update the browser list here, this confuses Windows.
                m_uiBroker.RequestBrowserUiUpdate(true);
            }

            else
            {
                KwsBrowserKwsNode k = node as KwsBrowserKwsNode;
                if (!Base.IsValidKwsName(e.Label)) 
                {
                    ReportRenameError("invalid " + Base.GetKwsString() + " name", true, sender as Control);
                    return;
                }
                m_uiBroker.RequestRenameKws(k.Kws, e.Label);
            }            
        }

        private void tvWorkspaces_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (StaleBrowser) return;

                TreeNode selectedTreeNode = tvWorkspaces.SelectedNode;
                if (selectedTreeNode == null) return;

                KwsBrowserNode bNode = Browser.GetNodeByPath((String)selectedTreeNode.Tag);
                String reason = "";
                if (e.KeyCode == Keys.F2)
                {
                    if (!CanEditTreeNode(bNode, ref reason))
                    {
                        SystemSounds.Beep.Play();
                        ReportRenameError(reason);
                    }
                    else
                        selectedTreeNode.BeginEdit();
                }

                else if (e.KeyCode == Keys.Delete)
                {
                    KwsBrowserNode node = GetTvWorkspacesSelectedBrowserNode();

                    if (node == null) return;

                    // Deleting a folder.
                    if (node is KwsBrowserFolderNode)
                    {
                        if (IsNodePermFolder(node as KwsBrowserFolderNode))
                        {
                            Misc.SetStatusBar(StatusBarIcon.Error, "Could not delete folder: you cannot delete your default folder.");
                            SystemSounds.Beep.Play();
                            return;
                        }
                        m_uiBroker.RequestDeleteFolder(node as KwsBrowserFolderNode);
                    }

                    // Deleting a workspace.
                    else
                    {
                        Workspace kws = (node as KwsBrowserKwsNode).Kws;
                        m_uiBroker.RequestGenericDeleteKws(kws);
                    }
                }
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        /// <summary>
        /// Return true and set reason to empty string if the given browser 
        /// node can be edited. Otherwise, return false and set the error 
        /// reason with the error description. Handles both folder and workspace renaming. 
        /// </summary>
        private bool CanEditTreeNode(KwsBrowserNode node, ref String reason)
        {
            // Logic statements are separated for clarity's sake.
            if (StaleBrowser || node == null)
            {
                reason = "selected node is invalid";
                return false;
            }

            // Folder nodes.
            if (node is KwsBrowserFolderNode &&
                ((KwsBrowserFolderNode)node).PermanentFlag)
            {
                reason = "cannot rename the default folder";
                return false;
            }

            // Kws nodes.
            if (node is KwsBrowserKwsNode)
            {
                KwsBrowserKwsNode kws = node as KwsBrowserKwsNode;
                if (!m_uiBroker.CanPerformKwsAction(KwsAction.Rename, kws.Kws, ref reason))
                {
                    Debug.Assert(reason != "");
                    return false;
                }
            }

            reason = "";
            return true;
        }

        private void ReportRenameError(String specificReason)
        {
            ReportRenameError(specificReason, false, null);
        }

        private void ReportRenameError(String specificReason, bool showSyntaxPopup, Control sender)
        {
            specificReason = "Unable to rename: " + specificReason;
            Misc.SetStatusBar(StatusBarIcon.Error, specificReason);
            if (showSyntaxPopup)
            {
                String popup = specificReason + Environment.NewLine + "A " +
                               Base.GetKwsString() +
                               " name cannot contain any of the following characters:" +
                               Environment.NewLine +
                               "*, \\, /, ?, :, |, <, >";
                Base.ShowHelpTooltip(popup, sender);
            }
        }

        /// <summary>
        /// Return true if the selected Treeview node is a Permanent folder node,
        /// false otherwise.
        /// </summary>
        private bool IsNodePermFolder(KwsBrowserFolderNode bNode)
        {
            return (bNode != null && bNode.PermanentFlag);
        }

        private void tvWorkspacesContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                KwsBrowserNode node = GetTvWorkspacesSelectedBrowserNode();

                // Cast the node to the appropriate type.
                KwsBrowserFolderNode folderNode = node as KwsBrowserFolderNode;
                KwsBrowserKwsNode kwsNode = node as KwsBrowserKwsNode;
                Workspace kws = kwsNode == null ? null : kwsNode.Kws;

                // Adjust the items.
                ToolStripItemCollection items = tvWorkspacesContextMenu.Items;

                HideAllTvKwsItems(items);

                if (folderNode != null)
                    UpdateFolderMenus(items, folderNode);

                else if (kwsNode != null)
                    UpdateKwsMenus(items, kwsNode);

                // Click on an empty area.
                else
                {
                    items["CmCreateNewFolder"].Visible = true;
                    items["CmCreateNewFolder"].Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void tvWorkspacesContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                HandleTvWorkspaceItemClicked(sender, e);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }             
        }

        private void CmAdvanced_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                HandleTvWorkspaceItemClicked(sender, e);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void HandleTvWorkspaceItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Logging.Log("HandleTvWorkspaceItemClicked:" + e.ClickedItem.Text);
            m_contextMenuItemClicked = e.ClickedItem;
        }
        
        private void tvWorkspacesContextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            HandleContextMenuClosing(sender, e);            
        }

        private void AdvancedSubmenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            HandleContextMenuClosing(sender, e);            
        }

        private void HandleContextMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            // Cancel if the item clicked is supposed to be disabled. 
            // That boolean value is stored in the Tag property of the 
            // items. It is possible for an item to not contain anything
            // in the Tag property. It is the case for separators.
            // This poutine is done to allow a tooltip to be shown
            // when a user hovers over a disabled item.
            if (m_contextMenuItemClicked != null &&
                (m_contextMenuItemClicked.Tag as bool?).HasValue &&
                !(m_contextMenuItemClicked.Tag as bool?).Value)
            {
                e.Cancel = true;
                m_contextMenuItemClicked = null;
            }
        }

        /// <summary>
        /// Mark the content of the context menu of tvWorkspacestreeview as not visible.
        /// </summary>
        private void HideAllTvKwsItems(ToolStripItemCollection items)
        {
            foreach (ToolStripItem i in items)
                i.Visible = false;
        }

        /// <summary>
        /// Make the right menus visible when the selected node is a folder and
        /// set the Enabled property properly.
        /// </summary>
        private void UpdateFolderMenus(ToolStripItemCollection items, KwsBrowserFolderNode folderNode)
        {
            items["CmCollapse"].Visible = folderNode.ExpandedFlag;
            items["CmExpand"].Visible = !folderNode.ExpandedFlag;

            items["CmCollapse"].Enabled = folderNode.ExpandedFlag;
            items["CmExpand"].Enabled = (!folderNode.ExpandedFlag && folderNode.FolderNodes.Count > 0);

            items["separator1"].Visible = true;

            items["CmCreateNewFolder"].Visible = true;
            items["CmCreateNewFolder"].Enabled = true;

            items["separator2"].Visible = true;

            items["CmDeleteFolder"].Visible = true;
            items["CmDeleteFolder"].Enabled = !folderNode.PermanentFlag;

            items["CmRenameFolder"].Visible = true;
            items["CmRenameFolder"].Enabled = !folderNode.PermanentFlag;
        }

        /// <summary>
        /// Make the right menus visible when the selected node is a Workspace and
        /// set the Enabled property properly.
        /// </summary>
        private void UpdateKwsMenus(ToolStripItemCollection items, KwsBrowserKwsNode kwsNode)
        {
            Workspace kws = kwsNode.Kws;
            if (kws.Sm.CanWorkOnline())
                items["CmWorkOnline"].Visible = true;
            else
                items["CmWorkOffline"].Visible = true;

            items["separator1"].Visible = true;

            items["CmRenameKws"].Visible = true;
            items["CmDeleteKws"].Visible = true;

            items["CmAdvanced"].Visible = true;

            ToolStripMenuItem advancedSubmenu = (ToolStripMenuItem)items["CmAdvanced"];
            advancedSubmenu.DropDown.Closing += new ToolStripDropDownClosingEventHandler(AdvancedSubmenu_Closing);
            advancedSubmenu.DropDownItems["CmDisableKws"].Visible = true;
            advancedSubmenu.DropDownItems["CmExport"].Visible = true;
            advancedSubmenu.DropDownItems["CmRebuild"].Visible = true;

            items["CmProperties"].Visible = true;

            SetKwsToolsStripItemStatus(items["CmWorkOnline"], KwsAction.Connect, kws);
            SetKwsToolsStripItemStatus(items["CmWorkOffline"], KwsAction.Disconnect, kws);
            
            SetKwsToolsStripItemStatus(items["CmRenameKws"], KwsAction.Rename, kws);

            SetKwsToolsStripItemStatus(advancedSubmenu.DropDownItems["CmDisableKws"], KwsAction.Stop, kws);
            SetKwsToolsStripItemStatus(advancedSubmenu.DropDownItems["CmExport"], KwsAction.Export, kws);
            SetKwsToolsStripItemStatus(advancedSubmenu.DropDownItems["CmRebuild"], KwsAction.Rebuild, kws);

            if (m_uiBroker.GetGenDeleteAction(kws) == KwsAction.DeleteFromServer)
            {
                SetKwsToolsStripItemStatus(items["CmDeleteKws"], KwsAction.DeleteFromServer, kws);
            }
            else
            {
                SetKwsToolsStripItemStatus(items["CmDeleteKws"], KwsAction.RemoveFromList, kws);
            }

            SetKwsToolsStripItemStatus(advancedSubmenu.DropDownItems["CmDeleteFromServer"], KwsAction.DeleteFromServer, kws);
            SetKwsToolsStripItemStatus(advancedSubmenu.DropDownItems["CmRemoveKwsFromList"], KwsAction.RemoveFromList, kws);
            SetKwsToolsStripItemStatus(items["CmProperties"], KwsAction.ShowProperties, kws);
        }

        /// <summary>
        /// Set the enabled status of the given item. If the item is disabled, a 
        /// tooltip string is set to explain why the command is unavailable.
        /// </summary>
        private void SetKwsToolsStripItemStatus(ToolStripItem item, KwsAction action, Workspace kws)
        {
            Debug.Assert(item != null && kws != null);
            
            String denyReason = "";
            item.Enabled = true;
            bool can = m_uiBroker.CanPerformKwsAction(action, kws, ref denyReason);
            if (can)
            {
                item.ForeColor = Color.Black;
            }
            else
            {
                item.ForeColor = Color.Gray;
            }

            item.Tag = can;
            item.ToolTipText = denyReason;
        }

        private void lstUsersContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                m_contextMenuItemClicked = null;

                SetUserToolsStripItemStatus(lstUsersContextMenu.Items["Copy"], KwsUserAction.CopyEmailAddr);
                SetUserToolsStripItemStatus(lstUsersContextMenu.Items["ShowProperties"], KwsUserAction.ShowProperties);
                SetUserToolsStripItemStatus(lstUsersContextMenu.Items["Delete"], KwsUserAction.Ban);
                SetUserToolsStripItemStatus(lstUsersContextMenu.Items["ResendInvitation"], KwsUserAction.ResendInvitation);
                SetUserToolsStripItemStatus(lstUsersContextMenu.Items["ResetPassword"], KwsUserAction.ResetPassword);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void lstUsersContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            m_contextMenuItemClicked = e.ClickedItem;
        }

        private void lstUsersContextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            // Cancel if the item clicked is supposed to be disabled. 
            // That boolean value is stored in the Tag property of the 
            // items. It is possible for an item to not contain anything
            // in the Tag property. It is the case for separators.
            // This poutine is done to allow a tooltip to be shown
            // when a user hovers over a disabled item.
            if (m_contextMenuItemClicked != null &&
                (m_contextMenuItemClicked.Tag as bool?).HasValue &&
                !(m_contextMenuItemClicked.Tag as bool?).Value)
            {
                e.Cancel = true;
                m_contextMenuItemClicked = null;
            }
        }

        /// <summary>
        /// Set the enabled status of the given item. If the item is disabled, a 
        /// tooltip string is set to explain why the command is unavailable. We
        /// do not set the Enabled property since tooltips are not shown on 
        /// disabled controls.
        /// </summary>
        private void SetUserToolsStripItemStatus(ToolStripItem item, KwsUserAction action)
        {
            Debug.Assert(item != null);

            KwsUser targetUser = m_uiBroker.GetSelectedUser();
            
            String denyReason = "";
            bool can;
            item.Enabled = true;

            if (targetUser == null)
            {
                can = false;
                denyReason = "No user is selected.";
            }
            else
            {
                can = m_uiBroker.CanPerformUserAction(action, m_uiBroker.Browser.SelectedKws, targetUser, ref denyReason);
            }

            if (can)
            {
                item.ForeColor = Color.Black;
            }
            else
            {
                item.ForeColor = Color.Gray;
            }
            item.Tag = can;

            item.ToolTipText = denyReason;
        }
        
        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_ignoreSelectFlag) return;
                if (Browser.SelectedKws == null) return;

                UInt32 userID = 0;
                if (lstUsers.SelectedItems.Count > 0)
                    userID = UInt32.Parse(lstUsers.SelectedItems[0].Name);

                m_uiBroker.RequestSelectUser(Browser.SelectedKws, userID, false);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        /// <summary>
        /// Request to exit the application.
        /// </summary>
        private void Exit()
        {
            m_exitFlag = true;
            StatusBarManager.Stop();
            m_uiBroker.RequestQuit();
        }

        /// <summary>
        /// Prompt the user for the file path where his workspaces should be exported.
        /// </summary>
        private String GetExportFilePath(String title, String defaultFileName)
        {
            SaveFileDialog dial = new System.Windows.Forms.SaveFileDialog();

            if (Misc.ApplicationSettings.ExportWsPath != "" && Directory.Exists(Misc.ApplicationSettings.ExportWsPath))
                dial.InitialDirectory = Misc.ApplicationSettings.ExportWsPath;
            else
                dial.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            dial.SupportMultiDottedExtensions = true;
            dial.Title = title;
            dial.DefaultExt = "wsl";
            dial.FileName = defaultFileName;
            dial.Filter = Base.GetKwsString() + " list (*.wsl)|*.wsl|All files (*.*)|*.*";
            dial.AddExtension = true;
            Misc.OnUiEntry();
            dial.ShowDialog(this);
            Misc.OnUiExit();
            string filename = dial.FileName;
            dial.Dispose();

            // If the user did not cancel, save the path so the next time
            // we can set the starting path of the dialog.
            if (filename != "")
            {
                Misc.ApplicationSettings.ExportWsPath = Path.GetDirectoryName(filename);
                Misc.ApplicationSettings.Save();
            }
            return filename;
        }

        public void OnShowAboutFormClick(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            Misc.OnUiEntry();
            about.ShowDialog();
            Misc.OnUiExit();
        }

        public void OnShowConsoleFormClick(object sender, EventArgs e)
        {
            if (m_consoleWindow == null)
            {
                m_consoleWindow = new ConsoleWindow();
                Logging.RegisterLogHandler(m_consoleWindow);
                m_consoleWindow.OnConsoleClosing += OnConsoleFormClosing;
                m_consoleWindow.Show();
                UpdateConsoleAndEventWindowMenu();
            }

            else
            {
                m_consoleWindow.Close();
            }
        }

        private void OnConsoleFormClosing(object sender, EventArgs e)
        {
            m_consoleWindow = null;
            UpdateConsoleAndEventWindowMenu();
        }

        /// <summary>
        /// Update the console & event window menu item in the main form.
        /// </summary>
        private void UpdateConsoleAndEventWindowMenu()
        {
            MmDebuggingConsoleToolStripMenuItem.Checked = (m_consoleWindow != null);
        }

        public void OnToolsOptionsFormClick(object sender, EventArgs e)
        {
            try
            {
                frmOptions opt = new frmOptions();
                Misc.OnUiEntry();
                DialogResult res = opt.ShowDialog();
                Misc.OnUiExit();

                if (res == DialogResult.OK)
                {
                    opt.SaveSettings();
                    ApplyOptions();

                    // Notify the OTC that its settings have been modified.
                    m_uiBroker.RequestNotifyKwmStateChanged();
                }
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        public void OnFileExitClick(object sender, EventArgs e)
        {
            Exit();
        }

        public void OnTrayOpenClick(object sender, EventArgs e)
        {
            ShowMainForm();
        }

        /// <summary>
        /// Return true if the user action linked to the given
        /// ToolStripItem can be performed or should be canceled.
        /// </summary>
        private bool UserItemActionAllowed(object toolStripItem)
        {
            return IsItemEnabled(toolStripItem as ToolStripMenuItem) &&
                   !StaleBrowser &&
                   m_uiBroker.Browser.SelectedKws != null &&
                   m_uiBroker.GetSelectedUser() != null;
        }

        /// <summary>
        /// Return true if the workspace action linked to the given
        /// ToolStripItem can be performed or should be canceled.
        /// </summary>
        private bool KwsItemActionAllowed(object toolStripItem)
        {
            return IsItemEnabled(toolStripItem as ToolStripItem) &&
                   !StaleBrowser && 
                   (GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode) != null;
        }

        /// <summary>
        /// Return true if the given ToolStripItem is enabled or not.
        /// Part of the poutine required to allow a tooltip be displayed
        /// on a disabled item.
        /// </summary>
        private bool IsItemEnabled (ToolStripItem i)
        {
            return (i != null && (i.Tag as bool?).HasValue && (i.Tag as bool?).Value);
        }

        // Folder commands.
        private void CmCreateNewFolder_Click(object sender, EventArgs e)
        {
            CreateNewFolder();
        }

        private void CmRenameFolder_Click(object sender, EventArgs e)
        {
            if (StaleBrowser) return;

            TreeNode tNode = GetTvWorkspacesTreeNodeByPath(GetTvWorkspacesSelectedPath());
            if (tNode == null) return;
            tNode.BeginEdit();
        }

        private void CmDeleteFolder_Click(object sender, EventArgs e)
        {
            if (StaleBrowser) return;
            KwsBrowserNode node = GetTvWorkspacesSelectedBrowserNode();
            if (node == null) return;
            Debug.Assert(node is KwsBrowserFolderNode);
            m_uiBroker.RequestDeleteFolder(node as KwsBrowserFolderNode);
        }

        private void cmExpandCollapseHelper(bool expandFlag)
        {
            if (StaleBrowser) return;
            KwsBrowserFolderNode bNode = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserFolderNode;
            if (bNode == null) return;
            TreeNode tNode = GetTvWorkspacesTreeNodeByPath(bNode.FullPath);
            if (tNode == null) return;
            m_uiBroker.RequestSetFolderExpansion(bNode, expandFlag);
            if (expandFlag) tNode.Expand();
            else tNode.Collapse();
        }

        private void CmExpand_Click(object sender, EventArgs e)
        {
            cmExpandCollapseHelper(true);
        }

        private void CmCollapse_Click(object sender, EventArgs e)
        {
            cmExpandCollapseHelper(false);
        }

        private void tvExpandCollapseHelper(TreeNode node, bool expandFlag)
        {
            KwsBrowserFolderNode bNode = m_uiBroker.Browser.GetFolderNodeByPath((String)node.Tag);
            if (bNode == null) return;
            m_uiBroker.RequestSetFolderExpansion(bNode, expandFlag);
        }

        private void tvWorkspaces_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            tvExpandCollapseHelper(e.Node, false);
        }

        private void tvWorkspaces_AfterExpand(object sender, TreeViewEventArgs e)
        {
            tvExpandCollapseHelper(e.Node, true);
        }

        // Workspace commands.

        private void CmWorkOnline_Click(object sender, EventArgs e)
        {
            if (!KwsItemActionAllowed(sender)) return;

            KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;
            m_uiBroker.RequestWorkOnline(node.Kws);
        }

        private void CmWorkOffline_Click(object sender, EventArgs e)
        {
            if (!KwsItemActionAllowed(sender)) return;

            KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;
            m_uiBroker.RequestWorkOffline(node.Kws);
        }

        private void CmRenameKws_Click(object sender, EventArgs e)
        {
            if (!KwsItemActionAllowed(sender)) return;

            TreeNode tNode = GetTvWorkspacesTreeNodeByPath(GetTvWorkspacesSelectedPath());
            if (tNode == null) return;
            tNode.BeginEdit();
        }

        private void CmExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KwsItemActionAllowed(sender)) return;

                KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;
                Debug.Assert(node != null);
                string filename = GetExportFilePath("Export " + Base.GetKwsString() + " as", node.Kws.CoreData.Credentials.KwsName);

                if (filename != "")
                    m_uiBroker.RequestExportBrowser(filename, UInt64.Parse(node.ID));
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void CmRebuild_Click(object sender, EventArgs e)
        {
            if (!KwsItemActionAllowed(sender)) return;

            KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;
            m_uiBroker.RequestRebuildKws(node.Kws);
        }

        private void CmDisableKws_Click(object sender, EventArgs e)
        {
            if (!KwsItemActionAllowed(sender)) return;

            KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;
            m_uiBroker.RequestStopKws(node.Kws);
        }

        private void CmDeleteKws_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KwsItemActionAllowed(sender)) return;

                KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;
                m_uiBroker.RequestGenericDeleteKws(node.Kws);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void CmRemoveKwsFromList_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KwsItemActionAllowed(sender)) return;

                KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;

                m_uiBroker.RequestRemoveKwsFromList(node.Kws, true);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void CmDeleteKwsOnServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KwsItemActionAllowed(sender)) return;

                KwsBrowserKwsNode node = GetTvWorkspacesSelectedBrowserNode() as KwsBrowserKwsNode;
                m_uiBroker.RequestDeleteKwsFromServer(node.Kws, true);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void CmKwsProperties_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KwsItemActionAllowed(sender)) return;
                m_uiBroker.RequestChangeKwsProperties();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        // User commands.
        private void ResendInvitation_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UserItemActionAllowed(sender)) return;

                PerformInvitation(m_uiBroker.GetSelectedUser().EmailAddress, true);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void resetPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UserItemActionAllowed(sender)) return;

                m_uiBroker.RequestSetUserPassword();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UserItemActionAllowed(sender)) return;

                m_uiBroker.RequestBanUser();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void UserProperties_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UserItemActionAllowed(sender)) return;

                m_uiBroker.RequestChangeUserProperties();
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void CopyEmailAddr_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UserItemActionAllowed(sender)) return;

                Clipboard.SetText(m_uiBroker.GetSelectedUser().EmailAddress, TextDataFormat.Text);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void trayExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Exit();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex, true);
            }
        }

        private void trayOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ShowMainForm();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        /// <summary>
        /// Method called by the UI hooks when a user wants to create
        /// a new folder to manage his workspaces in tvWorkspaces.
        /// </summary>
        private void CreateNewFolder()
        {
            if (StaleBrowser) return;

            // Get the parent folder.
            KwsBrowserFolderNode parentFolder = Browser.RootNode;

            String selPath = GetTvWorkspacesSelectedPath();
            if (selPath != "")
            {
                KwsBrowserNode n = Browser.GetNodeByPath(selPath);
                if (n == null) return;

                // We want to create a new folder inside the selected folder.
                if (n is KwsBrowserFolderNode)
                {
                    parentFolder = n as KwsBrowserFolderNode;
                }

                // We want to create a new folder inside the folder of the
                // selected workspace.
                else
                {
                    if (((KwsBrowserKwsNode)n).Parent == null) return;
                    parentFolder = ((KwsBrowserKwsNode)n).Parent;
                }
            }

            // Insert the node in the browser.
            KwsBrowserFolderNode bNode = m_uiBroker.RequestCreateFolder(parentFolder);

            // Rebuild tvWorkspaces.
            UpdateTvWorkspacesList(true, false);

            // Select the newly created folder.
            TreeNode tNode = GetTvWorkspacesTreeNodeByPath(bNode.FullPath);
            tvWorkspaces.SelectedNode = tNode;
            tNode.BeginEdit();
        }

        private void DeleteFolder()
        {
            if (StaleBrowser) return;
            KwsBrowserNode node = GetTvWorkspacesSelectedBrowserNode();
            if (node == null) return;
            Debug.Assert(node is KwsBrowserFolderNode);
            m_uiBroker.RequestDeleteFolder(node as KwsBrowserFolderNode);
        }

        /// <summary>
        /// Called to process the messages received by this window.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Misc.WM_COPYDATA)
            {
                Misc.COPYDATASTRUCT cds = (Misc.COPYDATASTRUCT)m.GetLParam(typeof(Misc.COPYDATASTRUCT));

                // This is our import workspace credentials message.
                if (cds.dwData.ToInt32() == Program.ImportKwsMsgID)
                {
                    Logging.Log("Received request to import workspace from window procedure.");

                    // Retrieve the workspace path. Clone the string to avoid
                    // any potential trouble.
                    String path = cds.lpData.Clone() as String;

                    // Perform the import. This is safe as we are in the UI
                    // context and we delay the import if we're not ready to
                    // process it yet.
                    m_uiBroker.RequestImportBrowser(path);
                }
            }

            base.WndProc(ref m);
        }

        private void btnCustomInvite_Click(object sender, EventArgs e)
        {
            try
            {
                PerformInvitation("", false);
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void lstUsers_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            ListViewItem i = lstUsers.Items[e.Item];

            // Don't allow edits of names that were already added.
            if (i.Name != "") e.CancelEdit = true;
        }

        private void lstusers_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            m_ignoreSelectFlag = false;

            // In this case, a null label means the text was not changed
            // so we remove the item and bail out.
            if (e.Label == null)
            {
                e.CancelEdit = true;
                lstUsers.Items.RemoveAt(e.Item);
                return;
            }

            try
            {
                PerformInvitation(e.Label, true);
            }

            catch (Exception ex)
            {
                e.CancelEdit = true;
                lstUsers.Items.RemoveAt(e.Item);

                Base.HandleException(ex);
            }
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {
            ListViewItem dynItem = new ListViewItem("user@email");
            lstUsers.Items.Add(dynItem);

            // Ignore the selection of the new flag.
            m_ignoreSelectFlag = true;
            
            dynItem.BeginEdit();
        }

        /// <summary>
        /// Perform an invitation to the current workspace. If 
        /// skipFirstWizardPage is set to true, the invitation is done without
        /// any user intervention (short way to invite).
        /// </summary>
        private void PerformInvitation(String inviteeEmailAddr, bool skipFirstWizardPage)
        {
            UpdateInviteStatus(null);

            KwsCoreOpRes res = m_uiBroker.RequestInviteToKws(m_uiBroker.Browser.SelectedKws, inviteeEmailAddr, skipFirstWizardPage);

            // Request a UI update to set back the invitation controls status
            // correctly since they were disabled at the beginning of this 
            // function.
            m_uiBroker.RequestSelectedKwsUiUpdate();
        }
        
        private void MmNewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateNewFolder();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void MmNewKwsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_uiBroker.RequestCreateKws();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void MmConfigurationWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_uiBroker.ShowConfigWizard();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void lstUsers_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Workspace kws = Browser.SelectedKws;
                KwsUser user = m_uiBroker.GetSelectedUser();
                String unused = "";
                if (e.KeyCode == Keys.Delete && m_uiBroker.CanPerformUserAction(KwsUserAction.Ban, kws, user, ref unused))
                    m_uiBroker.RequestBanUser();
                else
                    SystemSounds.Beep.Play();
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void lstUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                m_uiBroker.RequestChangeUserProperties();
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void upgradeAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Misc.OpenFileInWorkerThread(KwmStrings.BuyNowUrl);
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void quickStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Misc.OpenFileInWorkerThread(Base.GetKwmInstallationPath() + "quickstart.pdf");
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Misc.OpenFileInWorkerThread(Base.GetKwmInstallationPath() + "userguide.pdf");
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void helpToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                userManualToolStripMenuItem.Enabled = File.Exists(Misc.KwmUserGuidePath);
                quickStartToolStripMenuItem.Enabled = File.Exists(Misc.KwmQuickStartPath);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ToggleKwsHeaderDetails();
        }

        private void picExpand_MouseEnter(object sender, EventArgs e)
        {
            if (m_expandedKwsHeaderFlag)
                picExpand.Image = kwm.Properties.Resources.GroupCollapseHot;
            else
                picExpand.Image = kwm.Properties.Resources.GroupExpandHot;
        }

        private void picExpand_MouseLeave(object sender, EventArgs e)
        {
            if (m_expandedKwsHeaderFlag)
                picExpand.Image = kwm.Properties.Resources.GroupCollapse;
            else
                picExpand.Image = kwm.Properties.Resources.GroupExpand;
        }

        private void picExpand_Click(object sender, EventArgs e)
        {
            ToggleKwsHeaderDetails();
        }

        private void ToggleKwsHeaderDetails()
        {
            Misc.ApplicationSettings.ShowMainFormAdvancedView = !m_expandedKwsHeaderFlag;
            Misc.ApplicationSettings.Save();
            if (m_expandedKwsHeaderFlag) HideAdvancedView();
            else ShowAdvancedView();            
        }

        private void HideAdvancedView()
        {
            SplitHeaderAndFiles.SplitterDistance = 72;
            picExpand.Image = kwm.Properties.Resources.GroupExpand;
            lnkAdvancedView.Text = "Advanced view";

            m_expandedKwsHeaderFlag = false;
        }

        private void ShowAdvancedView()
        {
            SplitHeaderAndFiles.SplitterDistance = 115;
            picExpand.Image = kwm.Properties.Resources.GroupCollapse;
            lnkAdvancedView.Text = "Regular view";

            m_expandedKwsHeaderFlag = true;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                String reason = "";
                if (m_uiBroker.CanPerformKwsAction(KwsAction.ShowProperties, m_uiBroker.Browser.SelectedKws, ref reason))
                {
                    m_uiBroker.RequestChangeKwsProperties();
                }
                else
                {
                    Base.ShowHelpTooltip("Unable to display the " + Base.GetKwsString() + " properties: " + reason, lnkAdvancedView);
                }
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }            
        }

        private void btnGenNew_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                m_uiBroker.RequestCreateKws();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void btnNewKws_Click(object sender, EventArgs e)
        {
            try
            {
                m_uiBroker.RequestCreateKws();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void btnNewKwsFolder_Click(object sender, EventArgs e)
        {
            try
            {
                CreateNewFolder();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void btnDeleteKws_Click(object sender, EventArgs e)
        {
            try
            {
                KwsBrowserNode bNode = GetTvWorkspacesSelectedBrowserNode();
                if (bNode == null) return;

                if (bNode is KwsBrowserFolderNode)
                {
                    DeleteFolder();
                }

                else
                {
                    // Check if we can delete the selected workspace.
                    if (!KwsItemActionAllowed(sender)) return;
                    m_uiBroker.RequestGenericDeleteKws(m_uiBroker.Browser.SelectedKws);
                }
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void frmMain_Deactivate(object sender, EventArgs e)
        {
            try
            {
                NotifyFocusChange(false);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void btnInvite_DropDownOpening(object sender, EventArgs e)
        {
            SortedSet set = m_uiBroker.GetKwsEmailAddressSet(20);
            btnInvite.DropDown.Items.Clear();
            
            // Get the top-10 most invited addresses.
            foreach (String address in set)
            {
                ToolStripItem menuItem = new ToolStripMenuItem(address);
                menuItem.Tag = address;
                menuItem.Click += new EventHandler(delegate(object source, EventArgs ev)
                    {
                        PerformInvitation((string)((ToolStripMenuItem)source).Tag, true);
                    });
               btnInvite.DropDown.Items.Add(menuItem);
            }
        }
    }

    /// <summary>
    /// This class includes the logic for displaying and hiding a message in
    /// the application's status bar.
    /// </summary>
    public class StatusBarHelper
    {
        /// <summary>
        /// Delay to wait after a status is posted before clearing the
        /// status bar, in milliseconds.
        /// </summary>
        private const UInt32 ClearStatusDelay = 10000;

        private frmMain m_mainForm;

        private bool m_stopFlag = false;

        private System.Timers.Timer m_timer = new System.Timers.Timer();

        public StatusBarHelper(frmMain mainForm)
        {
            m_mainForm = mainForm;
            m_timer.SynchronizingObject = mainForm;
            m_timer.Interval = ClearStatusDelay;
            m_timer.AutoReset = false;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerTick);
            ClearStatusBar();
        }

        private void OnTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (m_stopFlag) return;
                Logging.Log("Status timer ticked. Clearing the status bar.");
                ClearStatusBar();
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        /// <summary>
        /// Clear any message in the status bar. Called by the internal timer
        /// only.
        /// </summary>
        private void ClearStatusBar()
        {
            m_mainForm.StatusBarImage = null;
            m_mainForm.StatusBarText = "";
        }

        /// <summary>
        /// Set an icon and a message in the status bar.
        /// </summary>
        public void PostStatus(StatusBarIcon icon, String message)
        {
            if (m_stopFlag) return;

            Logging.Log("Stopping the Status timer.");
            m_timer.Stop();

            // Set the icon and the message directly in the status bar.
            m_mainForm.StatusBarImage = GetImageForStatusBarIcon(icon);
            m_mainForm.StatusBarText = message;
            Logging.Log("Starting the Status timer.");
            m_timer.Start();
        }

        public void Stop()
        {
            m_stopFlag = true;
            m_timer.Stop();
        }

        /// <summary>
        /// Return the image associated to the given StatusBarIcon.
        /// </summary>
        private Image GetImageForStatusBarIcon(StatusBarIcon i)
        {
            switch (i)
            {
                case StatusBarIcon.OK:
                    return Properties.Resources.GreenCheck2;
                default:
                    return Properties.Resources.Red_x_48x48;
            }
        }
    }

    /// <summary>
    /// This class contains information about the current drag and drop operation
    /// in tvWorkspaces, if any.
    /// </summary>
    public class TvWorkspacesDragInfo
    {
        /// <summary>
        /// Path to the node being dragged and dropped.
        /// </summary>
        public String SrcPath;

        /// <summary>
        /// Dragged node.
        /// </summary>
        public KwsBrowserNode SrcNode;

        /// <summary>
        /// Destination folder.
        /// </summary>
        public KwsBrowserFolderNode DstFolder;

        /// <summary>
        /// Destination name.
        /// </summary>
        public String DstName;

        /// <summary>
        /// Destination index.
        /// </summary>
        public int DstIndex;

        /// <summary>
        /// Time at which we last scrolled.
        /// </summary>
        public DateTime ScrollTime = DateTime.MinValue;

        public override string ToString()
        {
            return "SrcPath: " + SrcPath + ",  DstFolder:" + DstFolder + ", DstName: " + DstName + ", DstIndex: " + DstIndex;
        }
    }
}