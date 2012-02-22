using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using kwm.Utils;
using Tbx.Utils;

namespace kwm
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            RegistryKey kwmKey = null;
            try
            {
                kwmKey = Base.GetKwmLMRegKey();
                lblRegVersion.Text = (String)kwmKey.GetValue("InstallVersion", "Unknown");
            }

            catch (Exception ex)
            {
                Logging.Log(2, "Unable to find KWM version."); 
                Logging.LogException(ex);
                lblRegVersion.Text = "Unknown";
            }
            finally
            {
                if (kwmKey != null) kwmKey.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.teambox.co");
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }
    }
}