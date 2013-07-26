using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Tbx.Utils;
using Wizard.UI;

namespace kwm
{
    public partial class ConfigKPPSignIn : InternalWizardPage
    {
        private KmodQuery m_query = null;
        private WmKmodBroker m_broker = null;
        private bool LoginSuccess = false;
        private bool m_autoSignIn = false;
        private bool m_skippedRegistration = false;

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if (value)
                {
                    if (m_skippedRegistration)
                        SetWizardButtons(WizardButtons.Next | WizardButtons.Cancel);
                    else
                        SetWizardButtons(WizardButtons.Back | WizardButtons.Next | WizardButtons.Cancel);
                }
                else
                    SetWizardButtons(WizardButtons.Cancel);
              
                base.Enabled = value;
            }
        }

        public bool AutoSignIn
        {
            set { m_autoSignIn = value; }
        }

        public bool SkippedRegistration
        {
            set { m_skippedRegistration = value; }
        }

        public ConfigKPPSignIn(WmKmodBroker broker)
        {
            InitializeComponent();
            creds.PopulateFromRegistry();
            m_broker = broker;
            QueryCancel += OnCancellation;
        }

        /// <summary>
        /// Cancel and clear the current KMOD query, if there is one.
        /// </summary>
        private void ClearKmodQuery()
        {
            if (m_query != null)
            {
                m_query.Cancel();
                m_query = null;
            }
        }

        private void ConfigKPPSignIn_SetActive(object sender, CancelEventArgs e)
        {
            try
            {
                if (m_autoSignIn)
                {
                    m_autoSignIn = false;
                    this.Enabled = false;

                    // Submit the login query.
                    K3p.K3pLoginTest cmd = creds.GetLoginCommand();
                    m_query = new KmodQuery();
                    m_query.Submit(m_broker, new K3pCmd[] { cmd }, OnLoginResult);

                    return;
                }

                if (m_skippedRegistration)
                    SetWizardButtons(WizardButtons.Next | WizardButtons.Cancel);
                else
                    SetWizardButtons(WizardButtons.Back | WizardButtons.Next | WizardButtons.Cancel);

                LoginSuccess = false;
                
                UpdateNextButton();
                creds.ResetError();
                creds.Focus();
                creds.AdjustFocus();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void ConfigKPPSignIn_WizardNext(object sender, WizardPageEventArgs e)
        {
            // Clear the current query if there is one.
            this.Enabled = false;
            e.Cancel = true;

            ClearKmodQuery();

            // Submit the login query.
            K3p.K3pLoginTest cmd = creds.GetLoginCommand();
            m_query = new KmodQuery();
            m_query.Submit(m_broker, new K3pCmd[] { cmd }, OnLoginResult);
        }

        /// <summary>
        /// Called when this page is cancelled.
        /// </summary>
        private void OnCancellation(Object sender, CancelEventArgs e)
        {
            // Cancelling while query is taking place, only cancel query, not wizard.
            if (m_query != null)
            {
                ClearKmodQuery();
                e.Cancel = true;
                this.Enabled = true;
            }
        }

        /// <summary>
        /// Called when the login query results are available.
        /// </summary>
        private void OnLoginResult(KmodQuery query)
        {
            Logging.Log("OnLoginResult() called.");
            ClearKmodQuery();
            K3p.kmo_server_info_ack ack = query.OutMsg as K3p.kmo_server_info_ack;

            if (ack != null)
            {
                creds.Token = ack.Token;
                WmLoginTicketQuery ticketQuery = new WmLoginTicketQuery();
                SaveCredentials();
                m_query = ticketQuery;
                ticketQuery.Submit(m_broker, WmWinRegistry.Spawn(), OnTicketResult);
            }

            else
            {
                K3p.kmo_server_info_nack nack = query.OutMsg as K3p.kmo_server_info_nack;

                if (nack != null)
                {
                    if (nack.Error.StartsWith("cannot resolve ")) creds.SetServerError(nack.Error);
                    else creds.SetCredError(nack.Error);
                }

                else
                {
                    creds.SetServerError(query.OutDesc);
                }

                this.Enabled = true;
            }
        }

        /// <summary>
        /// Called when the ticket query results are available.
        /// </summary>
        private void OnTicketResult(WmLoginTicketQuery query)
        {
            ClearKmodQuery();

            query.UpdateRegistry(WmWinRegistry.Spawn());
            if (query.Res != WmLoginTicketQueryRes.OK) creds.SetServerError(query.OutDesc);
            else LoginSuccess = true;

            this.Enabled = true;

            // Pass to the next page.
            if (LoginSuccess)
            {
                ((ConfigKPPWizard)GetWizard()).ShowSuccessPage();
                return;
            }

        }

        /// <summary>
        /// Set the 'Next' button enabled or disable, according to the status of
        /// the window.
        /// </summary>
        private void UpdateNextButton()
        {
            EnableWizardButton(WizardButtons.Next, (creds.KpsAddress != "" &&
                                                    creds.UserName != "" &&
                                                    creds.Password != ""));
        }

        private void creds_OnCredFieldChange(object sender, EventArgs e)
        {
            try
            {
                if (this.GetWizard() != null)
                    UpdateNextButton();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        /// <summary>
        /// Save the configuration to the registry.
        /// </summary>
        public void SaveCredentials()
        {
            creds.Save(true);
        }


        /// <summary>
        /// Should be used by one function only: ConfigKppWizard.SetSignInCredentials
        /// </summary>
        public void SetCredentials(string kpsAddress, string userName, string pwd)
        {
            creds.KpsAddress = kpsAddress;
            creds.UserName = userName;
            creds.Password = pwd;
        }

        public string getUserName()
        {
            return creds.UserName;
        }

        public string getPwd()
        {
            return creds.Password;
        }

        public string getKpsAddess()
        {
            return creds.KpsAddress;
        }
    }
}
