using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm;
using DofusOrigin.Realm.Characters;

namespace DofusOrigin.Database.Models.Items
{
    class ItemUsableModel
    {
        public int m_base { get; set; }
        public string m_args { get; set; }
        public bool m_mustDelete { get; set; }

        public ItemUsableModel()
        {
            m_base = -1;
            m_args = "";
            m_mustDelete = true;
        }

        public void AttributeItem()
        {
            if (Database.Cache.ItemsCache.m_itemsList.Any(x => x.m_id == m_base))
                Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == m_base).isUsable = true;
        }

        public void ParseEffect(Character _client)
        {
            var datas = m_args.Split('|');

            foreach (var effect in datas)
            {
                var infos = effect.Split(';');
                Realm.Effects.EffectAction.ParseEffect(_client, int.Parse(infos[0]), infos[1]);
            }
        }
    }
}
