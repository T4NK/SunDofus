using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.Effects;

namespace DofusOrigin.Database.Models.Items
{
    class SetModel
    {
        public int m_id { get; set; }

        public Dictionary<int, List<EffectItem>> m_bonusList { get; set; }
        public List<int> m_itemsList { get; set; }

        public SetModel()
        {
            m_id = -1;
            m_itemsList = new List<int>();
            m_bonusList = new Dictionary<int, List<EffectItem>>();
        }

        public void ParseItems(string _datas)
        {
            if (_datas == "") 
                return;

            foreach (var infos in _datas.Split(','))
            {
                var id = int.Parse(infos.Trim());

                if (Database.Cache.ItemsCache.m_itemsList.Any(x => x.m_id == id))
                    Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == id).m_set = this.m_id;

                m_itemsList.Add(m_id);
            }
        }

        public void ParseBonus(string _datas)
        {
            var num = 1;

            if (_datas == "") 
                return;

            foreach (var infos in _datas.Split(';'))
            {
                if (infos == "") 
                    continue;

                m_bonusList.Add(++num, new List<Realm.Effects.EffectItem>());

                foreach (var datas in infos.Split(','))
                {
                    if (datas == "") 
                        continue;

                    var bonus = new Realm.Effects.EffectItem();
                    bonus.m_id = int.Parse(datas.Split(':')[0]);
                    bonus.m_value = int.Parse(datas.Split(':')[1]);

                    m_bonusList[num].Add(bonus);
                }
            }
        }
    }
}
