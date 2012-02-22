using System;
using System.Collections.Generic;
using System.IO;
using kwm.Utils;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Collections;
using System.Windows.Forms;
using Tbx.Utils;

namespace kwm.KwmAppControls.AppKfs
{
    /// <summary>
    /// Status of the initial scan.
    /// </summary>
    public enum InitScanStatus
    {
        None,
        Fail,
        OK
    }

    /// <summary>
    /// Allowed user operations.
    /// </summary>
    public enum AllowedOpStatus
    {
        None,
        Download,
        All
    }

    [Serializable]
    public class KfsShare
    {
        AppKfs m_App;

        /// <summary>
        /// A flag indicated a license resitriction related erro was raised.
        /// </summary>
        [NonSerialized]
        bool m_licenseRestrictionError = false;
        
        /// <summary>
        /// The three primary file views.
        /// </summary>
        public KfsLocalView LocalView;
        public KfsServerView ServerView;
        [NonSerialized]
        public KfsStatusView StatusView;

        /// <summary>
        /// Active FileSystemWatcher instance, if any.
        /// </summary>
        [NonSerialized]
        private KfsFsWatcher FsWatcher;

        /// <summary>
        /// Tree of paths to eventually scan on disk.
        /// </summary>
        [NonSerialized]
        public KfsStaleTree StaleTree;

        /// <summary>
        /// Reference to the pipeline.
        /// </summary>
        [NonSerialized]
        public KfsPipeline Pipeline;

        /// <summary>
        /// Reference to the UI gate.
        /// </summary>
        [NonSerialized]
        public KfsGate Gate;

        /// <summary>
        /// Reference to the download, upload and meta-data managers.
        /// </summary>
        [NonSerialized]
        public KfsDownloadManager DownloadManager;
        [NonSerialized]
        public KfsUploadManager UploadManager;
        [NonSerialized]
        public KfsMetaDataManager MetaDataManager;

        /// <summary>
        /// Unique name associated to the share. This is the name of the 
        /// directory where the KFS user files are stored. This name never
        /// changes.
        /// WARNING: this field was not serialized in old versions of the KWM.
        /// </summary>
        private String ShareUniqueName = "";

        /// <summary>
        /// Path to the physical root directory of the share, e.g.
        /// "C:\my\share\". This path always ends with a delimiter.
        /// </summary>
        [NonSerialized]
        private String SharePath;

        /// <summary>
        /// Root part of the share path that is common to all KFS shares. 
        /// Can be either the default (My Documents\Teambox Shares) or 
        /// user-specified. Must be backslash-terminated.
        /// </summary>
        [NonSerialized]
        private String ShareCommonRoot;

        /// <summary>
        /// Path to the temporary directory common to all KFS shares.
        /// </summary>
        private String ShareCommonTempDir
        {
            get { return ShareCommonRoot + "Temp\\"; }
        }

        /// <summary>
        /// Path to this share's temporary directory. Backslash-terminated.
        /// </summary>
        private String ShareTempDir
        {
            get {  return ShareCommonTempDir + m_App.Helper.GetInternalKwsID() + "\\"; }
        }

        public bool LicenseErrorRaised
        {
            get
            {
                return m_licenseRestrictionError;
            }
        }

        public void ClearLicenseError()
        {
            m_licenseRestrictionError = false;
        }

        public String ShareFullPath
        {
            get
            {
                if (SharePath == null || SharePath == "") throw new Exception("SharePath is not set");
                return SharePath;
            }
        }

        /// <summary>
        /// List of pending server operations to apply.
        /// </summary>
        public Queue<KfsServerOp> OpList = new Queue<KfsServerOp>();

        /// <summary>
        /// List of phase 1 events that have pending phase 2 events to be received.
        /// This is used to properly do phase 2 events notifications.
        /// </summary>
        [NonSerialized]
        private SortedDictionary<UInt64, List<KfsServerOp>> m_phase1Events;

        /// <summary>
        /// Status of the initial scan:
        /// none: the initial scan was never performed.
        /// fail: the initial scan failed the last time we attempted it.
        /// ok: the initial scan was performed successfully.
        /// </summary>
        [NonSerialized]
        public InitScanStatus InitialScanStatus;

        /// <summary>
        /// Date at which we last performed the initial scan of the share, or at least
        /// tried to.
        /// </summary>
        [NonSerialized]
        public DateTime InitialScanDate;

        /// <summary>
        /// True if an initial or partial scan is running.
        /// </summary>
        [NonSerialized]
        public bool ScanRunningFlag;

        /// <summary>
        /// Date at which we received a FileSystemWatcher event indicating that
        /// we need to perform a partial scan.
        /// </summary>
        [NonSerialized]
        public DateTime StaleFsEventDate;

        /// <summary>
        /// Date at which we last performed a partial scan of the share, or at least
        /// tried to. If a partial scan fails, the state of the initial scan is set to
        /// "fail".
        /// </summary>
        [NonSerialized]
        public DateTime PartialScanDate;

        /// <summary>
        /// Date at which we last applied server operations, or at least tried to.
        /// </summary>
        [NonSerialized]
        public DateTime ServerOpAppDate;

        /// <summary>
        /// True if there is a server operation that could not be applied because
        /// something went wrong on the filesystem.
        /// </summary>
        public bool ServerOpFsFailFlag = false;

