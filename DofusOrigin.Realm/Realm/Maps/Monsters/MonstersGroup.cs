using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps.Monsters
{
    class MonstersGroup
    {
        public List<Database.Models.Monsters.MonsterModel> m_monsters;
        private Dictionary<int, List<int>> basemonsters;

        public int m_id;

        private Map m_map;
        private int cell;
        private int dir;

        public MonstersGroup(Dictionary<int, List<int>> monsters, Map map)
        {
            m_monsters = new List<Database.Models.Monsters.MonsterModel>();
            basemonsters = monsters;
            m_map = map;

            this.m_id = map.NextNpcID();

            RefreshMappos();
            RefreshMonsters();
        }

        private void RefreshMonsters()
        {
            //Random of monster from Dictionary to List
        }

        private void RefreshMappos()
        {
            dir = Utilities.Basic.Rand(0, 3) * 2 + 1;
            //Random cell
            //StartMove
        }

        public string PatternOnMap()
        {
            var packet = string.Format("|+{0};{1};0;{2};", cell, dir, m_id);

            //Finish packet

            return packet;
        }
    }
}
