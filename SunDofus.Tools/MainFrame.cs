using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunDofus.Tools
{
    public partial class MainFrame : Form
    {
        public MainFrame()
        {
            InitializeComponent();
        }

        private void me_willClose(object sender, FormClosingEventArgs e)
        {
            if (Class.DatabaseHandler.isStartToWrite == true)
                Class.DatabaseHandler.m_writer.Close();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_itemsModifier.Show();
        }

        private void openConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_databaseHandler.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Class.DatabaseHandler.isConnected == true)
            {
                pictureBox2.BackgroundImage = SunDofus.Tools.Properties.Resources.Accept24;
                label2.Text = "Connected !";
            }
            else
            {
                pictureBox2.BackgroundImage = SunDofus.Tools.Properties.Resources.cancel24;
                label2.Text = "Not connected !";
            }
        }

        private void closeConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Class.DatabaseHandler.Close();
        }

        private void itemToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Program.m_itemDeleter.Show();
        }

        private void itemToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.m_itemOpener.Show();
        }
    }
}