        /// <summary>
        /// Error message describing why the operation could not be applied.
        /// </summary>
        public string ServerOpFsFailExplanation = "";

        /// <summary>
        /// True if there is a server operation that could not be applied because the
        /// server apparently sent us garbage.
        /// </summary>
        public bool ServerOpPermFailFlag = false;

        /// <summary>
        /// True if we're waiting for the filesystem event indicating that all prior
        /// filesystem events have been received.
        /// </summary>
        [NonSerialized]
        public bool ServerOpRunningFlag;

        /// <summary>
        /// True if the LocalUpdateFlag flag of a server file has been set to true
        /// since we last updated the local status of the server files.
        /// </summary>
        [NonSerialized]
        public bool LocalUpdateFlag;

        /// <summary>
        /// Date at which we last performed an update of the local statuses.
        /// </summary>
        [NonSerialized]
        public DateTime LocalUpdateDate;

        /// <summary>
        /// True if the KFS share is active, i.e. if the user is using it.
        /// </summary>
        [NonSerialized]
        public bool ActiveFlag;

        /// <summary>
        /// True the status view and/or the transfer lists need to be updated.
        /// </summary>
        [NonSerialized]
        public bool UpdateStatusViewFlag;

        /// <summary>
        /// Date at which we received a request to update the status view.
        /// </summary>
        [NonSerialized]
        public DateTime StatusViewStaleDate;

        /// <summary>
        /// True if there are directory/file or file/directory conflicts in the status
        /// view.
        /// </summary>
        [NonSerialized]
        public bool TypeConflictFlag;

        /// <summary>
        /// True if the errors encountered should be reported to the user. This flag
        /// should only be set by a specific user action, such as the user clicking on
        /// "update view". This is cleared once the full pipeline has been flushed or
        /// when an error has been reported.
        /// </summary>
        [NonSerialized]
        public bool ReportErrorToUserFlag;

        // User operations currently allowed:
        // none: the user is not allowed to do anything.
        // download: the user is allowed to open and download files.
        // all: the user is allowed to perform any operation.
        // The allowed operations are recomputed in the entry stage and may
        // become more restricted as the pipeline runs.
        [NonSerialized]
        public AllowedOpStatus AllowedOp;

        /// <summary>
        /// AllowedOp value at beginning of pipeline. Used to trigger
        /// a UI update only if necessary.
        /// </summary>
        [NonSerialized]
        public AllowedOpStatus InitialAllowedOp;

        /// <summary>
        /// Current server operation ID. This is used to associate a unique ID to
        /// every server operation.
        /// </summary>
        public UInt64 ServerOpID = 1;

        /// <summary>
        /// Current transfer order ID. This is used both to associate a unique ID to
        /// each file upload / download and to order them. The dual use may eventually
        /// cause problems, e.g. if we want to re-order transfers, but it is fine for
        /// our immediate needs.
        /// </summary>
        [NonSerialized]
        public UInt64 TransferOrderID;

        /// <summary>
        /// Tree mapping paths to the number of files being uploaded or downloaded
        /// under that path. Due to operations such as move and delete, it is possible
        /// that two files are being uploaded or downloaded under the same path.
        /// </summary>
        [NonSerialized]
        public SortedDictionary<String, UInt32> FileTransferTree;

        /// <summary>
        /// Array of transfer errors.
        /// </summary>
        [NonSerialized]
        public List<KfsTransferError> TransferErrorArray;

        /// <summary>
        /// Compatibility for version inferior to 5.
        /// </summary>
        public UInt64 CompatLastKwsEventID = 0;

        /// <summary>
        /// Event fired whenever the KFS share is idle.
        /// </summary>
        public event EventHandler<EventArgs> OnKfsIdle;

        /// <summary>
        /// Time to wait in milliseconds before scanning the share again when a scan
        /// fails.
        /// </summary>
        public const UInt64 ScanFailDelay = 300 * 1000;

        /// <summary>
        /// Time to wait in milliseconds before starting a partial scan when a
        /// filesystem event is received.
        /// </summary>
        public const UInt64 PartialScanShortDelay = 20;

        /// <summary>
        /// Time to wait in milliseconds before doing another partial scan.
        /// </summary>
        public const UInt64 PartialScanLongDelay = 200;

        /// <summary>
        /// Time to wait in milliseconds before applying server operations again on
        /// success.
        /// </summary>
        public const UInt64 ServerOpSuccessDelay = 200;

        /// <summary>
        /// Time to wait in milliseconds before applying server operations again on
        /// failure.
        /// </summary>
        public const UInt64 ServerOpFailDelay = 30 * 1000;

        /// <summary>
        /// Time to wait in milliseconds for the filesystem marker event.
        /// </summary>
        public const UInt64 ServerOpMarkerDelay = 2000;

        /// <summary>
        /// Time to wait in milliseconds before updating the local statuses again on
        /// failure.
        /// </summary>
        public const UInt64 LocalUpdateFailDelay = 5 * 1000;

        /// <summary>
        /// Time to wait in milliseconds before updating the status view when
        /// it requires an update.
        /// </summary>
        public const UInt64 StatusViewDelay = 50;

        /// <summary>
        /// Name of the marker file we use to detect when the FileSystemWatcher has
        /// sent us all the updates we were waiting for.
        /// </summary>
        public const string FsMarker = "__teambox_marker_file__";

