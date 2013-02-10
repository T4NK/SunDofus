using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Items
{
    class ItemModel
    {
        public int m_id { get; set; }
        public int m_type { get; set; }
        public int m_level { get; set; }
        public int m_pods { get; set; }
        public int m_price { get; set; }
        public int m_set { get; set; }

        public bool isUsable { get; set; }
        public bool isTwoHands { get; set; }

        public string m_jet { get; set; }
        public string m_condistr { get; set; }

        public List<Realm.Effects.EffectItem> m_effectsList = new List<Realm.Effects.EffectItem>();

        public ItemModel()
        {
            m_price = 0;
            m_set = -1;
            m_jet = "";
            isTwoHands = false;
            isUsable = false;
        }

        public void ParseWeaponInfos(string _datas)
        { }

        public void ParseRandomJet()
        {
            if (m_effectsList.Count != 0)
                return;

            var jet = m_jet;

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

                m_effectsList.Add(myEffect);
            }
        }

        public string EffectInfos()
        {
            return string.Join(",", m_effectsList);
        }
    }
}
