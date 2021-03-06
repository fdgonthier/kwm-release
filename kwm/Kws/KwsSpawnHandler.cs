using System;
using System.Diagnostics;
using kwm.KwmAppControls;
using System.Collections.Generic;
using System.Runtime.Serialization;
using kwm.Utils;
using System.IO;
using Tbx.Utils;
using kwm.KwmAppControls.AppKfs;

namespace kwm
{
    /// <summary>
    /// This class contains code common to operations where workspaces are
    /// spawned.
    /// </summary>
    public class KwsSpawnOpHelper
    {
        /// <summary>
        /// Reference to the operation.
        /// </summary>
        public KwsCoreOp Op;

        /// <summary>
        /// Reference to the credentials.
        /// </summary>
        public KwsCredentials Creds;

        /// <summary>
        /// True if the workspace should be added as the first workspace in its
        /// parent folder.
        /// </summary>
        public bool FirstInBrowserFlag = false;

        /// <summary>
        /// True if the workspace should be selected in the workspace browser.
        /// </summary>
        public bool SelectInBrowserFlag = true;

        public KwsSpawnOpHelper(KwsCoreOp op, KwsCredentials creds)
        {
            Op = op;
            Creds = creds;
        }

        /// <summary>
        /// Post a login ticket query.
        /// </summary>
        public void PostLoginTicketQuery(WmWinRegistry registry, WmLoginTicketQueryDelegate callback)
        {
            Logging.Log(1, "PostLoginTicketQuery()");
            WmLoginTicketQuery loginQuery = new WmLoginTicketQuery();
            loginQuery.Submit(Op.Wm.KmodBroker, registry, callback);
            Op.m_kmodQuery = loginQuery;
        }

        /// <summary>
        /// Handle the results of the login ticket query.
        /// </summary>
        public void HandleLoginTicketQueryResult(WmLoginTicketQuery query, WmWinRegistry registry)
        {
            Logging.Log(1, "HandleLoginTicketQueryResult()");
            Debug.Assert(Op.m_kmodQuery == query);
            Op.m_kmodQuery = null;

            // Update the registry, if required.
            query.UpdateRegistry(registry);

            // Update the credentials, if required.
            if (query.Res == WmLoginTicketQueryRes.OK)
            {
                Creds.Ticket = query.Ticket.BinaryTicket;
                Creds.UserName = query.Ticket.AnpTicket.Elements[0].String;
                Creds.UserEmailAddress = query.Ticket.AnpTicket.Elements[1].String;
            }
        }

        /// <summary>
        /// Helper to create the workspace object.
        /// </summary>
        public void CreateKwsObject(KwsBrowserFolderNode folder)
        {
            Logging.Log(1, "CreateKwsObject()");
            Op.m_opStep = KwmCoreKwsOpStep.CreateKwsObject;
            Op.m_kws = Op.Wm.CreateWorkspaceObject(Creds, folder, FirstInBrowserFlag);
            Op.RegisterToKws(true);
            Op.m_kws.Sm.RequestTaskSwitch(KwsTask.Spawn);
        }

        /// <summary>
        /// Helper to complete the spawning of the workspace.
        /// </summary>
        public void CompleteSpawn(bool unregisterFlag, bool serializeFlag)
        {
            Logging.Log(1, "CompleteSpawn()");
            Debug.Assert(Op.m_kws.Sm.GetCurrentTask() == KwsTask.Spawn);
            Debug.Assert(Op.m_kws.MainStatus == KwsMainStatus.NotYetSpawned);

            // Unregister from the workspace.
            if (unregisterFlag) Op.UnregisterFromKws();

            // Update the main status of the workspace.
            Op.m_kws.MainStatus = KwsMainStatus.Good;
            Op.m_kws.SetDirty();

            // Rebuild the browser list to show the workspace node.
            Op.Wm.UiBroker.RequestBrowserUiUpdate(true);

            // Ask the state machine to work online.
            Op.m_kws.Sm.RequestTaskSwitch(KwsTask.WorkOnline);

            // Select the workspace if requested.
            if (!Op.m_doneFlag && SelectInBrowserFlag)
                Op.Wm.UiBroker.RequestSelectKws(Op.m_kws, true);

            // Serialize the KWM so we don't lose the workspace if the KWM
            // crashes, unless this is an import (too slow).
            if (serializeFlag) Op.Wm.Serialize(false);
        }

        /// <summary>
        /// Get the KAS ID used to create a new workspace.
        /// </summary>
        public KasIdentifier GetKasID(WmWinRegistry registry)
        {
            String host;
            UInt16 port;

            if (Misc.ApplicationSettings.UseCustomKas && Misc.ApplicationSettings.CustomKasAddress != "")
            {
                host = Misc.ApplicationSettings.CustomKasAddress;

                if (Misc.ApplicationSettings.CustomKasPort == "")
                    port = 443;

                else
                {
                    try
                    {
                        port = UInt16.Parse(Misc.ApplicationSettings.CustomKasPort);
                    }

                    catch (Exception e)
                    {
                        Logging.LogException(e);
                        port = 443;
                    }
                }

                Logging.Log("Using custom KAS server: " + host + ":" + port.ToString());
            }

            else
            {
                host = registry.KcdAddr;
                port = registry.KcdPort;
            }

            return new KasIdentifier(host, port);
        }
    }

    /// <summary>
    /// Base class for operations which create a workspace on the KAS.
    /// </summary>
    public abstract class BaseNewKwsCoreOp : KwsCoreOp
    {
        /// <summary>
        /// Workspace credentials.
        /// </summary>
        public KwsCredentials Creds = new KwsCredentials();

        /// <summary>
        /// Reference to the spawn helper.
        /// </summary>
        protected KwsSpawnOpHelper m_sh;

