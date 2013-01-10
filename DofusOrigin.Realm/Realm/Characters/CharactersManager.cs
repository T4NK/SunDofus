using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters
{
    class CharactersManager
    {
        public static List<Character> m_charactersList = new List<Character>();

        public static bool ExistsName(string _name)
        {
            return m_charactersList.Any(x => x.m_name == _name);
        }
    }
}
