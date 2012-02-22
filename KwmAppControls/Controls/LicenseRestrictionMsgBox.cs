using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tbx.Utils;

namespace kwm.Utils
{
    public partial class LicenseRestrictionMsgBox : Form
    {
        public LicenseRestrictionMsgBox()
        {
            InitializeComponent();
        }

        public LicenseRestrictionMsgBox(String msg, bool showUpgradeAccount) : this()
        {
            lblMsg.Text = msg;
            lblUpgradeAccnt.Visible = showUpgradeAccount;
        }

        private void lblUpgradeAccnt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
