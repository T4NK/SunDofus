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
    }
}
