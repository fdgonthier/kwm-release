using System.Collections.Generic;
using System;
using kwm.KwmAppControls;
using System.Diagnostics;
using System.Runtime.Serialization;
using kwm.Utils;
using Tbx.Utils;

namespace kwm.KwmAppControls
{
    /// <summary>
    /// Main status of the workspace. 
    /// </summary>
    public enum KwsMainStatus
    {
        /// <summary>
        /// The workspace has not been spawned successfully (yet).
        /// </summary>
        NotYetSpawned,

        /// <summary>
        /// The workspace was created successfully and it is working as 
        /// advertised.
        /// </summary>
        Good,

        /// <summary>
        /// The workspace needs to be rebuilt to be functional. The type
        /// of rebuild required is given by the RebuildInfo field.
        /// </summary>
        RebuildRequired,

        /// <summary>
        /// The workspace has been scheduled for deletion. Kiss it goodbye.
        /// </summary>
        OnTheWayOut
    }

    /// <summary>
    /// Task to perform in the workspace.
    /// </summary>
    public enum KwsTask
    {
        /// <summary>
        /// Stop the workspace. Don't run applications, don't connect to
        /// anything, don't delete anything.
        /// </summary>
        Stop,

        /// <summary>
        /// Create a new workspace or join or import an existing workspace.
        /// </summary>
        Spawn,

        /// <summary>
        /// Work offline.
        /// </summary>
        WorkOffline,

        /// <summary>
        /// Work online.
        /// </summary>
        WorkOnline,

        /// <summary>
        /// Rebuild the workspace.
        /// </summary>
        Rebuild,

        /// <summary>
        /// Remove the workspace locally.
        /// </summary>
        Remove,

        /// <summary>
        /// Delete the workspace from the server and remove it locally.
        /// </summary>
        DeleteOnServer
    }

    /// <summary>
    /// Represent the run level of a workspace.
    /// </summary>
    public enum KwsRunLevel
    {
        /// <summary>
        /// The workspace isn't ready to work offline.
        /// </summary>
        Stopped,

        /// <summary>
        /// The workspace is ready to work offline.
        /// </summary>
        Offline,

        /// <summary>
        /// The workspace is ready to work online.
        /// </summary>
        Online
    }

    /// <summary>
    /// Status of an application of the workspace.
    /// </summary>
    public enum KwsAppStatus
    {
        /// <summary>
        /// The application is stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// The application is stopping.
        /// </summary>
        Stopping,

        /// <summary>
        /// The application is started.
        /// </summary>
        Started,

        /// <summary>
        /// The application is starting.
        /// </summary>
        Starting
    }

    /// <summary>
    /// Status of the login with the KAS.
    /// </summary>
    public enum KwsLoginStatus
    {
        /// <summary>
        /// The user of the workspace is logged out.
        /// </summary>
        LoggedOut,

        /// <summary>
        /// Waiting for logout command reply.
        /// </summary>
        LoggingOut,

        /// <summary>
        /// Waiting for login command reply.
        /// </summary>
        LoggingIn,

        /// <summary>
        /// The user of the workspace is logged in.
        /// </summary>
        LoggedIn
    }

    /// <summary>
    /// Processing status of an ANP event. These statuses are used in the 
    /// database.
    /// </summary>
    public enum KwsAnpEventStatus
    {
        /// <summary>
        /// The event has not been processed yet.
        /// </summary>
        Unprocessed = 0,

        /// <summary>
        /// The event was processed successfully.
        /// </summary>
        Processed
    }

    /// <summary>
    /// Privilege level associated to a user.
    /// </summary>
    public enum UserPrivLevel
    {
        /// <summary>
        /// The user needs no special permissions.
        /// </summary>
        User,

        /// <summary>
        /// The user must be a workspace manager.
        /// </summary>
        Manager,

        /// <summary>
        /// The user must be a workspace administrator.
        /// </summary>
        Admin,

        /// <summary>
        /// The user must be a system administrator.
        /// </summary>
        Root
    }

    /// <summary>
    /// Type of an immediate notification sent by a workspace state machine.
    /// </summary>
    public enum KwsSmNotif
    {
        /// <summary>
        /// The KAS is connected.
        /// </summary>
        Connected,

        /// <summary>
        /// The KAS is disconnecting or has disconnected. 'Ex' is non-null if
        /// the connection was lost because an error occurred.
        /// </summary>
        Disconnecting,

        /// <summary>
        /// The workspace has logged in.
        /// </summary>
        Login,
        
