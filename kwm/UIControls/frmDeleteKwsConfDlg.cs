using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Tbx.Utils;

namespace kwm
{
    public partial class frmDeleteKwsConfDlg : frmKBaseForm
    {
        private WmUiBroker m_uiBroker;

        public bool DeleteOnServerFlag
        {
            get { return chkDeleteFromServer.Checked; }
        }

        public frmDeleteKwsConfDlg()
        {
            InitializeComponent();
        }

        public frmDeleteKwsConfDlg(WmUiBroker broker) : this()
        {
            m_uiBroker = broker;
            InitUI();
        }

        private void InitUI()
        {
            Workspace kws = m_uiBroker.Browser.SelectedKws;
            Debug.Assert(kws != null);

            String reason = "";
            if (m_uiBroker.CanPerformKwsAction(KwsAction.DeleteFromServer, kws, ref reason))
            {
                lblQuestion.Text = "You are about to permanently remove this " + Base.GetKwsString() + 
                    " from the server. Are you sure you want to continue?" + Environment.NewLine + Environment.NewLine +
                    "If you want to remove this " + Base.GetKwsString() + " from your " + Base.GetKwmString() + ", uncheck the " +
                    " checkbow below.";
                chkDeleteFromServer.Checked = true;
                chkDeleteFromServer.Enabled = true;
            }
            else
            {
                lblQuestion.Text = "Only administrators can delete a " + Base.GetKwsString() + " from the server." + 
                    Environment.NewLine + Environment.NewLine +
                    "Do you want to remove it from your " + Base.GetKwmString() + 
                    " instead? Note that other members will still have access to the " 
                    + Base.GetKwsString() + ".";
                chkDeleteFromServer.Checked = false;
                chkDeleteFromServer.Enabled = false;
            }
        }
    }
}
