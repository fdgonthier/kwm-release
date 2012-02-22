using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tbx.Utils;
using kwm.KwmAppControls;
using System.Diagnostics;
using kwm.Utils;

namespace kwm
{
    /// <summary>
    /// Form allowing a user to view and change a user's properties.
    /// Note: the Tag property of the various fields always contains 
    /// a String. When the control is disabled, the string is a user
    /// friendly message telling why the control is disabled. When the
    /// control is enabled, the string is empty.
    /// </summary>
    public partial class frmUserProperties : frmKBaseForm
    {
        private String HelpChangeLockString
        {
            get
            {
                return "A locked user cannot connect to the " + Base.GetKwsString() + ". " + 
                       "This can be used to temporarily suspend a user's access instead of " + 
                       "completely removing the user from the " + Base.GetKwsString() + ".";
            }
        }

        private String HelpChangeRoleString
        {
            get
            {
                return "More details on role and their restrictions, see the User Guide available under the Help menu.";
            }
        }

        private String HelpChangeNameString
        {
            get
            {
                return "You can change this user's display name here.";
            }
        }

        /// <summary>
        /// Reference to the UI broker.
        /// </summary>
        private WmUiBroker m_uiBroker;

        /// <summary>
        /// Remember if the KWM user can set the targer user the
        /// Administrator role. Used because we can't disable this one
        /// dropdown item in cboRole: instead if the user selects the
        /// Administrator role, a warning is displayed to the user.
        /// </summary>
        private bool m_canSetAdmin;

        /// <summary>
        /// Remember the last selected role so that we can revert
        /// to this role if the user selects the Administrator role
        /// and doesn't have the permission.
        /// </summary>
        private Int32 m_lastSelectedRole;

        /// <summary>
        /// Target user's name when the dialog is displayed. Used to know if it
        /// was changed.
        /// </summary>
        public String InitialUserName;

        /// <summary>
        /// Target user's role when the dialog is displayed. Used to know if it
        /// was changed.
        /// </summary>
        public Int32 InitialRole;

        /// <summary>
        /// Target user's lock status when the dialog is displayed. Used to 
        /// know if it was changed.
        /// </summary>
        public bool InitialLockedAccountChecked;

        /// <summary>
        /// Return the trimmed content of the username textbox.
        /// </summary>
        public String FinalUserName
        {
            get { return txtUserName.Text.Trim(); }
        }

        /// <summary>
        /// Return the selected index of the Role combobox.
        /// 0 => Admin
        /// 1 => Manager
        /// 2 => User
        /// </summary>
        public Int32 FinalRole
        {
            get { return cboRole.SelectedIndex; }
        }

        /// <summary>
        /// Return the Checked state of the Lock Account checkbox.
        /// </summary>
        public bool FinalLockedAccountChecked
        {
            get { return chkLockedAccount.Checked; }
        }

        public frmUserProperties()
        {
            InitializeComponent();
        }

        public frmUserProperties(WmUiBroker broker) : this()
        {
            m_uiBroker = broker;            
            InitUI();
        }

        /// <summary>
        /// Initialize the form's content.
        /// </summary>
        private void InitUI()
        {
            KwsUser targetUser = m_uiBroker.GetSelectedUser();
            Debug.Assert(targetUser != null); 
            
            String unused = "";
            m_canSetAdmin = m_uiBroker.CanPerformUserAction(KwsUserAction.ChangeAdminFlag, m_uiBroker.Browser.SelectedKws, targetUser, ref unused);

            FillFields(targetUser);
            SetFieldStatus(targetUser);
        }

        /// <summary>
        /// Fill the content of the various fields.
        /// </summary>
        private void FillFields(KwsUser targetUser)
        {            
            lblUserName.Text = targetUser.UiSimpleName;
            txtUserName.Text = lblUserName.Text;
            lblUserEmail.Text = targetUser.EmailAddress;

            if (targetUser.AdminFlag) cboRole.SelectedIndex = 0;
            else if (targetUser.ManagerFlag) cboRole.SelectedIndex = 1;
            else cboRole.SelectedIndex = 2;

            chkLockedAccount.Checked = targetUser.LockFlag;

            // Remember the original data so that we can know what was changed.
            InitialUserName = lblUserName.Text;
            InitialRole = cboRole.SelectedIndex;
            m_lastSelectedRole = InitialRole;
            InitialLockedAccountChecked = chkLockedAccount.Checked;
        }

        /// <summary>
        /// Set the field's enabled status. Only the fields that can be
        /// changed by the user are enabled.
        /// </summary>
        private void SetFieldStatus(KwsUser targetUser)
        {
            String deniedExpl = "";
            Workspace kws = m_uiBroker.Browser.SelectedKws;
            txtUserName.Enabled = m_uiBroker.CanPerformUserAction(KwsUserAction.SetName, kws, targetUser, ref deniedExpl);
            txtUserName.Tag = deniedExpl;

            cboRole.Enabled = m_uiBroker.CanPerformUserAction(KwsUserAction.ChangeManagerFlag, kws, targetUser, ref deniedExpl);
            cboRole.Tag = deniedExpl;

            chkLockedAccount.Enabled = m_uiBroker.CanPerformUserAction(KwsUserAction.ChangeLockFlag, kws, targetUser, ref deniedExpl);
            chkLockedAccount.Tag = deniedExpl;
        }

        private void helpLock_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(chkLockedAccount, HelpChangeLockString), sender as  Control);
        }

        private void helpRole_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(cboRole, HelpChangeRoleString), sender as Control);
        }

        private void helpName_Click(object sender, EventArgs e)
        {
            Base.ShowHelpTooltip(Misc.GetHelpString(txtUserName, HelpChangeNameString), sender as Control);
        }

        private void cboRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (InitialRole != 0 && cboRole.SelectedIndex == 0 && !m_canSetAdmin)
                {
                    Misc.KwmTellUser("Only the System Administrator may set the Administrator role.");
                    cboRole.SelectedIndex = m_lastSelectedRole;
                }

                m_lastSelectedRole = cboRole.SelectedIndex;
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }
    }
}
