using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class EffectsItem
    {
        public int ID, Value, Value2, Value3 = 0;
        public string Effect = "0d0+0";

        public override string ToString()
        {
            return SunDofus.Basic.DeciToHex(ID) + "#" + (Value <= 0 ? "" : SunDofus.Basic.DeciToHex(Value)) + 
                "#" + (Value2 <= 0 ? "" : SunDofus.Basic.DeciToHex(Value2)) + "#0#" + Effect;
        }
    }
}
