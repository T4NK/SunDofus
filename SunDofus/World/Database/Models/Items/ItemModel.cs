using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Database.Models.Items
{
    class ItemModel
    {
        public int ID;
        public int Type;
        public int Level;
        public int Pods;
        public int Price;
        public int Set;

        public bool isUsable;
        public bool isTwoHands;

        public string Jet;
        public string Condistr;

        public List<Realm.Effects.EffectItem> EffectsList;

        public ItemModel()
        {
            Price = 0;
            Set = -1;
            Jet = "";
            isTwoHands = false;
            isUsable = false;

            EffectsList = new List<Realm.Effects.EffectItem>();
        }

        public void ParseWeaponInfos(string datas)
        { }

        public void ParseRandomJet()
        {
            if (EffectsList.Count != 0)
                return;

            var jet = Jet;

            foreach (var _jet in jet.Split(','))
            {
                if (_jet == "") continue;
                var infos = _jet.Split('#');

                var myEffect = new Realm.Effects.EffectItem();
                myEffect.ID = Utilities.Basic.HexToDeci(infos[0]);

                if (infos.Length > 1) myEffect.Value = Utilities.Basic.HexToDeci(infos[1]);
                if (infos.Length > 2) myEffect.Value2 = Utilities.Basic.HexToDeci(infos[2]);
                if (infos.Length > 3) myEffect.Value3 = Utilities.Basic.HexToDeci(infos[3]);
                if (infos.Length > 4) myEffect.Effect = infos[4];

                lock(EffectsList)
                    EffectsList.Add(myEffect);
            }
        }

        public string EffectInfos()
        {
            return string.Join(",", EffectsList);
        }
    }
}
