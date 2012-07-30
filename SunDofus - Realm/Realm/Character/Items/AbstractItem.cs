using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class AbstractItem
    {
        public int ID, Type, Level, Pods, Price = 0;
        public string  Jet = "";
        public bool TwoHands = false;
        public bool Usable = false;

        public List<EffectsItem> EffectsList = new List<EffectsItem>();
    }
}