        /// <summary>
        /// Reference to the invite helper.
        /// </summary>
        protected KwsInviteOpHelper m_ih;

        /// <summary>
        /// Users to invite.
        /// </summary>
        public KwsInviteOpParams InviteParams
        {
            get { return m_ih.InviteParams; }
            set { m_ih.InviteParams = value; }
        }

        public BaseNewKwsCoreOp(WorkspaceManager wm)
            : base(wm)
        {
            m_sh = new KwsSpawnOpHelper(this, Creds);
            m_ih = new KwsInviteOpHelper(this, Creds);
        }

        public override void HandleKasConnected()
        {
            Logging.Log(1, "HandleKasConnected()");
            // We were waiting for the KAS connection.
            if (!m_doneFlag && m_opStep == KwmCoreKwsOpStep.KasConnectAndGetTicket) HandleKasConnectAndGetTicketResult();
        }

        public override void HandleKwsLoginSuccess()
        {
            Logging.Log(1, "HandleKwsLoginSuccess()");
            // We were waiting for the KAS workspace login.
            if (!m_doneFlag && m_opStep == KwmCoreKwsOpStep.KasLogin) HandleLoggedIn();
        }

        /// <summary>
        /// Connect to the KAS and get a ticket.
        /// </summary>
        public void KasConnectAndGetTicket(WmLoginTicketQueryDelegate callback)
        {
            m_opStep = KwmCoreKwsOpStep.KasConnectAndGetTicket;
            Creds.Ticket = null;
            m_kws.Sm.SetSpawnStep(KwsSpawnTaskStep.Connect);
            m_sh.PostLoginTicketQuery(WmWinRegistry.Spawn(), callback);
        }

        /// <summary>
        /// Called when the KAS becomes connected or when the ticket is received.
        /// </summary>
        protected void HandleKasConnectAndGetTicketResult()
        {
            Logging.Log(1, "HandleKasConnectAndGetTicketResult()");
            if (!CheckCtx(KwmCoreKwsOpStep.KasConnectAndGetTicket) ||
                m_kws.Kas.ConnStatus != KasConnStatus.Connected ||
                Creds.Ticket == null)
            {
                return;
            }

            // Create the workspace on the KAS.
            CreateKwsOnKas();
        }

        /// <summary>
        /// Create the workspace on the KAS.
        /// </summary>
        private void CreateKwsOnKas()
        {
            Logging.Log(1, "CreateKwsOnKas()");
            m_opStep = KwmCoreKwsOpStep.KasCreate;
            AnpMsg msg = Wm.NewKAnpCmd(m_kws.Kas.MinorVersion, KAnpType.KANP_CMD_MGT_CREATE_KWS);
            msg.AddString(Creds.KwsName);
            msg.AddBin(Creds.Ticket);
            msg.AddUInt32(Convert.ToUInt32(Creds.PublicFlag));
            msg.AddUInt32(Convert.ToUInt32(Creds.SecureFlag));

            // Thin KFS available in >4. Always use Thin for public workspaces,
            // otherwise use the user setting (which defaults to true).
            if (m_kws.Kas.MinorVersion >= 4)
            {
                if (Creds.PublicFlag) msg.AddUInt32(1);
                else msg.AddUInt32(Convert.ToUInt32(Misc.ApplicationSettings.NewKwsThinKfsDefaultFlag));
            }
            m_kws.PostNonAppKasQuery(msg, null, HandleCreateKwsOnKasResult, false);
        }

        /// <summary>
        /// Called when the create workspace command reply is received.
        /// </summary>
        private void HandleCreateKwsOnKasResult(KasQuery query)
        {
            if (!CheckCtx(KwmCoreKwsOpStep.KasCreate)) return;

            try
            {
                AnpMsg res = query.Res;

                // Handle failures.
                if (res.Type != KAnpType.KANP_RES_MGT_KWS_CREATED)
                    throw Misc.HandleUnexpectedKAnpReply("create " + Base.GetKwsString(), res);

                // Update the workspace credentials.
                Creds.ExternalID = res.Elements[0].UInt64;
                Creds.EmailID = res.Elements[1].String;

                // Perform the login.
                PerformKasLogin();
            }

            catch (Exception ex)
            {
                HandleMiscFailure(ex);
            }
        }

        /// <summary>
        /// Perform the KAS workspace login.
        /// </summary>
        private void PerformKasLogin()
        {
            Logging.Log(1, "PerformKasLogin()");
            m_opStep = KwmCoreKwsOpStep.KasLogin;
            m_kws.Sm.SetLoginType(KwsLoginType.Cached);
            m_kws.Sm.SetSpawnStep(KwsSpawnTaskStep.Login);
        }

        /// <summary>
        /// Called when the workspace is logged in.
        /// </summary>
        public abstract void HandleLoggedIn();
    }

    /// <summary>
    /// Create workspace operation of the KWM.
    /// </summary>
    public class KwmCreateKwsOp : BaseNewKwsCoreOp
    {
        /// <summary>
        /// Folder in which the new workspace will be created.
        /// </summary>
        private KwsBrowserFolderNode m_parentNode;

        public KwmCreateKwsOp(WorkspaceManager wm, KwsBrowserFolderNode parentNode, String kwsName)
            : base(wm)
        {
            m_parentNode = parentNode;
            Creds.KwsName = kwsName;
        }

        public override void HandleLoggedIn()
        {
            PerformKasInvite();
        }

        public override void HandleMiscFailure(Exception ex)
        {
            Logging.Log(1, "HandleMiscFailure()");
            // Cancel the operation.
            CancelOp(false, ex);
        }

