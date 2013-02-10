using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps.Monsters
{
    class Monster
    {
        public Database.Models.Monsters.MonsterModel Model;

        private int _level;

        public int Level
        {
            get
            {
                return _level;
            }
        }

        public Monster(Database.Models.Monsters.MonsterModel model, int grade)
        {
            Model = model;
            _level = grade;
        }
    }
}
