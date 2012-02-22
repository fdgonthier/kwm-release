using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using kwm.Utils;
using System.Diagnostics;
using Tbx.Utils;

namespace kwm
{
    public partial class PageNoSecure : Wizard.UI.InternalWizardPage
    {
        private frmCreateKwsWizard m_wiz
        {
            get { return (frmCreateKwsWizard)GetWizard(); }
        }

        public PageNoSecure()
        {
            InitializeComponent();
        }

        private void PageFailure_SetActive(object sender, CancelEventArgs e)
        {
            try
            {
                SetWizardButtons(Wizard.UI.WizardButtons.Cancel);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.Assert(m_wiz.CreateOp != null);

                // Retry with the updated SecureFlag.
                m_wiz.CreateOp.Creds.SecureFlag = !chkSwitchToStd.Checked;
                m_wiz.CreateOp.RetryOp(true);
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
    }
}

