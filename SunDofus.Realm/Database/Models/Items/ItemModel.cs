using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Items
{
    class ItemModel
    {
        public int myID, myType, myLevel, myPods, myPrice = 0;
        public int mySet = -1;
        public string  myJet = "";
        public bool meTwoHands = false;
        public string myConditions = "";
        public bool meUsable = false;

        public List<Realm.Effect.EffectsItems> myEffectsList = new List<Realm.Effect.EffectsItems>();
        public List<Realm.World.Conditions.ItemCondition> myConds;

        public void ParseConditions()
        {
            myConds = new List<Realm.World.Conditions.ItemCondition>();
        }
    }
}
