using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace SunDofus.Editor.Forms
{
    public partial class SaveMap : Form
    {
        public SaveMap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFile = new FolderBrowserDialog();
            openFile.ShowDialog();

            if (openFile.SelectedPath != "")
            {
                textBox1.Text = openFile.SelectedPath;
                Class.Infos.MapPath = textBox1.Text;
            }
        }

        private void meClose(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            button3.Enabled = true;

            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try 
            {
                button3.Enabled = false;

                if (!File.Exists("./utilities/default.swf"))
                {
                    Program.Write("Le fichier swf modele default.swf n'existe pas; La création du swf est donc impossible");
                    Hide();
                    return;
                }

                Program.ActualMap.id = int.Parse(textBox2.Text);
                Program.ActualMap.date = textBox3.Text;

                var name = Program.ActualMap.id + "_00.swf";
                var path = Class.Infos.MapPath + "\\" + name;
                var flmpath = path.Replace(".swf", ".flm");

                if (File.Exists(path)) 
                    File.Delete(path);
                if (File.Exists(flmpath)) 
                    File.Delete(flmpath);

                File.Copy("./utilities/default.swf", path);

                ProcessStartInfo PSI = new ProcessStartInfo(Application.StartupPath + "/utilities/flasm.exe", path);
                PSI.WindowStyle = ProcessWindowStyle.Hidden;
                (Process.Start(PSI)).WaitForExit();
                
                StreamReader SR = new StreamReader(flmpath);
                string flmFile = SR.ReadToEnd();
                SR.Close();

                flmFile = flmFile.Replace("INSERTMAPIDHERE", Program.ActualMap.id.ToString());
                flmFile = flmFile.Replace("INSERTMAPWIDTHHERE", Program.ActualMap.width.ToString());
                flmFile = flmFile.Replace("INSERTMAPHEIGHTHERE", Program.ActualMap.height.ToString());
                flmFile = flmFile.Replace("INSERTBGIDHERE", Program.ActualMap.background.ToString());
                flmFile = flmFile.Replace("INSERTAMBIDHERE", Program.ActualMap.ambiance.ToString());
                flmFile = flmFile.Replace("INSERTMUSICIDHERE", Program.ActualMap.music.ToString());
                flmFile = flmFile.Replace("INSERTOUTDOORHERE", Program.ActualMap.outdoor ? "1" : "0");
                flmFile = flmFile.Replace("INSERTCAPABILITIESHERE", Program.ActualMap.capabilities.ToString());
                flmFile = flmFile.Replace("INSERTMAPDATAHERE", Program.GetMapData());

                File.Delete(flmpath);

                StreamWriter SW = new StreamWriter(flmpath, true);
                SW.Write(flmFile);
                SW.Flush();
                SW.Close();

                PSI = new ProcessStartInfo(Application.StartupPath + "/utilities/flasm.exe", flmpath);
                PSI.WindowStyle = ProcessWindowStyle.Hidden;
                (Process.Start(PSI)).WaitForExit();

                File.Delete(flmpath);
                Program.Write("La compilation de la map a réussie");

                Network.DBClient.InsertNewMap();
                if (Program.myServer.isConnected == true)
                    Program.myServer.Send("ANM");

                this.Close();
            }
            catch (Exception ex)
            {
                Program.Write("Impossible de sauvegarder la map ! " + ex.ToString());
                this.Close();
            }
        }
    }
}
