using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character
{
    class CharacterState
    {
        Character Client;

        public CharacterState(Character myCharacter)
        {
            Client = myCharacter;
            Created = false;
        }

        public bool Created = false;

        public bool OnMove = false;
        public int MoveCell = -1;
    }
}