        /// <summary>
        /// Maximum size of the download cache.
        /// </summary>
        public const UInt64 MaxCacheSize = 100 * 1024 * 1024;

        /// <summary>
        /// Reference to the application.
        /// </summary>
        public AppKfs App
        {
            get { return m_App; }
        }

        /// <summary>
        /// Path to the upload directory. This path ends with a delimiter.
        /// </summary>
        public String UploadDirPath
        {
            get { return ShareTempDir + "KfsUpload" + "\\"; }
        }

        /// <summary>
        /// Path to the download cache directory. This path ends with a delimiter.
        /// </summary>
        public String CacheDirPath
        {
            get { return ShareTempDir + "KfsCache" + "\\"; }
        }

        /// <summary>
        /// Path to the file containing the ID of the last operation applied.
        /// </summary>
        public String AppliedOpFilePath
        {
            get { return App.Helper.KwsRoamingStatePath + "AppliedOpID.txt"; }
        }

        /// <summary>
        /// Path to the filesystem marker file.
        /// </summary>
        public String FsMarkerFilePath
        {
            get { return MakeAbsolute(FsMarker); }
        }
        
        /// <summary>
        /// Create the share.
        /// </summary>
        public KfsShare(AppKfs App)
        {
            m_App = App;

            new KfsServerView(this);
            new KfsLocalView(this);

            Initialize();
        }

        /// <summary>
        /// This method contains the code that has to be executed both on 
        /// deserialization and on construction of this object. This method
        /// should not throw an exception.
        /// </summary>
        public void Initialize()
        {
            m_licenseRestrictionError = false;

            InitialScanStatus = InitScanStatus.None;
            InitialScanDate = DateTime.MinValue;

            StaleFsEventDate = DateTime.MinValue;
            PartialScanDate = DateTime.MinValue;
            ScanRunningFlag = false;
            
            ServerOpAppDate = DateTime.MinValue;
            ServerOpRunningFlag = false;

            LocalUpdateFlag = true;
            LocalUpdateDate = DateTime.MinValue;
            ActiveFlag = false;

            StatusViewStaleDate = DateTime.MinValue;
            UpdateStatusViewFlag = false;

            TypeConflictFlag = false;

            ReportErrorToUserFlag = false;

            AllowedOp = InitialAllowedOp = AllowedOpStatus.None;

            TransferOrderID = 1;

            FileTransferTree = new SortedDictionary<String, UInt32>(KfsPath.Comparer);

            TransferErrorArray = new List<KfsTransferError>();

            new KfsStatusView(this);
            new KfsPipeline(this);
            new KfsGate(this);
            new KfsDownloadManager(this);
            new KfsUploadManager(this);
            new KfsMetaDataManager(this);

            m_phase1Events = new SortedDictionary<UInt64, List<KfsServerOp>>();
        }

        /// <summary>
        /// Return true if the KFS share is idle. This is the case if the KFS 
        /// gate and the meta data manager are idle.
        /// </summary>
        public bool IsKfsIdle()
        {
            return Gate.EntryCount == 0 && MetaDataManager.Status == MetaDataManagerStatus.Idle;
        }

        /// <summary>
        /// Notify the listeners when the KFS share is idle.
        /// </summary>
        public void NotifyIfKfsIdle()
        {
            if (IsKfsIdle() && OnKfsIdle != null) OnKfsIdle(this, null);
        }

        /// <summary>
        /// Normalize the application state on startup.
        /// </summary>
        public void NormalizeState()
        {
            // Get the share root.
            if (Misc.ApplicationSettings.KfsStorePath == "") ShareCommonRoot = Misc.GetKfsDefaultStorePath();
            else ShareCommonRoot = KfsPath.AddTrailingBackslash(Misc.ApplicationSettings.KfsStorePath);

            // Set the unique name of the workspace if necessary.
            if (ShareUniqueName == null || ShareUniqueName == "")
            {
                ShareUniqueName = App.Helper.GetKwsUniqueName();
                App.SetDirty("Set KFS share unique name");
            }

            // Set the share path.
            SharePath = Path.Combine(ShareCommonRoot, ShareUniqueName) + "\\";

            // Create and hide the temporary directory.
            CreateTempDirIfRequired();

            // Create the share, upload and cache directories.
            if (!Directory.Exists(UploadDirPath))
                Directory.CreateDirectory(UploadDirPath);
             
            if (!Directory.Exists(CacheDirPath))
                Directory.CreateDirectory(CacheDirPath);

            File.SetAttributes(UploadDirPath, FileAttributes.Normal);
            File.SetAttributes(CacheDirPath, FileAttributes.Normal);

            // Normalize the local view and server view dictionaries, if required.
            if (ServerView.Root.ChildTree.Comparer.Compare("a", "A") != 0)
            {
                NormalizeServerDirectory(ServerView.Root);
                NormalizeLocalDirectory(LocalView.Root);
                App.SetDirty("Normalized KFS directories");
            } 
        }

        /// <summary>
        /// Helper method for NormalizeState().
        /// </summary>
        private void NormalizeServerDirectory(KfsServerDirectory d)
        {
            SortedDictionary<String, KfsServerObject> newTree = new SortedDictionary<String, KfsServerObject>(KfsPath.Comparer);
            foreach (KfsServerObject o in d.ChildTree.Values)
            {
                newTree.Add(o.Name, o);
                KfsServerDirectory od = o as KfsServerDirectory;
                if (od != null) NormalizeServerDirectory(od);
            }
            d.ChildTree = newTree;
        }

