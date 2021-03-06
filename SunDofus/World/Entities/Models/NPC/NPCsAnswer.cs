﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.World.Entities.Models.NPC
{
    class NPCsAnswer
    {
        public int AnswerID;
        public string Effects;

        public List<Realm.World.Conditions.NPCConditions> Conditions;

        public NPCsAnswer()
        {
            Conditions = new List<Realm.World.Conditions.NPCConditions>();
        }

        public void ApplyEffects(Realm.Characters.Character character)
        {
            try
            {
                foreach (var effect in Effects.Split('|'))
                {
                    var infos = effect.Split(';');
                    Realm.Effects.EffectAction.ParseEffect(character, int.Parse(infos[0]), infos[1]);
                }
            }
            catch { }
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
