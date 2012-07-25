using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character
{
    class CharacterState
    {
        Character Client;

        public CharacterState(Character m_C)
        {
            Client = m_C;
            Created = false;
        }

        public bool Created = false;
    }
}
