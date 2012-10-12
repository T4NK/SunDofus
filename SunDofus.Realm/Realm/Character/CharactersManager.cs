using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character
{
    class CharactersManager
    {
        public static List<Character> CharactersList = new List<Character>();

        public static bool ExistsName(string Name)
        {
            foreach (var myCharacter in CharactersList)
            {
                if (myCharacter.myName == Name) return true;
            }
            return false;
        }
    }
}
