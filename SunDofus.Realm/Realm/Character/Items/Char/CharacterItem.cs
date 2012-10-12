using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class CharacterItem
    {
        public Database.Models.Items.ItemModel myBaseItem;
        public int myID, myPosition = -1;
        public int myQuantity = 0;
        public List<Effect.EffectsItems> myEffectsList;

        public CharacterItem(Database.Models.Items.ItemModel myBase)
        {
            myBaseItem = myBase;
            myEffectsList = new List<Effect.EffectsItems>();
        }

        public string EffectsInfos()
        {
            var Infos = "";

            foreach (var Effect in myEffectsList)
                Infos += string.Format("{0},", Effect.ToString());

            if (Infos == "") Infos = ",";

            return Infos.Substring(0, Infos.Length - 1);
        }

        public void ParseJet()
        {
            var Jet = myBaseItem.myJet;

            foreach (var myJet in Jet.Split(','))
            {
                if (myJet == "") continue;
                string[] Infos = myJet.Split('#');

                var myEffect = new Effect.EffectsItems();
                myEffect.ID = Utilities.Basic.HexToDeci(Infos[0]);
                if (Infos.Length > 1) myEffect.Value = Utilities.Basic.HexToDeci(Infos[1]);
                if (Infos.Length > 2) myEffect.Value2 = Utilities.Basic.HexToDeci(Infos[2]);
                if (Infos.Length > 3) myEffect.Value3 = Utilities.Basic.HexToDeci(Infos[3]);
                if (Infos.Length > 4) myEffect.Effect = Infos[4];

                myEffectsList.Add(myEffect);
            }
        }

        public void GeneratItem()
        {
            this.myQuantity = 1;
            this.myPosition = -1;

            foreach (var Effect in myEffectsList)
                NewJetAvaliable(Effect);
        }

        public void NewJetAvaliable(Effect.EffectsItems myEffect)
        {
            if (myEffect.ID == 91 | myEffect.ID == 92 | myEffect.ID == 93 | myEffect.ID == 94 | myEffect.ID == 95 | myEffect.ID == 96 | myEffect.ID == 97 | myEffect.ID == 98 | myEffect.ID == 99 | myEffect.ID == 100 | myEffect.ID == 101)
            {

            }
            else if (myEffect.ID == 800)
            {
                myEffect.Value3 = 10; // PDV Des familiers !
            }
            else
            {
                myEffect.Value = Utilities.Basic.GetRandomJet(myEffect.Effect);
                myEffect.Value2 = -1;
            }
        }

        public string SaveString()
        {
            return string.Format("{0}~{1}~{2}~{3}", Utilities.Basic.DeciToHex(myBaseItem.myID), Utilities.Basic.DeciToHex(myQuantity),
                (myPosition == -1 ? "" : Utilities.Basic.DeciToHex(myPosition)), EffectsInfos());
        }

        public override string ToString()
        {
            return string.Format("{0}~{1}~{2}~{3}~{4}",Utilities.Basic.DeciToHex(myID), Utilities.Basic.DeciToHex(myBaseItem.myID),
                Utilities.Basic.DeciToHex(myQuantity), (myPosition == -1 ? "" : Utilities.Basic.DeciToHex(myPosition)), EffectsInfos());
        }
    }
}
