using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DofusOrigin.Frames
{
    public partial class Database : Form
    {
        public Database()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Class.DatabaseHandler.isConnected == false)
                Class.DatabaseHandler.Initialise(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);

            this.Hide();
        }

        private void me_willClose(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
