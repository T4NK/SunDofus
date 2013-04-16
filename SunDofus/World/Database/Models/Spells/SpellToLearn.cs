using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Database.Models.Spells
{
    class SpellToLearnModel
    {
        public int Race;
        public int Level;
        public int SpellID;
        public int Pos;

        public SpellToLearnModel()
        {
            Race = 0;
            Level = 0;
            SpellID = 0;
            Pos = 0;
        }
    }
}
