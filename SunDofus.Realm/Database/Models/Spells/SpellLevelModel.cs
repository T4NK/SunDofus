using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character.Spells;

namespace realm.Database.Models.Spells
{
    class SpellLevelModel
    {
        public List<Realm.Effect.EffectsSpells> myEffects;
        public List<Realm.Effect.EffectsSpells> myCriticalEffects;

        public int Level = -1, Cost = 0, MinPO = 0, MaxPO = 1;
        public int CC = 0, EC = 0, MaxPerTurn = 0, MaxPerPlayer = 0, TurnNumber = 0;
        public bool OnlyLine = false, OnlyViewLine = false, AlterablePO = false;
        public bool ECEndTurn = false, EmptyCell = false;

        public string Type = "";

        public SpellLevelModel()
        {
            myCriticalEffects = new List<Realm.Effect.EffectsSpells>();
            myEffects = new List<Realm.Effect.EffectsSpells>();
        }

        public void ParseEffect(string Data, bool CC)
        {
            string[] List = Data.Split('|');

            foreach (string ActualEffect in List)
            {
                if (ActualEffect == "-1" | ActualEffect == "") continue;
                Realm.Effect.EffectsSpells m_E = new Realm.Effect.EffectsSpells();
                string[] Infos = ActualEffect.Split(';');

                m_E.id = int.Parse(Infos[0]);
                m_E.Value = int.Parse(Infos[1]);
                m_E.Value2 = int.Parse(Infos[2]);
                m_E.Value3 = int.Parse(Infos[3]);

                if (Infos.Length >= 8)
                {
                    m_E.Tour = int.Parse(Infos[4]);
                    m_E.Chance = int.Parse(Infos[5]);
                    m_E.Effect = Infos[6];
                    m_E.Target = new Target(int.Parse(Infos[7]));
                }
                else if (Infos.Length >= 7)
                {
                    m_E.Tour = int.Parse(Infos[4]);
                    m_E.Chance = int.Parse(Infos[5]);
                    m_E.Effect = Infos[6];
                    m_E.Target = new Target(23);
                }
                else if (Infos.Length >= 6)
                {
                    m_E.Tour = int.Parse(Infos[4]);
                    m_E.Chance = int.Parse(Infos[5]);
                    m_E.Effect = "0d0+0";
                    m_E.Target = new Target(23);
                }
                else if (Infos.Length >= 5)
                {
                    m_E.Tour = int.Parse(Infos[4]);
                    m_E.Chance = -1;
                    m_E.Effect = "0d0+0";
                    m_E.Target = new Target(23);
                }
                else
                {
                    m_E.Tour = 0;
                    m_E.Chance = -1;
                    m_E.Effect = "0d0+0";
                    m_E.Target = new Target(23);
                }

                if (CC == true)
                    myCriticalEffects.Add(m_E);
                else
                    myEffects.Add(m_E);
            }
        }
    }
}
