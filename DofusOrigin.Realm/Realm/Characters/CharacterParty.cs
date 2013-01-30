using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters
{
    class CharacterParty
    {
        public Dictionary<Character, int> myMembers;

        public CharacterParty(Character mener)
        {
            myMembers = new Dictionary<Character, int>();
            myMembers.Add(mener, 1);
        }

        public void AddMember(Character newMember)
        {
            myMembers.Add(newMember, 0);
            //Packet info
        }
    }
}