        /// <summary>
        /// Start the operation. This method blocks.
        /// </summary>
        public void StartOp()
        {
            Debug.Assert(m_opStep == KwmCoreKwsOpStep.None);
            m_opStep = KwmCoreKwsOpStep.WizardEntry;
            m_wizard = new frmCreateKwsWizard(this, Wm.KmodBroker);
            m_ih.Wiz.ShowAndReenter();
        }

        /// <summary>
        /// Cancel the operation in progress. The wizard end page is shown
        /// unless cancelWizardFlag is true, in which case the wizard is 
        /// cancelled as well. This can be called from any context.
        /// </summary>
        public void CancelOp(bool cancelWizardFlag, Exception err)
        {
            Logging.Log(1, "CancelOp()");
            if (m_doneFlag) return;
            if (!wizEntryFlag) cancelWizardFlag = true;
            OpRes = KwsCoreOpRes.MiscError;
            ErrorEx = err;
            CancelCtx(cancelWizardFlag);
            if (!cancelWizardFlag) ShowWizardEnd();
        }

        /// <summary>
        /// Retry the operation. If confirmFlag is true, the user powers will
        /// be confirmed if needs be. This can be called from any context once
        /// the wizard has been reentered.
        /// </summary>
        public void RetryOp(bool confirmFlag)
        {
            Logging.Log(1, "RetryOp()");
            Debug.Assert(wizEntryFlag);
            if (m_doneFlag) return;

            // Cancel the operations in progress, if any.
            CancelCtx(false);

            // When the operation is retried, the credentials may have been 
            // assigned to a workspace scheduled for deletion, so clone them.
            CloneCreds();

            // Create the workspace if possible.
            WmWinRegistry registry = WmWinRegistry.Spawn();
            if (CheckCanCreateKws(registry, true))
            {
                Creds.KasID = m_sh.GetKasID(registry);
                m_opStep = KwmCoreKwsOpStep.UIStep;
                PerformNextUiStep();
            }
        }

        /// <summary>
        /// Called by the wizard when it has displayed itself.
        /// </summary>
        public void WizEntry()
        {
            Logging.Log(1, "WizEntry()");
            if (!CheckCtx(KwmCoreKwsOpStep.WizardEntry)) return;
            wizEntryFlag = true;
            RetryOp(true);
        }

        /// <summary>
        /// Called when the wizard has obtained the global workspace
        /// information.
        /// </summary>
        public void WizCreatePageNext()
        {
            Logging.Log(1, "WizCreatePageNext()");
            if (!CheckCtx(KwmCoreKwsOpStep.UIStep)) return;
            m_uiStep = KwmCoreKwsUiStep.Create;
            PerformNextUiStep();
        }

        /// <summary>
        /// Called when the wizard has obtained the invited users information.
        /// </summary>
        public void WizInvitePageNext()
        {
            Logging.Log(1, "WizInvitePageNext()");
            if (!CheckCtx(KwmCoreKwsOpStep.UIStep)) return;
            m_uiStep = KwmCoreKwsUiStep.Invite;
            PerformNextUiStep();
        }

        /// <summary>
        /// Called when the wizard has obtained the passwords we needed.
        /// </summary>
        public void WizPasswordPageNext()
        {
            Logging.Log(1, "WizPasswordPageNext()");
            if (!CheckCtx(KwmCoreKwsOpStep.UIStep)) return;
            m_uiStep = KwmCoreKwsUiStep.Pwd;
            PerformNextUiStep();
        }

        /// <summary>
        // Clone and update the current credentials.
        /// </summary>
        private void CloneCreds()
        {
            Creds = new KwsCredentials(Creds);
            m_sh.Creds = Creds;
            m_ih.Creds = Creds;
        }

        /// <summary>
        /// Cancel all pending operations and the wizard itself if required.
        /// </summary>
        private void CancelCtx(bool cancelWizardFlag)
        {
            if (m_doneFlag) return;

            // The operation is completed if the wizard is to be closed.
            m_doneFlag = cancelWizardFlag;

            // Clear the current KMOD query.
            ClearKmodQuery();

            // Unregister from the workspace and remove it.
            UnregisterAndRemoveKws();

            // Close the wizard.
            if (m_doneFlag) ClearWizard();
        }

        /// <summary>
        /// If the registry information indicates that we can create a
        /// workspace, this method returns true. Otherwise, it returns false 
        /// and either sends a get ticket query if confirmFlag is true and we
        /// have a login token or goes to the result page otherwise.
        /// </summary>
        private bool CheckCanCreateKws(WmWinRegistry registry, bool confirmFlag)
        {
            Logging.Log(1, "CheckCanCreateKws()");
            bool okFlag = false;
            if (!registry.CanLoginOnKps()) OpRes = KwsCoreOpRes.InvalidCfg;
            else if (!registry.CanCreateKws()) OpRes = KwsCoreOpRes.NoPower;
            else okFlag = true;

            if (!okFlag)
            {
                if (confirmFlag && registry.CanLoginOnKps())
                {
                    m_ih.Wiz.ShowPleaseWait("Getting authorization from SOS server " + registry.KpsAddr + ":" + registry.KpsPort);
                    m_opStep = KwmCoreKwsOpStep.InitialPowerQuery;
                    m_sh.PostLoginTicketQuery(registry, HandleLoginTicketQueryResult);
                }

                else
                {
                    ShowWizardEnd();
                }
            }

            return okFlag;
        }

        /// <summary>
        /// Call the handler corresponding to the next UI step to perform.
        /// </summary>
        private void PerformNextUiStep()
        {
            Logging.Log(1, "PerformNextUiStep()");
            if (m_uiStep == KwmCoreKwsUiStep.None) m_ih.Wiz.ShowCreationPage();
            else if (m_uiStep == KwmCoreKwsUiStep.Create) m_ih.Wiz.ShowInvitePage();
            else if (m_uiStep == KwmCoreKwsUiStep.Invite) LookupRecAddr();
            else CreateKwsObject();
        }

