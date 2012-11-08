using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SunDofus.Editor.Forms
{
    public partial class BackgroundChanger : Form
    {
        static int CurBackID = 0;

        public BackgroundChanger()
        {
            InitializeComponent();
        }

        private void BackgroundChanger_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("./tiles/backgrounds/"))
            {
                Program.Write("Le dossier de background n'a pas été trouvé");
                Hide();
            }

            foreach (string path in Directory.GetFiles("./tiles/backgrounds/"))
            {
                this.treeView1.Nodes.Add(path, path, path);
            }
        }

        private void meClose(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text.Split('/')[treeView1.SelectedNode.Text.Split('/').Count() - 1].Split('.')[0];
            CurBackID = int.Parse(str);
            Program.Write("Nouveau background = " + CurBackID);
            Program.ActualMap.background = CurBackID;
            Program.RefreshMapData();
            Hide();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox1.Image = new Bitmap(Image.FromFile(treeView1.SelectedNode.Text), pictureBox1.Width, pictureBox1.Height);
        }
    }
}
