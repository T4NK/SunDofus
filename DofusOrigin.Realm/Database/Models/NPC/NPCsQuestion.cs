using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.NPC
{
    class NPCsQuestion
    {
        public int QuestionID;
        public int RescueQuestionID;

        public NPCsQuestion RescueQuestion;
        public List<NPCsAnswer> Answers; 

        public List<Realm.World.Conditions.NPCConditions> Conditions;

        public NPCsQuestion()
        {
            Answers = new List<NPCsAnswer>();
            Conditions = new List<Realm.World.Conditions.NPCConditions>();
        }

        public bool HasConditions(Realm.Characters.Character _character)
        {
            foreach (var condi in Conditions)
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
