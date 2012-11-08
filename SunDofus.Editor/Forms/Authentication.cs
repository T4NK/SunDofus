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
    public partial class Authentication : Form
    {
        public Authentication()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            try
            {
                Class.Infos.Username = textBox3.Text;
                Class.Infos.Password = textBox4.Text;

                Program.myServer.ConnectTo(textBox1.Text, int.Parse(textBox2.Text));
            }
            catch
            {
                Program.Message("Valeurs non valables !");
            }

            this.Close();
        }

        private void ClosingForm(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            if(!Program.myServer.isConnected)
                button1.Enabled = true;

            Hide();
        }
    }
}
