using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Wizard.UI;


namespace kwm
{
    public partial class ConfigKPPWizard : WizardSheet
    {
        // Keep a reference to the sign-in page so we can 
        // programatically read/write its fields.
        private ConfigKPPSignIn m_signInPage;

        public ConfigKPPWizard(WmKmodBroker broker)
        {
            InitializeComponent();
            this.AcceptButton = this.nextButton;
            this.Icon = Properties.Resources.TeamboxIcon;
            m_signInPage = new ConfigKPPSignIn(broker);

            this.Pages.Add(new ConfigKPPRegistration(broker));
            this.Pages.Add(m_signInPage);
            this.Pages.Add(new ConfigKPPEmailVerif(broker));
            this.Pages.Add(new ConfigKPPSuccess());

            ResizeToFit();
        }

        public void ShowSignInPage(bool autoSignin, bool skippedReg)
        {
            m_signInPage.AutoSignIn = autoSignin;
            m_signInPage.SkippedRegistration = skippedReg;
            this.SetActivePage("ConfigKPPSignIn");
        }

        public void ShowEmailVerificationPage()
        {
            this.SetActivePage("ConfigKPPEmailVerif");
        }

        public void ShowRegistrationPage()
        {
            this.SetActivePage("ConfigKPPRegistration");
        }

        public void ShowSuccessPage()
        {
            this.SetActivePage("ConfigKPPSuccess");
        }

        public void SaveCreds()
        {
            m_signInPage.SaveCredentials();
        }

        public void SetSignInCredentials(string kpsAddress, string userName, string pwd)
        {
            m_signInPage.SetCredentials(kpsAddress, userName, pwd);
        }

        public string getSignInUserName()
        {
            return m_signInPage.getUserName();
        }

        public string getSignInPwd()
        {
            return m_signInPage.getPwd();
        }

        public string getSignInKpsAddess()
        {
            return m_signInPage.getKpsAddess();
        }
    }
}
