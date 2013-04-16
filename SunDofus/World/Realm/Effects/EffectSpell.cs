using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.World.Realm.Characters.Spells;
namespace SunDofus.World.Realm.Effects
{
    class EffectSpell
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

        private int _value;

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private int _value2;

        public int Value2
        {
            get
            {
                return _value2;
            }
            set
            {
                _value2 = value;
            }
        }

        private int _value3;

        public int Value3
        {
            get
            {
                return _value3;
            }
            set
            {
                _value3 = value;
            }
        }

        private int _round;

        public int Round
        {
            get
            {
                return _round;
            }
            set
            {
                _round = value;
            }
        }

        private int _chance;

        public int Chance
        {
            get
            {
                return _chance;
            }
            set
            {
                _chance = value;
            }
        }

        private string _effect;

        public string Effect
        {
            get
            {
                return _effect;
            }
            set
            {
                _effect = value;
            }
        }

        public Target Target;

        public EffectSpell()
        {
            _value = 0;
            _value2 = 0;
            _value3 = 0;

            _round = 0;
            _chance = 0;

            _effect = "1d5+0";
        }
    }
}