        /// <summary>
        /// Helper method for NormalizeState().
        /// </summary>
        private void NormalizeLocalDirectory(KfsLocalDirectory d)
        {
            SortedDictionary<String, KfsLocalObject> newTree = new SortedDictionary<String, KfsLocalObject>(KfsPath.Comparer);
            foreach (KfsLocalObject o in d.ChildTree.Values)
            {
                newTree.Add(o.Name, o);
                KfsLocalDirectory od = o as KfsLocalDirectory;
                if (od != null) NormalizeLocalDirectory(od);
            }
            d.ChildTree = newTree;
        }

        /// <summary>
        /// Create the specified directory if it does not already exist. Makes
        /// sure the directory has the Hidden attribute set.
        /// </summary>
        public void CreateTempDirIfRequired()
        {
            if (!Directory.Exists(ShareCommonTempDir))
                Directory.CreateDirectory(ShareCommonTempDir);

            DirectoryInfo di = new DirectoryInfo(ShareCommonTempDir);
            di.Attributes |= FileAttributes.Hidden;
        }
        /// <summary>
        /// Called when the share becomes active.
        /// </summary>
        public void Activate()
        {
            if (ActiveFlag) return;
            Logging.Log(GetActivationLogPrefix() + "Share is now active");

            ActiveFlag = true;
            RequestStatusViewUpdate(GetActivationLogPrefix() + "Share became active");
            Pipeline.Run(GetActivationLogPrefix() + "share became active", true);
        }

        /// <summary>
        /// Called when the share becomes inactive.
        /// </summary>
        public void Deactivate()
        {
            Logging.Log(GetActivationLogPrefix() + "Share is now inactive");
            ActiveFlag = false;
        }

        private String GetActivationLogPrefix()
        {
            return App.Helper.GetKwsName() + " (" + App.Helper.GetExternalKwsID() + "): ";
        }
        /// <summary>
        /// Perform the initial scan of the share. This method returns true
        /// if the initial scan could be started.
        /// </summary>
        public bool StartInitialScan()
        {
            Logging.Log("Doing initial scan.");

            Debug.Assert(!ScanRunningFlag);
            Debug.Assert(InitialScanStatus != InitScanStatus.OK);
            Debug.Assert(FsWatcher == null);
            InitialScanStatus = InitScanStatus.None;

            // Try to start the watcher.
            try
            {
                // Create the share directory if it does not exist.
                if (!Directory.Exists(ShareFullPath)) Directory.CreateDirectory(ShareFullPath);
                StaleFsEventDate = DateTime.MinValue;
                FsWatcher = new KfsFsWatcher(this);
                StaleTree = new KfsStaleTree(null);
            }

            catch (Exception ex)
            {
                Logging.Log(2, "Failed to start file system watcher: " + ex.Message + ".");
                OnScanFailed();
                InitialScanDate = DateTime.Now;
                return false;
            }

            // Try to start the thread.
            try
            {
                ScanRunningFlag = true;
                KfsInitialScanThread scanner = new KfsInitialScanThread(this);
                scanner.Start();
            }

            catch (Exception e)
            {
                Base.HandleException(e, true);
            }

            return true;
        }

        /// <summary>
        /// Perform a partial scan of the share's stale paths.
        /// </summary>
        public void StartPartialScan()
        {
            Logging.Log("Doing partial scan.");

            try
            {
                Debug.Assert(!ScanRunningFlag);
                Debug.Assert(InitialScanStatus == InitScanStatus.OK);
                Debug.Assert(StaleTree.IsStale);

                ScanRunningFlag = true;

                // Reset StaleFsEventDate so that we restart a scan at the 
                // appropriate time.
                StaleFsEventDate = DateTime.MinValue;

                KfsPartialScanThread scanner = new KfsPartialScanThread(this, StaleTree.GetStalePathArray());
                scanner.Start();

                // Clear the paths we sent to be scanned.
                StaleTree.Clear();
            }

            catch (Exception e)
            {
                Base.HandleException(e, true);
            }
        }