        /// <summary>
        /// The workspace has logged out normally or the login has failed. 'Ex'
        /// is non-null if the login has failed.
        Logout,

        /// <summary>
        /// A task switch has occurred. 'Task' is set to the task that was
        /// switched to.
        /// </summary>
        TaskSwitch,

        /// <summary>
        /// An application has failed. 'Ex' is set to the exception that
        /// occurred.
        /// </summary>
        AppFailure
    }

    /// <summary>
    /// Notification sent by a workspace state machine.
    /// </summary>
    public class KwsSmNotifEventArgs : EventArgs
    {
        /// <summary>
        /// Type of the notification.
        /// </summary>
        public KwsSmNotif Type;

        /// <summary>
        /// Exception associated to the notification, if any.
        /// </summary>
        public Exception Ex;

        /// <summary>
        /// Task associated to the notification, if any.
        /// </summary>
        public KwsTask Task;

        public KwsSmNotifEventArgs(KwsSmNotif type)
        {
            Type = type;
        }
        public KwsSmNotifEventArgs(KwsSmNotif type, Exception ex)
        {
            Type = type;
            Ex = ex;
        }

        public KwsSmNotifEventArgs(KwsSmNotif type, KwsTask task)
        {
            Type = type;
            Task = task;
        }
    }

    /// <summary>
    /// Information required to rebuild a workspace.
    /// </summary>
    [Serializable]
    public class KwsRebuildInfo
    {
        /// <summary>
        /// True if the events cached in the DB should be deleted.
        /// </summary>
        public bool DeleteCachedEventsFlag = false;

        /// <summary>
        /// True if the local application data should be deleted. 
        /// </summary>
        public bool DeleteLocalDataFlag = false;

        /// <summary>
        /// True if we are rebuilding for a KWM upgrade.
        /// </summary>
        public bool UpgradeFlag = false;
    }

    /// <summary>
    /// Represent a user in a workspace.
    /// 
    /// IMPORTANT: do NOT compare users by object pointers. Use the user ID
    /// do to so. The user objects can change dynamically.
    /// </summary>
    [Serializable]
    public class KwsUser : ISerializable
    {
        /// <summary>
        /// ID of the user.
        /// </summary>
        public UInt32 UserID = 0;

        /// <summary>
        /// Date at which the user was added.
        /// </summary>
        public UInt64 InvitationDate = 0;

        /// <summary>
        /// ID of the inviting user. 0 if none.
        /// </summary>
        public UInt32 InvitedBy = 0;

        /// <summary>
        /// Name given by the workspace administrator.
        /// </summary>
        public String AdminName = "";

        /// <summary>
        /// Name given by the user himself.
        /// </summary>
        public String UserName = "";

        /// <summary>
        /// Email address of the user, if any.
        /// </summary>
        public String EmailAddress = "";

        /// <summary>
        /// Organization name, if the user is a member.
        /// </summary>
        public String OrgName = "";

        /// <summary>
        /// Flags of the user.
        /// </summary>
        public UInt32 Flags = 0;

        /// <summary>
        /// True if the user is a virtual user. A virtual user is a user for 
        /// which no invitation event is associated. The root user is a virtual
        /// user. The KWM user is also a virtual user if its invitation event 
        /// was not yet processed.
        /// NonSerialized.
        /// </summary>
        public bool VirtualFlag = false;

        public bool AdminFlag
        {
            get { return (Flags & KAnpType.KANP_USER_FLAG_ADMIN) > 0; }
            set { SetFlagValue(KAnpType.KANP_USER_FLAG_ADMIN, value); }
        }

        public bool ManagerFlag
        {
            get { return (Flags & KAnpType.KANP_USER_FLAG_MANAGER) > 0; }
            set { SetFlagValue(KAnpType.KANP_USER_FLAG_MANAGER, value); }
        }

        public bool RegisterFlag
        {
            get { return (Flags & KAnpType.KANP_USER_FLAG_REGISTER) > 0; }
            set { SetFlagValue(KAnpType.KANP_USER_FLAG_REGISTER, value); }
        }

        public bool LockFlag
        {
            get { return (Flags & KAnpType.KANP_USER_FLAG_LOCK) > 0; }
            set { SetFlagValue(KAnpType.KANP_USER_FLAG_LOCK, value); }
        }

        public bool BanFlag
        {
            get { return (Flags & KAnpType.KANP_USER_FLAG_BAN) > 0; }
            set { SetFlagValue(KAnpType.KANP_USER_FLAG_BAN, value); }
        }

