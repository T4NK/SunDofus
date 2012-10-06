using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Spells
{
    class AbstractSpell
    {
        public int id = -1, sprite = -1;
        public string spriteInfos = "";
        public List<AbstractSpellLevel> myLevels;

        public AbstractSpell()
        {
            myLevels = new List<AbstractSpellLevel>();
        }

        public void ParseLevel(string Data)
        {
            if (Data == "-1")
                return;

            AbstractSpellLevel m_L = new AbstractSpellLevel();

            string[] Stats = Data.Split(',');
            string Effect = Stats[0];
            string EffectCC = Stats[1];

            if (Stats[2] != "" && Stats[2] != "-1")
                m_L.Cost = int.Parse(Stats[2]);
            else
                m_L.Cost = 6;

            m_L.MinPO = int.Parse(Stats[3]);
            m_L.MaxPO = int.Parse(Stats[4]);
            m_L.CC = int.Parse(Stats[5]);
            m_L.EC = int.Parse(Stats[6]);

            m_L.OnlyLine = (Stats[7] == "true" ? true : false);
            m_L.OnlyViewLine = (Stats[8] == "true" ? true : false);
            m_L.EmptyCell = (Stats[9] == "true" ? true : false);
            m_L.AlterablePO = (Stats[10] == "true" ? true : false);

            m_L.MaxPerTurn = int.Parse(Stats[12]);
            m_L.MaxPerPlayer = int.Parse(Stats[13]);
            m_L.TurnNumber = int.Parse(Stats[14]);
            m_L.Type = Stats[15];
            m_L.ECEndTurn = (Stats[19] == "true" ? true : false);

            m_L.ParseEffect(Effect, false);
            m_L.ParseEffect(EffectCC, true);

            myLevels.Add(m_L);
        }
    }
}
