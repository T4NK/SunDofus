using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.World.Entities.Models.Levels
{
    class LevelModel
    {
        public int ID;

        public long Character;
        public long Job;
        public long Alignment;
        public long Guild;
        public long Mount;

        public LevelModel(long _max = 0)
        {
            Character = _max;
            Job = _max;
            Mount = _max;
            Alignment = _max;
            Guild = _max;
        }
    }
}
