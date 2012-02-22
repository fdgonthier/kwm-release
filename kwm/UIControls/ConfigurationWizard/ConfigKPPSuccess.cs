using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Wizard.UI;
using kwm.Utils;
using System.Collections;
using Tbx.Utils;
using System.IO;

namespace kwm
{
    public partial class ConfigKPPSuccess : InternalWizardPage
    {
        public ConfigKPPSuccess()
        {
            InitializeComponent();
        }

        private void ConfigKPPSuccess_SetActive(object sender, CancelEventArgs e)
        {
            try
            {
                SetWizardButtons(WizardButtons.Finish);
                chkShowQuickstart.Checked = File.Exists(Misc.KwmQuickStartPath);
                chkShowQuickstart.Enabled = File.Exists(Misc.KwmQuickStartPath);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void ConfigKPPSuccess_WizardFinish(object sender, CancelEventArgs e)
        {
            try
            {
                ((ConfigKPPWizard)GetWizard()).SaveCreds();

                if (chkShowQuickstart.Checked)
                    Misc.OpenFileInWorkerThread(Misc.KwmQuickStartPath);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Base.HandleException(ex);
            }
        }
    }
}
