using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SunDofus.Editor.Forms
{
    public partial class AuthenticationDB : Form
    {
        public AuthenticationDB()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            if (!Network.DBClient.isConnected == true)
                Network.DBClient.InitialiseConnection(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);

            Close();
        }

        void meClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();

            if (Network.DBClient.isConnected == false)
                button1.Enabled = true;
        }
    }
}
