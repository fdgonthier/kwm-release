using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tbx.Utils;
using System.Diagnostics;
using kwm.Utils;
using kwm.KwmAppControls;

namespace kwm
{
    /// <summary>
    /// Form allowing a user to view and change the selected workspace's
    /// properties.
    /// Note: the Tag property of the various fields always contains 
    /// a String. When the control is disabled, the string is a user
    /// friendly message telling why the control is disabled. When the
    /// control is enabled, the string is empty.
    /// </summary>
    public partial class frmKwsProperties : frmKBaseForm
    {
        private String HelpChangeKwsNameString
        {
            get
            {
                return "Administrators can change the name of a " + Base.GetKwsString() + ".";
            }
        }

        private String HelpAuthByString
        {
            get
            {
                return "Shows which organization authenticated the " + 
                       Base.GetKwsString() + " creator.";
            }
        }

        private String HelpKwsTypeString
        {
            get
            {
                return "There exists two different " + Base.GetKwsString() + " types." +
                         Environment.NewLine +
                         "Standard: " + Base.GetStdKwsDescription() +
                         Environment.NewLine +
                         "Secure: " + Base.GetSecureKwsDescription();
            }
        }

        private String HelpFreezeString
        {
            get
            {
                return "A moderated " + Base.GetKwsString() +
                       " restricts the right to modify the " + Base.GetKwsString() +
                       " to Administrators only. For example, no files can be added" +
                       " or removed, no screen sharing session can be started, etc.";
            }
        }

        private String HelpDeepFreezeString
        {
            get
            {
                return "A locked " + Base.GetKwsString() + " prevents that any user," +
                       "even Administrators, do any modifications to the " + 
                       Base.GetKwsString() + ". Only the Server Administrator " +
                       " can change this setting.";
            }
        }

        private String HelpThinKfsString
        {
            get
            {
                return "This option allows you to preserve a complete history of any file that ever transited by the " + Base.GetKwsString() +
                        ". It will allow you to retrieve any version of any file, even deleted files, when the History feature is available in the " 
                        + Base.GetKwmString() + Environment.NewLine + Environment.NewLine + 
                        "Remember that the history counts in the storage quota.";
            }
        }

        /// <summary>
        /// Reference to the UI broker.
        /// </summary>
        private WmUiBroker m_uiBroker;

        /// <summary>
        /// Target workspace name when the dialog is displayed. Used to know if
        /// it was changed.
        /// </summary>
        public String InitialKwsName;

        /// <summary>
        /// Target workspace type when the dialog is displayed. Used to know if
        /// it was changed.
        /// </summary>
        public int InitialKwsType;

        /// <summary>
        /// Target workspace freeze flag when the dialog is displayed. Used to 
        /// know if it was changed.
        /// </summary>
        public bool InitialFreezeFlag;

        /// <summary>
        /// Target workspace deep freeze flag when the dialog is displayed. 
        /// Used to know if it was changed.
        /// </summary>
        public bool InitialDeepFreezeFlag;

        /// <summary>
        /// Target workspace thin KFS flag when the dialog is displayed. Used 
        /// to know if it was changed.
        /// </summary>
        public bool InitialThinKfsFlag;

        /// <summary>
        /// Return the trimmed content of the workspace name textbox.
        /// </summary>
        public String FinalKwsName
        {
            get { return txtKwsName.Text.Trim(); }
        }

        /// <summary>
        /// Return the state of the Teambox type radio buttons.
        /// 1 => Standard
        /// 2 => Secure
        /// </summary>
        public int FinalKwsType
        {
            get 
            {
                if (rdoStandard.Checked) return 1;
                else return 2;
            }
        }

        /// <summary>
        /// Return the state of the Freeze checkbox.
        /// </summary>
        public bool FinalFreezeFlag
        {
            get { return chkFreezeFlag.Checked; }
        }

        /// <summary>
        /// Return the state of the Deep Freeze checkbox.
        /// </summary>
        public bool FinalDeepFreezeFlag
        {
            get { return chkDeepFreezeFlag.Checked; }
        }

        /// <summary>
        /// Return the thin kfs flag set by the user.
        /// </summary>
        public bool FinalThinKfsFlag
        {
            get { return !chkNoThinKfsFlag.Checked; }
        }

        public frmKwsProperties()
        {
            InitializeComponent();
        }

        public frmKwsProperties(WmUiBroker broker) : this()
        {
            m_uiBroker = broker;
            InitUI();
        }

