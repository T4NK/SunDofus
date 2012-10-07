using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Effect
{
    class EffectsItems
    {
        public int ID, Value, Value2, Value3 = 0;
        public string Effect = "0d0+0";
        public Character.Character Client;

        public override string ToString()
        {
            return Utilities.Basic.DeciToHex(ID) + "#" + (Value <= 0 ? "" : Utilities.Basic.DeciToHex(Value)) +
                "#" + (Value2 <= 0 ? "" : Utilities.Basic.DeciToHex(Value2)) + "#" + (Value3 <= 0 ? "0" : Utilities.Basic.DeciToHex(Value3)) + "#" + Effect;
        }

        public string SetString()
        {
            return Utilities.Basic.DeciToHex(ID) + "#" + (Value <= 0 ? "" : Utilities.Basic.DeciToHex(Value)) +
                "#" + (Value2 <= 0 ? "" : Utilities.Basic.DeciToHex(Value2));
        }

        public void ParseEffect(Character.Character _Client)
        {
            Client = _Client;

            switch (ID)
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
            Client.m_Stats.Life.Items += Value;
        }

        private void SubLife()
        {
            Client.m_Stats.Life.Items -= Value;
        }

        private void AddPA()
        {
            Client.m_Stats.PA.Items += Value;
        }

        private void AddPM()
        {
            Client.m_Stats.PM.Items += Value;
        }

        private void AddPO()
        {
            Client.m_Stats.PO.Items += Value;
        }

        private void AddWisdom()
        {
            Client.m_Stats.Wisdom.Items += Value;
        }

        private void AddStrenght()
        {
            Client.m_Stats.Strenght.Items += Value;
        }

        private void AddIntel()
        {
            Client.m_Stats.Intelligence.Items += Value;
        }

        private void AddLuck()
        {
            Client.m_Stats.Luck.Items += Value;
        }

        private void AddAgility()
        {
            Client.m_Stats.Agility.Items += Value;
        }

        private void AddInitiative()
        {
            Client.m_Stats.Initiative.Items += Value;
        }

        private void AddProspection()
        {
            Client.m_Stats.Prospection.Items += Value;
        }

        private void AddPercentDamage()
        {
            Client.m_Stats.BonusDamagePercent.Items += Value;
        }

        private void AddCritic()
        {
            Client.m_Stats.BonusCritical.Items += Value;
        }

        private void AddMagicDamage()
        {
            Client.m_Stats.BonusDamageMagic.Items += Value;
        }

        private void AddPhysicDamage()
        {
            Client.m_Stats.BonusDamagePhysic.Items += Value;
        }

        private void AddTrapDamage()
        {
            Client.m_Stats.BonusDamageTrap.Items += Value;
        }

        private void AddTrapPercentDamage()
        {
            Client.m_Stats.BonusDamageTrapPercent.Items += Value;
        }

        private void AddFail()
        {
            Client.m_Stats.BonusFail.Items += Value;
        }

        private void AddDodgePA()
        {
            Client.m_Stats.DodgePA.Items += Value;
        }

        private void AddDodgePM()
        {
            Client.m_Stats.DodgePM.Items += Value;
        }

        private void AddMonster()
        {
            Client.m_Stats.MaxMonsters.Items += Value;
        }

        private void AddPods()
        {
            Client.m_Stats.MaxPods.Items += Value;
        }

        private void AddHeal()
        {
            Client.m_Stats.BonusHeal.Items += Value;
        }

        private void AddDamage()
        {
            Client.m_Stats.BonusDamage.Items += Value;
        }

        private void SubAgility()
        {
            Client.m_Stats.Agility.Items -= Value;
        }

        private void SubLuck()
        {
            Client.m_Stats.Luck.Items -= Value;
        }

        private void SubDamage()
        {
            Client.m_Stats.BonusDamage.Items -= Value;
        }

        private void SubCritic()
        {
            Client.m_Stats.BonusCritical.Items -= Value;
        }

        private void SubMagic()
        {
            Client.m_Stats.BonusDamageMagic.Items -= Value;
        }

        private void SubPhysic()
        {
            Client.m_Stats.BonusDamagePhysic.Items -= Value;
        }

        private void SubDodgePA()
        {
            Client.m_Stats.DodgePA.Items -= Value;
        }

        private void SubDodgePM()
        {
            Client.m_Stats.DodgePM.Items -= Value;
        }

        private void SubStrenght()
        {
            Client.m_Stats.Strenght.Items -= Value;
        }

        private void SubInitiative()
        {
            Client.m_Stats.Initiative.Items -= Value;
        }

        private void SubIntelligence()
        {
            Client.m_Stats.Intelligence.Items -= Value;
        }

        private void SubPA()
        {
            Client.m_Stats.PA.Items -= Value;
        }

        private void SubPM()
        {
            Client.m_Stats.PM.Items -= Value;
        }

        private void SubPO()
        {
            Client.m_Stats.PO.Items -= Value;
        }

        private void SubPods()
        {
            Client.m_Stats.MaxPods.Items -= Value;
        }

        private void SubProspection()
        {
            Client.m_Stats.Prospection.Items -= Value;
        }

        private void SubWisdom()
        {
            Client.m_Stats.Wisdom.Items -= Value;
        }

        private void SubHeal()
        {
            Client.m_Stats.BonusHeal.Items -= Value;
        }

        private void SubVitality()
        {
            Client.m_Stats.Life.Items -= Value;
        }

        private void AddArmorStrenght()
        {
            Client.m_Stats.ArmorStrenght.Items += Value;
        }

        private void AddArmorNeutral()
        {
            Client.m_Stats.ArmorNeutral.Items += Value;
        }

        private void AddArmorLuck()
        {
            Client.m_Stats.ArmorLuck.Items += Value;
        }

        private void AddArmorAgility()
        {
            Client.m_Stats.ArmorAgility.Items += Value;
        }

        private void AddArmorIntelligence()
        {
            Client.m_Stats.ArmorIntelligence.Items += Value;
        }

        private void AddArmorPercentStrenght()
        {
            Client.m_Stats.ArmorPercentStrenght.Items += Value;
        }

        private void AddArmorPercentIntelligence()
        {
            Client.m_Stats.ArmorPercentIntelligence.Items += Value;
        }

        private void AddArmorPercentAgility()
        {
            Client.m_Stats.ArmorPercentAgility.Items += Value;
        }

        private void AddArmorPercentLuck()
        {
            Client.m_Stats.ArmorPercentLuck.Items += Value;
        }

        private void AddArmorPercentNeutral()
        {
            Client.m_Stats.ArmorPercentNeutral.Items += Value;
        }

        private void AddArmorPvpStrenght()
        {
            Client.m_Stats.ArmorPvpStrenght.Items += Value;
        }

        private void AddArmorPvpNeutral()
        {
            Client.m_Stats.ArmorPvpNeutral.Items += Value;
        }

        private void AddArmorPvpLuck()
        {
            Client.m_Stats.ArmorPvpLuck.Items += Value;
        }

        private void AddArmorPvpAgility()
        {
            Client.m_Stats.ArmorPvpAgility.Items += Value;
        }

        private void AddArmorPvpIntelligence()
        {
            Client.m_Stats.ArmorPvpIntelligence.Items += Value;
        }

        private void AddArmorPvpStrenghtPercent()
        {
            Client.m_Stats.ArmorPvpPercentStrenght.Items += Value;
        }

        private void AddArmorPvpIntelligencePercent()
        {
            Client.m_Stats.ArmorPvpPercentIntelligence.Items += Value;
        }

        private void AddArmorPvpAgilityPercent()
        {
            Client.m_Stats.ArmorPvpPercentAgility.Items += Value;
        }

        private void AddArmorPvpLuckPercent()
        {
            Client.m_Stats.ArmorPvpPercentLuck.Items += Value;
        }

        private void AddArmorPvpNeutralPercent()
        {
            Client.m_Stats.ArmorPvpPercentNeutral.Items += Value;
        }

        private void SubArmorPercentStrenght()
        {
            Client.m_Stats.ArmorPercentStrenght.Items -= Value;
        }

        private void SubArmorPercentLuck()
        {
            Client.m_Stats.ArmorPercentLuck.Items -= Value;
        }

        private void SubArmorPercentAgility()
        {
            Client.m_Stats.ArmorPercentAgility.Items -= Value;
        }

        private void SubArmorPercentNeutral()
        {
            Client.m_Stats.ArmorPercentNeutral.Items -= Value;
        }

        private void SubArmorPercentIntelligence()
        {
            Client.m_Stats.ArmorPercentIntelligence.Items -= Value;
        }

        private void SubArmorStrenght()
        {
            Client.m_Stats.ArmorStrenght.Items -= Value;
        }

        private void SubArmorIntelligence()
        {
            Client.m_Stats.ArmorIntelligence.Items -= Value;
        }

        private void SubArmorAgility()
        {
            Client.m_Stats.ArmorAgility.Items -= Value;
        }

        private void SubArmorLuck()
        {
            Client.m_Stats.ArmorLuck.Items -= Value;
        }

        private void SubArmorNeutral()
        {
            Client.m_Stats.ArmorNeutral.Items -= Value;
        }

        private void SubArmorPvpStrenghtPercent()
        {
            Client.m_Stats.ArmorPvpPercentStrenght.Items -= Value;
        }

        private void SubArmorPvpNeutralPercent()
        {
            Client.m_Stats.ArmorPvpPercentNeutral.Items -= Value;
        }

        private void SubArmorPvpLuckPercent()
        {
            Client.m_Stats.ArmorPvpPercentLuck.Items -= Value;
        }

        private void SubArmorPvpAgilityPercent()
        {
            Client.m_Stats.ArmorPvpPercentAgility.Items -= Value;
        }

        private void SubArmorPvpIntelligencePercent()
        {
            Client.m_Stats.ArmorPvpPercentIntelligence.Items -= Value;
        }

        #endregion
    }
}
