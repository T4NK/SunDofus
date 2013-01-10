using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Spells
{
    class SpellModel
    {
        public int m_id  { get; set; }
        public int m_sprite { get; set; }

        public string m_spriteInfos { get; set; }

        public List<SpellLevelModel> m_levels { get; set; }

        public SpellModel()
        {
            m_levels = new List<SpellLevelModel>();
            m_id = -1;
            m_sprite = -1;
            m_spriteInfos = "";
        }

        public void ParseLevel(string _datas)
        {
            if (_datas == "-1")
                return;

            var level = new SpellLevelModel();
            var stats = _datas.Split(',');

            var effect = stats[0];
            var effectCC = stats[1];

            if (stats[2] != "" && stats[2] != "-1")
                level.m_cost = int.Parse(stats[2]);
            else
                level.m_cost = 6;

            level.m_minRP = int.Parse(stats[3]);
            level.m_maxRP = int.Parse(stats[4]);
            level.m_CC = int.Parse(stats[5]);
            level.m_EC = int.Parse(stats[6]);

            level.isOnlyLine = (stats[7] == "true" ? true : false);
            level.isOnlyViewLine = (stats[8] == "true" ? true : false);
            level.isEmptyCell = (stats[9] == "true" ? true : false);
            level.isAlterablePO = (stats[10] == "true" ? true : false);

            level.m_maxPerTurn = int.Parse(stats[12]);
            level.m_maxPerPlayer = int.Parse(stats[13]);
            level.m_turnNumber = int.Parse(stats[14]);
            level.m_type = stats[15];
            level.isECEndTurn = (stats[19] == "true" ? true : false);

            level.ParseEffect(effect, false);
            level.ParseEffect(effectCC, true);

            m_levels.Add(level);
        }
    }
}
