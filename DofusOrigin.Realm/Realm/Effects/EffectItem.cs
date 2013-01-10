using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Effects
{
    class EffectItem
    {
        public int m_id { get; set; }
        public int m_value { get; set; }
        public int m_value2 { get; set; }
        public int m_value3 { get; set; }

        public string m_effect { get; set; }
        public Characters.Character m_client { get; set; }

        public EffectItem()
        {
            m_value = 0;
            m_value2 = 0;
            m_value3 = 0;

            m_effect = "0d0+0";
        }

        public override string ToString()
        {
            return string.Format("{0}#{1}#{2}#{3}#{4}", Utilities.Basic.DeciToHex(m_id), (m_value <= 0 ? "" : Utilities.Basic.DeciToHex(m_value)),
                (m_value2 <= 0 ? "" : Utilities.Basic.DeciToHex(m_value2)), (m_value3 <= 0 ? "0" : Utilities.Basic.DeciToHex(m_value3)), m_effect);
        }

        public string SetString()
        {
            return string.Format("{0}#{1}#{2}", Utilities.Basic.DeciToHex(m_id), (m_value <= 0 ? "" : Utilities.Basic.DeciToHex(m_value)),
                (m_value2 <= 0 ? "" : Utilities.Basic.DeciToHex(m_value2)));
        }

        public void ParseEffect(Characters.Character _client)
        {
            m_client = _client;

            switch (m_id)
            {
                case 110: // + Vie
                    AddLife();
                    break;

                case 111: // + PA
                    AddPA();
                    break;

                case 112: // + Dommage
                    AddDamage();
                    break;

                case 115: // + Coups critiques
                    AddCritic();
                    break;

                case 116: // - PO
                    SubPO();
                    break;

                case 117: // + PO
                    AddPO();
                    break;

                case 118: // + Force
                    AddStrenght();
                    break;

                case 119: // + Agilité
                    AddAgility();
                    break;

                case 122: // + Echec
                    AddFail();
                    break;

                case 123: // + Chance
                    AddLuck();
                    break;

                case 124: // + Wisdom
                    AddWisdom();
                    break;

                case 125: // + Vitalité
                    AddLife();
                    break;

                case 126: // + Intel
                    AddIntel();
                    break;

                case 128: // + PM
                    AddPM();
                    break;

                case 138: // + %Dommage
                    AddPercentDamage();
                    break;

                case 142: // + Dommage Physique
                    AddPhysicDamage();
                    break;

                case 143: // + Dommage Magique
                    AddMagicDamage();
                    break;

                case 152: // - Chance
                    SubLuck();
                    break;

                case 153: // - Vitalité
                    SubLife();
                    break;

                case 154: // - Agilité
                    SubAgility();
                    break;

                case 155: // - Intelligence
                    SubIntelligence();
                    break;

                case 156 : // - Sagesse
                    SubWisdom();
                    break;

                case 157: // - Force
                    SubStrenght();
                    break;

                case 158: // + Pods
                    AddPods();
                    break;

                case 159: // - Pods
                    SubPods();
                    break;

                case 160: // + Esquive PA
                    AddDodgePA();
                    break;

                case 161: // + Esquive PM
                    AddDodgePM();
                    break;

                case 162: // - Esquive PA
                    SubDodgePA();
                    break;

                case 163: // - Esquive PM
                    SubDodgePM();
                    break;
                    
                case 164: // - Dommage
                    SubDamage();
                    break;

                case 168: // - PA
                    SubPA();
                    break;

                case 169: // - PM
                    SubPM();
                    break;

                case 171: // - Coups critiques
                    SubCritic();
                    break;

                case 172: // - Dommage Magique
                    SubMagic();
                    break;

                case 173: // - Dommage Physique
                    SubPhysic();
                    break;

                case 174: // + Initiative
                    AddInitiative();
                    break;

                case 175: // - Initiative
                    SubInitiative();
                    break;

                case 176: // + Prospection
                    AddProspection();
                    break;

                case 177: // - Prospection
                    SubProspection();
                    break;

                case 178: // + Soins
                    AddHeal();
                    break;

                case 179: // - Soins
                    SubHeal();
                    break;

                case 182: // + Invocations
                    AddMonster();
                    break;

                case 210: // + %Armure terre
                    AddArmorStrenght();
                    break;

                case 211: // + %Armure Chance
                    AddArmorLuck();
                    break;

                case 212: // + %Armure Agilité
                    AddArmorAgility();
                    break;

                case 213: // + %Armure Intelligence
                    AddArmorIntelligence();
                    break;

                case 214: // + %Armure Neutre
                    AddArmorNeutral();
                    break;

                case 215: // - %Armure Terre
                    SubArmorPercentStrenght();
                    break;

                case 216: // -%Armure Chance
                    SubArmorPercentLuck();
                    break;

                case 217: // - %Armure Agilité
                    SubArmorPercentAgility();
                    break;

                case 218: // - %Armure Intel
                    SubArmorPercentIntelligence();
                    break;

                case 219: // - %Armure Neutre
                    SubArmorPercentNeutral();
                    break;

                case 225: // + Dommage Piege
                    AddTrapDamage();
                    break;

                case 226: // + %Dommage Piege
                    AddTrapPercentDamage();
                    break;

                case 240: // + Armor Force
                    AddArmorStrenght();
                    break;

                case 241: // + Armor Chance
                    AddArmorLuck();
                    break;

                case 242: // + Armor Agilité
                    AddArmorAgility();
                    break;

                case 243: // + Armor Intelligence
                    AddArmorIntelligence();
                    break;

                case 244: // + Armor Neutre
                    AddArmorNeutral();
                    break;

                case 245: // - Armor Force
                    SubArmorStrenght();
                    break;

                case 246: // - Armor Chance
                    SubArmorLuck();
                    break;

                case 247: // - Armor Agilité
                    SubArmorAgility();
                    break;

                case 248: // - Armor Intel
                    SubArmorIntelligence();
                    break;

                case 249: // - Armor Neutre
                    SubArmorNeutral();
                    break;

                case 250: // + %Pvp Armor Force
                    AddArmorPvpStrenghtPercent();
                    break;

                case 251: // + %Pvp Armor Chance
                    AddArmorPvpLuckPercent();
                    break;

                case 252: // + %Pvp Armor Agilité
                    AddArmorPvpAgilityPercent();
                    break;

                case 253: // + %Pvp Armor Intel
                    AddArmorPvpIntelligencePercent();
                    break;

                case 254: // + %Pvp Armor Neutre
                    AddArmorPvpNeutralPercent();
                    break;

                case 255: // -%Pvp Armor Force
                    SubArmorPvpStrenghtPercent();
                    break;

                case 256: // - %Pvp Armor Chance
                    SubArmorPvpLuckPercent();
                    break;

                case 257: // -%Pvp Armor Agilité
                    SubArmorPvpAgilityPercent();
                    break;

                case 258: // -%Pvp Armor Intel
                    SubArmorPvpIntelligencePercent();
                    break;

                case 259: // -%Pvp Armor Neutre
                    SubArmorPvpNeutralPercent();
                    break;

                case 260: // + Pvp Armor Force
                    AddArmorPvpStrenght();
                    break;

                case 261: // + Pvp Armor Chance
                    AddArmorPvpLuck();
                    break;

                case 262: // + Pvp Armor Agilité
                    AddArmorPvpAgility();
                    break;

                case 263: // + Pvp Armor Intelligence
                    AddArmorPvpIntelligence();
                    break;

                case 264: // + Pvp Armor Neutre
                    AddArmorPvpNeutral();
                    break;
            }
        }

        #region Effects' Void

        private void AddLife()
        {
            m_client.m_stats.life.m_items += m_value;
        }

        private void SubLife()
        {
            m_client.m_stats.life.m_items -= m_value;
        }

        private void AddPA()
        {
            m_client.m_stats.PA.m_items += m_value;
        }

        private void AddPM()
        {
            m_client.m_stats.PM.m_items += m_value;
        }

        private void AddPO()
        {
            m_client.m_stats.PO.m_items += m_value;
        }

        private void AddWisdom()
        {
            m_client.m_stats.wisdom.m_items += m_value;
        }

        private void AddStrenght()
        {
            m_client.m_stats.strenght.m_items += m_value;
        }

        private void AddIntel()
        {
            m_client.m_stats.intelligence.m_items += m_value;
        }

        private void AddLuck()
        {
            m_client.m_stats.luck.m_items += m_value;
        }

        private void AddAgility()
        {
            m_client.m_stats.agility.m_items += m_value;
        }

        private void AddInitiative()
        {
            m_client.m_stats.initiative.m_items += m_value;
        }

        private void AddProspection()
        {
            m_client.m_stats.prospection.m_items += m_value;
        }

        private void AddPercentDamage()
        {
            m_client.m_stats.bonusDamagePercent.m_items += m_value;
        }

        private void AddCritic()
        {
            m_client.m_stats.bonusCritical.m_items += m_value;
        }

        private void AddMagicDamage()
        {
            m_client.m_stats.bonusDamageMagic.m_items += m_value;
        }

        private void AddPhysicDamage()
        {
            m_client.m_stats.bonusDamagePhysic.m_items += m_value;
        }

        private void AddTrapDamage()
        {
            m_client.m_stats.bonusDamageTrap.m_items += m_value;
        }

        private void AddTrapPercentDamage()
        {
            m_client.m_stats.bonusDamageTrapPercent.m_items += m_value;
        }

        private void AddFail()
        {
            m_client.m_stats.bonusFail.m_items += m_value;
        }

        private void AddDodgePA()
        {
            m_client.m_stats.dodgePA.m_items += m_value;
        }

        private void AddDodgePM()
        {
            m_client.m_stats.dodgePM.m_items += m_value;
        }

        private void AddMonster()
        {
            m_client.m_stats.maxMonsters.m_items += m_value;
        }

        private void AddPods()
        {
            m_client.m_stats.maxPods.m_items += m_value;
        }

        private void AddHeal()
        {
            m_client.m_stats.bonusHeal.m_items += m_value;
        }

        private void AddDamage()
        {
            m_client.m_stats.bonusDamage.m_items += m_value;
        }

        private void SubAgility()
        {
            m_client.m_stats.agility.m_items -= m_value;
        }

        private void SubLuck()
        {
            m_client.m_stats.luck.m_items -= m_value;
        }

        private void SubDamage()
        {
            m_client.m_stats.bonusDamage.m_items -= m_value;
        }

        private void SubCritic()
        {
            m_client.m_stats.bonusCritical.m_items -= m_value;
        }

        private void SubMagic()
        {
            m_client.m_stats.bonusDamageMagic.m_items -= m_value;
        }

        private void SubPhysic()
        {
            m_client.m_stats.bonusDamagePhysic.m_items -= m_value;
        }

        private void SubDodgePA()
        {
            m_client.m_stats.dodgePA.m_items -= m_value;
        }

        private void SubDodgePM()
        {
            m_client.m_stats.dodgePM.m_items -= m_value;
        }

        private void SubStrenght()
        {
            m_client.m_stats.strenght.m_items -= m_value;
        }

        private void SubInitiative()
        {
            m_client.m_stats.initiative.m_items -= m_value;
        }

        private void SubIntelligence()
        {
            m_client.m_stats.intelligence.m_items -= m_value;
        }

        private void SubPA()
        {
            m_client.m_stats.PA.m_items -= m_value;
        }

        private void SubPM()
        {
            m_client.m_stats.PM.m_items -= m_value;
        }

        private void SubPO()
        {
            m_client.m_stats.PO.m_items -= m_value;
        }

        private void SubPods()
        {
            m_client.m_stats.maxPods.m_items -= m_value;
        }

        private void SubProspection()
        {
            m_client.m_stats.prospection.m_items -= m_value;
        }

        private void SubWisdom()
        {
            m_client.m_stats.wisdom.m_items -= m_value;
        }

        private void SubHeal()
        {
            m_client.m_stats.bonusHeal.m_items -= m_value;
        }

        private void SubVitality()
        {
            m_client.m_stats.life.m_items -= m_value;
        }

        private void AddArmorStrenght()
        {
            m_client.m_stats.armorStrenght.m_items += m_value;
        }

        private void AddArmorNeutral()
        {
            m_client.m_stats.armorNeutral.m_items += m_value;
        }

        private void AddArmorLuck()
        {
            m_client.m_stats.armorLuck.m_items += m_value;
        }

        private void AddArmorAgility()
        {
            m_client.m_stats.armorAgility.m_items += m_value;
        }

        private void AddArmorIntelligence()
        {
            m_client.m_stats.armorIntelligence.m_items += m_value;
        }

        private void AddArmorPercentStrenght()
        {
            m_client.m_stats.armorPercentStrenght.m_items += m_value;
        }

        private void AddArmorPercentIntelligence()
        {
            m_client.m_stats.armorPercentIntelligence.m_items += m_value;
        }

        private void AddArmorPercentAgility()
        {
            m_client.m_stats.armorPercentAgility.m_items += m_value;
        }

        private void AddArmorPercentLuck()
        {
            m_client.m_stats.armorPercentLuck.m_items += m_value;
        }

        private void AddArmorPercentNeutral()
        {
            m_client.m_stats.armorPercentNeutral.m_items += m_value;
        }

        private void AddArmorPvpStrenght()
        {
            m_client.m_stats.armorPvpStrenght.m_items += m_value;
        }

        private void AddArmorPvpNeutral()
        {
            m_client.m_stats.armorPvpNeutral.m_items += m_value;
        }

        private void AddArmorPvpLuck()
        {
            m_client.m_stats.armorPvpLuck.m_items += m_value;
        }

        private void AddArmorPvpAgility()
        {
            m_client.m_stats.armorPvpAgility.m_items += m_value;
        }

        private void AddArmorPvpIntelligence()
        {
            m_client.m_stats.armorPvpIntelligence.m_items += m_value;
        }

        private void AddArmorPvpStrenghtPercent()
        {
            m_client.m_stats.armorPvpPercentStrenght.m_items += m_value;
        }

        private void AddArmorPvpIntelligencePercent()
        {
            m_client.m_stats.armorPvpPercentIntelligence.m_items += m_value;
        }

        private void AddArmorPvpAgilityPercent()
        {
            m_client.m_stats.armorPvpPercentAgility.m_items += m_value;
        }

        private void AddArmorPvpLuckPercent()
        {
            m_client.m_stats.armorPvpPercentLuck.m_items += m_value;
        }

        private void AddArmorPvpNeutralPercent()
        {
            m_client.m_stats.armorPvpPercentNeutral.m_items += m_value;
        }

        private void SubArmorPercentStrenght()
        {
            m_client.m_stats.armorPercentStrenght.m_items -= m_value;
        }

        private void SubArmorPercentLuck()
        {
            m_client.m_stats.armorPercentLuck.m_items -= m_value;
        }

        private void SubArmorPercentAgility()
        {
            m_client.m_stats.armorPercentAgility.m_items -= m_value;
        }

        private void SubArmorPercentNeutral()
        {
            m_client.m_stats.armorPercentNeutral.m_items -= m_value;
        }

        private void SubArmorPercentIntelligence()
        {
            m_client.m_stats.armorPercentIntelligence.m_items -= m_value;
        }

        private void SubArmorStrenght()
        {
            m_client.m_stats.armorStrenght.m_items -= m_value;
        }

        private void SubArmorIntelligence()
        {
            m_client.m_stats.armorIntelligence.m_items -= m_value;
        }

        private void SubArmorAgility()
        {
            m_client.m_stats.armorAgility.m_items -= m_value;
        }

        private void SubArmorLuck()
        {
            m_client.m_stats.armorLuck.m_items -= m_value;
        }

        private void SubArmorNeutral()
        {
            m_client.m_stats.armorNeutral.m_items -= m_value;
        }

        private void SubArmorPvpStrenghtPercent()
        {
            m_client.m_stats.armorPvpPercentStrenght.m_items -= m_value;
        }

        private void SubArmorPvpNeutralPercent()
        {
            m_client.m_stats.armorPvpPercentNeutral.m_items -= m_value;
        }

        private void SubArmorPvpLuckPercent()
        {
            m_client.m_stats.armorPvpPercentLuck.m_items -= m_value;
        }

        private void SubArmorPvpAgilityPercent()
        {
            m_client.m_stats.armorPvpPercentAgility.m_items -= m_value;
        }

        private void SubArmorPvpIntelligencePercent()
        {
            m_client.m_stats.armorPvpPercentIntelligence.m_items -= m_value;
        }

        #endregion
    }
}