        /// <summary>
        /// Apply buffered server operations.
        /// </summary>
        public void ApplyServerOperation()
        {
            Logging.Log("Applying server operations.");

            // No error at this point.
            ServerOpFsFailFlag = false;
            ServerOpFsFailExplanation = "";

            // We need to update the status view.
            RequestStatusViewUpdate("Applying Server Operation");

            // Remember whether we modified the filesystem.
            bool globalFsModifiedFlag = false;

            // Queue the server operations that have been successfully
            // applied on the server view. Used to notify the user.
            List<KfsServerOp> appliedOps = new List<KfsServerOp>();

            // Apply the operations.
            while (HaveBufferedServerOp())
            {
                // Get the next operation, without removing it from the
                // queue yet.
                KfsServerOp o = OpList.Peek();

                // Validate the operation.
                try
                {
                    o.Validate();
                }

                catch (Exception ex)
                {
                    Logging.Log(2, "Received corrupted server operation: " + ex.Message + ".");
                    string msg = "A communication error has occured with the server. " +
                                 "The file sharing will be permanently disabled for this workspace." +
                                 Environment.NewLine +
                                 "Please report this problem to your technical support ;" + Environment.NewLine +
                                 ex.Message + Environment.NewLine + ex.StackTrace;

                    ServerOpFsFailFlag = true;
                    ServerOpFsFailExplanation = msg;
                    ServerOpPermFailFlag = true;
                    break;
                }

                // Apply the operation on the filesystem.
                bool fsModifiedFlag;
                KfsApplyOpRequest req = o.ApplyOnFileSystem(out fsModifiedFlag);
                if (fsModifiedFlag) globalFsModifiedFlag = true;

                // We could not apply the operation on the filesystem.
                if (req != null)
                {
                    switch (req.Type)
                    {
                        // FIXME: prompt the user for something in the relevant cases.

                        //case ApplyOpRequestType.Close:
                        //case ApplyOpRequestType.Rename:
                        case ApplyOpRequestType.Error:
                        default:
                            ServerOpFsFailExplanation = req.Error;
                            ServerOpFsFailFlag = true;
                            break;
                    }
                    break;
                }

                // Apply the operation on the server view.
                o.ApplyOnServerView();

                // Remove the operation from the queue.
                KfsServerOp o2 = OpList.Dequeue();
                Debug.Assert(o == o2);

                // We do not want to notify for operations that were triggered by ourself or
                // that occured before we logged on the workspace.
                // All operations that we want to notify for must be stored in 'appliedOps'.
                // We also need to store server operations that announce an eventual phase 2
                // operation in m_phase1Events so that we can notify correctly when the phase 2
                // operation is applied.
                // This buffering of the corresponding phase 1 event is necessary because once
                // we receive the phase 2 event, it is possible that we won't have any way to 
                // retrieve the right information about the files in question. For example, 
                // user A creates file 'foo' and user B deletes the file before user A finished 
                // uploading. When the upload is completed, the information related to file 
                // foo is not present in the Share anymore.
                if (o2.UserID != App.Helper.KwmUser.UserID && 
                    o2.EventID > m_App.Helper.KwsLoginLatestEventId)
                {
                    if ((o2 is KfsCreateServerOp && ((KfsCreateServerOp)o2).IsFile) ||
                         o2 is KfsUpdateServerOp)
                    {
                        if (m_phase1Events.ContainsKey(o2.CommitID))
                        {
                            m_phase1Events[o2.CommitID].Add(o2);
                        }

                        else
                        {
                            List<KfsServerOp> lst = new List<KfsServerOp>();
                            lst.Add(o2);
                            m_phase1Events.Add(o2.CommitID, lst);
                        }

                        appliedOps.Add(o2);
                    }
                    else if (o2 is KfsDeleteServerOp ||
                          o2 is KfsMoveServerOp ||
                          o2 is KfsPhase2ServerOp ||
                          o2 is KfsCreateServerOp)
                    {
                        appliedOps.Add(o2);
                    }
                }
            }

            while (true)
            {
                // Get out when we're done.
                if (appliedOps.Count == 0) break;

                // AppliedOps contains a list of operations that were potentially done
                // by different users. We parse the array until we encounter another user
                // id, notify the user, and continue parsing the array in another run of
                // the while loop.
                UInt64 uid = appliedOps[0].UserID;
                
                List<KfsServerOp> currentUserOps = new List<KfsServerOp>();
                List<KfsServerOp> opsFromOtherUsers = new List<KfsServerOp>();
                                
                foreach (KfsServerOp o in appliedOps)
                {
                    if (o.UserID == uid)
                        currentUserOps.Add(o);
                    else
                        opsFromOtherUsers.Add(o);
                }              

                // Notify the user. Careful not to cause UI reentrance.
                KfsNotification notif = new KfsNotification(App.Helper, currentUserOps, m_phase1Events);

                foreach (AppKfsNotificationItem i in notif.Items)
                    App.Helper.NotifyUser(i);
                
                // Store the rest of the ops coming from another user
                // so we can make another run.
                appliedOps = opsFromOtherUsers;
            }

            // Update the paths of the transferred files.
            DownloadManager.UpdateLastFullPath();
            UploadManager.UpdateLastFullPath();

            // If we modified the filesystem, write the marker and wait for it.
            if (!ServerOpFsFailFlag && globalFsModifiedFlag)
            {
                try
                {
                    LocalView.WriteFsMarker();
                    ServerOpRunningFlag = true;
                }

                catch (Exception ex)
                {
                    ServerOpFsFailExplanation = "Cannot write filesystem marker: " + ex.Message + ".";
                    ServerOpFsFailFlag = true;
                }
            }

            // Remember the date at which we finished applying the operations.
            ServerOpAppDate = DateTime.Now;

            App.SetDirty("applied server operations");
        }

        /// <summary>
        /// Update the local statuses.
        /// </summary>
        public void UpdateLocalStatus()
        {
            LocalUpdateFlag = false;

            // Update status view only if at least one local status has changed.
            bool reqStatusViewUpdate = false;

            foreach (String s in ServerView.GetPathArray(false))
            {
                KfsServerFile f = ServerView.GetObjectByPath(s) as KfsServerFile;
                if (f != null && f.LocalUpdateFlag)
                {
                    LocalStatus orig = f.CurrentLocalStatus;
                    
                    f.UpdateLocalStatus();

                    if (orig != f.CurrentLocalStatus)
                        reqStatusViewUpdate = true;
                }
            }
            if (reqStatusViewUpdate)
                RequestStatusViewUpdate("Local status changed");

            LocalUpdateDate = DateTime.Now;
        }

