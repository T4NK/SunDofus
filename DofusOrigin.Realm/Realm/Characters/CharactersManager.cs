using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters
{
    class CharactersManager
    {
        public static List<Character> CharactersList = new List<Character>();

        public static bool ExistsName(string name)
        {
            return CharactersList.Any(x => x.m_name == name);
        }
    }
}
