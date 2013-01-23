using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps.Monsters
{
    class MonstersGroup
    {
        public List<Monster> m_monsters;
        private Dictionary<int, List<int>> basemonsters;

        public int m_id;
        public int maxSize = 8;

        private Map m_map;
        private int cell;
        private int dir;

        public MonstersGroup(Dictionary<int, List<int>> monsters, Map map)
        {

            m_monsters = new List<Monster>();
            basemonsters = monsters;

            m_map = map;
            maxSize = map.m_map.maxGroupSize;

            this.m_id = map.NextNpcID();

            RefreshMappos();

            if(basemonsters.Count != 0)
                RefreshMonsters();
        }

        private void RefreshMonsters()
        {
            var i = Utilities.Basic.Rand(1, maxSize);
            for (int size = 1; size <= i; size++)
            {
                var mob = ReturnNewMonster();

                if (mob == null)
                    continue;

                m_monsters.Add(mob);
            }
        }

        private Monster ReturnNewMonster()
        {
            //Monster ID
            var key = basemonsters.Keys.ToList()[Utilities.Basic.Rand(0, basemonsters.Count - 1)];

            //Monster Grade
            var value = basemonsters[key][Utilities.Basic.Rand(0, basemonsters[key].Count - 1)];

            if (!Database.Cache.MonstersCache.m_monsters.Any(x => x.m_id == key))
                return null;

            return new Monster(Database.Cache.MonstersCache.m_monsters.First(x => x.m_id == key), value);
        }

        private void RefreshMappos()
        {
            dir = Utilities.Basic.Rand(0, 3) * 2 + 1;
            cell = m_map.m_rushablesCells[Utilities.Basic.Rand(0, m_map.m_rushablesCells.Count - 1)];
            //StartMove
        }

        public string PatternOnMap()
        {
            var packet = string.Format("|+{0};{1};0;{2};", cell, dir, m_id);

            var ids ="";
            var skins = "";
            var lvls = "";
            var colors = "";

            var first = true;
            foreach (var monster in m_monsters)
            {
                if (first)
                    first = false;

                else
                {
                    ids += ",";
                    skins += ",";
                    lvls += ",";
                    colors += ",";
                }

                var model = monster.m_model;
                ids += model.m_id;
                skins += model.m_gfx + "^100";
                lvls += model.m_levels.First(x => x.m_creature == model.m_id).m_level;

                colors += string.Format("{0},{1},{2};0,0,0,0", Utilities.Basic.DeciToHex(model.m_color),
                    Utilities.Basic.DeciToHex(model.m_color2), Utilities.Basic.DeciToHex(model.m_color3));
            }

            packet += string.Format("{0};-3;{1};{2};{3}", ids, skins, lvls, colors);

            return packet;
        }
    }
}
