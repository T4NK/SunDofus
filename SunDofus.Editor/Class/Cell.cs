using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.Drawing.Drawing2D;

namespace SunDofus.Editor.Class
{
    class Cell
    {
        public bool active = true;
        public int id;
        public int gID = -1;
        public int o1ID = -1;
        public int o2ID = -1;
        public int type = 4;//Walkable
        public bool LoS = true;
        public int glvl = 7;
        public int gslope = 1;
        public bool gflip = false;
        public bool o1flip = false;
        public bool o2flip = false;
        public int grot = 0;
        public int o1rot = 0;
        public bool o2interactive = false;
        public Point pos;
        
        public Cell(int i,int w,int h)
        {
            id = i;
            pos = getPositionByCellID(i,glvl,w,h);
        }
        public Cell(int aid,string CellData,int w,int h)
        {
            id = aid;
            byte[] CellInfo = {0,0,0,0,0,0,0,0,0,0};
            for (int i = CellData.Length-1; i >=0; i--)
                CellInfo[i]  = (byte)Program.Hash.IndexOf(CellData[i]);

            active = ((CellInfo[0] & 32) >> 5) != 0;
            if (!active)
            {
                gID = 0;
                return;
            }
            LoS = (CellInfo[0] & 1) != 0;
            grot = (CellInfo[1] & 48) >> 4;
            glvl = CellInfo[1] & 15;
            type = (CellInfo[2] & 56) >> 3;
            gID = ((CellInfo[0] & 24) << 6) + ((CellInfo[2] & 7) << 6) + CellInfo[3];
            gslope = (CellInfo[4] & 60) >> 2;
            gflip = ((CellInfo[4] & 2) >> 1) != 0;
            o1ID = ((CellInfo[0] & 4) << 11) + ((CellInfo[4] & 1) << 12) + (CellInfo[5] << 6) + CellInfo[6];
            o1rot = (CellInfo[7] & 48) >> 4;
            o1flip = ((CellInfo[7] & 8) >> 3) != 0;
            o2flip = ((CellInfo[7] & 4) >> 2) != 0;
            o2interactive = Convert.ToBoolean((CellInfo[7] & 2) >> 1);
            o2ID = ((CellInfo[0] & 2) << 12) + ((CellInfo[7] & 1) << 12) + (CellInfo[8] << 6) + CellInfo[9];

            pos = getPositionByCellID(id,glvl,w,h);
        }
        public Point getPositionByCellID(int id, int glvl,int w, int h)
        {
            double x = 0, y = 0;
            int j = -1;//loc9
            double k = 0;//loc11
            int l = 0;//loc10
            int s = w - 1;//loc14

            for (int z = 0; z < id; z++)
            {
                if (j == s)
                {
                    j = 0;
                    l++;
                    if (k == 0)
                    {
                        k = Program.CELL_W_HALF;
                        s--;
                    }
                    else
                    {
                        k = 0;
                        s++;
                    } // end else if
                }
                else
                {
                    ++j;
                }
            }
            x = j * Program.CELL_W + k + Program.CELL_W_HALF;
            y = (l + 1) * Program.CELL_H_HALF - Program.LEVEL_H * (glvl - 7);

            return new Point((int)x, (int)y);
        }
        
        public Point[] getPolygon()
        {
            List<Point> pts = new List<Point>();
            pts.Add(new Point((int)(pos.X + Program.CELL_W_HALF), pos.Y));//sommet de la case
            pts.Add(new Point((int)(pos.X + Program.CELL_W), (int)(pos.Y + Program.CELL_H_HALF)));//Droite
            pts.Add(new Point((int)(pos.X + Program.CELL_W_HALF), (int)(pos.Y + Program.CELL_H)));//Bas
            pts.Add(new Point(pos.X, (int)(pos.Y + Program.CELL_H_HALF)));//Gauche
            return pts.ToArray();
        }
        public bool containsPoint(Point p)
        {
            Point p1, p2;
            Point[] poly = getPolygon();
            bool inside = false;
            if (poly.Length < 3) return inside;
            Point oldPoint = new Point(poly[poly.Length - 1].X, poly[poly.Length - 1].Y);
            for (int i = 0; i < poly.Length; i++)
            {
                Point newPoint = new Point(poly[i].X, poly[i].Y);
                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }
                if ((newPoint.X < p.X) == (p.X <= oldPoint.X) && ((long)p.Y - (long)p1.Y) * (long)(p2.X - p1.X) < ((long)p2.Y - (long)p1.Y) * (long)(p.X - p1.X))
                    inside = !inside;
                oldPoint = newPoint;
            }
            return inside;
        }
    }
}
