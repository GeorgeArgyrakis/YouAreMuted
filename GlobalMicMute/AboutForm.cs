using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlobalMicMute
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            string version = Application.ProductVersion;
            labelVersion.Text = "v."+version;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var ps = new ProcessStartInfo("https://github.com/GeorgeArgyrakis/YouAreMuted")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);

        }
    }
}
