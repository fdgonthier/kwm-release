using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using kwm.Utils;
using System.Diagnostics;
using Tbx.Utils;

namespace kwm
{
    public partial class Credentials : UserControl
    {
        private const String DefaultKpsAddress = "tbsos01.teambox.co";
        public String Token = null;

        /// <summary>
        /// Event fired whenever the content of a field in this 
        /// user control changed. Used to enable or disable the 
        /// 'Next' button.
        /// </summary>
        [Description("One of the configuration fields changed.")]
        public event EventHandler<EventArgs> OnCredFieldChange;

        public Credentials()
        {
            InitializeComponent();
            txtKpsAddress.Text = DefaultKpsAddress;
        }

        /// <summary>
        /// Fill the control with the information present in the registry. Called
        /// from a constructor.
        /// </summary>
        public void PopulateFromRegistry()
        {
            WmWinRegistry reg = WmWinRegistry.Spawn();

            UserName = reg.KpsUserName;

            // If KpsAddr is empty, KPSAddress is automatically set to 
            // a default value.
            KpsAddress = reg.KpsAddr;
        }

        private void chkUseDefaultServer_CheckedChanged(object sender, EventArgs e)
        {
            txtKpsAddress.Enabled = !chkUseDefaultServer.Checked;

            if (chkUseDefaultServer.Checked)
            {
                // Overwrite what was in the textbox with the default KPS.
                txtKpsAddress.Text = DefaultKpsAddress;
                txtUsername.Select();
            }
            else
            {
                txtKpsAddress.Select();
            }
        }

        /// <summary>
        /// Get or set the user name.
        /// </summary>
        public String UserName
        {
            get { return txtUsername.Text.Trim(); }
            set { txtUsername.Text = value; }
        }

        /// <summary>
        /// Get or set the password.
        /// </summary>
        public String Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        /// <summary>
        /// Get or set the KPS address. When setting the address, if an empty
        /// string is given, the default KPS address is set.
        /// </summary>
        public String KpsAddress
        {
            get
            {
                if (chkUseDefaultServer.Checked)
                    return DefaultKpsAddress;
                else
                    return txtKpsAddress.Text;
            }
            set
            {
                if (value == "")
                    value = DefaultKpsAddress;

                chkUseDefaultServer.Checked = (value == DefaultKpsAddress);
                txtKpsAddress.Text = value;
            }
        }

        public Boolean SigninMode
        {
            get
            {
                return (chkUseDefaultServer.Visible && txtKpsAddress.Visible);
            }
            set
            {
                chkUseDefaultServer.Visible = value;
                txtKpsAddress.Visible = value;
                lblServer.Visible = value;
                if (value)
                {
                    lblUserName.Text = "Login:";
                    lblPassword.Text = "Password:";
                }
                else
                {
                    lblUserName.Text = "Your email:";
                    lblPassword.Text = "New password:";
                }
            }
        }
        public void ResetError()
        {
            KPSAdressLBL.ForeColor = Color.Black;
            lblPassword.ForeColor = Color.Black;
            lblUserName.ForeColor = Color.Black;
            ErrorMsgTip.SetToolTip(KPSAdressLBL, "");
            ErrorMsgTip.SetToolTip(lblPassword, "");
            ErrorMsgTip.SetToolTip(lblUserName, "");
        }

        public void SetServerError(String errorStr)
        {
            KPSAdressLBL.ForeColor = Color.Red;
            ErrorMsgTip.SetToolTip(KPSAdressLBL, errorStr);
        }

        public void SetCredError(String errorStr)
        {
            lblPassword.ForeColor = Color.Red;
            lblUserName.ForeColor = Color.Red;
            ErrorMsgTip.SetToolTip(lblPassword, errorStr);
            ErrorMsgTip.SetToolTip(lblUserName, errorStr);
        }

        /// <summary>
        /// Fill a login command with the required parameters and return it.
        /// </summary>
        public K3p.K3pLoginTest GetLoginCommand()
        {
            K3p.K3pLoginTest cmd = new K3p.K3pLoginTest();
            cmd.Info.kps_login = UserName;
            cmd.Info.kps_secret = Password;
            cmd.Info.secret_is_pwd = 1;
            cmd.Info.kps_net_addr = KpsAddress;
            cmd.Info.kps_port_num = 443;
            return cmd;
	}

        /// <summary>
        /// Save the KPS address, login and ticket to the registry if 
        /// accountFlag is set, clears the fields otherwise.
        /// </summary>
        public void Save(bool accountFlag)
        {
            WmWinRegistry reg = WmWinRegistry.Spawn();

            if (accountFlag)
            {
                reg.KpsAddr = KpsAddress;
                reg.KpsUserName = UserName;
                reg.KpsLoginToken = Token;
            }

            else
            {
                reg.Clear();
            }

            // We sucessfully ran the wizard. Never prompt it again 
            // automatically.
            reg.NoAutomaticWizardFlag = true;
            
            // Effectively save the data to the registry.
            reg.WriteRegistry();
        }

        private void DoOnCredFieldChange()
        {
            if (OnCredFieldChange != null)
                OnCredFieldChange(this, EventArgs.Empty);
        }

        private void txtKpsAddress_TextChanged(object sender, EventArgs e)
        {
            DoOnCredFieldChange();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            DoOnCredFieldChange();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            DoOnCredFieldChange();
        }

        /// <summary>
        /// Common handler for the three textboxes. 
        /// </summary>
        private void TextBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        public void AdjustFocus()
        {
            if (txtUsername.Text == "") txtUsername.Select();
            else txtPassword.Select();
        }
    }
}

