using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.NPC
{
    class NPCsAnswer
    {
        public int m_answerID { get; set; }
        public string m_effects { get; set; }

        public List<Realm.World.Conditions.NPCConditions> m_conditions;

        public NPCsAnswer()
        {
            m_conditions = new List<Realm.World.Conditions.NPCConditions>();
        }

        public void ApplyEffects(Realm.Characters.Character character)
        {
            try
            {
                foreach (var effect in m_effects.Split('|'))
                {
                    var infos = effect.Split(';');
                    Realm.Effects.EffectAction.ParseEffect(character, int.Parse(infos[0]), infos[1]);
                }
            }
            catch { }
        }

        public bool HasConditions(Realm.Characters.Character _character)
        {
            foreach (var condi in m_conditions)
            {
                if (condi.HasCondition(_character))
                    continue;
                else
                    return false;
            }

            return true;
        }
    }
}
