using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Character.Stats
{
    class Stats
    {
        public AbstractStats Life = new AbstractStats();
        public AbstractStats Wisdom = new AbstractStats();
        public AbstractStats Strenght = new AbstractStats();
        public AbstractStats Intelligence = new AbstractStats();
        public AbstractStats Luck = new AbstractStats();
        public AbstractStats Agility = new AbstractStats();

        public AbstractStats PA = new AbstractStats();
        public AbstractStats PM = new AbstractStats();
        public AbstractStats PO = new AbstractStats();

        public AbstractStats MaxPods = new AbstractStats();

        public AbstractStats MaxMonsters = new AbstractStats();

        public AbstractStats BonusDamage = new AbstractStats();
        public AbstractStats BonusDamagePhysic = new AbstractStats();
        public AbstractStats BonusDamageMagic = new AbstractStats();
        public AbstractStats BonusDamagePercent = new AbstractStats();
        public AbstractStats BonusHeal = new AbstractStats();
        public AbstractStats BonusDamageTrap = new AbstractStats();
        public AbstractStats BonusDamageTrapPercent = new AbstractStats();
        public AbstractStats BonusCritical = new AbstractStats();
        public AbstractStats BonusFail = new AbstractStats();

        public AbstractStats ReturnDamage = new AbstractStats();

        public AbstractStats Initiative = new AbstractStats();
        public AbstractStats Prospection = new AbstractStats();
        public AbstractStats DodgePA = new AbstractStats();
        public AbstractStats DodgePM = new AbstractStats();

        public AbstractStats ArmorNeutral = new AbstractStats();
        public AbstractStats ArmorPercentNeutral = new AbstractStats();
        public AbstractStats ArmorPvpNeutral = new AbstractStats();
        public AbstractStats ArmorPvpPercentNeutral = new AbstractStats();

        public AbstractStats ArmorStrenght = new AbstractStats();
        public AbstractStats ArmorPercentStrenght = new AbstractStats();
        public AbstractStats ArmorPvpStrenght = new AbstractStats();
        public AbstractStats ArmorPvpPercentStrenght = new AbstractStats();

        public AbstractStats ArmorLuck = new AbstractStats();
        public AbstractStats ArmorPercentLuck = new AbstractStats();
        public AbstractStats ArmorPvpLuck = new AbstractStats();
        public AbstractStats ArmorPvpPercentLuck = new AbstractStats();

        public AbstractStats ArmorAgility = new AbstractStats();
        public AbstractStats ArmorPercentAgility = new AbstractStats();
        public AbstractStats ArmorPvpAgility = new AbstractStats();
        public AbstractStats ArmorPvpPercentAgility = new AbstractStats();

        public AbstractStats ArmorIntelligence = new AbstractStats();
        public AbstractStats ArmorPercentIntelligence = new AbstractStats();
        public AbstractStats ArmorPvpIntelligence = new AbstractStats();
        public AbstractStats ArmorPvpPercentIntelligence = new AbstractStats();
    }
}
