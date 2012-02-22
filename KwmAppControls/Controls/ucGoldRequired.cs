using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Tbx.Utils;
using kwm.Utils;

namespace kwm.KwmAppControls.Controls
{
    public partial class ucGoldRequired : UserControl
    {
        public ucGoldRequired()
        {
            InitializeComponent();
        }

        private void lnkLearnMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Misc.OpenFileInWorkerThread(KwmStrings.MoreInfos);
            }
            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }
    }
}
