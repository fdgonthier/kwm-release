using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using kwm.Utils;
using Tbx.Utils;

namespace kwm
{
    public partial class ucCreateKwsHeader : UserControl
    {
        /// <summary>
        /// Reference to the UI Broker.
        /// </summary>
        public WmUiBroker UiBroker;

        public ucCreateKwsHeader()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                UiBroker.RequestCreateKws();
            }

            catch (Exception ex)
            {
                Base.HandleException(ex);
            }
        }
    }
}