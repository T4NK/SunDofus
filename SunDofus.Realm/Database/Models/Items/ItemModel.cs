using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Items
{
    class ItemModel
    {
        public int ID, Type, Level, Pods, Price = 0;
        public int Set = -1;
        public string  Jet = "";
        public bool TwoHands = false;
        public string Conditions = "";
        public bool Usable = false;

        public List<Realm.Effect.EffectsItems> EffectsList = new List<Realm.Effect.EffectsItems>();
    }
}
