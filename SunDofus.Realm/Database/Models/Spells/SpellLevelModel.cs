using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character.Spells;

namespace realm.Database.Models.Spells
{
    class SpellLevelModel
    {
        public List<Realm.Effect.EffectsSpells> myEffects;
        public List<Realm.Effect.EffectsSpells> myCriticalEffects;

        public int myLevel = -1, myCost = 0, myMinPO = 0, myMaxPO = 1;
        public int myCC = 0, myEC = 0, myMaxPerTurn = 0, myMaxPerPlayer = 0, myTurnNumber = 0;
        public bool myOnlyLine = false, myOnlyViewLine = false, myAlterablePO = false;
        public bool myECEndTurn = false, myEmptyCell = false;

        public string myType = "";

        public SpellLevelModel()
        {
            myCriticalEffects = new List<Realm.Effect.EffectsSpells>();
            myEffects = new List<Realm.Effect.EffectsSpells>();
        }

        public void ParseEffect(string Data, bool CC)
        {
            string[] List = Data.Split('|');

            foreach (var ActualEffect in List)
            {
                if (ActualEffect == "-1" | ActualEffect == "") continue;

                var myEffect = new Realm.Effect.EffectsSpells();
                string[] Infos = ActualEffect.Split(';');

                myEffect.myId = int.Parse(Infos[0]);
                myEffect.Value = int.Parse(Infos[1]);
                myEffect.Value2 = int.Parse(Infos[2]);
                myEffect.Value3 = int.Parse(Infos[3]);

                if (Infos.Length >= 8)
                {
                    myEffect.Tour = int.Parse(Infos[4]);
                    myEffect.Chance = int.Parse(Infos[5]);
                    myEffect.Effect = Infos[6];
                    myEffect.Target = new Target(int.Parse(Infos[7]));
                }
                else if (Infos.Length >= 7)
                {
                    myEffect.Tour = int.Parse(Infos[4]);
                    myEffect.Chance = int.Parse(Infos[5]);
                    myEffect.Effect = Infos[6];
                    myEffect.Target = new Target(23);
                }
                else if (Infos.Length >= 6)
                {
                    myEffect.Tour = int.Parse(Infos[4]);
                    myEffect.Chance = int.Parse(Infos[5]);
                    myEffect.Effect = "0d0+0";
                    myEffect.Target = new Target(23);
                }
                else if (Infos.Length >= 5)
                {
                    myEffect.Tour = int.Parse(Infos[4]);
                    myEffect.Chance = -1;
                    myEffect.Effect = "0d0+0";
                    myEffect.Target = new Target(23);
                }
                else
                {
                    myEffect.Tour = 0;
                    myEffect.Chance = -1;
                    myEffect.Effect = "0d0+0";
                    myEffect.Target = new Target(23);
                }

                if (CC == true)
                    myCriticalEffects.Add(myEffect);
                else
                    myEffects.Add(myEffect);
            }
        }
    }
}
