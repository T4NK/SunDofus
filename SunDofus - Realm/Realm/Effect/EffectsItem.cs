using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Effect
{
    class EffectsItem
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
                case 125:
                    AddLife();
                    break;
            }
        }

        #region Effects' Void

        private void AddLife()
        {
            Client.m_Stats.Life.Items += Value;
        }

        #endregion
    }
}
