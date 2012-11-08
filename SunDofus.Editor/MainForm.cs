using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SunDofus.Editor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox2.Image = new Bitmap(Image.FromFile(treeView1.SelectedNode.Text), pictureBox2.Width, pictureBox2.Height);
        }

        private void solToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Ground)
                Program.Ground = false;
            else
                Program.Ground = true;

            Program.RefreshMapData();
        }

        private void erCalqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Object1)
                Program.Object1 = false;
            else
                Program.Object1 = true;

            Program.RefreshMapData();
        }

        private void èmeCalqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Object2)
                Program.Object2 = false;
            else
                Program.Object2 = true;

            Program.RefreshMapData();
        }

        private void modificationDuFondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Program.BackChangerF.Show();
            }
            catch { }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point MP = pictureBox1.PointToClient(MousePosition);
            Class.Cell DC = null;

            if (Program.ActualMap.cells.Values.Any(x => x.containsPoint(MP)))
                DC = Program.ActualMap.cells.Values.First(x => x.containsPoint(MP));
            else
            {
                Program.Write("Case inconnue : " + MP.ToString());
                return;
            }

            Program.Write("Case trouvée : " + DC.id);

            Program.ActualCell = DC;

            switch (Program.ActualModif)
            {
                case Program.CurrentModif.Ground:

                    try
                    {
                        if(e.Button == System.Windows.Forms.MouseButtons.Left)
                            DC.gID = int.Parse(treeView1.SelectedNode.ImageKey);
                        else
                            DC.gID = -1;
                    }
                    catch { };

                    break;

                case Program.CurrentModif.Object1:

                    try
                    {
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                            DC.o1ID = int.Parse(treeView1.SelectedNode.ImageKey);
                        else
                            DC.o1ID = -1;
                    }
                    catch { };

                    break;

                case Program.CurrentModif.Object2:

                    try
                    {
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                            DC.o2ID = int.Parse(treeView1.SelectedNode.ImageKey);
                        else
                            DC.o2ID = -1;
                    }
                    catch { };

                    break;

                case Program.CurrentModif.LOS:

                    try
                    {
                        DC.LoS = !DC.LoS;
                        Program.RefreshCellLoS();
                    }
                    catch { }

                    break;

                case Program.CurrentModif.Walk:

                    try
                    {
                        switch (DC.type)
                        {
                            case 0:
                                DC.type = 2;
                                break;
                            case 2:
                                DC.type = 4;
                                break;
                            case 4:
                                DC.type = 0;
                                break;
                        }

                        Program.RefreshCellWalk();
                    }
                    catch { }

                    break;
            }
            Program.RefreshMapData();
        }

        private void grilleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Grid)
                Program.Grid = false;
            else
                Program.Grid = true;

            Program.RefreshMapData();
        }

        private void solToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.ActualModif = Program.CurrentModif.None;
            Program.RefreshTiles();
        }

        private void solToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Program.ActualModif = Program.CurrentModif.Ground;
            Program.RefreshTiles();
        }

        private void erCalqueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.ActualModif = Program.CurrentModif.Object1;
            Program.RefreshTiles();
        }

        private void èmeCalqueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.ActualModif = Program.CurrentModif.Object2;
            Program.RefreshTiles();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.CanChallenge == true)
                Program.CanChallenge = false;
            else
                Program.CanChallenge = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.CanAgro == true)
                Program.CanAgro = false;
            else
                Program.CanAgro = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.isOut == true)
                Program.isOut = false;
            else
                Program.isOut = true;
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.LoadMap(new Class.Map(0, "a"));
        }

        private void sauvegarderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Class.Infos.MapPath == "")
            {
                sauvegarderSousToolStripMenuItem_Click(sender, e);
                return;
            }

            //Update SQL & SWF

            if (Program.myServer.isConnected == true)
                Program.myServer.Send("ANM");
        }

        private void sauvegarderSousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Program.SaveMapF.Show();
            }
            catch { }
        }

        private void seConnecterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Program.ConnectF.Show();
            }
            catch { }
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void seConnecterAuServeurMySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Program.DBF.Show();
            }
            catch { }
        }

        private void ligneDeVueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.LOS = !Program.LOS;
            Program.RefreshMapData();
        }

        private void typesDesCasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Walk = !Program.Walk;
            Program.RefreshMapData();
        }

        private void ligneDeVueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.ActualModif = Program.CurrentModif.LOS;
            Program.Walk = false;
            Program.LOS = true;
            Program.RefreshMapData();
        }

        private void typesDesCasesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.ActualModif = Program.CurrentModif.Walk;
            Program.LOS = false;
            Program.Walk = true;
            Program.RefreshMapData();
        }
    }
}