        /// <summary>
        /// Mark the status view as requiring an update.
        /// </summary>
        public void RequestStatusViewUpdate(string reason)
        {
            if (UpdateStatusViewFlag) return;

            UpdateStatusViewFlag = true;
            StatusViewStaleDate = DateTime.Now;
        }

        /// <summary>
        /// Update the UI for WS progression.
        /// </summary>
        public void UpdateXferUI()
        {
            App.DoOnXferUpdateRequired();
        }

        /// <summary>
        /// Update the status view and the UI view if
        /// UpdateStatusViewFlag is true.
        /// </summary>
        public void UpdateStatusViewIfNeeded()
        {
            if (!UpdateStatusViewFlag) return;

            UpdateStatusViewFlag = false;
            StatusView.UpdateView();
            App.DoOnUIUpdateRequired();
        }

        /// <summary>
        /// Ask the server for a download ticket.
        /// </summary>
        public void AskDownloadTicket()
        {
            AnpMsg m = App.Helper.NewKAnpCmd(KAnpType.KANP_CMD_KFS_DOWNLOAD_REQ);
            m.AddUInt32(0);
            App.PostKasQuery(m, null, OnDownloadTicketReply);
        }

        /// <summary>
        /// Ask the server for an upload ticket.
        /// </summary>
        public void AskUploadTicket()
        {
            AnpMsg m = App.Helper.NewKAnpCmd(KAnpType.KANP_CMD_KFS_UPLOAD_REQ);
            m.AddUInt32(0);
            App.PostKasQuery(m, null, OnUploadTicketReply);
        }

        /// <summary>
        /// Handle a fatal exception. This method does not return.
        /// </summary>
        public void FatalError(Exception ex)
        {
            Base.HandleException(ex, true);
        }

        /// <summary>
        /// This method should be called to disallow user operations. The method
        /// will only allow operations for the mininum of AllowedOp and
        /// requestedLevel.
        /// </summary>
        public void DisallowUserOp(String reason, AllowedOpStatus requestedLevel)
        {
            if (requestedLevel == AllowedOpStatus.None)
                AllowedOp = AllowedOpStatus.None;

            else if (requestedLevel == AllowedOpStatus.Download)
                if (AllowedOp == AllowedOpStatus.All)
                    AllowedOp = AllowedOpStatus.Download;

            if (InitialAllowedOp != AllowedOp)
                RequestStatusViewUpdate("DisallowUserOp, AllowedOp == " + AllowedOp);
        }

        /// <summary>
        /// Create an ANP message that can be sent to the server in transfer mode.
        /// </summary>
        public AnpMsg CreateTransferMsg(UInt32 type)
        {
            AnpMsg m = new AnpMsg();
            m.Minor = KAnp.Minor;
            m.Type = type;
            return m;
        }

        /// <summary>
        /// Return true if there are buffered server operations to process.
        /// </summary>
        public bool HaveBufferedServerOp()
        {
            return (OpList.Count > 0 && !ServerOpPermFailFlag);
        }

        /// <summary>
        /// Return true if the file specified is being downloaded or uploaded.
        /// </summary>
        public bool IsTransferringFile(String path)
        {
            return FileTransferTree.ContainsKey(path);
        }

        /// <summary>
        /// This method must be called when a file transfer is added to the share.
        /// </summary>
        public void RegisterFileTransfer(KfsFileTransfer t)
        {
            if (FileTransferTree.ContainsKey(t.LastFullPath)) FileTransferTree[t.LastFullPath]++;
            else FileTransferTree[t.LastFullPath] = 1;
            RequestStatusViewUpdate("RegisterFileTransfer");
        }

        /// <summary>
        /// This method must be called when a file transfer is removed from the
        /// share.
        /// </summary>
        public void UnregisterFileTransfer(KfsFileTransfer t)
        {
            Debug.Assert(FileTransferTree.ContainsKey(t.LastFullPath));
            if (FileTransferTree[t.LastFullPath] == 1) FileTransferTree.Remove(t.LastFullPath);
            else FileTransferTree[t.LastFullPath]--;
            RequestStatusViewUpdate("Unregister File Transfer");
        }

        /// <summary>
        /// Convert a relative path to a full path in Windows format.
        /// </summary>
        public String MakeAbsolute(String path)
        {
            return KfsPath.GetWindowsFilePath(ShareFullPath + path, false);
        }

