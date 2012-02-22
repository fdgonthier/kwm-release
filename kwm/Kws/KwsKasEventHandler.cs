using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using kwm.Utils;
using kwm.KwmAppControls;
using Tbx.Utils;

namespace kwm
{
    /// <summary>
    /// Handle the workspace events received from the KAS that do not concern 
    /// the applications.
    /// </summary>
    public class KwsKasEventHandler
    {
        /// <summary>
        /// Reference to the workspace.
        /// </summary>
        private Workspace m_kws;

        public KwsKasEventHandler(Workspace kws)
        {
            m_kws = kws;
        }

        /// <summary>
        /// Handle an ANP event.
        /// </summary>
        public KwsAnpEventStatus HandleAnpEvent(AnpMsg msg)
        {
            UInt32 type = msg.Type;

            // Dispatch.
            if (type == KAnpType.KANP_EVT_KWS_CREATED) return HandleKwsCreatedEvent(msg);
            else if (type == KAnpType.KANP_EVT_KWS_INVITED) return HandleKwsInvitationEvent(msg);
            else if (type == KAnpType.KANP_EVT_KWS_USER_REGISTERED) return HandleUserRegisteredEvent(msg);
            else if (type == KAnpType.KANP_EVT_KWS_DELETED) return HandleKwsDeletedEvent();
            else if (type == KAnpType.KANP_EVT_KWS_LOG_OUT) return HandleKwsLogOut(msg);
            else if (type == KAnpType.KANP_EVT_KWS_PROP_CHANGE) return HandleKwsPropChange(msg);
            else return KwsAnpEventStatus.Unprocessed;
        }

        private KwsAnpEventStatus HandleKwsCreatedEvent(AnpMsg msg)
        {
            KwsCredentials creds = m_kws.CoreData.Credentials;

            // Add the creator to the user list.
            KwsUser user = new KwsUser();
            user.UserID = msg.Elements[2].UInt32;
            user.InvitationDate = msg.Elements[1].UInt64;
            user.AdminName = msg.Elements[3].String;
            user.EmailAddress = msg.Elements[4].String;
            user.OrgName = msg.Elements[msg.Minor <= 2 ? 7 : 5].String;
            user.AdminFlag = true;
            user.ManagerFlag = true;
            user.RegisterFlag = true;
            m_kws.CoreData.UserInfo.UserTree[user.UserID] = user;

            // Update the workspace data.
            if (msg.Minor <= 2)
            {
                creds.SecureFlag = true;
            }

            if (msg.Minor >= 3)
            {
                creds.KwsName = msg.Elements[6].String;
                creds.Flags = msg.Elements[7].UInt32;
                creds.KwmoAddress = msg.Elements[8].String;
            }

            m_kws.StateChangeUpdate(false);

            return KwsAnpEventStatus.Processed;
        }

        private KwsAnpEventStatus HandleKwsInvitationEvent(AnpMsg msg)
        {
            UInt32 nbUser = msg.Elements[msg.Minor <= 2 ? 2 : 3].UInt32;

            // This is not supposed to happen, unless in the case of a broken
            // KWM. Indeed, the server does not enforce any kind of restriction
            // regarding the number of invitees in an INVITE command. If a KWM
            // sends such a command with no invitees, the server will fire an
            // empty INVITE event.
            if (nbUser < 1) return KwsAnpEventStatus.Processed;

            List<KwsUser> users = new List<KwsUser>();

            // Add the users in the user list.
            int j = (msg.Minor <= 2) ? 3 : 4;
            for (int i = 0; i < nbUser; i++)
            {                
                KwsUser user = new KwsUser();
                user.UserID = msg.Elements[j++].UInt32;
                user.InvitationDate = msg.Elements[1].UInt64;
                if (msg.Minor >= 3) user.InvitedBy = msg.Elements[2].UInt32;
                user.AdminName = msg.Elements[j++].String;
                user.EmailAddress = msg.Elements[j++].String;
                if (msg.Minor <= 2) j += 2;
                user.OrgName = msg.Elements[j++].String;
                users.Add(user);
                m_kws.CoreData.UserInfo.UserTree[user.UserID] = user;
            }

            m_kws.StateChangeUpdate(false);

            // Never notify new public workspace invitations. They are automatically
            // generated when a recipient takes an action on the Web page.
            if (!m_kws.IsPublicKws())
            {
                // Notify the new invitees to the user if it was not him that invited them.
                // Note: we only have this information from v3 and later. In case of an older
                // version, notify in all cases.
                if (msg.Minor >= 3)
                {
                    if (msg.Elements[2].UInt32 != m_kws.CoreData.Credentials.UserID)
                        m_kws.NotifyUser(new KwsInvitationNotificationItem(m_kws, users));
                }

                else
                {
                    m_kws.NotifyUser(new KwsInvitationNotificationItem(m_kws, users));
                }
            }
            
            return KwsAnpEventStatus.Processed;
        }

