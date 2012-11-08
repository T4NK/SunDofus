using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;

namespace SunDofus.Editor
{
    static class Program
    {
        static MainForm MainF;
        public static Forms.BackgroundChanger BackChangerF;
        public static Forms.BrowseMaps BrowserMapsF;
        public static Forms.SaveMap SaveMapF;
        public static Forms.Authentication ConnectF;
        public static Forms.AuthenticationDB DBF;

        public static Network.TCPClient myServer;

        public static Class.Map ActualMap = null;
        public static Class.Cell ActualCell = null;

        public static bool Grid = true;
        public static Bitmap bmpGrid = null;
        public static Graphics graphGrid = null;
        
        public static bool Object2 = true;
        public static Graphics graphObject2;
        public static Bitmap bmpObject2;
        
        public static bool Object1 = true;
        public static Graphics graphObject1;
        public static Bitmap bmpObject1;

        public static bool Ground = true;
        public static Graphics graphGround;
        public static Bitmap bmpGround;

        public static bool LOS = false;
        public static Graphics graphLOS;
        public static Bitmap bmpLOS;

        public static bool Walk = false;
        public static Graphics graphWalk;
        public static Bitmap bmpWalk;

        public static bool Background = true;
        public static Graphics graphBG;
        public static Bitmap bmpBG;

        public static bool CanChallenge = false;
        public static bool CanAgro = false;
        public static bool isOut = false;

        public const int CELL_W = 53;
        public const double CELL_W_HALF = 26.5;
        public const double CELL_H_HALF = 13.5;
        public const int CELL_H = 27;
        public const double LEVEL_H = 20;
        public const string Hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

        public static CurrentModif ActualModif;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            myServer = new Network.TCPClient();

            MainF = new MainForm();
            BackChangerF = new Forms.BackgroundChanger();
            BrowserMapsF = new Forms.BrowseMaps();
            SaveMapF = new Forms.SaveMap();
            ConnectF = new Forms.Authentication();
            DBF = new Forms.AuthenticationDB();

            Write("Lancement de SunEditor...");
            Write("Chargement des paramètres...");

            ActualModif = CurrentModif.Ground;
            RefreshTiles();
            LoadMap(new Class.Map(0, "a"));

            Application.Run(MainF);
        }

        public static void Message(string Message)
        {
            MessageBox.Show(Message);
        }

        public static void Write(string Message)
        {
            MainF.Logger.AppendText(Message + Environment.NewLine);
        }

        public static void LoadMap(Class.Map myMap)
        {
            ActualCell = null;
            ActualMap = myMap;

            InitializeMapGraphics();
            RefreshCellLoS();
            RefreshCellWalk();
            RefreshMapData();
        }