        /// <summary>
        /// Convert a full path to a relative path. This method throws an
        /// exception on error.
        /// </summary>
        public String FullPathToRelPath(String fullPath)
        {
            try
            {
                // Get the path to the share without the trailing delimiter.
                String prefix = KfsPath.StripTrailingDelim(ShareFullPath);

                // The full path should start with the prefix.
                if (!fullPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Full path " + fullPath + " does not start with " + prefix + ".");

                // Remove the prefix from the full path.
                fullPath = fullPath.Remove(0, prefix.Length);

                // If the full path has a leading delimiter, remove it.
                if (fullPath.Length > 0 && KfsPath.IsDelim(fullPath[0])) fullPath = fullPath.Remove(0, 1);

                // Convert the full path to a relative path.
                return KfsPath.GetUnixFilePath(fullPath, false);
            }

            catch (Exception ex)
            {
                FatalError(ex);
                return "";
            }
        }

        /// <summary>
        /// Add the missing local directories on the relative path specified.
        /// </summary>
        public void AddMissingLocalDirectories(String path)
        {
            if (path != "") Directory.CreateDirectory(MakeAbsolute(path));
        }

        /// <summary>
        /// Called when the initial file system scan completes.
        /// </summary>
        /// <param name="scan">The scan object</param>
        /// <param name="e">Exception that was raised if the scan failed, null on success.</param>
        public void OnInitialScanCompleted(KfsScan scan, bool success, Exception e)
        {
            Logging.Log("Initial scan finished.");

            Debug.Assert(ScanRunningFlag);
            Debug.Assert(InitialScanStatus != InitScanStatus.OK);

            InitialScanDate = DateTime.Now;
            ScanRunningFlag = false;

            if (success)
            {
                // If the status != none, it means the FSWatcher encountered an error.
                // The initial scan is NOT completed and must be run again. Otherwise,
                // synchronize the results.
                if (InitialScanStatus == InitScanStatus.None)
                {
                    InitialScanStatus = InitScanStatus.OK;
                    RequestStatusViewUpdate("InitialScan completed with success");
                    scan.Sync();
                }
            }
            else
            {
                OnScanFailed();

                DisallowUserOp("initial scan failed", AllowedOpStatus.None);
                if (e != null)
                {
                    Logging.Log("Initial scan failed : " + e.Message + ".");
                    Logging.LogException(e);
                }
            }

            Pipeline.Run("initial scan completed", success);
        }   

        /// <summary>
        /// Called when a partial scan run has completed.
        /// </summary>
        /// <param name="e">Exception that was raised if the scan failed, null on success.</param>
        public void OnPartialScanCompleted(bool success, List<KfsScan> scans, Exception e)
        {
            Logging.Log("Partial scan finished.");

            Debug.Assert(ScanRunningFlag);

            PartialScanDate = DateTime.Now;
            ScanRunningFlag = false;

            if (success)
            {
                // Ignore the results if the watcher failed during our partial
                // scan. Otherwise, synchronize the results.
                if (InitialScanStatus == InitScanStatus.OK)
                {
                    foreach (KfsScan scan in scans) scan.Sync();
                    RequestStatusViewUpdate("PartialScan completed with success");
                }
            }

            else
            {
                OnScanFailed();                

                DisallowUserOp("partial scan failed", AllowedOpStatus.None);
                if (e != null)
                {
                    Logging.Log("Partial scan failed : " + e.Message + ".");
                    Logging.LogException(e);
                }
            }

            Pipeline.Run("partial scan ended", false);
        }

        /// <summary>
        /// Called when a file/directory has been modified.
        /// </summary>
        public void OnWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed && Directory.Exists(e.FullPath))
            {
                // no op.
            }

            else if (e.ChangeType == WatcherChangeTypes.Deleted &&
                e.Name == KfsShare.FsMarker)
            {
                if (!ServerOpRunningFlag)
                {
                    Logging.Log(2, "Spurious FS watcher notification " + e.Name);
                }
                else
                {
                    ServerOpRunningFlag = false;
                    Pipeline.Run("received filesystem marker event", false);
                }
            }
            else
            {
                bool firstFlag = !StaleTree.IsStale;

                if (e.ChangeType == WatcherChangeTypes.Renamed)
                {
                    RenamedEventArgs ren = (RenamedEventArgs)e;
                    StaleTree.Add(FullPathToRelPath(ren.OldFullPath));
                    StaleTree.Add(FullPathToRelPath(ren.FullPath));
                }
                else
                {
                    StaleTree.Add(FullPathToRelPath(e.FullPath));
                }

                if (firstFlag)
                {
                    StaleFsEventDate = DateTime.Now;
                    Pipeline.Run("received filesystem event", false);
                }
            }
        }

        /// <summary>
        /// Called when the watcher has failed.
        /// </summary>
        public void OnWatcherError(object sender, ErrorEventArgs e)
        {
            Logging.Log(2, "File system watcher error: " + e.GetException().Message + ".");
            OnScanFailed();
            Pipeline.Run("FsWatcher error'ed on us.", true);
        }

        /// <summary>
        /// Called when a download ticket reply is received.
        /// </summary>
        public void OnDownloadTicketReply(KasQuery query)
        {
            Logging.Log("Download ticket received.");
            AnpMsg m = query.Res;

            ValidateTicketReply(m, false);

            // Pass the ticket to the download manager.
            if (DownloadManager.Status == DownloadManagerStatus.Ticket) DownloadManager.OnTicket(m);

            // We do not need the ticket anymore.
            else Logging.Log("Discarded unneeded download ticket");
        }

        /// <summary>
        /// Called when a upload ticket reply is received.
        /// </summary>
        public void OnUploadTicketReply(KasQuery query)
        {
            Logging.Log("Upload ticket received.");
            AnpMsg m = query.Res;
            
            ValidateTicketReply(m, true);

            // Pass the ticket to the upload manager.
            if (UploadManager.Status == UploadManagerStatus.Ticket) UploadManager.OnTicket(m);

            // Pass the ticket to the meta-data manager.
            else if (MetaDataManager.Status == MetaDataManagerStatus.Ticket) MetaDataManager.OnTicket(m);

            // We do not need the ticket anymore.
            else Logging.Log("Discarded unneeded upload ticket");
        }