        private KwsAnpEventStatus HandleUserRegisteredEvent(AnpMsg msg)
        {
            UInt32 userID = msg.Elements[2].UInt32;
            String userName = msg.Elements[3].String;

            KwsUser user = m_kws.CoreData.UserInfo.GetNonVirtualUserByID(userID);
            if (user == null) throw new Exception("no such user");
            user.UserName = userName;

            // Refresh the user list.
            m_kws.StateChangeUpdate(false);

            return KwsAnpEventStatus.Processed;
        }

        private KwsAnpEventStatus HandleKwsDeletedEvent()
        {
            m_kws.KasLoginHandler.LoginResult = KwsLoginResult.DeletedKws;
            m_kws.KasLoginHandler.LoginResultString = "the " + Base.GetKwsString() + " has been deleted";
            m_kws.Sm.RequestTaskSwitch(KwsTask.WorkOffline);
            return KwsAnpEventStatus.Processed;
        }

        private KwsAnpEventStatus HandleKwsLogOut(AnpMsg msg)
        {
            KwsKasLoginHandler handler = m_kws.KasLoginHandler;
            handler.LoginResult = handler.TranslateKcdLoginStatusCode(msg.Elements[2].UInt32);
            handler.LoginResultString = msg.Elements[3].String;
            m_kws.Sm.RequestTaskSwitch(KwsTask.WorkOffline);
            return KwsAnpEventStatus.Processed;
        }

        private KwsAnpEventStatus HandleKwsPropChange(AnpMsg msg)
        {
            bool kwsNameChangeFlag = false;
            KwsCredentials creds = m_kws.CoreData.Credentials;
            KwsUserInfo userInfo = m_kws.CoreData.UserInfo;

            int i = 3;
            UInt32 nbChange = msg.Elements[i++].UInt32;
            String notifString = "";

            for (UInt32 j = 0; j < nbChange; j++)
            {
                UInt32 type = msg.Elements[i++].UInt32;

                if (type == KAnpType.KANP_PROP_KWS_NAME)
                {
                    kwsNameChangeFlag = true;
                    creds.KwsName = msg.Elements[i++].String;
                    notifString = "This " + Base.GetKwsString() + " has just been renamed.";
                }

                else if (type == KAnpType.KANP_PROP_KWS_FLAGS)
                    creds.Flags = msg.Elements[i++].UInt32;

                else
                {
                    KwsUser user = userInfo.GetNonVirtualUserByID(msg.Elements[i++].UInt32);
                    if (user == null) throw new Exception("no such user");
                    
                    if (type == KAnpType.KANP_PROP_USER_NAME_ADMIN)
                        user.AdminName = msg.Elements[i++].String;

                    else if (type == KAnpType.KANP_PROP_USER_NAME_USER)
                        user.UserName = msg.Elements[i++].String;

                    else if (type == KAnpType.KANP_PROP_USER_FLAGS)
                        user.Flags = msg.Elements[i++].UInt32;

                    else throw new Exception("invalid user property type");
                }
            }

            // Refresh the user list.
            m_kws.StateChangeUpdate(kwsNameChangeFlag);

            return KwsAnpEventStatus.Processed;
        }
    }
}