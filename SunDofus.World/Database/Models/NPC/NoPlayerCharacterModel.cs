using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Database.Models.NPC
{
    class NoPlayerCharacterModel
    {
        public int ID;
        public int GfxID;
        public int Size;
        public int Sex;

        public int Color;
        public int Color2;
        public int Color3;

        //public int ArtWork;
        //public int Bonus;

        public NPCsQuestion Question;

        public string Name;
        public string Items;

        public List<int> SellingList;

        public NoPlayerCharacterModel()
        {
            SellingList = new List<int>();
            Question = null;
        }
    }
}
