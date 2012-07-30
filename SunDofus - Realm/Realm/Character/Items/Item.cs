using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Items
{
    class Item
    {
        public AbstractItem BaseItem;
        public int ID, Position = -1;
        public int Quantity = 0;
        public List<EffectsItem> EffectsList;

        public Item(AbstractItem b_I)
        {
            BaseItem = b_I;
            EffectsList = new List<EffectsItem>();
        }

        public string EffectsInfos()
        {
            string Infos = "";

            foreach (EffectsItem Effect in EffectsList)
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

                EffectsItem e_I = new EffectsItem();
                e_I.ID = SunDofus.Basic.HexToDeci(Infos[0]);
                if (Infos.Length > 1) e_I.Value = SunDofus.Basic.HexToDeci(Infos[1]);
                if (Infos.Length > 2) e_I.Value2 = SunDofus.Basic.HexToDeci(Infos[2]);
                if (Infos.Length > 3) e_I.Value3 = SunDofus.Basic.HexToDeci(Infos[3]);
                if (Infos.Length > 4) e_I.Effect = Infos[4];

                EffectsList.Add(e_I);
            }
        }

        public void GeneratItem()
        {
            this.Quantity = 1;
            this.Position = -1;

            foreach (EffectsItem Effect in EffectsList)
            {
                Effect.Value = SunDofus.Basic.GetRandomJet(Effect.Effect);
                Effect.Value2 = -1;
            }
        }

        public string SaveString()
        {
            return SunDofus.Basic.DeciToHex(BaseItem.ID) + "~" + SunDofus.Basic.DeciToHex(Quantity) + "~"
                + (Position == -1 ? "" : SunDofus.Basic.DeciToHex(Position)) + "~" + EffectsInfos();
        }

        public override string ToString()
        {
            return SunDofus.Basic.DeciToHex(ID) + "~" + SunDofus.Basic.DeciToHex(BaseItem.ID) + "~" + SunDofus.Basic.DeciToHex(Quantity) + "~" 
                + (Position == -1 ? "" : SunDofus.Basic.DeciToHex(Position)) + "~" + EffectsInfos();
        }
    }
}
