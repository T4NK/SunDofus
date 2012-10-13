using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Levels
{
    class LevelModel
    {
        public int Id = 0;
        public long Character = 0, Job = 0, Mount = 0, Alignment = 0, Guild = 0;

        public LevelModel(long Max = 0)
        {
            Character = Max;
            Job = Max;
            Mount = Max;
            Alignment = Max;
            Guild = Max;
        }
    }
}