        /// <summary>
        /// Return the privilege level of the user.
        /// </summary>
        public UserPrivLevel PrivLevel
        {
            get
            {
                if (UserID == 0) return UserPrivLevel.Root;
                if (AdminFlag) return UserPrivLevel.Admin;
                if (ManagerFlag) return UserPrivLevel.Manager;
                return UserPrivLevel.User;
            }
        }

        /// <summary>
        /// Helper method to set or clear a user flag.
        /// </summary>
        private void SetFlagValue(UInt32 flag, bool value)
        {
            if (value) Flags |= flag;
            else Flags &= ~flag; 
        }

        /// <summary>
        /// Get the username to display in the UI, without the user's email address (unless no
        /// username exists).
        /// </summary>
        public String UiSimpleName
        {
            get 
            {
                if (AdminName != "") return AdminName;
                if (UserName != "") return UserName;
                return EmailAddress;
            }
        }

        /// <summary>
        /// Get the given name of the user (prénom). If no AdminName or UserName is
        /// set, use the left part of the email address.
        /// </summary>
        public String UiSimpleGivenName
        {
            get
            {
                // Give priority to AdminName.
                String name = AdminName != "" ? AdminName : UserName;

                // Try to get a valid given name.
                name = GetGivenName(name);

                if (name == "")
                    return GetEmailAddrLeftPart(EmailAddress);
                else
                    return name;
            }
        }

        /// <summary>
        /// Non-deserializing constructor.
        /// </summary>
        public KwsUser()
        {
        }

        /// <summary>
        /// Deserializing constructor.
        /// </summary>
        public KwsUser(SerializationInfo info, StreamingContext context)
        {
            Int32 version = Misc.GetSerializationVersion(info);
            
            UserID = info.GetUInt32("UserID");
            Misc.TryGetUInt64(info, ref InvitationDate, "InvitationDate", 0);
            Misc.TryGetUInt32(info, ref InvitedBy, "InvitedBy", 0);
            AdminName = info.GetString("AdminName");
            UserName = info.GetString("UserName");
            EmailAddress = info.GetString("EmailAddress");
            OrgName = info.GetString("OrgName");

            if (version < 7)
            {
                if (info.GetUInt32("Power") > 0)
                {
                    AdminFlag = true;
                    ManagerFlag = true;
                }

                if (UserName != "")
                {
                    RegisterFlag = true;
                }
            }

            if (version >= 7)
            {
                Flags = info.GetUInt32("Flags");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Misc.AddSerializationVersion(info);
            info.AddValue("UserID", UserID);
            info.AddValue("InvitationDate", InvitationDate);
            info.AddValue("InvitedBy", InvitedBy);
            info.AddValue("AdminName", AdminName);
            info.AddValue("UserName", UserName);
            info.AddValue("EmailAddress", EmailAddress);
            info.AddValue("OrgName", OrgName);
            info.AddValue("Flags", Flags);
        }

        /// <summary>
        /// Return the first characters of the given name until a space
        /// is found.
        /// </summary>
        private String GetGivenName(String name)
        {
            String[] splitted = name.Split(new char[] {' '});
            if (splitted.Length > 0) return splitted[0];
            else return "";
        }

        /// <summary>
        /// Return the left part of an email address, the entire address
        /// if any problem occurs.
        /// </summary>
        private String GetEmailAddrLeftPart(String addr)
        {
            String[] splitted = addr.Split(new char[] { '@' });
            if (splitted.Length > 0) return splitted[0];
            else return addr;
        }

        /// <summary>
        /// Get the username to display in the UI, with its email address appended. If no
        /// username is present, return the email address only.
        /// </summary>
        public String UiFullName
        {
            get 
            {
                if (AdminName == "" && UserName == "") return EmailAddress;
                if (UiSimpleName == EmailAddress) return EmailAddress;
                return UiSimpleName + " (" + EmailAddress + ")";
            }
        }

        /// <summary>
        /// Get the KwsUser description text.
        /// </summary>
        public String UiTooltipText
        {
            get
            {
                if (UiSimpleName == EmailAddress) return EmailAddress;
                return UiSimpleName + Environment.NewLine + EmailAddress;
            }
        }

        /// <summary>
        /// Return true if the user has an administrative name set.
        /// </summary>
        public bool HasAdminName()
        {
            return AdminName != "";
        }
    }
}