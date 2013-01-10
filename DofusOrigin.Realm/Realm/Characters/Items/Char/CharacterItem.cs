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
            m_position = -1;
        }

        public string EffectsInfos()
        {
            return string.Join(",", m_effectsList);
        }

        public void ParseJet()
        {
            var jet = m_base.m_jet;

            foreach (var m_jet in jet.Split(','))
            {
                if (m_jet == "") continue;
                var infos = m_jet.Split('#');

                var myEffect = new Effects.EffectItem();
                myEffect.m_id = Utilities.Basic.HexToDeci(infos[0]);
                if (infos.Length > 1) myEffect.m_value = Utilities.Basic.HexToDeci(infos[1]);
                if (infos.Length > 2) myEffect.m_value2 = Utilities.Basic.HexToDeci(infos[2]);
                if (infos.Length > 3) myEffect.m_value3 = Utilities.Basic.HexToDeci(infos[3]);
                if (infos.Length > 4) myEffect.m_effect = infos[4];

                m_effectsList.Add(myEffect);
            }
        }

        public void GeneratItem(int _jet = 3)
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
