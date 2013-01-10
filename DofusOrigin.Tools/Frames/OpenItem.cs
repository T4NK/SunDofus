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
    public partial class OpenItem : Form
    {
        public OpenItem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Program.willOpen = int.Parse(textBox1.Text);
                Program.m_itemsModifier.UpdateItem();
                Program.m_itemsModifier.Show();
                textBox1.Text = "";
                Hide();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Format : " + ex.ToString());
            }
        }

        private void me_willClose(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