        static void InitializeMapGraphics()
        {
            bmpGround = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height);
            graphGround = Graphics.FromImage(bmpGround);
            bmpObject1 = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height);
            graphObject1 = Graphics.FromImage(bmpObject1);
            bmpObject2 = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height);
            graphObject2 = Graphics.FromImage(bmpObject2);
            bmpLOS = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height);
            graphLOS = Graphics.FromImage(bmpLOS);
            bmpWalk = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height);
            graphWalk = Graphics.FromImage(bmpWalk);
            bmpGrid = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height);
            graphGrid = Graphics.FromImage(bmpGrid);
            bmpBG = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height);
            graphBG = Graphics.FromImage(bmpBG);
            
            graphGround.SmoothingMode = SmoothingMode.HighQuality;
            graphGrid.SmoothingMode = SmoothingMode.HighQuality;
            graphObject1.SmoothingMode = SmoothingMode.HighQuality;
            graphObject2.SmoothingMode = SmoothingMode.HighQuality;
            graphLOS.SmoothingMode = SmoothingMode.HighQuality;
            graphWalk.SmoothingMode = SmoothingMode.HighQuality;

            var size = 1;
            Pen p = new Pen(Color.White, size);
            graphGrid.DrawLine(p, new Point(0, 0), new Point(MainF.pictureBox1.Width - size + 1, 0));
            graphGrid.DrawLine(p, new Point(0, 0), new Point(0, MainF.pictureBox1.Height - size + 1));
            graphGrid.DrawLine(p, new Point(MainF.pictureBox1.Width - size + 1, 0), new Point(MainF.pictureBox1.Width - size + 1, MainF.pictureBox1.Width - size + 1));
            graphGrid.DrawLine(p, new Point(MainF.pictureBox1.Width - size + 1, MainF.pictureBox1.Width - size + 1), new Point(MainF.pictureBox1.Width - size + 1, MainF.pictureBox1.Width - size + 1));

            for (int x = 0; x < (int)((ActualMap.width + 1) * (1 + ActualMap.height) / 2); x++)
            {
                int X1 = (int)(CELL_W_HALF + x * CELL_W);
                int Y2 = (int)(CELL_H_HALF + x * CELL_H);
                graphGrid.DrawLine(p, new Point(X1, 0), new Point(0, Y2));
                X1 = (int)((x + 1) * CELL_W);
                Y2 = (int)(CELL_H_HALF + (ActualMap.height - x) * CELL_H);
                graphGrid.DrawLine(p, new Point(X1, (int)(CELL_H_HALF + (ActualMap.height + 1) * CELL_H)), new Point(0, Y2));
            }
        }

        public static void RefreshMapData()
        {
            Bitmap i = new Bitmap(MainF.pictureBox1.Width, MainF.pictureBox1.Height),t;
            RefreshCells();

            string bgpath = "";
            graphBG.Clear(Color.Black);
            if (ActualMap.background > 0)
            {
                if (File.Exists("./tiles/backgrounds/" + ActualMap.background + ".jpg"))
                {
                    t = new Bitmap(Image.FromFile("./tiles/backgrounds/" + ActualMap.background + ".jpg"), MainF.pictureBox1.Size);
                    graphBG.DrawImage(t, new Point(0, 0));
                    bgpath = "./tiles/backgrounds/" + ActualMap.background + ".jpg";
                }
                if (File.Exists("./tiles/backgrounds/" + ActualMap.background + ".png"))
                {
                    t = new Bitmap(Image.FromFile("./tiles/backgrounds/" + ActualMap.background + ".png"), MainF.pictureBox1.Size);
                    graphBG.DrawImage(t, new Point(0, 0));
                    bgpath = "./tiles/backgrounds/" + ActualMap.background + ".png";
                }
                if (File.Exists("./tiles/backgrounds/" + ActualMap.background + ".bmp"))
                {
                    t = new Bitmap(Image.FromFile("./tiles/backgrounds/" + ActualMap.background + ".bmp"), MainF.pictureBox1.Size);
                    graphBG.DrawImage(t, new Point(0, 0));
                    bgpath = "./tiles/backgrounds/" + ActualMap.background + ".bmp";
                }
            }

            if (Background) (Graphics.FromImage(i)).DrawImage(bmpBG, new Point());
            if (Ground) (Graphics.FromImage(i)).DrawImage(bmpGround, new Point());
            if (Object1) (Graphics.FromImage(i)).DrawImage(bmpObject1, new Point());
            if (Object2) (Graphics.FromImage(i)).DrawImage(bmpObject2, new Point());
            if (LOS) (Graphics.FromImage(i)).DrawImage(bmpLOS, new Point());
            if (Walk) (Graphics.FromImage(i)).DrawImage(bmpWalk, new Point());
            if (Grid) (Graphics.FromImage(i)).DrawImage(bmpGrid, new Point());

            MainF.pictureBox1.Image = i;
            MainF.pictureBox1.Width = (ActualMap.width + 1) * CELL_W;
            MainF.pictureBox1.Height = (ActualMap.height + 1) * CELL_H;
        }

        public static string GetMapData()
        {
            return ActualMap.CompressedDatas();
        }

        public static void RefreshCellLoS()
        {
            graphLOS.Clear(Color.Transparent);
            foreach (Class.Cell DC in ActualMap.cells.Values)
            {
                //Color LoS
                Color c;
                if (DC.LoS) c = Color.FromArgb(127, Color.Yellow);
                else c = Color.FromArgb(127, Color.YellowGreen);
                graphLOS.FillPolygon(new SolidBrush(c), DC.getPolygon());
            }
        }

        public static void RefreshCellWalk()
        {
            graphWalk.Clear(Color.Transparent);
            foreach (Class.Cell DC in ActualMap.cells.Values)
            {
                Color c;
                if (DC.type == 0) c = Color.FromArgb(127, Color.Red);
                else if (DC.type == 2) c = Color.FromArgb(127, Color.Purple);
                else c = Color.FromArgb(127, Color.Blue);
                graphWalk.FillPolygon(new SolidBrush(c), DC.getPolygon());
            } 
        }

        static void RefreshCells()
        {
            graphGround.Clear(Color.Transparent);
            graphObject1.Clear(Color.Transparent);
            graphObject2.Clear(Color.Transparent);
            foreach (Class.Cell DC in ActualMap.cells.Values)
            {
                Point pos = DC.pos;

                if (DC.gID >= 0)
                {
                    Bitmap b = getGroundBitmap(DC.gID);
                    if (b != null)
                    {
                        if (DC.grot != 0)
                        {
                            int rot = DC.grot * 90;
                            b = RotateImage(b, rot);
                            if (rot % 180 == 0) b.SetResolution((float)1.928600E+002, (float)5.185000E+001);//From Dofus Client _xscale _yscale

                        }
                        if (DC.gflip) b.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        graphGround.DrawImage(b, pos);
                    }
                }
                pos = DC.pos;

                if (DC.o1ID >= 0)
                {
                    Bitmap b = getObjectBitmap(DC.o1ID);
                    if (b != null)
                    {
                        if (DC.o1rot != 0)
                        {
                            int rot = DC.o1rot * 90;
                            b = RotateImage(b, rot);
                            if (rot % 180 == 0) b.SetResolution((float)1.928600E+002, (float)5.185000E+001);//From Dofus Client _xscale _yscale

                        }
                        if (DC.o1flip) b.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        //*
                        pos.X += (int)(CELL_W_HALF - b.Width / 2);
                        pos.Y += (int)(CELL_H_HALF - b.Height / 2);
                        //*/
                        graphObject1.DrawImage(b, pos);
                    }
                }
                pos = DC.pos;

                if (DC.o2ID >= 0)
                {
                    Bitmap b = getObjectBitmap(DC.o2ID);
                    if (b != null)
                    {
                        if (DC.o2flip) b.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        //*
                        pos.X += (int)(CELL_W_HALF - b.Width / 2);
                        pos.Y += (int)(CELL_H_HALF - b.Height);
                        //*/
                        graphObject2.DrawImage(b, pos);
                    }
                }
            }
        }

        public static Bitmap getGroundBitmap(int id)
        {
            if (File.Exists("./tiles/grounds/" + id + ".png"))
                return new Bitmap(Image.FromFile("./tiles/grounds/" + id + ".png"));
            else if (File.Exists("./tiles/grounds/" + id + ".jpg"))
                return new Bitmap(Image.FromFile("./tiles/grounds/" + id + ".jpg"));
            return null;
        }

        public static Bitmap getObjectBitmap(int id)
        {
            if (File.Exists("./tiles/objects/" + id + ".png"))
                return new Bitmap(Image.FromFile("./tiles/objects/" + id + ".png"));
            else if (File.Exists("./tiles/objects/" + id + ".jpg"))
                return new Bitmap(Image.FromFile("./tiles/objects/" + id + ".jpg"));
            return null;
        }
        public static Bitmap RotateImage(Bitmap img, float rotationAngle)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            gfx.RotateTransform(rotationAngle);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.DrawImage(img, new Point(0, 0));
            gfx.Dispose();
            return bmp;
        }

        public static void RefreshTiles()
        {
            switch (ActualModif)
            {
                case CurrentModif.Ground:

                    MainF.pictureBox2.Image = new Bitmap(MainF.pictureBox2.Width, MainF.pictureBox2.Height); 
                    MainF.treeView1.Nodes.Clear();

                    foreach (var path in System.IO.Directory.GetFiles("./tiles/grounds/"))
                    {
                        if (path.Contains("Thumbs")) continue;
                        var str = path.Split('/')[path.Split('/').Count() - 1].Split('.')[0];
                        MainF.treeView1.Nodes.Add(str, path, str);
                    }

                    break;

                case CurrentModif.None:

                    MainF.pictureBox2.Image = new Bitmap(MainF.pictureBox2.Width, MainF.pictureBox2.Height);
                    MainF.treeView1.Nodes.Clear();

                    break;

                case CurrentModif.Object1:
                case CurrentModif.Object2:
                default:
                    
                    MainF.pictureBox2.Image = new Bitmap(MainF.pictureBox2.Width, MainF.pictureBox2.Height); 
                    MainF.treeView1.Nodes.Clear();

                    foreach (var path in System.IO.Directory.GetFiles("./tiles/objects/"))
                    {
                        if (path.Contains("Thumbs")) continue;
                        var str = path.Split('/')[path.Split('/').Count() - 1].Split('.')[0];
                        MainF.treeView1.Nodes.Add(str, path, str);
                    }

                    break;
            }
        }

        public enum CurrentModif
        {
            None,
            Ground,
            Object1,
            Object2,
            LOS,
            Walk,
        }
    }
}
