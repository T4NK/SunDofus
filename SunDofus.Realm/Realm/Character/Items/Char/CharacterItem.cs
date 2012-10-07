using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class CharacterItem
    {
        public Database.Models.Items.ItemModel BaseItem;
        public int ID, Position = -1;
        public int Quantity = 0;
        public List<Effect.EffectsItems> EffectsList;

        public CharacterItem(Database.Models.Items.ItemModel b_I)
        {
            BaseItem = b_I;
            EffectsList = new List<Effect.EffectsItems>();
        }

        public string EffectsInfos()
        {
            string Infos = "";

            foreach (Effect.EffectsItems Effect in EffectsList)
            {
                Infos += Effect.ToString() + ",";
            }

            if (Infos == "") Infos = ",";

            return Infos.Substring(0, Infos.Length - 1);
        }

        public void ParseJet()
        {
            string Jet = BaseItem.Jet;

            foreach (string m_J in Jet.Split(','))
            {
                if (m_J == "") continue;
                string[] Infos = m_J.Split('#');

                Effect.EffectsItems e_I = new Effect.EffectsItems();
                e_I.ID = Utilities.Basic.HexToDeci(Infos[0]);
                if (Infos.Length > 1) e_I.Value = Utilities.Basic.HexToDeci(Infos[1]);
                if (Infos.Length > 2) e_I.Value2 = Utilities.Basic.HexToDeci(Infos[2]);
                if (Infos.Length > 3) e_I.Value3 = Utilities.Basic.HexToDeci(Infos[3]);
                if (Infos.Length > 4) e_I.Effect = Infos[4];

                EffectsList.Add(e_I);
            }
        }

        public void GeneratItem()
        {
            this.Quantity = 1;
            this.Position = -1;

            foreach (Effect.EffectsItems Effect in EffectsList)
            {
                NewJetAvaliable(Effect);
            }
        }

        public void NewJetAvaliable(Effect.EffectsItems EID)
        {
            if (EID.ID == 91 | EID.ID == 92 | EID.ID == 93 | EID.ID == 94 | EID.ID == 95 | EID.ID == 96 | EID.ID == 97 | EID.ID == 98 | EID.ID == 99 | EID.ID == 100 | EID.ID == 101)
            {

            }
            else if (EID.ID == 800)
            {
                EID.Value3 = 10; // PDV Des familiers !
            }
            else
            {
                EID.Value = Utilities.Basic.GetRandomJet(EID.Effect);
                EID.Value2 = -1;
            }
        }

        public string SaveString()
        {
            return Utilities.Basic.DeciToHex(BaseItem.ID) + "~" + Utilities.Basic.DeciToHex(Quantity) + "~"
                + (Position == -1 ? "" : Utilities.Basic.DeciToHex(Position)) + "~" + EffectsInfos();
        }

        public override string ToString()
        {
            return Utilities.Basic.DeciToHex(ID) + "~" + Utilities.Basic.DeciToHex(BaseItem.ID) + "~" + Utilities.Basic.DeciToHex(Quantity) + "~"
                + (Position == -1 ? "" : Utilities.Basic.DeciToHex(Position)) + "~" + EffectsInfos();
        }
    }
}
