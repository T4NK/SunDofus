using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Characters
{
    class CharacterState
    {
        Character m_client;

        public CharacterState(Character _character)
        {
            m_client = _character;
            created = false;
        }

        public bool created = false;

        public bool onMove = false;
        public int moveToCell = -1;
    }
}
