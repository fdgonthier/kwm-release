using System;
using System.Diagnostics;
using kwm.KwmAppControls;
using System.Collections.Generic;
using System.Runtime.Serialization;
using kwm.Utils;
using System.IO;
using Tbx.Utils;

/* This file contains miscellaneous stuff related to the workspace internals.
 * Create separate files as needed.
 */

namespace kwm
{
    /// <summary>
    /// Compare two workspaces by name. On equality, the workspaces are compared
    /// by internal ID.
    /// </summary>
    class KwsNameComparer : Comparer<Workspace>
    {
        public override int Compare(Workspace x, Workspace y)
        {
            int res = x.GetKwsName().CompareTo(y.GetKwsName());
            if (res != 0) return res;
            return x.InternalID.CompareTo(y.InternalID);
        }
    }

    /// <summary>
    /// This class is used to report a major error to the user.
    /// </summary>
    public class WmMajorErrorGer : GuiExecRequest
    {
        private String m_context;
        private Exception m_ex;

        public WmMajorErrorGer(String context, Exception ex)
        {
            m_context = context;
            m_ex = ex;
            Logging.LogException(ex);
        }

        public override void Run()
        {
            Misc.KwmTellUser(m_context + ": " + m_ex.Message + ".", System.Windows.Forms.MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// This class is a pure data store that contains information about the
    /// ANP data received from the KAS.
    /// </summary>
    [Serializable]
    public class KwsKAnpState
    {
        /// <summary>
        /// True if the local workspace has caught up with the KAS as far
        /// as ANP events are concerned. This flag is set when the KAS has no
        /// more events to send us and we have no more unprocessed events in
        /// the DB.
        /// </summary>
        [NonSerialized]
        public bool CaughtUpFlag;

        /// <summary>
        /// If CaughtUpFlag is false, this value indicates whether the
        /// workspace is catching up because it is has reconnected or because
        /// it is rebuilding from scratch.
        /// </summary>
        public bool RebuildFlag;

        /// <summary>
        /// ID of the last ANP event received. This is 0 if no event has been
        /// received yet.
        /// </summary>
        public UInt64 LastReceivedEventId = 0;

        /// <summary>
        /// ID of the latest event available on the KAS when the workspace
        /// logs in. This is 0 if no event is available on the KAS.
        /// </summary>
        [NonSerialized]
        public UInt64 LoginLatestEventId;

        /// <summary>
        /// Number of unprocessed ANP events lingering in the DB.
        /// </summary>
        public UInt64 NbUnprocessedEvent = 0;

        /// <summary>
        /// Non-deserializing constructor.
        /// </summary>
        public KwsKAnpState()
        {
            Initialize(new StreamingContext());
        }

        /// <summary>
        /// Initialization code common to both the deserialized and
        /// non-deserialized cases.
        /// </summary>
        [OnDeserialized]
        public void Initialize(StreamingContext context)
        {
            CaughtUpFlag = false;
            LoginLatestEventId = 0;
        }
    }

    /// <summary>
    /// Current step of the spawn task of the workspace.
    /// </summary>
    public enum KwsSpawnTaskStep
    {
        /// <summary>
        /// Wait for the authorization to connect.
        /// </summary>
        Wait,

        /// <summary>
        /// Connect to the KAS.
        /// </summary>
        Connect,

        /// <summary>
        /// Login to the KAS.
        /// </summary>
        Login
    }

    /// <summary>
    /// Current step of the rebuilding task of the workspace.
    /// </summary>
    public enum KwsRebuildTaskStep
    {
        /// <summary>
        /// Not yet started.
        /// </summary>
        None,

        /// <summary>
        /// In progress.
        /// </summary>
        InProgress
    }

    /// <summary>
    /// Current step of the server deletion task of the workspace.
    /// </summary>
    public enum KwsDeleteOnServerStep
    {
        /// <summary>
        /// Waiting for the KAS to be connected and the workspace to be logged 
        /// out.
        /// </summary>
        ConnectedAndLoggedOut,

        /// <summary>
        /// Login to the KAS to delete the workspace.
        /// </summary>
        Login
    }

    /// <summary>
    /// Workspace credentials.
    /// </summary>
    [Serializable]
    public class KwsCredentials : ISerializable
    {
        /// <summary>
        /// Current serialization version when exported in a file. This is 
        /// different than the serialization version of the object in RAM!
        /// </summary>
        public const Int32 ExportVersion = 3;

        /// <summary>
        /// KAS identifier.
        /// </summary>
        public KasIdentifier KasID;

        /// <summary>
        /// External workspace ID.
        /// </summary>
        public UInt64 ExternalID;

        /// <summary>
        /// Email ID associated to this workspace.
        /// </summary>
        public String EmailID = "";

        /// <summary>
        /// Name of the workspace.
        /// </summary>
        public String KwsName = "";

        /// <summary>
        /// Name of the user using this workspace. This field should only be
        /// used when the KWM user is a virtual user.
        public String UserName = "";

        /// <summary>
        /// Email address of the user using this workspace. This field should 
        /// only be used when the KWM user is a virtual user.
        /// </summary>
        public String UserEmailAddress = "";

        /// <summary>
        /// Name of the person who has invited the user, if any. This field 
        /// should only be used when the KWM user is a virtual user.
        /// </summary>
        public String InviterName = "";

        /// <summary>
        /// Email address of the person who has invited the user, if any. This 
        /// field should only be used when the KWM user is a virtual user.
        /// </summary>
        public String InviterEmailAddress = "";

        /// <summary>
        /// ID of the user.
        /// </summary>
        public UInt32 UserID;

        /// <summary>
        /// Login ticket.
        /// </summary>
        public byte[] Ticket = null;

        /// <summary>
        /// Login password.
        /// </summary>
        public String Pwd = "";

        /// <summary>
        /// Address of the KWMO server.
        /// </summary>
        public String KwmoAddress = "";

        /// <summary>
        /// Workspace flags.
        /// </summary>
        public UInt32 Flags = 0;

        public bool PublicFlag
        {
            get { return (Flags & KAnpType.KANP_KWS_FLAG_PUBLIC) > 0; }
            set { SetFlagValue(KAnpType.KANP_KWS_FLAG_PUBLIC, value); }
        }

        public bool FreezeFlag
        {
            get { return (Flags & KAnpType.KANP_KWS_FLAG_FREEZE) > 0; }
            set { SetFlagValue(KAnpType.KANP_KWS_FLAG_FREEZE, value); }
        }

        public bool DeepFreezeFlag
        {
            get { return (Flags & KAnpType.KANP_KWS_FLAG_DEEP_FREEZE) > 0; }
            set { SetFlagValue(KAnpType.KANP_KWS_FLAG_DEEP_FREEZE, value); }
        }

        public bool ThinKfsFlag
        {
            get { return (Flags & KAnpType.KANP_KWS_FLAG_THIN_KFS) > 0; }
            set { SetFlagValue(KAnpType.KANP_KWS_FLAG_THIN_KFS, value); }
        }

        public bool SecureFlag
        {
            get { return (Flags & KAnpType.KANP_KWS_FLAG_SECURE) > 0; }
            set { SetFlagValue(KAnpType.KANP_KWS_FLAG_SECURE, value); }
        }

        /// <summary>
        /// Non-deserializing constructor.
        /// </summary>
        public KwsCredentials()
        {
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public KwsCredentials(KwsCredentials other)
        {
            KasID = other.KasID;
            ExternalID = other.ExternalID;
            EmailID = other.EmailID;
            KwsName = other.KwsName;
            UserName = other.UserName;
            UserEmailAddress = other.UserEmailAddress;
            InviterName = other.InviterName;
            InviterEmailAddress = other.InviterEmailAddress;
            UserID = other.UserID;
            if (other.Ticket != null) Ticket = (byte[])other.Ticket.Clone();
            Pwd = other.Pwd;
            KwmoAddress = other.KwmoAddress;
            Flags = other.Flags;
        }

        /// <summary>
        /// Deserializing constructor.
        /// </summary>
        public KwsCredentials(SerializationInfo info, StreamingContext context)
        {
            Int32 version = Misc.GetSerializationVersion(info);

            KasID = (KasIdentifier)info.GetValue("KasID", typeof(KasIdentifier));
            ExternalID = info.GetUInt64("ExternalID");
            Misc.TryGetString(info, ref EmailID, "EmailID");
            KwsName = info.GetString("KwsName");
            UserName = info.GetString("UserName");
            UserEmailAddress = info.GetString("UserEmailAddress");
            Misc.TryGetString(info, ref InviterName, "InviterName");
            Misc.TryGetString(info, ref InviterEmailAddress, "InviterEmailAddress");
            Misc.TryGetString(info, ref KwmoAddress, "KwmoAddress");            
            UserID = info.GetUInt32("UserID");
            Ticket = (byte[])info.GetValue("Ticket", typeof(byte[]));
            Pwd = info.GetString("Pwd");

            if (version < 7)
            {
                UInt32 val = 0;
                Misc.TryGetUInt32(info, ref val, "PublicFlag", 0);
                if (val > 0) PublicFlag = true;
                
                // When the SecureFlag field was absent, all workspaces were
                // secure.
                Misc.TryGetUInt32(info, ref val, "SecureFlag", 1);
                if (val > 0) SecureFlag = true;
            }

            if (version >= 7)
            {
                Flags = info.GetUInt32("Flags");
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Misc.AddSerializationVersion(info);
            info.AddValue("KasID", KasID);
            info.AddValue("ExternalID", ExternalID);
            info.AddValue("EmailID", EmailID);
            info.AddValue("KwsName", KwsName);
            info.AddValue("UserName", UserName);
            info.AddValue("UserEmailAddress", UserEmailAddress);
            info.AddValue("InviterName", InviterName);
            info.AddValue("InviterEmailAddress", InviterEmailAddress);
            info.AddValue("UserID", UserID);
            info.AddValue("Ticket", Ticket);
            info.AddValue("Pwd", Pwd);
            info.AddValue("KwmoAddress", KwmoAddress);
            info.AddValue("Flags", Flags);
        }

        /// <summary>
        /// Helper method to set or clear a workspace flag.
        /// </summary>
        private void SetFlagValue(UInt32 flag, bool value)
        {
            if (value) Flags |= flag;
            else Flags &= ~flag;
        }
    }

    /// <summary>
    /// This class contains information about the users of a workspace.
    /// </summary>
    [Serializable]
    public class KwsUserInfo : ISerializable
    {
        /// <summary>
        /// Reference to the workspace.
        /// </summary>
        private Workspace m_kws;

        /// <summary>
        /// Tree of non-virtual users indexed by user ID.
        /// </summary>
        public SortedDictionary<UInt32, KwsUser> UserTree = new SortedDictionary<UInt32, KwsUser>();

        /// <summary>
        /// Reference to the root virtual user.
        /// NonSerialized.
        /// </summary>
        public KwsUser RootUser;

        /// <summary>
        /// Reference to the workspace creator, if any.
        /// </summary>
        public KwsUser Creator
        {
            get
            {
                return GetNonVirtualUserByID(1);
            }
        }

        /// <summary>
        /// Reference to the KWM user. This user may be virtual.
        /// </summary>
        public KwsUser KwmUser
        {
            get
            {
                return GetUserByID(m_kws.CoreData.Credentials.UserID);
            }
        }

        /// <summary>
        /// Non-deserializing constructor.
        /// </summary>
        public KwsUserInfo()
        {
        }

        /// <summary>
        /// Deserializing constructor.
        /// </summary>
        public KwsUserInfo(SerializationInfo info, StreamingContext context)
        {
            UserTree = (SortedDictionary<UInt32, KwsUser>)info.GetValue("UserTree", typeof(SortedDictionary<UInt32, KwsUser>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Misc.AddSerializationVersion(info);
            info.AddValue("UserTree", UserTree);
        }

        /// <summary>
        /// Initialization code common to both the deserialized and
        /// non-deserialized cases.
        /// </summary>
        public void Initialize(Workspace kws)
        {
            m_kws = kws;
            RootUser = new KwsUser();
            RootUser.AdminFlag = true;
            RootUser.ManagerFlag = true;
            RootUser.RegisterFlag = true;
            RootUser.VirtualFlag = true;
            RootUser.AdminName = RootUser.UserName = "System Administrator";
        }

        /// <summary>
        /// Return the user having the ID specified, if any. Virtual users
        /// may be returned.
        /// </summary>
        public KwsUser GetUserByID(UInt32 ID)
        {
            if (ID == 0) return RootUser;
            if (UserTree.ContainsKey(ID)) return UserTree[ID];

            KwsCredentials creds = m_kws.CoreData.Credentials;
            if (ID == creds.UserID)
            {
                KwsUser user = new KwsUser();
                user.UserID = creds.UserID;
                user.AdminName = creds.UserName;
                user.UserName = creds.UserName;
                user.EmailAddress = creds.UserEmailAddress;
                user.VirtualFlag = true;
                return user;
            }

            return null;
        }

        /// <summary>
        /// Return the non-virtual user having the ID specified, if any.
        /// </summary>
        public KwsUser GetNonVirtualUserByID(UInt32 ID)
        {
            if (UserTree.ContainsKey(ID)) return UserTree[ID];
            return null;
        }

        /// <summary>
        /// Return the non-virtual user having the email address specified,
        /// if any.
        /// </summary>
        public KwsUser GetUserByEmailAddress(String emailAddress)
        {
            foreach (KwsUser u in UserTree.Values) if (emailAddress.ToLower() == u.EmailAddress.ToLower()) return u;
            return null;
        }
    }

    /// <summary>
    /// This class contains the core data of the workspace (credentials, user
    /// list, etc).
    /// </summary>
    [Serializable]
    public class KwsCoreData : ISerializable
    {
        /// <summary>
        /// Workspace credentials.
        /// </summary>
        public KwsCredentials Credentials = null;

        /// <summary>
        /// Users of the workspace.
        /// </summary>
        public KwsUserInfo UserInfo = new KwsUserInfo();

        /// <summary>
        /// Information required to rebuild the workspace.
        /// </summary>
        public KwsRebuildInfo RebuildInfo = new KwsRebuildInfo();

        /// <summary>
        /// Creation date of the workspace if known, otherwise UINT64.MaxValue.
        /// </summary>
        public UInt64 CreationDate
        {
            get
            {
                if (UserInfo.Creator != null) return UserInfo.Creator.InvitationDate;
                return UInt64.MaxValue;
            }
        }

        /// <summary>
        /// Non-deserializing constructor.
        /// </summary>
        public KwsCoreData()
        {
        }

        /// <summary>
        /// Deserializing constructor.
        /// </summary>
        public KwsCoreData(SerializationInfo info, StreamingContext context)
        {
            Credentials = (KwsCredentials)info.GetValue("Credentials", typeof(KwsCredentials));
            UserInfo = (KwsUserInfo)info.GetValue("UserInfo", typeof(KwsUserInfo));
            RebuildInfo = (KwsRebuildInfo)info.GetValue("RebuildInfo", typeof(KwsRebuildInfo));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Misc.AddSerializationVersion(info);
            info.AddValue("Credentials", Credentials);
            info.AddValue("UserInfo", UserInfo);
            info.AddValue("RebuildInfo", RebuildInfo);
        }

        /// <summary>
        /// Initialization code common to both the deserialized and
        /// non-deserialized cases.
        /// </summary>
        public void Initialize(Workspace kws)
        {
            UserInfo.Initialize(kws);
        }
    }

    /// <summary>
    /// Contains the entire input of the user when inviting users to a 
    /// workspace.
    /// </summary>
    public class KwsInviteOpParams
    {
        /// <summary>
        /// List of users being invited.
        /// </summary>
        public List<KwsInviteOpUser> UserArray = new List<KwsInviteOpUser>();

        /// <summary>
        /// Personalized message in plain text to send along the invitation 
        /// email. Empty if none.
        /// </summary>
        public String Message = "";

        /// <summary>
        /// WLEU returned by the KCD.
        /// </summary>
        public String WLEU = "";

        /// <summary>
        /// Set to true to tell the KCD to send the invitation email
        /// by itself. Used when the invitation is done via the KWM.
        /// </summary>
        public bool KcdSendInvitationEmailFlag;

        /// <summary>
        /// Set the AlreadyInvitedFlag for the users which have already been 
        /// invited to the workspace.
        /// </summary>
        public void FlagInvitedUsers(KwsUserInfo kui)
        {
            foreach (KwsInviteOpUser iu in UserArray)
                iu.AlreadyInvitedFlag = (kui.GetUserByEmailAddress(iu.EmailAddress) != null);
        }

        /// <summary>
        /// Return the list of users that are not already invited to the workspace.
        /// </summary>
        public List<KwsInviteOpUser> NotAlreadyInvitedUserArray
        {
            get
            {
                List<KwsInviteOpUser> list = new List<KwsInviteOpUser>();
                foreach (KwsInviteOpUser u in UserArray)
                    if (!u.AlreadyInvitedFlag) list.Add(u);
                return list;
            }
        }
        /// <summary>
        /// Get the invitees formatted as a single string, each email address 
        /// separated by a space. 
        /// </summary>
        public String GetInviteesLine()
        {
            String line = "";

            foreach (KwsInviteOpUser i in UserArray)
            {
                if (line != "") line += " ";
                line += i.EmailAddress;
            }

            return line;            
        }

        /// <summary>
        /// Return true if at least one user has not been successfully invited.
        /// </summary>
        public bool HasInvitationErrors()
        {
            foreach (KwsInviteOpUser u in UserArray)
            {
                if (u.Error != "")
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Return the list of email addresses that could not be invited, followed by
        /// the error string.
        /// </summary>
        public String GetErroneousInviteesText()
        {
            String msg = "";
            foreach (KwsInviteOpUser o in UserArray)
            {
                if (o.Error != "")
                {
                    if (msg != "") msg += Environment.NewLine;
                    msg += o.EmailAddress + " (" + o.Error + ")";
                }
            }
            return msg;
        }
    }

    /// <summary>
    /// Represent a user invited to a workspace.
    /// </summary>
    public class KwsInviteOpUser
    {
        /// <summary>
        /// Name of the user, if any.
        /// </summary>
        public String UserName = "";

        /// <summary>
        /// User email address.
        /// </summary>
        public String EmailAddress = "";

        /// <summary>
        /// Inviter-specified password, if any.
        /// </summary>
        public String Pwd = "";

        /// <summary>
        /// User key ID. If none, set to 0.
        /// </summary>
        public UInt64 KeyID = 0;

        /// <summary>
        /// User organization's name, if any.
        /// </summary>
        public String OrgName = "";

        /// <summary>
        /// Email ID used to invite the user, if any.
        /// </summary>
        public String EmailID;

        /// <summary>
        /// URL that should appear in the invitation mail.
        /// </summary>
        public String Url;

        /// <summary>
        /// Invitation error for the user, if any.
        /// </summary>
        public String Error;

        /// <summary>
        /// Flag telling if the user has already been invited to the workspace.
        /// </summary>
        public bool AlreadyInvitedFlag = false;

        public KwsInviteOpUser()
        {
        }

        public KwsInviteOpUser(String emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}