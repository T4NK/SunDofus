using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Spells
{
    class CharacterSpell
    {
        public int myId = -1, myLevel = 0, myPosition = -1;

        public CharacterSpell(int mid, int mlvl, int mpos)
        {
            myId = mid;
            myLevel = mlvl;
            myPosition = mpos;
        }
    }
}
