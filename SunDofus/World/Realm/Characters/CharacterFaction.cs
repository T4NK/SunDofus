using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.World.Realm.Characters
{
    class CharacterFaction
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private int _honor;

        public int Honor
        {
            get
            {
                return _honor;
            }
            set
            {
                _honor = value;
            }
        }

        private int _deshonor;

        public int Deshonor
        {
            get
            {
                return _deshonor;
            }
            set
            {
                _deshonor = value;
            }
        }

        public bool isEnabled = false;
    }
}
