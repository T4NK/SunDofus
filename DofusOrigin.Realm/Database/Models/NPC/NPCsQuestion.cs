using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.NPC
{
    class NPCsQuestion
    {
        public int m_questionID { get; set; }
        public int m_rescueQuestionID { get; set; }

        public NPCsQuestion m_rescueQuestion { get; set; }

        public List<NPCsAnswer> m_answers;
        public List<Realm.World.Conditions.NPCConditions> m_conditions;

        public NPCsQuestion()
        {
            m_answers = new List<NPCsAnswer>();
            m_conditions = new List<Realm.World.Conditions.NPCConditions>();
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
