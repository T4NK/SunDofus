using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Stats
{
    class AbstractStats
    {
        public int Bases, Items, Dons, Boosts = 0;

        public int Total()
        {
            return (Bases + Items + Dons + Boosts);
        }

        public int Base()
        {
            return Bases;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", Bases, Items, Dons, Boosts);
        }
    }
}