        private void InitUI()
        {
            Workspace kws = m_uiBroker.Browser.SelectedKws;
            Debug.Assert(kws != null);
            Debug.Assert(!kws.IsPublicKws());

            KwsUser creator = kws.CoreData.UserInfo.Creator;

            // Misc unchangeable workspace infos.
            txtCreator.Text = creator != null ? creator.UiFullName : "<Unknown>";
            lblOrganization.Text = creator != null ? creator.OrgName : "<Unknown>";
            lblCreationDate.Text = creator != null ? Base.KDateToDateTime(creator.InvitationDate).ToString() : "<Unknown>";
            txtServer.Text = kws.CoreData.Credentials.KasID.Host;
            
            // Kws name.
            txtKwsName.Text = kws.CoreData.Credentials.KwsName;
            this.Text = kws.CoreData.Credentials.KwsName + " properties";
            lblKwsName.Text = kws.CoreData.Credentials.KwsName;
            InitialKwsName = kws.CoreData.Credentials.KwsName;

            // Kws type.
            if (kws.CoreData.Credentials.SecureFlag)
            {
                InitialKwsType = 2;
                rdoSecure.Checked = true;
            }
            else
            {
                InitialKwsType = 1;
                rdoStandard.Checked = true;
            }
            
            // KWS status.
            chkDeepFreezeFlag.Checked = kws.CoreData.Credentials.DeepFreezeFlag;
            InitialDeepFreezeFlag = kws.CoreData.Credentials.DeepFreezeFlag;

            chkFreezeFlag.Checked = kws.CoreData.Credentials.FreezeFlag;
            InitialFreezeFlag = kws.CoreData.Credentials.FreezeFlag;

            chkNoThinKfsFlag.Checked = !kws.CoreData.Credentials.ThinKfsFlag;
            InitialThinKfsFlag = kws.CoreData.Credentials.ThinKfsFlag;

            // Enable or disable the various controls.
            String denyReason = "";
            txtKwsName.Enabled = m_uiBroker.CanPerformKwsAction(KwsAction.Rename, kws, ref denyReason);
            txtKwsName.Tag = denyReason;

            grpKwsType.Enabled = m_uiBroker.CanPerformKwsAction(KwsAction.ChangeSecureFlag, kws, ref denyReason);
            grpKwsType.Tag = denyReason;

            chkFreezeFlag.Enabled = m_uiBroker.CanPerformKwsAction(KwsAction.ChangeFreezeFlag, kws, ref denyReason);
            chkFreezeFlag.Tag = denyReason;

            chkDeepFreezeFlag.Enabled = m_uiBroker.CanPerformKwsAction(KwsAction.ChangeDeepFreezeFlag, kws, ref denyReason);
            chkDeepFreezeFlag.Tag = denyReason;

            chkNoThinKfsFlag.Enabled = m_uiBroker.CanPerformKwsAction(KwsAction.ChangeThinKfsFlag, kws, ref denyReason);
            chkNoThinKfsFlag.Tag = denyReason;
        }

        private void UpdateOkButton()
        {
            btnOK.Enabled = Base.IsValidKwsName(txtKwsName.Text);
        }

        private void helpKwsName_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(txtKwsName, HelpChangeKwsNameString), sender as Control);
        }

        private void helpAuthBy_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(HelpAuthByString, sender as Control);
        }

        private void helpKwsType_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(grpKwsType, HelpKwsTypeString), sender as Control);
        }

        private void helpFreeze_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(chkFreezeFlag, HelpFreezeString), sender as Control);
        }

        private void helpDeepFreeze_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(chkDeepFreezeFlag, HelpDeepFreezeString), sender as Control);
        }

        private void helpThinKfs_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(chkNoThinKfsFlag, HelpThinKfsString), sender as Control);
        }

        private void chkPreserve_CheckedChanged(object sender, EventArgs e)
        {
            // If unchecking the checkbox, it means the user does not want
            // to preserve the deleted files. Warn him that this action is
            // irreversible.
            if (!chkNoThinKfsFlag.Checked)
            {
                if (Misc.KwmTellUser("Warning: removing this option is retroactive and will permanently remove past and future file history from the server. The effects of this operation are not reversible."
                    + Environment.NewLine + Environment.NewLine +
                    "Are you sure you want to continue?", "Preserve deleted files", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    chkNoThinKfsFlag.Checked = true;
                }
            }
        }

        private void txtKwsName_TextChanged(object sender, EventArgs e)
        {
            UpdateOkButton();
        }
    }
}