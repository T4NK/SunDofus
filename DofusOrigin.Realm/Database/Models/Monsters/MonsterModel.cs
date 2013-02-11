using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Monsters
{
    class MonsterModel
    {
        public int ID;
        public int GfxID;
        public int Align;
        public int Color;
        public int Color2;
        public int Color3;
        public int IA;

        public int Max_kamas;
        public int Min_kamas;

        public string Name;

        public List<MonsterLevelModel> Levels;
        public List<MonsterItem> Items;

        public MonsterModel()
        {
            Levels = new List<MonsterLevelModel>();
            Items = new List<MonsterItem>();
        }

        public class MonsterItem
        {
            public int ID;
            public double Chance;
            public int Max;

            public MonsterItem(int newID, double newChance, int newMax)
            {
                ID = newID;
                Chance = newChance;
                Max = newMax;
            }
        }
    }
}
