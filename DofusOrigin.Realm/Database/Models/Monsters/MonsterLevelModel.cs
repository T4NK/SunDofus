using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Monsters
{
    class MonsterLevelModel
    {
        public int ID;
        public int CreatureID;
        public int GradeID;

        public int Level;
        public int AP;
        public int MP;
        public int Life;

        public int RNeutral;
        public int RStrenght;
        public int RIntel;
        public int RLuck;
        public int RAgility;

        public int RPa;
        public int RPm;

        public int Wisdom;
        public int Strenght;
        public int Intel;
        public int Luck;
        public int Agility;

        public int Exp;
        public List<Realm.Characters.Spells.CharacterSpell> Spells;

        public MonsterLevelModel()
        {
            Spells = new List<Realm.Characters.Spells.CharacterSpell>();
        }
    }
}
