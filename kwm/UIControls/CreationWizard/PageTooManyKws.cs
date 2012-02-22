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
    public partial class PageTooManyKws : Wizard.UI.InternalWizardPage
    {
        private frmCreateKwsWizard m_wiz
        {
            get { return (frmCreateKwsWizard)GetWizard(); }
        }

        public PageTooManyKws()
        {
            InitializeComponent();
        }

        private void PageTooManyKws_SetActive(object sender, CancelEventArgs e)
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

        private void lnkUpgrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

