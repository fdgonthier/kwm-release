using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using kwm.Utils;
using kwm.KwmAppControls;
using Tbx.Utils;

namespace kwm
{
    public partial class ucKwsHeaderToolbar : UserControl
    {
        /// <summary>
        /// Reference to the UI broker.
        /// </summary>
        public WmUiBroker UiBroker;

        /// <summary>
        /// Last task associated to the workspace task button in the home tab.
        /// </summary>
        private KwsTask m_kwsTaskBtnTask = KwsTask.WorkOnline;

        /// <summary>
        /// True if the browser tree does not match tvWorkspaces.
        /// </summary>
        public bool StaleBrowser { get { return UiBroker.RebuildBrowserFlag; } }

        /// <summary>
        /// True if the KAS error field is non-empty.
        /// </summary>
        public bool HasError { get { return lblKasError.Text != ""; } }

        public ucKwsHeaderToolbar()
        {
            InitializeComponent();            
        }

        public void UpdateUI(Workspace kws)
        {
            if (kws == null) return;
            // Determine the status to display and the task the user would
            // most likely want to undertake.
            KwsTask curTask = kws.Sm.GetCurrentTask();
            String statusText = "";
            bool btnEnabled = false;
            KwsTask btnTask = KwsTask.WorkOnline;
            String btnText = "Connect";

            if (curTask == KwsTask.Spawn)
            {
                statusText = "Creating " + Base.GetKwsString();
            }

            else if (curTask == KwsTask.Remove)
            {
                statusText = "Deleting " + Base.GetKwsString();
            }

            else if (curTask == KwsTask.Rebuild)
            {
                statusText = "Rebuilding " + Base.GetKwsString();
            }

            else if (curTask == KwsTask.Stop)
            {
                if (kws.MainStatus == KwsMainStatus.RebuildRequired)
                {
                    statusText = "Rebuild required";
                    btnEnabled = true;
                    btnTask = KwsTask.Rebuild;
                    btnText = "Rebuild " + Base.GetKwsString();
                }

                // Assume the workspace was disabled voluntarily and that
                // the user can work online. This is normally the case.
                else
                {
                    statusText = Base.GetKwsString() + " disabled";
                    btnEnabled = true;
                }
            }

            else if (curTask == KwsTask.WorkOffline)
            {
                statusText = "Disconnected";
                btnEnabled = true;
            }

            else if (curTask == KwsTask.WorkOnline)
            {
                if (kws.IsOnlineCapable())
                {
                    statusText = "Connected";
                    btnEnabled = true;
                    btnTask = KwsTask.WorkOffline;
                    btnText = "Disconnect";
                }

                // We're not currently connecting but we can request to.
                else if (kws.Sm.CanWorkOnline())
                {
                    statusText = "Disconnected";
                    btnEnabled = true;
                }

                // We're connecting, allow disconnection.
                else
                {
                    statusText = "Connecting";
                    btnEnabled = true;
                    btnTask = KwsTask.WorkOffline;
                    btnText = "Cancel";
                }
            }
            else if (curTask == KwsTask.DeleteOnServer)
            {
                statusText = "Deleting from server...";
            }

            // Update the information.
            KwsBrowserKwsNode node = UiBroker.Browser.GetKwsNodeByKws(kws);
            lblStatus.Text = statusText;
            lblStatus.ForeColor = node.GetKwsIconImageKey();
            imgKwsStatus.Image = KwmMisc.GetKwmStateImageFromKey(lblStatus.ForeColor);
            btnKwsTask.Enabled = btnEnabled;
            btnKwsTask.Text = btnText;
            m_kwsTaskBtnTask = btnTask;

            picSecure.Visible = kws.CoreData.Credentials.SecureFlag;

            UpdateKwsNameLocation();
            lblKwsName.Text = kws.GetKwsName();

            KwsUser creator = kws.CoreData.UserInfo.Creator;
            if (creator != null)
            {
                DateTime creationDate = Base.KDateToDateTime(creator.InvitationDate);
                lblCreator.Text = creator.EmailAddress + " (" + creationDate.ToString("ddd, dd MMM yyyy") + ")";
            }

            else
            {
                lblCreator.Text = "unkown";
            }

            lblServer.Text = kws.Kas.KasID.Host + " (ID: " + kws.GetExternalKwsID() + ")";

            SetKasErrorField(kws);
        }

        /// <summary>
        /// Moves the workspace name appropriatly.
        /// </summary>
        private void UpdateKwsNameLocation()
        {
            if (picSecure.Visible)
                lblKwsName.Location = new Point(picSecure.Location.X + picSecure.Size.Width + 5, lblKwsName.Location.Y);
            else
                lblKwsName.Location = new Point(picSecure.Location.X, lblKwsName.Location.Y);
        }

        private void SetKasErrorField(Workspace kws)
        {
            if (kws.AppException != null)
            {
                lblKasError.Text = Base.FormatErrorMsg("application error", kws.AppException);
            }

            else if (kws.Kas.ErrorEx != null)
            {
                lblKasError.Text = Base.FormatErrorMsg("KAS error", kws.Kas.ErrorEx);
            }

            else if ((kws.Sm.GetCurrentTask() != KwsTask.WorkOnline ||
                     (kws.Kas.ConnStatus != KasConnStatus.Connected &&
                      kws.Kas.ConnStatus != KasConnStatus.Connecting)) &&
                     (kws.KasLoginHandler.LoginResult != KwsLoginResult.Accepted &&
                      kws.KasLoginHandler.LoginResult != KwsLoginResult.None))
            {
                lblKasError.Text = Base.FormatErrorMsg("login error", kws.KasLoginHandler.LoginResultString);

                if (kws.KasLoginHandler.LoginResult == KwsLoginResult.BadSecurityCreds &&
                    kws.KasLoginHandler.TicketRefusalString != "")
                {
                    lblKasError.Text += Environment.NewLine + Base.FormatErrorMsg(kws.KasLoginHandler.TicketRefusalString);
                }
            }

            else
            {
                lblKasError.Text = "";
            }
        }

        private void btnKwsTask_Click(object sender, EventArgs e)
        {
            try
            {
                if (StaleBrowser) return;

                Workspace kws = UiBroker.Browser.SelectedKws;
                if (kws == null) return;

                if (m_kwsTaskBtnTask == KwsTask.WorkOnline) UiBroker.RequestWorkOnline(kws);
                else if (m_kwsTaskBtnTask == KwsTask.WorkOffline) UiBroker.RequestWorkOffline(kws);
                else if (m_kwsTaskBtnTask == KwsTask.Rebuild) UiBroker.RequestRebuildKws(kws);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }
    }
}
