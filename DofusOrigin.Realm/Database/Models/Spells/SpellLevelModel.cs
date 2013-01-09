using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Characters.Spells;

namespace realm.Database.Models.Spells
{
    class SpellLevelModel
    {
        public List<Realm.Effects.EffectSpell> m_effects { get; set; }
        public List<Realm.Effects.EffectSpell> m_criticalEffects { get; set; }

        public int m_level { get; set; }
        public int m_cost { get; set; }
        public int m_minRP { get; set; }
        public int m_maxRP { get; set; }
        public int m_CC { get; set; }
        public int m_EC { get; set; }
        public int m_maxPerTurn { get; set; }
        public int m_maxPerPlayer  { get; set; }
        public int m_turnNumber { get; set; }

        public bool isOnlyViewLine { get; set; }
        public bool isOnlyLine { get; set; }
        public bool isAlterablePO { get; set; }
        public bool isECEndTurn { get; set; }
        public bool isEmptyCell { get; set; }

        public string m_type { get; set; }

        public SpellLevelModel()
        {
            m_criticalEffects = new List<Realm.Effects.EffectSpell>();
            m_effects = new List<Realm.Effects.EffectSpell>();

            m_level = -1;
            m_CC = 0;
            m_cost = 0;
            m_minRP = -1;
            m_maxRP = 1;
            m_EC = 0;
            m_maxPerPlayer = 0;
            m_maxPerTurn = 0;
            m_turnNumber = 0;
            isOnlyLine = false;
            isOnlyViewLine = false;
            isAlterablePO = false;
            isECEndTurn = false;
            isEmptyCell = false;
        }

        public void ParseEffect(string _datas, bool _CC)
        {
            var List = _datas.Split('|');

            foreach (var actualEffect in List)
            {
                if (actualEffect == "-1" | actualEffect == "") 
                    continue;

                var effect = new Realm.Effects.EffectSpell();
                var infos = actualEffect.Split(';');

                effect.m_id = int.Parse(infos[0]);
                effect.m_value = int.Parse(infos[1]);
                effect.m_value2 = int.Parse(infos[2]);
                effect.m_value3 = int.Parse(infos[3]);

                if (infos.Length >= 8)
                {
                    effect.m_tour = int.Parse(infos[4]);
                    effect.m_chance = int.Parse(infos[5]);
                    effect.m_effect = infos[6];
                    effect.m_target = new Target(int.Parse(infos[7]));
                }
                else if (infos.Length >= 7)
                {
                    effect.m_tour = int.Parse(infos[4]);
                    effect.m_chance = int.Parse(infos[5]);
                    effect.m_effect = infos[6];
                    effect.m_target = new Target(23);
                }
                else if (infos.Length >= 6)
                {
                    effect.m_tour = int.Parse(infos[4]);
                    effect.m_chance = int.Parse(infos[5]);
                    effect.m_effect = "0d0+0";
                    effect.m_target = new Target(23);
                }
                else if (infos.Length >= 5)
                {
                    effect.m_tour = int.Parse(infos[4]);
                    effect.m_chance = -1;
                    effect.m_effect = "0d0+0";
                    effect.m_target = new Target(23);
                }
                else
                {
                    effect.m_tour = 0;
                    effect.m_chance = -1;
                    effect.m_effect = "0d0+0";
                    effect.m_target = new Target(23);
                }

                if (_CC == true)
                    m_criticalEffects.Add(effect);
                else
                    m_effects.Add(effect);
            }
        }
    }
}
