using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using kwm.Utils;
using System.Diagnostics;
using Tbx.Utils;

namespace kwm
{
    public partial class ucInviteToKws : UserControl
    {
        public MAPIAddressBook m_mapiAddrBook;

        /// <summary>
        /// Fired whenever the user typed some text in the invitation text box.
        /// </summary>
        public event EventHandler<EventArgs> OnInviteListChanged;

        private static String PersonalMsgText = "Add a personal message";

        /// <summary>
        /// Set to true when the user wants to add a personal message to the invitation. The
        /// UI will display a textbox and a cancel link label.
        /// </summary>
        private bool m_addPersonalMsgFlag = false;

        /// <summary>
        /// Name of the workspace this invitation is realted.
        /// </summary>
        private String m_kwsName = "";

        // These getters and setters are there to abstract the real UI control used.

        /// <summary>
        /// Get or set the custom user message to include in the invitation email.
        /// </summary>
        public String Message
        {
            get { return (m_addPersonalMsgFlag ? txtMessage.Text : ""); }
            set { txtMessage.Text = value; }
        }

        /// <summary>
        /// Set the Kws name displayed in the introduction text.
        /// </summary>
        public String KwsName
        {
            get { return m_kwsName; }
            set { m_kwsName = value; UpdateLblIntro(); }
        }

        /// <summary>
        /// Get or set a default invitation string.
        /// </summary>
        public String InvitationString
        {
            get { return txtRecipients.Text; }
            set { txtRecipients.Text = value; }
        }

        public ucInviteToKws()
        {
            InitializeComponent();
            
            // Try to create an instance of the MAPI address book. 
            // If this fails, this means the address book won't be
            // accessible and thus we need to hide the button that
            // accesses it.
            try
            {
                m_mapiAddrBook = new MAPIAddressBook();

                m_mapiAddrBook.WindowCaption = "Invite to workspace";
                m_mapiAddrBook.FieldCaption = "Invite";
                m_mapiAddrBook.FieldGroupCaption = "Users to invite";
            }
            catch (Exception ex)
            {
                Logging.LogException(ex);

                m_mapiAddrBook = null;
                btnOutlook.Visible = false;
            }
        }

        /// <summary>
        /// Return true if the user's input is empty or valid email address(es).
        /// </summary>
        public bool IsEmailAddressListValid()
        {
            foreach (String s in GetEmailAddressList())
                if (!Base.IsEmail(s)) return false;
            return true;
        }

        public KwsInviteOpParams GetInvitationParams()
        {
            KwsInviteOpParams p = new KwsInviteOpParams();
            p.KcdSendInvitationEmailFlag = true;

            // Prepare the char array used for the Split function.
            List<char> split = new List<char>();
            split.Add(';');
            split.Add(' ');
            split.Add(',');
            split.AddRange(Environment.NewLine.ToCharArray());

            foreach (String s in txtRecipients.Text.Split(split.ToArray(), StringSplitOptions.RemoveEmptyEntries))
                p.UserArray.Add(new KwsInviteOpUser(s.Trim()));

            p.Message = Message;

            return p;
        }

        /// <summary>
        /// Return the list of email addresses. No validation is performed.
        /// </summary>
        public List<String> GetEmailAddressList()
        {
            List<String> l = new List<String>();

            // Prepare the char array used for the Split function.
            List<char> split = new List<char>();
            split.Add(';');
            split.Add(' ');
            split.Add(',');
            split.AddRange(Environment.NewLine.ToCharArray());

            foreach (String s in txtRecipients.Text.Split(split.ToArray(), StringSplitOptions.RemoveEmptyEntries))
                l.Add(s.Trim());
            return l;
        }

        /// <summary>
        /// Update the linklabels status. If the user clicked on Add a personal
        /// message, show the Cancel linklabel and delinkify the Add a personnal
        /// message one.
        /// </summary>
        private void UpdateUI()
        {
            if (m_addPersonalMsgFlag)
            {
                lnkAddPersonalMsg.Text = PersonalMsgText;
                lnkAddPersonalMsg.LinkArea = new LinkArea(0, 0);
                lnkCancel.Visible = true;
                txtMessage.Visible = true;
                txtMessage.Select();
            }
            else
            {
                lnkAddPersonalMsg.Text = PersonalMsgText + "...";
                lnkAddPersonalMsg.LinkArea = new LinkArea(0, lnkAddPersonalMsg.Text.Length);
                lnkCancel.Visible = false;
                txtMessage.Visible = false;
            }
        }

        private void UpdateLblIntro()
        {
            lblKwsName.Text = m_kwsName;
        }

        private void txtRecipients_TextChanged(object sender, EventArgs e)
        {
            if (OnInviteListChanged != null) OnInviteListChanged(this, new EventArgs());
        }

        private void txtRecipients_Enter(object sender, EventArgs e)
        {
            txtRecipients.SelectAll();
        }

        private void lnkPersonalizedMsg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Debug.Assert(m_addPersonalMsgFlag == false);
                m_addPersonalMsgFlag = true;
                UpdateUI();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void lnkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Debug.Assert(m_addPersonalMsgFlag == true);
                m_addPersonalMsgFlag = false;
                UpdateUI();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void btnOutlook_Click(object sender, EventArgs e)
        {
            string[] addrs;

            try
            {
                if (m_mapiAddrBook != null)
                {
                    // Try to logon, this may display the profile selection dialog.
                    if (!m_mapiAddrBook.LoggedOn())
                        m_mapiAddrBook.Logon();

                    // Display the address book.
                    addrs = m_mapiAddrBook.Show(this.Handle);

                    if (addrs != null)
                    {
                        string s = string.Join(", ", addrs);

                        if (txtRecipients.Text.Trim() != "")
                            txtRecipients.Text += ", " + s;
                        else
                            txtRecipients.Text += s;
                    }
                }
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }        
    }
}
