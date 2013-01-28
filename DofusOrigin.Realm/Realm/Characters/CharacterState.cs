using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters
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
        public bool onExchange = false;
        public bool onExchangePanel = false;

        public int moveToCell = -1;
        public int actualNPC = -1;
        public int actualTraided = -1;
        public int actualTraider = -1;

        public bool Occuped
        {
            get
            {
                return onMove || onExchange;
            }
        }
    }
}
