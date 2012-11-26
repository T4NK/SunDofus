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

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_itemsmodifier.Show();
        }
    }
}
