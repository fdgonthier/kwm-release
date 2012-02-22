using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using Tbx.Utils;
using kwm.Utils;
using Wizard.UI;

namespace kwm
{
    public partial class ConfigKPPRegistration : InternalWizardPage
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
                    SetWizardButtons(WizardButtons.Next | WizardButtons.Cancel);
                else
                    SetWizardButtons(WizardButtons.Cancel);
                base.Enabled = value;
            }
        }

        public ConfigKPPRegistration(WmKmodBroker broker)
        {
            InitializeComponent();
            creds.PopulateFromRegistry();
            m_broker = broker;
            QueryCancel += OnCancellation;
        }

        private void ConfigKPPRegistration_SetActive(object sender, CancelEventArgs e)
        {
            try
            {
                // If the registry tells us so, go directly to the sign-in
                // page. 
                WmWinRegistry reg = WmWinRegistry.Spawn();
                if (reg.SkipRegistrationFlag)
                {
                    ((ConfigKPPWizard)GetWizard()).ShowSignInPage(false, true);
                    return;
                }

                this.Enabled = true;
                creds.Focus();
                
                creds.ResetError();
                UpdateNextButton();
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void ConfigKPPCredentials_WizardBack(object sender, WizardPageEventArgs e)
        {
            e.Cancel = true;
            ((ConfigKPPWizard)GetWizard()).ShowRegistrationPage();
        }

        private void ConfigKPPCredentials_WizardNext(object sender, WizardPageEventArgs e)
        {
            try
            {
                e.Cancel = true;
                // Hardcode freemium address for now
                this.creds.KpsAddress = "tbsos01.teambox.co";
                
                WmWinRegistry reg = WmWinRegistry.Spawn();
                if (!(string.IsNullOrEmpty(reg.FreemiumKpsAddr)))
                    this.creds.KpsAddress = reg.FreemiumKpsAddr;

                // Must be disabled after changing the creds.KpsAddress property
                this.Enabled = false;

                string email = this.creds.UserName;
                string pwd = this.creds.Password;
                string ws_url = "https://" + this.creds.KpsAddress + "/freemium/registration";

                string post_params = "";

                email = System.Web.HttpUtility.UrlEncode(email);
                pwd = System.Web.HttpUtility.UrlEncode(pwd);
                post_params = "email=" + email + "&pwd=" + pwd;

                m_httpQuery = new HttpQuery(new Uri(ws_url + "?" + post_params));
                m_httpQuery.UseCache = false;
                m_httpQuery.OnHttpQueryEvent += new EventHandler<HttpQueryEventArgs>(HandleHttpQueryResult);
                m_httpQuery.StartQuery();
            }

            catch (Exception ee)
            {
                // Make sure to release the query if required.
                CancelCtx();
                Base.HandleException(ee);
            }
        }

        // FIXME refactor with ConfigKPPEmailVerif.
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
                    ((ConfigKPPWizard)GetWizard()).SetSignInCredentials(creds.KpsAddress, creds.UserName, creds.Password);
                    if (m_httpQuery.Result == "ok")
                    {
                        ((ConfigKPPWizard)GetWizard()).ShowSignInPage(true, false);
                        return;
                    }

                    else if (m_httpQuery.Result == "confirm")
                    {
                        ((ConfigKPPWizard)GetWizard()).ShowEmailVerificationPage();
                        return;
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


                    if (m_httpQuery.StatusCode == HttpStatusCode.Forbidden)
                    {
                        // When the web services responds with a 403, the response body is not an error string to display to the user,
                        // instead, it's a protocol response that can be safely used here for exact match, to indicate the reason of faliure of the registration process.
                        switch (m_httpQuery.Result)
                        {
                            case "registration_disabled":
                                Misc.KwmTellUser("Free registration is currently closed. Please try again later.", "Registration is closed", MessageBoxIcon.Information);
                                break;
                            case "user_login_taken":
                                Misc.KwmTellUser("The email address you entered is not available. Please pick another one.", "Registration incomplete", MessageBoxIcon.Information);
                                break;
                            case "user_registration_locked":
                                Misc.KwmTellUser("The email address you entered is not available. Please pick another one.", "Registration incomplete", MessageBoxIcon.Information);
                                break;
                            default:
                                Misc.KwmTellUser(String.Format(errTmpl, "protocol error."), "Sign-up failed", MessageBoxIcon.Error);
                                break;
                        }
 
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(m_httpQuery.Result))
                            if (args.Ex != null)
                                errMsg = args.Ex.Message;
                            else
                                errMsg = "unknown server error";
                        else
                                errMsg = m_httpQuery.Result;

                        Misc.KwmTellUser(String.Format(errTmpl, errMsg), "Sign-up failed", MessageBoxIcon.Error);
                    }
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
            // Cancelling while query is taking place, only cancel query, not wizard.
            if (m_httpQuery != null) e.Cancel = true;
            CancelCtx(); 
            this.Enabled = true;
        }

        private void CancelCtx()
        {
            if (m_httpQuery != null)
            {
                m_httpQuery.Cancel();
                m_httpQuery = null;
            }
        }

        /// <summary>
        /// Set the 'Next' button enabled or disable, according to the status of
        /// the window.
        /// </summary>
        private void UpdateNextButton()
        {
            if (String.IsNullOrEmpty(this.creds.UserName) ||
                !Base.IsEmail(this.creds.UserName) ||
                String.IsNullOrEmpty(this.creds.Password))
            {
                EnableWizardButton(WizardButtons.Next, false);
            }

            else
            {
                EnableWizardButton(WizardButtons.Next, true);
            }
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ((ConfigKPPWizard)GetWizard()).ShowSignInPage(false, false);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Misc.OpenFileInWorkerThread("http://www.teambox.co/downloads/");
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }
    }
}
