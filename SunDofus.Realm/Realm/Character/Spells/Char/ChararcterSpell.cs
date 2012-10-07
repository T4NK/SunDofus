using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Spells
{
    class CharacterSpell
    {
        public int id = -1, level = 0, position = -1;

        public CharacterSpell(int mid, int mlvl, int mpos)
        {
            id = mid;
            level = mlvl;
            position = mpos;
        }
    }
}
