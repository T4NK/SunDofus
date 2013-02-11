using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.Characters.Spells;

namespace DofusOrigin.Database.Models.Spells
{
    class SpellLevelModel
    {
        public List<Realm.Effects.EffectSpell> Effects;
        public List<Realm.Effects.EffectSpell> CriticalEffects;

        public int Level;
        public int Cost;
        public int MinRP;
        public int MaxRP;
        public int CC;
        public int EC;
        public int MaxPerTurn;
        public int MaxPerPlayer;
        public int TurnNumber;

        public bool isOnlyViewLine;
        public bool isOnlyLine;
        public bool isAlterablePO;
        public bool isECEndTurn;
        public bool isEmptyCell;

        public string Type;

        public SpellLevelModel()
        {
            CriticalEffects = new List<Realm.Effects.EffectSpell>();
            Effects = new List<Realm.Effects.EffectSpell>();

            Level = -1;
            CC = 0;
            Cost = 0;
            MinRP = -1;
            MaxRP = 1;
            EC = 0;
            MaxPerPlayer = 0;
            MaxPerTurn = 0;
            TurnNumber = 0;
            isOnlyLine = false;
            isOnlyViewLine = false;
            isAlterablePO = false;
            isECEndTurn = false;
            isEmptyCell = false;
        }

        public void ParseEffect(string _datas, bool _CC)
        {
            var List = _datas.Split('|');

            foreach (var actualEffect in List)
            {
                if (actualEffect == "-1" | actualEffect == "") 
                    continue;

                var effect = new Realm.Effects.EffectSpell();
                var infos = actualEffect.Split(';');

                effect.ID = int.Parse(infos[0]);
                effect.Value = int.Parse(infos[1]);
                effect.Value2 = int.Parse(infos[2]);
                effect.Value3 = int.Parse(infos[3]);

                if (infos.Length >= 8)
                {
                    effect.Round = int.Parse(infos[4]);
                    effect.Chance = int.Parse(infos[5]);
                    effect.Effect = infos[6];
                    effect.Target = new Target(int.Parse(infos[7]));
                }
                else if (infos.Length >= 7)
                {
                    effect.Round = int.Parse(infos[4]);
                    effect.Chance = int.Parse(infos[5]);
                    effect.Effect = infos[6];
                    effect.Target = new Target(23);
                }
                else if (infos.Length >= 6)
                {
                    effect.Round = int.Parse(infos[4]);
                    effect.Chance = int.Parse(infos[5]);
                    effect.Effect = "0d0+0";
                    effect.Target = new Target(23);
                }
                else if (infos.Length >= 5)
                {
                    effect.Round = int.Parse(infos[4]);
                    effect.Chance = -1;
                    effect.Effect = "0d0+0";
                    effect.Target = new Target(23);
                }
                else
                {
                    effect.Round = 0;
                    effect.Chance = -1;
                    effect.Effect = "0d0+0";
                    effect.Target = new Target(23);
                }

                if (_CC == true)
                {
                    lock(CriticalEffects)
                        CriticalEffects.Add(effect);
                }
                else
                {
                    lock(Effects)
                        Effects.Add(effect);
                }
            }
        }
    }
}
