using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Editor.Class
{
    class Map
    {
        public int id;
        public string date;
        public string key = "";
        public int music = 0;
        public int ambiance = 0;
        public int capabilities = 0;
        public Dictionary<int, Cell> cells = new Dictionary<int, Cell>();
        public int background = 0;
        public int width = 19;
        public int height = 22;
        public bool outdoor = true;

        public Map(int i, string d)
        {
            id = i;
            date = d;

            cells = new Dictionary<int, Cell>();

            int maxCellID = width * height + (width - 1) * (height - 1);
            for (int a = 1; a <= maxCellID; a++) cells.Add(a, new Cell(a, width, height));
        }

        public string CompressedDatas()
        {
            var datas = "";

            foreach (var Cell in cells.Values)
                datas += CompressCell(Cell);

            return datas;
        }

        public string CompressCell(Cell DC)
        {
            string datas = "";
            int[] infos = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            infos[0] = (DC.active ? (1) : (0)) << 5;
            infos[0] = infos[0] | (DC.LoS ? (1) : (0));
            infos[0] = infos[0] | (DC.gID & 1536) >> 6;
            infos[0] = infos[0] | (DC.o1ID & 8192) >> 11;
            infos[0] = infos[0] | (DC.o2ID & 8192) >> 12;

            infos[1] = (DC.grot & 3) << 4;
            infos[1] = infos[1] | DC.glvl & 15;

            infos[2] = (DC.type & 7) << 3;
            infos[2] = infos[2] | DC.gID >> 6 & 7;

            infos[3] = DC.gID & 63;

            infos[4] = (DC.gslope & 15) << 2;
            infos[4] = infos[4] | (DC.gflip ? (1) : (0)) << 1;
            infos[4] = infos[4] | DC.o1ID >> 12 & 1;

            infos[5] = DC.o1ID >> 6 & 63;

            infos[6] = DC.o1ID & 63;

            infos[7] = (DC.o1rot & 3) << 4;
            infos[7] = infos[7] | (DC.o1flip ? (1) : (0)) << 3;
            infos[7] = infos[7] | (DC.o2flip ? (1) : (0)) << 2;
            infos[7] = infos[7] | (DC.o2interactive ? (1) : (0)) << 1;
            infos[7] = infos[7] | DC.o2ID >> 12 & 1;

            infos[8] = DC.o2ID >> 6 & 63;

            infos[9] = DC.o2ID & 63;

            char[] HASH = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
	            't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
	            'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

            foreach (int i in infos)
            {
                datas += HASH[i];
            }
            return datas;
        }        
    }
}
