using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DofusOrigin.Tools.Frames
{
    public partial class DeleteItem : Form
    {
        public DeleteItem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Class.DatabaseHandler.isConnected == false)
                {
                    MessageBox.Show("Your MySQL connection must active !");
                    return;
                }

                Class.DatabaseHandler.DeleteItemDB(int.Parse(textBox1.Text));
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
