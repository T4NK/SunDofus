using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Effects
{
    class EffectItem
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private int _value;

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private int _value2;

        public int Value2
        {
            get
            {
                return _value2;
            }
            set
            {
                _value2 = value;
            }
        }

        private int _value3;

        public int Value3
        {
            get
            {
                return _value3;
            }
            set
            {
                _value3 = value;
            }
        }

        private string _effect;

        public string Effect
        {
            get
            {
                return _effect;
            }
            set
            {
                _effect = value;
            }
        }

        public Characters.Character Client;

        public EffectItem()
        {
            _value = 0;
            _value2 = 0;
            _value3 = 0;

            _effect = "0d0+0";
        }

        public EffectItem(EffectItem x)
        {
            _ID = x.ID;

            _value = x.Value;
            _value2 = x.Value2;
            _value3 = x.Value3;

            _effect = x.Effect;
        }

        public override string ToString()
        {
            return string.Format("{0}#{1}#{2}#{3}#{4}", Utilities.Basic.DeciToHex(_ID), (_value <= 0 ? "" : Utilities.Basic.DeciToHex(_value)),
                (_value2 <= 0 ? "" : Utilities.Basic.DeciToHex(_value2)), (_value3 <= 0 ? "0" : Utilities.Basic.DeciToHex(_value3)), _effect);
        }

        public string SetString()
        {
            return string.Format("{0}#{1}#{2}", Utilities.Basic.DeciToHex(_ID), (_value <= 0 ? "" : Utilities.Basic.DeciToHex(_value)),
                (_value2 <= 0 ? "" : Utilities.Basic.DeciToHex(_value2)));
        }

        public void ParseEffect(Characters.Character client)
        {
            Client = client;

            switch (_ID)
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
            Client.m_stats.life.m_items += Value;
        }

        private void SubLife()
        {
            Client.m_stats.life.m_items -= Value;
        }

        private void AddPA()
        {
            Client.m_stats.PA.m_items += Value;
        }

        private void AddPM()
        {
            Client.m_stats.PM.m_items += Value;
        }

        private void AddPO()
        {
            Client.m_stats.PO.m_items += Value;
        }

        private void AddWisdom()
        {
            Client.m_stats.wisdom.m_items += Value;
        }

        private void AddStrenght()
        {
            Client.m_stats.strenght.m_items += Value;
        }

        private void AddIntel()
        {
            Client.m_stats.intelligence.m_items += Value;
        }

        private void AddLuck()
        {
            Client.m_stats.luck.m_items += Value;
        }

        private void AddAgility()
        {
            Client.m_stats.agility.m_items += Value;
        }

        private void AddInitiative()
        {
            Client.m_stats.initiative.m_items += Value;
        }

        private void AddProspection()
        {
            Client.m_stats.prospection.m_items += Value;
        }

        private void AddPercentDamage()
        {
            Client.m_stats.bonusDamagePercent.m_items += Value;
        }

        private void AddCritic()
        {
            Client.m_stats.bonusCritical.m_items += Value;
        }

        private void AddMagicDamage()
        {
            Client.m_stats.bonusDamageMagic.m_items += Value;
        }

        private void AddPhysicDamage()
        {
            Client.m_stats.bonusDamagePhysic.m_items += Value;
        }

        private void AddTrapDamage()
        {
            Client.m_stats.bonusDamageTrap.m_items += Value;
        }

        private void AddTrapPercentDamage()
        {
            Client.m_stats.bonusDamageTrapPercent.m_items += Value;
        }

        private void AddFail()
        {
            Client.m_stats.bonusFail.m_items += Value;
        }

        private void AddDodgePA()
        {
            Client.m_stats.dodgePA.m_items += Value;
        }

        private void AddDodgePM()
        {
            Client.m_stats.dodgePM.m_items += Value;
        }

        private void AddMonster()
        {
            Client.m_stats.maxMonsters.m_items += Value;
        }

        private void AddPods()
        {
            Client.m_stats.maxPods.m_items += Value;
        }

        private void AddHeal()
        {
            Client.m_stats.bonusHeal.m_items += Value;
        }

        private void AddDamage()
        {
            Client.m_stats.bonusDamage.m_items += Value;
        }

        private void SubAgility()
        {
            Client.m_stats.agility.m_items -= Value;
        }

        private void SubLuck()
        {
            Client.m_stats.luck.m_items -= Value;
        }

        private void SubDamage()
        {
            Client.m_stats.bonusDamage.m_items -= Value;
        }

        private void SubCritic()
        {
            Client.m_stats.bonusCritical.m_items -= Value;
        }

        private void SubMagic()
        {
            Client.m_stats.bonusDamageMagic.m_items -= Value;
        }

        private void SubPhysic()
        {
            Client.m_stats.bonusDamagePhysic.m_items -= Value;
        }

        private void SubDodgePA()
        {
            Client.m_stats.dodgePA.m_items -= Value;
        }

        private void SubDodgePM()
        {
            Client.m_stats.dodgePM.m_items -= Value;
        }

        private void SubStrenght()
        {
            Client.m_stats.strenght.m_items -= Value;
        }

        private void SubInitiative()
        {
            Client.m_stats.initiative.m_items -= Value;
        }

        private void SubIntelligence()
        {
            Client.m_stats.intelligence.m_items -= Value;
        }

        private void SubPA()
        {
            Client.m_stats.PA.m_items -= Value;
        }

        private void SubPM()
        {
            Client.m_stats.PM.m_items -= Value;
        }

        private void SubPO()
        {
            Client.m_stats.PO.m_items -= Value;
        }

        private void SubPods()
        {
            Client.m_stats.maxPods.m_items -= Value;
        }

        private void SubProspection()
        {
            Client.m_stats.prospection.m_items -= Value;
        }

        private void SubWisdom()
        {
            Client.m_stats.wisdom.m_items -= Value;
        }

        private void SubHeal()
        {
            Client.m_stats.bonusHeal.m_items -= Value;
        }

        private void SubVitality()
        {
            Client.m_stats.life.m_items -= Value;
        }

        private void AddArmorStrenght()
        {
            Client.m_stats.armorStrenght.m_items += Value;
        }

        private void AddArmorNeutral()
        {
            Client.m_stats.armorNeutral.m_items += Value;
        }

        private void AddArmorLuck()
        {
            Client.m_stats.armorLuck.m_items += Value;
        }

        private void AddArmorAgility()
        {
            Client.m_stats.armorAgility.m_items += Value;
        }

        private void AddArmorIntelligence()
        {
            Client.m_stats.armorIntelligence.m_items += Value;
        }

        private void AddArmorPercentStrenght()
        {
            Client.m_stats.armorPercentStrenght.m_items += Value;
        }

        private void AddArmorPercentIntelligence()
        {
            Client.m_stats.armorPercentIntelligence.m_items += Value;
        }

        private void AddArmorPercentAgility()
        {
            Client.m_stats.armorPercentAgility.m_items += Value;
        }

        private void AddArmorPercentLuck()
        {
            Client.m_stats.armorPercentLuck.m_items += Value;
        }

        private void AddArmorPercentNeutral()
        {
            Client.m_stats.armorPercentNeutral.m_items += Value;
        }

        private void AddArmorPvpStrenght()
        {
            Client.m_stats.armorPvpStrenght.m_items += Value;
        }

        private void AddArmorPvpNeutral()
        {
            Client.m_stats.armorPvpNeutral.m_items += Value;
        }

        private void AddArmorPvpLuck()
        {
            Client.m_stats.armorPvpLuck.m_items += Value;
        }

        private void AddArmorPvpAgility()
        {
            Client.m_stats.armorPvpAgility.m_items += Value;
        }

        private void AddArmorPvpIntelligence()
        {
            Client.m_stats.armorPvpIntelligence.m_items += Value;
        }

        private void AddArmorPvpStrenghtPercent()
        {
            Client.m_stats.armorPvpPercentStrenght.m_items += Value;
        }

        private void AddArmorPvpIntelligencePercent()
        {
            Client.m_stats.armorPvpPercentIntelligence.m_items += Value;
        }

        private void AddArmorPvpAgilityPercent()
        {
            Client.m_stats.armorPvpPercentAgility.m_items += Value;
        }

        private void AddArmorPvpLuckPercent()
        {
            Client.m_stats.armorPvpPercentLuck.m_items += Value;
        }

        private void AddArmorPvpNeutralPercent()
        {
            Client.m_stats.armorPvpPercentNeutral.m_items += Value;
        }

        private void SubArmorPercentStrenght()
        {
            Client.m_stats.armorPercentStrenght.m_items -= Value;
        }

        private void SubArmorPercentLuck()
        {
            Client.m_stats.armorPercentLuck.m_items -= Value;
        }

        private void SubArmorPercentAgility()
        {
            Client.m_stats.armorPercentAgility.m_items -= Value;
        }

        private void SubArmorPercentNeutral()
        {
            Client.m_stats.armorPercentNeutral.m_items -= Value;
        }

        private void SubArmorPercentIntelligence()
        {
            Client.m_stats.armorPercentIntelligence.m_items -= Value;
        }

        private void SubArmorStrenght()
        {
            Client.m_stats.armorStrenght.m_items -= Value;
        }

        private void SubArmorIntelligence()
        {
            Client.m_stats.armorIntelligence.m_items -= Value;
        }

        private void SubArmorAgility()
        {
            Client.m_stats.armorAgility.m_items -= Value;
        }

        private void SubArmorLuck()
        {
            Client.m_stats.armorLuck.m_items -= Value;
        }

        private void SubArmorNeutral()
        {
            Client.m_stats.armorNeutral.m_items -= Value;
        }

        private void SubArmorPvpStrenghtPercent()
        {
            Client.m_stats.armorPvpPercentStrenght.m_items -= Value;
        }

        private void SubArmorPvpNeutralPercent()
        {
            Client.m_stats.armorPvpPercentNeutral.m_items -= Value;
        }

        private void SubArmorPvpLuckPercent()
        {
            Client.m_stats.armorPvpPercentLuck.m_items -= Value;
        }

        private void SubArmorPvpAgilityPercent()
        {
            Client.m_stats.armorPvpPercentAgility.m_items -= Value;
        }

        private void SubArmorPvpIntelligencePercent()
        {
            Client.m_stats.armorPvpPercentIntelligence.m_items -= Value;
        }

        #endregion
    }
}
