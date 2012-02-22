using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Tbx.Utils;
using Wizard.UI;
using kwm.Utils;

namespace kwm
{
    public partial class ConfigKPPEmailVerif : InternalWizardPage
    {
        private WmKmodBroker m_broker = null;

        // Async query for the freemium web service.
        private HttpQuery m_httpQuery = null;

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if (value)
                    SetWizardButtons(WizardButtons.Back | WizardButtons.Next | WizardButtons.Cancel);
                else
                    SetWizardButtons(WizardButtons.Cancel);
                base.Enabled = value;
            }
        }

        public ConfigKPPEmailVerif(WmKmodBroker broker)
        {
            InitializeComponent();
            m_broker = broker;
            QueryCancel += OnCancellation;
        }

        private void ConfigKPPEmailVerif_SetActive(object sender, CancelEventArgs e)
        {
            this.emailLabel.Text = ((ConfigKPPWizard)this.GetWizard()).getSignInUserName();
            SetWizardButtons(WizardButtons.Next | WizardButtons.Back | WizardButtons.Cancel);
        }

        private void ConfigKPPEmailVerif_WizardBack(object sender, WizardPageEventArgs e)
        {
            e.Cancel = true;
            ((ConfigKPPWizard)GetWizard()).ShowRegistrationPage();
        }

        private void ConfigKPPEmailVerif_WizardNext(object sender, WizardPageEventArgs e)
        {
            try
            {
                this.Enabled = false;
                e.Cancel = true;

                String kpsAddress = ((ConfigKPPWizard)this.GetWizard()).getSignInKpsAddess();
                String email = ((ConfigKPPWizard)this.GetWizard()).getSignInUserName();

                email = System.Web.HttpUtility.UrlEncode(email);
                String ws_url = "https://" + kpsAddress + "/freemium/registration/" + email + "/verify";

                m_httpQuery = new HttpQuery(new Uri(ws_url));
                m_httpQuery.UseCache = false;
                m_httpQuery.OnHttpQueryEvent += new EventHandler<HttpQueryEventArgs>(HandleHttpQueryResult);
                m_httpQuery.StartQuery();
            }
            catch (Exception ee)
            {
                CancelCtx();
                Base.HandleException(ee);
            } 
        }

        // TODO: refactor callback to be re-useable.
        private void HandleHttpQueryResult(Object sender, HttpQueryEventArgs args)
        {
            try
            {
                string errTmpl = "We were unable to complete your sign-up: {0}. Please try again later or contact your network administrator.";

                this.Enabled = true;

                // Bailout if required.
                if (m_httpQuery == null) return;
                
                if (args.Type == HttpQueryEventType.Done)
                {
                    // Query succeeded, auto-signin with obtained credentials.
                    if (m_httpQuery.Result == "ok")
                    {
                        ((ConfigKPPWizard)GetWizard()).ShowSignInPage(true, false);
                        return;
                    }
                    // Email has not been confirmed yet.
                    else if (m_httpQuery.Result == "confirm")
                    {
                        Misc.KwmTellUser("Your email address has not been confirmed yet. " +
                                         "Please click on the link present in your confirmation email. " +
                                         "If you did not receive the confirmation email, look at your spam folder.",
                                         MessageBoxIcon.Information);
                    }

                    else
                    {
                        throw new Exception("Unexpected server error: " + m_httpQuery.Result + Environment.NewLine + Environment.NewLine +
                                            "Please report this to support@teambox.co.");
                    }
                }

                else if (args.Type == HttpQueryEventType.DnsError)
                {
                    Misc.KwmTellUser(String.Format(errTmpl, "Sign-up failed", args.Ex.Message));
                }

                else if (args.Type == HttpQueryEventType.HttpError)
                {
                    string errMsg = "";
                    if (string.IsNullOrEmpty(m_httpQuery.Result))
                        if (args.Ex != null)
                            errMsg = args.Ex.Message;
                        else
                            errMsg = "unknown server error.";
                    else
                        errMsg = m_httpQuery.Result;

                    Misc.KwmTellUser(String.Format(errTmpl, errMsg), "Sign-up failed", MessageBoxIcon.Error);
                }
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
            finally
            {
                CancelCtx();
            }
        }

        /// <summary>
        /// Called when this page is cancelled.
        /// </summary>
        private void OnCancellation(Object sender, CancelEventArgs e)
        {
            // If canceling while a query is in progress, cancel only the query
            // and not the wizard. e.Cancel tells wether or not the Wizard cancellation
            // should be canceled or not.
            if (m_httpQuery != null) e.Cancel = true;
            CancelCtx();
        }

        private void CancelCtx()
        {
            if (m_httpQuery != null)
            {
                m_httpQuery.Cancel();
                m_httpQuery = null;
            }
        }
    }
}
