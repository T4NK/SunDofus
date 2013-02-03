using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.World.Conditions
{
    class NPCConditions
    {
        public int condiID { get; set; }
        public string args { get; set; }

        public bool HasCondition(Realm.Characters.Character character)
        {
            return true;
        }
    }
}
