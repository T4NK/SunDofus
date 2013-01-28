using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Database.Models.Items;

namespace DofusOrigin.Realm.Characters.Items
{
    class CharacterItem
    {
        public int m_id { get; set; }
        public int m_position { get; set; }
        public int m_quantity { get; set; }

        public ItemModel m_base { get; set; }
        public List<Effects.EffectItem> m_effectsList { get; set; }

        public CharacterItem(ItemModel _base)
        {
            m_base = _base;

            m_effectsList = new List<Effects.EffectItem>();
            m_base.m_effectsList.ForEach(x => m_effectsList.Add(new Effects.EffectItem(x)));

            m_position = -1;
        }

        public string EffectsInfos()
        {
            return string.Join(",", m_effectsList);
        }

        public void GeneratItem(int _jet = 4)
        {
            this.m_quantity = 1;
            this.m_position = -1;

            foreach (var Effect in m_effectsList)
                GetNewJet(Effect, _jet);
        }

        public void GetNewJet(Effects.EffectItem _effect, int _jet = 3)
        {
            if (_effect.m_id == 91 | _effect.m_id == 92 | _effect.m_id == 93 | _effect.m_id == 94 | _effect.m_id == 95 | _effect.m_id == 96 | _effect.m_id == 97 | _effect.m_id == 98 | _effect.m_id == 99 | _effect.m_id == 100 | _effect.m_id == 101) { }
            else if (_effect.m_id == 800)
            {
                _effect.m_value3 = 10; // PDV Des familiers !
            }
            else
            {
                _effect.m_value = Utilities.Basic.GetRandomJet(_effect.m_effect, _jet);
                _effect.m_value2 = -1;
            }
        }

        public CharacterItem Copy()
        {
            var newItem = new CharacterItem(m_base);
            m_effectsList.ForEach(x => newItem.m_effectsList.Add(new Effects.EffectItem(x)));

            newItem.m_position = m_position;
            newItem.m_quantity = m_quantity;

            return newItem;
        }

        public string SaveString()
        {
            return string.Format("{0}~{1}~{2}~{3}", Utilities.Basic.DeciToHex(m_base.m_id), Utilities.Basic.DeciToHex(m_quantity),
                (m_position == -1 ? "" : Utilities.Basic.DeciToHex(m_position)), EffectsInfos());
        }

        public override string ToString()
        {
            return string.Format("{0}~{1}~{2}~{3}~{4}",Utilities.Basic.DeciToHex(m_id), Utilities.Basic.DeciToHex(m_base.m_id),
                Utilities.Basic.DeciToHex(m_quantity), (m_position == -1 ? "" : Utilities.Basic.DeciToHex(m_position)), EffectsInfos());
        }
    }
}