        /// <summary>
        /// Called when a phase 1 server event is received.
        /// </summary>
        public void OnPhase1Event(AnpMsg m)
        {   
            // Call the global handler for phase 1 and phase 2 events.
            OnPhase1Or2Event(m, true);
            
            // The meta-data manager might be interested by the commit ID.
            MetaDataManager.OnPhase1Event(m.Elements[4].UInt64);
        }

        /// <summary>
        /// Called when a phase 2 server event is received.
        /// </summary>
        public void OnPhase2Event(AnpMsg m)
        {
            // Call the global handler for phase 1 and phase 2 events.
            OnPhase1Or2Event(m, false);
            
            // The upload manager might be interested by the commit ID.
            UploadManager.OnPhase2Event(m.Elements[4].UInt64);
        }

        /// <summary>
        /// Called when a transfer error has occurred.
        /// </summary>
        public void OnTransferError(KfsTransferError error)
        {
            TransferErrorArray.Add(error);
            RequestStatusViewUpdate("OnTransferError");
        }

        public void onLicenseError()
        {
            m_licenseRestrictionError = true;
            RequestStatusViewUpdate("onLicenseError");
        }
        /// <summary>
        /// Called when network connectivity has been lost. The pipeline
        /// must run after a call to this method.
        /// </summary>
        public void OnNoLongerOnline()
        {
            CancelAllNetworkOperations();
        }

        /// <summary>
        /// Cancel the operations of the DownloadManager, the UploadManager and
        /// the MetaDataManager.
        /// </summary>
        private void CancelAllNetworkOperations()
        {
            DownloadManager.CancelOperation();
            UploadManager.CancelOperation();
            MetaDataManager.CancelOperation();
        }

        // Called when a phase 1 or 2 server event is received.
        private void OnPhase1Or2Event(AnpMsg m, bool phaseOneFlag)
        {
            // Add the operations in the operation list.
            bool firstFlag = (OpList.Count == 0);

            if (phaseOneFlag)
            {
                List<KfsServerOp> opList;
                ServerView.DecomposePhase1Event(m, out opList);
                foreach (KfsServerOp o in opList)
                    OpList.Enqueue(o);                
            }

            else
            {
                KfsPhase2ServerOp o;
                ServerView.DecomposePhase2Event(m, out o);
                OpList.Enqueue(o);
            }

            // Run the pipeline if there were no other buffered events.
            if (firstFlag) Pipeline.Run("received server event", false);
        }

        /// <summary>
        /// Validate a ticket reply.
        /// </summary>
        private void ValidateTicketReply(AnpMsg m, bool uploadFlag)
        {
            if (m.Type == KAnpType.KANP_RES_KFS_DOWNLOAD_REQ)
            {
                if (uploadFlag) throw new Exception("invalid ticket reply type");
                if (m.Elements[0].Type != AnpMsg.AnpType.Bin)
                    throw new Exception("invalid ticket format");
            }

            else if (m.Type == KAnpType.KANP_RES_KFS_UPLOAD_REQ)
            {
                if (!uploadFlag) throw new Exception("invalid ticket reply type");
                if (m.Elements[0].Type != AnpMsg.AnpType.Bin)
                    throw new Exception("invalid ticket format");
            }

            else if (m.Type == KAnpType.KANP_RES_FAIL)
            {
                if (m.Elements[0].Type != AnpMsg.AnpType.String)
                    throw new Exception("server refused to provide ticket");
            }

            else throw new Exception("invalid ticket reply type");
        }

        /// <summary>
        /// This method must be called if any scan failed.
        /// </summary>
        private void OnScanFailed()
        {
            Logging.Log("OnScanFailed() called. InitialScanStatus = " + InitialScanStatus);
            // The watcher might have failed on us before we finished the scan.
            // In that case, the watcher has already been disabled.
            if (InitialScanStatus != InitScanStatus.Fail)
            {
                InitialScanStatus = InitScanStatus.Fail;
                StopWatcher();
            }
        }

        /// <summary>
        /// Stop the watcher if it is running.
        /// </summary>
        public void StopWatcher()
        {
            try
            {
                Logging.Log("StopWatcher() called.");
                if (FsWatcher != null)
                {
                    FsWatcher.StopWatching();
                    FsWatcher = null;
                }
            }
            catch (Exception ex)
            {
                Logging.LogException(ex);
                Base.HandleException(ex, true);
            }
        }

        /// <summary>
        /// This method is called to stop the share. It returns true when the
        /// share is ready to stop.
        /// </summary>
        public bool TryStop()
        {
            // Eventually make the scanner thread cancellable.
            StopWatcher();
            CancelAllNetworkOperations();
            InitialScanStatus = InitScanStatus.None;
            ReportErrorToUserFlag = false;
            AllowedOp = AllowedOpStatus.None;
            InitialAllowedOp = AllowedOpStatus.None;

            return (Gate.EntryCount == 0 &&
                    !ScanRunningFlag &&
                    DownloadManager.Status == DownloadManagerStatus.Idle &&
                    UploadManager.Status == UploadManagerStatus.Idle &&
                    MetaDataManager.Status == MetaDataManagerStatus.Idle);
        }
    }
}