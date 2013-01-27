using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps.Monsters
{
    class Monster
    {
        public Database.Models.Monsters.MonsterModel m_model;
        public int m_level;

        public Monster(Database.Models.Monsters.MonsterModel model, int grade)
        {
            m_model = model;
            m_level = grade;
        }
    }
}