        /// <summary>
        /// Handle the results of the login ticket query.
        /// </summary>
        private void HandleLoginTicketQueryResult(WmLoginTicketQuery query)
        {
            if (m_doneFlag) return;
            Debug.Assert(m_opStep == KwmCoreKwsOpStep.InitialPowerQuery ||
                         m_opStep == KwmCoreKwsOpStep.KasConnectAndGetTicket);

            WmWinRegistry registry = WmWinRegistry.Spawn();
            m_sh.HandleLoginTicketQueryResult(query, registry);

            // An error occurred.
            if (query.Res == WmLoginTicketQueryRes.MiscError)
            {
                OpRes = KwsCoreOpRes.MiscError;
                ErrorEx = new Exception(query.OutDesc);
                ShowWizardEnd();
            }

            // We can create the workspace.
            else if (CheckCanCreateKws(registry, false))
            {
                // We were doing the initial power query. Retry the operation.
                if (m_opStep == KwmCoreKwsOpStep.InitialPowerQuery) RetryOp(false);

                // We obtained the login ticket for creating the workspace.
                else HandleKasConnectAndGetTicketResult();
            }
        }

        /// <summary>
        /// Lookup the recipient addresses to determine if they are members. 
        /// </summary>
        private void LookupRecAddr()
        {
            Logging.Log(1, "LookupRecAddr()");
            if (!m_ih.LookupRecAddr(HandleLookupRecAddrResult, true)) PromptForPwd();
        }

        /// <summary>
        /// Handle the results of the lookup query.
        /// </summary>
        private void HandleLookupRecAddrResult(KmodQuery query)
        {
            Logging.Log(1, "HandleLookupRecAddrResult()");
            if (!CheckCtx(KwmCoreKwsOpStep.LookupRec)) return;
            m_ih.HandleLookupRecAddrResult(query);
            PromptForPwd();
        }

        /// <summary>
        /// Prompt the user for the passwords if required.
        /// </summary>
        private void PromptForPwd()
        {
            Logging.Log(1, "PromptForPwd()");
            if (m_ih.MustPromptForPwd())
            {
                m_opStep = KwmCoreKwsOpStep.UIStep;
                m_ih.Wiz.ShowPasswordPage();
            }

            else
            {
                m_uiStep = KwmCoreKwsUiStep.Pwd;
                CreateKwsObject();
            }
        }

        /// <summary>
        /// Create the workspace object.
        /// </summary>
        private void CreateKwsObject()
        {
            Logging.Log(1, "CreateKwsObject()");
            m_ih.Wiz.ShowPleaseWait("Creating the new " + Base.GetKwsString());
            m_sh.CreateKwsObject(m_parentNode);
            if (m_opStep == KwmCoreKwsOpStep.CreateKwsObject) KasConnectAndGetTicket(HandleLoginTicketQueryResult);
        }

        /// <summary>
        /// Process the invitations on the KAS.
        /// </summary>
        private void PerformKasInvite()
        {
            Logging.Log(1, "PerformKasInvite()");
            if (InviteParams.UserArray.Count == 0) CompleteSpawn();
            else m_ih.PerformKasInvite(HandleKasInviteResult);
        }

        /// <summary>
        /// Called when the invite to workspace command reply is received.
        /// </summary>
        private void HandleKasInviteResult(KasQuery query)
        {
            Logging.Log(1, "HandleKasInviteResult()");
            if (!CheckCtx(KwmCoreKwsOpStep.KasInvite)) return;
            if (m_ih.HandleKasInviteResult(query)) CompleteSpawn();
        }

        /// <summary>
        /// Complete the spawning of the workspace.
        /// </summary>
        private void CompleteSpawn()
        {
            Logging.Log(1, "CompleteSpawn()");

            // Perform the common portion.
            m_sh.CompleteSpawn(true, true);

            // Show the success page.
            m_doneFlag = true;
            OpRes = KwsCoreOpRes.Success;
            ShowWizardEnd();
        }

        /// <summary>
        /// Ask the wizard to display the result page. If the result is not
        /// success, the context is cleared.
        /// </summary>
        private void ShowWizardEnd()
        {
            Logging.Log(1, "ShowWizardEnd()");
            Debug.Assert(OpRes != KwsCoreOpRes.None);
            m_opStep = KwmCoreKwsOpStep.End;
            if (OpRes != KwsCoreOpRes.Success) CancelCtx(false);

            // FIXME Save the Teambox type so we can propose it again on the next creation.
            
            m_ih.Wiz.ShowResults();
        }
    }

    /// <summary>
    /// Base class for OutlookCreateKwsOp and OutlookPublicKwsOp.
    /// </summary>
    public abstract class OutlookNewKwsOp : BaseNewKwsCoreOp
    {
        /// <summary>
        /// Remember whether we actually tried to create a workspace. New 
        /// workspaces are removed on error.
        /// </summary>
        private bool m_createdKwsFlag = false;

        /// <summary>
        /// True if the workspace to create is a public workspace.
        /// </summary>
        private bool m_publicKwsFlag = false;

        public OutlookNewKwsOp(WorkspaceManager wm, WmOutlookRequest request, bool publicKwsFlag)
            : base(wm)
        {
            m_publicKwsFlag = publicKwsFlag;
            RegisterOutlookRequest(request);
        }

        public override void HandleMiscFailure(Exception ex)
        {
            if (m_doneFlag) return;

            // Clean up the operation.
            CleanUpOnDone(false);

            // Send a failure reply to Outlook.
            m_outlookRequest.SendFailure(ex.Message);
        }

