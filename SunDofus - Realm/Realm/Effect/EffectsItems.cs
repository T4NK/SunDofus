using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Effect
{
    class EffectsItems
    {
        public int ID, Value, Value2, Value3 = 0;
        public string Effect = "0d0+0";
        public Character.Character Client;

        public override string ToString()
        {
            return SunDofus.Basic.DeciToHex(ID) + "#" + (Value <= 0 ? "" : SunDofus.Basic.DeciToHex(Value)) + 
                "#" + (Value2 <= 0 ? "" : SunDofus.Basic.DeciToHex(Value2)) + "#" + (Value3 <= 0 ? "0" : SunDofus.Basic.DeciToHex(Value3)) + "#" + Effect;
        }

        public string SetString()
        {
            return SunDofus.Basic.DeciToHex(ID) + "#" + (Value <= 0 ? "" : SunDofus.Basic.DeciToHex(Value)) +
                "#" + (Value2 <= 0 ? "" : SunDofus.Basic.DeciToHex(Value2));
        }

        public void ParseEffect(Character.Character _Client)
        {
            Client = _Client;

            switch (ID)
            {
                case 110: // + Vie
                    AddLife();
                    break;

                case 111: // + PA
                    AddPA();
                    break;

                case 117: // + PO
                    AddPO();
                    break;

                case 118: // + Strenght
                    AddStrenght();
                    break;

                case 119: // + Agility
                    AddAgility();
                    break;

                case 123: // + Luck
                    AddLuck();
                    break;

                case 124: // + Wisdom
                    AddWisdom();
                    break;

                case 125: // + Vitalité
                    AddLife();
                    break;

                case 126: // + Intel
                    AddIntel();
                    break;

                case 128: // + PM
                    AddPM();
                    break;

                case 153: // - Vitalité
                    SubLife();
                    break;

                case 174: // + Initiative
                    AddInitiative();
                    break;

                case 176: // + Prospection
                    AddProspection();
                    break;
            }
        }

        #region Effects' Void

        private void AddLife()
        {
            Client.m_Stats.Life.Items += Value;
        }

        private void SubLife()
        {
            Client.m_Stats.Life.Items -= Value;
        }

        private void AddPA()
        {
            Client.m_Stats.PA.Items += Value;
        }

        private void AddPM()
        {
            Client.m_Stats.PM.Items += Value;
        }

        private void AddPO()
        {
            Client.m_Stats.PO.Items += Value;
        }

        private void AddWisdom()
        {
            Client.m_Stats.Wisdom.Items += Value;
        }

        private void AddStrenght()
        {
            Client.m_Stats.Strenght.Items += Value;
        }

        private void AddIntel()
        {
            Client.m_Stats.Intelligence.Items += Value;
        }

        private void AddLuck()
        {
            Client.m_Stats.Luck.Items += Value;
        }

        private void AddAgility()
        {
            Client.m_Stats.Agility.Items += Value;
        }

        private void AddInitiative()
        {
            Client.m_Stats.Initiative.Items += Value;
        }

        private void AddProspection()
        {
            Client.m_Stats.Prospection.Items += Value;
        }

        #endregion
    }
}
