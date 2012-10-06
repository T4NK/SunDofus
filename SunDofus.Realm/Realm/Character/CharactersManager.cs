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
            foreach (Character m_C in CharactersList)
            {
                if (m_C.m_Name == Name) return true;
            }
            return false;
        }
    }
}