        /// <summary>
        /// Clean up the operation when it has completed.
        /// </summary>
        protected virtual void CleanUpOnDone(bool successFlag)
        {
            m_doneFlag = true;

            // Clear the current KMOD query.
            ClearKmodQuery();

            // Unregister from the workspace and remove it, if required.
            if (!successFlag && m_createdKwsFlag) UnregisterAndRemoveKws();
            else UnregisterFromKws();
        }

        /// <summary>
        /// If the registry information indicates that we can create a
        /// workspace, this method returns true. Otherwise, it cancels the 
        /// operation.
        /// </summary>
        protected bool CheckCanCreateKws(WmWinRegistry registry)
        {
            if (!registry.CanCreateKws())
            {
                HandleMiscFailure(new Exception("not authorized to create a " + Base.GetKwsString()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create a workspace on the KAS.
        /// </summary>
        protected void CreateKws()
        {
            m_createdKwsFlag = true;
            WmWinRegistry registry = WmWinRegistry.Spawn();
            Creds.KasID = m_sh.GetKasID(registry);
            m_sh.FirstInBrowserFlag = m_publicKwsFlag;
            m_sh.SelectInBrowserFlag = true;
            m_sh.CreateKwsObject(Wm.UiBroker.Browser.PrimaryFolder);
            if (m_opStep == KwmCoreKwsOpStep.CreateKwsObject && CheckCanCreateKws(registry))
                KasConnectAndGetTicket(HandleLoginTicketQueryResult);
        }

        /// <summary>
        /// Handle the results of the login ticket query.
        /// </summary>
        private void HandleLoginTicketQueryResult(WmLoginTicketQuery query)
        {
            if (!CheckCtx(KwmCoreKwsOpStep.KasConnectAndGetTicket)) return;
            WmWinRegistry registry = WmWinRegistry.Spawn();
            m_sh.HandleLoginTicketQueryResult(query, registry);
            if (query.Res == WmLoginTicketQueryRes.MiscError) HandleMiscFailure(new Exception(query.OutDesc));
            else if (CheckCanCreateKws(registry)) HandleKasConnectAndGetTicketResult();
        }
    }

    /// <summary>
    /// Outlook create regular workspace operation.
    /// </summary>
    public class OutlookCreateKwsOp : OutlookNewKwsOp
    {
        public OutlookCreateKwsOp(WorkspaceManager wm, WmOutlookRequest request)
            : base(wm, request, false)
        {
        }

        public override void HandleLoggedIn()
        {
            PerformKasInvite();
        }

        /// <summary>
        /// Start the operation. 
        /// </summary>
        public void StartOp()
        {
            Debug.Assert(m_opStep == KwmCoreKwsOpStep.None);
            if (ParseOutlookRequest()) CreateKws();
        }

        /// <summary>
        /// Parse the request from Outlook. Return true on success.
        /// </summary>
        private bool ParseOutlookRequest()
        {
            try
            {
                AnpMsg cmd = m_outlookRequest.Cmd;
                Creds.KwsName = cmd.Elements[0].String;
                Creds.SecureFlag = (cmd.Elements[1].UInt32 > 0);
                m_ih.FillInviteParamsFromOutlookCmd(cmd, 2);
                return true;
            }

            catch (Exception ex)
            {
                HandleMiscFailure(ex);
                return false;
            }
        }

        /// <summary>
        /// Process the invitations on the KAS.
        /// </summary>
        private void PerformKasInvite()
        {
            if (InviteParams.UserArray.Count == 0) CompleteSpawn();
            else m_ih.PerformKasInvite(HandleKasInviteResult);
        }

        /// <summary>
        /// Called when the invite to workspace command reply is received.
        /// </summary>
        private void HandleKasInviteResult(KasQuery query)
        {
            if (!CheckCtx(KwmCoreKwsOpStep.KasInvite)) return;
            if (m_ih.HandleKasInviteResult(query)) CompleteSpawn();
        }

        /// <summary>
        /// Complete the spawning of the workspace.
        /// </summary>
        private void CompleteSpawn()
        {
            Logging.Log(1, "CompleteSpawn()");

            // Perform the common portion.
            m_sh.CompleteSpawn(false, true);
            
            // Send the reply to Outlook.
            AnpMsg res = m_outlookRequest.MakeReply(OAnpType.OANP_RES_CREATE_KWS);
            m_ih.FillOutlookResultWithInviteParms(res);
            m_outlookRequest.SendReply(res);

            // We're done.
            CleanUpOnDone(true);
        }
    }

    /// <summary>
    /// Operation to create a public workspace if possible and get a SKURL.
    /// </summary>
    public class OutlookSkurlKwsOp : OutlookNewKwsOp
    {
        /// <summary>
        /// List of absolute paths to the files to upload provided by Outlook.
        /// </summary>
        private List<String> m_outlookAttachList = new List<String>();

        /// <summary>
        /// Raw mail subject provided by Outlook.
        /// </summary>
        private String m_mailSubject;

        /// <summary>
        /// Relative path to a share directory where the mail
        /// attachments will be stored.
        /// </summary>
        private String m_shareAttachDirRelPath;

        /// <summary>
        /// SKURL returned by the KCD.
        /// </summary>
        private String m_skurl;

        /// <summary>
        /// Set to 1 if our public workspace supports attachment expiration.
        /// </summary>
        private UInt32 m_expirationSupportFlag = 0;

        /// <summary>
        /// Email ID returned by the KCD.
        /// </summary>
        private ulong m_emailID;
        
        public OutlookSkurlKwsOp(WorkspaceManager wm, WmOutlookRequest request)
            : base(wm, request, true)
        {
        }

        public override void HandleLoggedIn()
        {
            // Complete the spawn.
            m_sh.CompleteSpawn(false, true);

            // Post the SKURL query if no error occurred.
            if (!m_doneFlag) PostSkurlQuery();
        }

        protected override void CleanUpOnDone(bool successFlag)
        {
            KfsShare share = GetKfsShare();
            if (share != null) share.OnKfsIdle -= OnKfsIdle;
            base.CleanUpOnDone(successFlag);
        }

        /// <summary>
        /// Return a reference to the KFS share associated to the workspace,
        /// if any.
        /// </summary>
        private KfsShare GetKfsShare()
        {
            if (m_kws == null) return null;
            return ((AppKfs)m_kws.AppTree[KAnpType.KANP_NS_KFS]).Share;
        }

        /// <summary>
        ///  Sanitize the subject name. The output MUST be a valid Windows
        ///  file name.
        /// </summary>
        private String SanitizeSubject(string originalSubject)
        {
            int[] allowedChars = new int[256] {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
            };

            char[] subjectChars = m_mailSubject.ToCharArray();
            String saneSubject = "";

            for (int i = 0; i < subjectChars.Length; i++)
            {
                // Replace invalid chars by '_'. Eliminate the trailing dot.
                char theChar = subjectChars[i];
                if (allowedChars[theChar] == 0)
                {
                    saneSubject += "_";
                }
                else if (theChar == '.' && i == subjectChars.Length - 1)
                {
                    // Ignore the trailing slash.
                }
                else
                {
                    saneSubject += theChar;
                }
            }

            if (saneSubject == "") return "No subject";
            else return saneSubject.Trim();
        }

        /// <summary>
        /// Start the operation. 
        /// </summary>
        public void StartOp()
        {
            Debug.Assert(m_opStep == KwmCoreKwsOpStep.None);

            // Get the public workspace, if any.
            m_kws = Wm.GetPublicKws();

            // There is no public workspace.
            if (m_kws == null)
            {
                // We cannot create a public workspace.
                if (!WmWinRegistry.Spawn().CanCreateKws())
                    HandleNoSkurl("not authorized to use Attachment Management");

                // Create a public workspace.
                else
                {
                    Creds.KwsName = "Attachment Management";
                    Creds.PublicFlag = true;
                    CreateKws();
                }
            }

            // There is a public workspace.
            else
            {
                RegisterToKws(false);

                // Use the existing public workspace credentials for the operation.
                Creds = m_kws.CoreData.Credentials;

                // The public workspace is not working online.
                if (m_kws.Sm.GetRunLevel() != KwsRunLevel.Online)
                {
                    HandleNoSkurl("Attachment Management is not available (disconnected from server)");
                    
                    // Ask the state machine to work online.
                    m_kws.Sm.RequestTaskSwitch(KwsTask.WorkOnline);
                }

                // Send the SKURL request.
                else PostSkurlQuery();
            }
        }

        /// <summary>
        /// This method is called when no SKURL may be obtained because some
        /// conditions are not met.
        /// </summary>
        private void HandleNoSkurl(String reason)
        {
            HandleMiscFailure(new Exception(reason));
        }

        /// <summary>
        /// Post a query to obtain a SKURL.
        /// </summary>
        private void PostSkurlQuery()
        {
            m_opStep = KwmCoreKwsOpStep.KasSkurl;

            try
            {
                AnpMsg outlookCmd = m_outlookRequest.Cmd;
                AnpMsg kasCmd = m_kws.NewKAnpCmd(KAnpType.KANP_CMD_KWS_GET_UURL);

                int i = 0;
                kasCmd.AddString(Creds.UserName);
                kasCmd.AddString(Creds.UserEmailAddress);
                m_mailSubject = outlookCmd.Elements[0].String;

                // Fallback to something sensible if there is no subject.
                m_mailSubject = SanitizeSubject(m_mailSubject);
                
                int nbRecipient = (int)outlookCmd.Elements[1].UInt32;
                int nbAttach = (int)outlookCmd.Elements[1 + nbRecipient * 2 + 1].UInt32;
                
                kasCmd.AddString(m_mailSubject);
                kasCmd.AddUInt32((UInt32)nbAttach);
                kasCmd.AddUInt32((UInt32)nbRecipient);
                kasCmd.AddUInt32(2);

                i = 2;
                for (int j = 0; j < nbRecipient; j++)
                {
                    kasCmd.AddString(outlookCmd.Elements[i++].String);
                    kasCmd.AddString(outlookCmd.Elements[i++].String);
                }

                i += 1; // Skip the number of attachments.
                for (int k = 0; k < nbAttach; k++) m_outlookAttachList.Add(outlookCmd.Elements[i++].String);

                UInt64 expirationDelay = outlookCmd.Elements[i++].UInt64;
                if (m_kws.Kas.MinorVersion >= 6)
                {
                    m_expirationSupportFlag = 1;
                    kasCmd.AddUInt64(expirationDelay);
                }

                m_kws.PostNonAppKasQuery(kasCmd, null, HandleSkurlQueryResult, true);
            }

            catch (Exception ex)
            {
                HandleMiscFailure(ex);
            }
        }

        /// <summary>
        /// Handle the result of the SKURL query.
        /// </summary>
        private void HandleSkurlQueryResult(KasQuery query)
        {
            if (!CheckCtx(KwmCoreKwsOpStep.KasSkurl)) return;

            try
            {
                AnpMsg res = query.Res;
                AppKfs appKfs = (AppKfs)m_kws.AppTree[KAnpType.KANP_NS_KFS];

                // Handle failures.
                if (res.Type != KAnpType.KANP_RES_KWS_UURL)
                    throw Misc.HandleUnexpectedKAnpReply("get SKURL", res);

                // Get the SKURL.
                m_skurl = res.Elements[0].String;
                
                // Get the date.
                DateTime attachDate = Base.KDateToDateTimeUTC(res.Elements[1].UInt64);

                // Get the email ID.
                if (m_kws.Kas.MinorVersion >= 6) { m_emailID = res.Elements[2].UInt64; }
                else { m_emailID = 0; }

                // Handle file upload attachments.
                if (m_outlookAttachList.Count > 0)
                {
                    // Set the share attachment directory path.
                    m_shareAttachDirRelPath = m_kws.CoreData.Credentials.UserEmailAddress + "/" +
                                              attachDate.ToString("yyyy-MM-dd HH\\hmm\\mss") + " " +
                                              m_mailSubject + "/" +
                                              "Original attachments";

                    // Wait for the share to become idle.
                    m_opStep = KwmCoreKwsOpStep.KfsIdle;
                    KfsShare share = GetKfsShare();
                    if (share.IsKfsIdle()) OnKfsIdle(null, null);
                    else share.OnKfsIdle += OnKfsIdle;
                }

                // We're done.
                else
                {
                    HandleSuccess();
                }
            }

            catch (Exception ex)
            {
                HandleMiscFailure(ex);
            }
        }

        /// <summary>
        /// Called when the KFS share becomes idle.
        /// </summary>
        private void OnKfsIdle(Object sender, EventArgs args)
        {
            if (!CheckCtx(KwmCoreKwsOpStep.KfsIdle)) return;

            // Wait for the share to be idle, if required.
            KfsShare share = GetKfsShare();
            if (!share.IsKfsIdle()) return;

            // Make sure we do not receive any more notifications.
            share.OnKfsIdle -= OnKfsIdle;

            // Enter the KFS gate.
            share.Gate.GateEntry("SKURL file upload");

            try
            {
                // Get the list of relative paths to the attachment files in 
                // the share.
                List<String> shareAttachList = new List<String>();
                foreach (String srcPath in m_outlookAttachList)
                    shareAttachList.Add(m_shareAttachDirRelPath + "/" + Path.GetFileName(srcPath));

                // Get the absolute path to the share attachment directory.
                String shareAttachDirAbsPath = share.MakeAbsolute(m_shareAttachDirRelPath);

                // Create the share attachment directory.
                Directory.CreateDirectory(shareAttachDirAbsPath);

                // Move the Outlook attachments inside the share.
                for (int i = 0; i < m_outlookAttachList.Count; i++)
                    File.Move(m_outlookAttachList[i], share.MakeAbsolute(shareAttachList[i]));

                // Scan the files to upload and synchronize with the share.
                KfsScan scan = new KfsScan(share, m_shareAttachDirRelPath);
                scan.Scan();
                scan.Sync();

                // The scan should have found the files to upload and requested
                // the update of their status. Update the local status and the
                // status view as required.
                share.UpdateLocalStatus();
                share.RequestStatusViewUpdate("SKURL file scan");
                share.UpdateStatusViewIfNeeded();

                // Make sure all the files are Not Added.
                foreach (String filePath in shareAttachList)
                {
                    KfsStatusPath statusPath = share.StatusView.GetPath(filePath);
                    if (statusPath == null || statusPath.Status != PathStatus.NotAdded)
                        throw new Exception("file " + filePath + " is not ready for upload");
                }

                // Flag the files for upload.
                share.Gate.AddInternalFiles(shareAttachList, m_emailID);

                // Notify the user.
                Wm.UiBroker.NotifyUser(new AttachManagementNotificationItem(m_kws, m_outlookRequest.Cmd));

                // Try to remove the directory where Outlook put its 
                // attachments. This assumes that all files provided by 
                // Outlook come from the same directory, which is a more than 
                // reasonable assumption considering that it's programmed to do
                // that.
                try
                {
                    Directory.Delete(Path.GetDirectoryName(m_outlookAttachList[0]));
                }

                catch (Exception) { }

                // We're done.
                HandleSuccess();
            }

            catch (Exception ex)
            {
                HandleMiscFailure(ex);
            }

            finally
            {
                // Exit the KFS gate.
                share.Gate.GateExit("SKURL file upload");
            }
        }

        /// <summary>
        /// Called when the SKURL request has been completed.
        /// </summary>
        private void HandleSuccess()
        {
            // Send the reply.
            AnpMsg rep = m_outlookRequest.MakeReply(OAnpType.OANP_RES_GET_SKURL);
            rep.AddString(m_skurl);
            rep.AddUInt32(m_expirationSupportFlag);
            m_outlookRequest.SendReply(rep);

            // We're done.
            CleanUpOnDone(true);
        }
    }

    /// <summary>
    /// Base class for KwmImportKwsOp and OutlookJoinKwsOp.
    /// </summary>
    public abstract class BaseImportKwsOp : KwsCoreOp
    {
        /// <summary>
        /// Path of the destination folder.
        /// </summary>
        protected String m_folderPath;

        /// <summary>
        /// True if this is a join operation. In a join operation, we prompt
        /// for passwords and remove the workspace on error.
        /// </summary>
        protected bool m_joinFlag;

        /// <summary>
        /// Reference to the spawn helper.
        /// </summary>
        protected KwsSpawnOpHelper m_sh;

        public BaseImportKwsOp(WorkspaceManager wm, KwsCredentials creds, String folderPath, bool joinFlag)
            : base(wm)
        {
            m_folderPath = folderPath;
            m_joinFlag = joinFlag;
            m_sh = new KwsSpawnOpHelper(this, creds);
        }

        public override void HandleMiscFailure(Exception ex)
        {
            if (m_doneFlag) return;

            // We're done.
            m_doneFlag = true;
            m_opStep = KwmCoreKwsOpStep.End;

            // If this is a KWM join, show the error to the user, unless he already
            // knows about it.
            if (m_joinFlag &&
                m_outlookRequest == null &&
                (m_kws == null || m_kws.KasLoginHandler.LoginResult != KwsLoginResult.PwdRequired))
            {
                Wm.Sm.PostGuiExecRequest(new WmMajorErrorGer("Cannot join " + Base.GetKwsString(), ex), null);
            }

            // If this is an Outlook join, report the error to Outlook.
            else if (m_outlookRequest != null)
            {
                m_outlookRequest.SendFailure(ex.Message);
            }

            // Unregister from the workspace and remove it.
            UnregisterAndRemoveKws();
        }

        public override void HandleKwsLoginSuccess()
        {
            Logging.Log(1, "HandleKwsLoginSuccess()");

            // We're done.
            if (m_opStep == KwmCoreKwsOpStep.KasLogin) CompleteSpawn();
        }

        /// <summary>
        /// Create the workspace object.
        /// </summary>
        protected void CreateKwsObject()
        {
            Logging.Log(1, "CreateKwsObject()");
            m_sh.SelectInBrowserFlag = m_joinFlag;
            m_sh.CreateKwsObject(Wm.UiBroker.Browser.CreateFolderFromPath(m_folderPath));
            if (m_opStep == KwmCoreKwsOpStep.CreateKwsObject) HandlePostCreate();
        }

        /// <summary>
        /// Called when the workspace object has been created.
        /// </summary>
        protected void HandlePostCreate()
        {
            // We are joining the workspace. Wait for login.
            if (m_joinFlag)
            {
                m_opStep = KwmCoreKwsOpStep.KasLogin;
                m_kws.Sm.SetLoginType(KwsLoginType.All);
                m_kws.Sm.SetSpawnStep(KwsSpawnTaskStep.Login);
            }

            // We are not joining the workspace. We're done.
            else
            {
                m_kws.Sm.SetLoginType(KwsLoginType.NoPwdPrompt);
                CompleteSpawn();
            }
        }

        /// <summary>
        /// Complete the spawning of the workspace.
        /// </summary>
        protected virtual void CompleteSpawn()
        {
            Logging.Log(1, "CompleteSpawn()");

            // Perform the common portion.
            m_sh.CompleteSpawn(true, m_joinFlag);

            // We're done.
            m_doneFlag = true;
        }
    }

    /// <summary>
    /// Import workspace operation of the KWM.
    /// </summary>
    public class KwmImportKwsOp : BaseImportKwsOp
    {
        public KwmImportKwsOp(WorkspaceManager wm, KwsCredentials creds, String folderPath, bool joinFlag)
            : base(wm, creds, folderPath, joinFlag)
        {
        }

        /// <summary>
        /// Start the operation.
        /// </summary>
        public void StartOp()
        {
            Logging.Log(1, "StartOp()");
            Debug.Assert(m_opStep == KwmCoreKwsOpStep.None);
            CreateKwsObject();
        }
    }

    /// <summary>
    /// Outlook join workspace operation.
    /// </summary>
    public class OutlookJoinKwsOp : BaseImportKwsOp
    {
        public OutlookJoinKwsOp(WorkspaceManager wm, WmOutlookRequest request)
            : base(wm, new KwsCredentials(), wm.UiBroker.Browser.PrimaryFolder.FullPath, true)
        {
            RegisterOutlookRequest(request);
        }

        protected override void CompleteSpawn()
        {
            base.CompleteSpawn();
            SendSuccessReply(m_kws);
        }

        /// <summary>
        /// Start the operation.
        /// </summary>
        public void StartOp()
        {
            Logging.Log(1, "StartOp()");
            Debug.Assert(m_opStep == KwmCoreKwsOpStep.None);
            if (GetCredentials()) DoJoin();
        }

        /// <summary>
        /// Obtain the workspace credentials. Return true on success. 
        /// </summary>
        private bool GetCredentials()
        {
            String tmpPath = "";

            try
            {
                String content = m_outlookRequest.Cmd.Elements[0].String;
                tmpPath = Path.GetTempFileName();
                File.AppendAllText(tmpPath, content);
                WmImportData importData = WorkspaceManager.GetImportDataFromFile(tmpPath);
                if (importData.KwsList.Count != 1) throw new Exception("the credentials file does not contain a single workspace");
                m_sh.Creds = importData.KwsList[0].Creds;
                return true;
            }

            catch (Exception ex)
            {
                HandleMiscFailure(ex);
                return false;
            }

            finally
            {
                try
                {
                    if (tmpPath != "") File.Delete(tmpPath);
                }

                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Join the workspace using the credentials obtained.
        /// </summary>
        private void DoJoin()
        {
            Workspace kws = Wm.GetKwsByExternalID(m_sh.Creds.KasID, m_sh.Creds.ExternalID);
            if (kws != null) ImportExistingKws(kws);
            else ImportNewKws();
        }

        /// <summary>
        /// "Import" a workspace that already exists in the KWM.
        /// </summary>
        private void ImportExistingKws(Workspace kws)
        {
            // Note: don't set m_kws here. m_kws is only used if the workspace
            // is actually being created.

            // Import as much as possible.
            Wm.ImportExistingKws(kws, m_sh.Creds, true);

            // Send a reply to Outlook.
            SendSuccessReply(kws);
        }

        /// <summary>
        /// Join a workspace that does not exist in the KWM.
        /// </summary>
        private void ImportNewKws()
        {
            CreateKwsObject();
        }

        /// <summary>
        /// Send back a successful reply to Outlook.
        /// </summary>
        private void SendSuccessReply(Workspace kws)
        {
            AnpMsg res = m_outlookRequest.MakeReply(OAnpType.OANP_RES_JOIN_KWS);
            res.AddUInt64(kws.InternalID);
            m_outlookRequest.SendReply(res);
            m_doneFlag = true;
        }
    }
}