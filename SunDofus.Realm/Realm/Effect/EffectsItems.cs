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
            return string.Format("{0}#{1}#{2}#{3}#{4}", Utilities.Basic.DeciToHex(ID), (Value <= 0 ? "" : Utilities.Basic.DeciToHex(Value)),
                (Value2 <= 0 ? "" : Utilities.Basic.DeciToHex(Value2)), (Value3 <= 0 ? "0" : Utilities.Basic.DeciToHex(Value3)), Effect);
        }

        public string SetString()
        {
            return string.Format("{0}#{1}#{2}", Utilities.Basic.DeciToHex(ID), (Value <= 0 ? "" : Utilities.Basic.DeciToHex(Value)),
                (Value2 <= 0 ? "" : Utilities.Basic.DeciToHex(Value2)));
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
            Client.myStats.Life.Items += Value;
        }

        private void SubLife()
        {
            Client.myStats.Life.Items -= Value;
        }

        private void AddPA()
        {
            Client.myStats.PA.Items += Value;
        }

        private void AddPM()
        {
            Client.myStats.PM.Items += Value;
        }

        private void AddPO()
        {
            Client.myStats.PO.Items += Value;
        }

        private void AddWisdom()
        {
            Client.myStats.Wisdom.Items += Value;
        }

        private void AddStrenght()
        {
            Client.myStats.Strenght.Items += Value;
        }

        private void AddIntel()
        {
            Client.myStats.Intelligence.Items += Value;
        }

        private void AddLuck()
        {
            Client.myStats.Luck.Items += Value;
        }

        private void AddAgility()
        {
            Client.myStats.Agility.Items += Value;
        }

        private void AddInitiative()
        {
            Client.myStats.Initiative.Items += Value;
        }

        private void AddProspection()
        {
            Client.myStats.Prospection.Items += Value;
        }

        private void AddPercentDamage()
        {
            Client.myStats.BonusDamagePercent.Items += Value;
        }

        private void AddCritic()
        {
            Client.myStats.BonusCritical.Items += Value;
        }

        private void AddMagicDamage()
        {
            Client.myStats.BonusDamageMagic.Items += Value;
        }

        private void AddPhysicDamage()
        {
            Client.myStats.BonusDamagePhysic.Items += Value;
        }

        private void AddTrapDamage()
        {
            Client.myStats.BonusDamageTrap.Items += Value;
        }

        private void AddTrapPercentDamage()
        {
            Client.myStats.BonusDamageTrapPercent.Items += Value;
        }

        private void AddFail()
        {
            Client.myStats.BonusFail.Items += Value;
        }

        private void AddDodgePA()
        {
            Client.myStats.DodgePA.Items += Value;
        }

        private void AddDodgePM()
        {
            Client.myStats.DodgePM.Items += Value;
        }

        private void AddMonster()
        {
            Client.myStats.MaxMonsters.Items += Value;
        }

        private void AddPods()
        {
            Client.myStats.MaxPods.Items += Value;
        }

        private void AddHeal()
        {
            Client.myStats.BonusHeal.Items += Value;
        }

        private void AddDamage()
        {
            Client.myStats.BonusDamage.Items += Value;
        }

        private void SubAgility()
        {
            Client.myStats.Agility.Items -= Value;
        }

        private void SubLuck()
        {
            Client.myStats.Luck.Items -= Value;
        }

        private void SubDamage()
        {
            Client.myStats.BonusDamage.Items -= Value;
        }

        private void SubCritic()
        {
            Client.myStats.BonusCritical.Items -= Value;
        }

        private void SubMagic()
        {
            Client.myStats.BonusDamageMagic.Items -= Value;
        }

        private void SubPhysic()
        {
            Client.myStats.BonusDamagePhysic.Items -= Value;
        }

        private void SubDodgePA()
        {
            Client.myStats.DodgePA.Items -= Value;
        }

        private void SubDodgePM()
        {
            Client.myStats.DodgePM.Items -= Value;
        }

        private void SubStrenght()
        {
            Client.myStats.Strenght.Items -= Value;
        }

        private void SubInitiative()
        {
            Client.myStats.Initiative.Items -= Value;
        }

        private void SubIntelligence()
        {
            Client.myStats.Intelligence.Items -= Value;
        }

        private void SubPA()
        {
            Client.myStats.PA.Items -= Value;
        }

        private void SubPM()
        {
            Client.myStats.PM.Items -= Value;
        }

        private void SubPO()
        {
            Client.myStats.PO.Items -= Value;
        }

        private void SubPods()
        {
            Client.myStats.MaxPods.Items -= Value;
        }

        private void SubProspection()
        {
            Client.myStats.Prospection.Items -= Value;
        }

        private void SubWisdom()
        {
            Client.myStats.Wisdom.Items -= Value;
        }

        private void SubHeal()
        {
            Client.myStats.BonusHeal.Items -= Value;
        }

        private void SubVitality()
        {
            Client.myStats.Life.Items -= Value;
        }

        private void AddArmorStrenght()
        {
            Client.myStats.ArmorStrenght.Items += Value;
        }

        private void AddArmorNeutral()
        {
            Client.myStats.ArmorNeutral.Items += Value;
        }

        private void AddArmorLuck()
        {
            Client.myStats.ArmorLuck.Items += Value;
        }

        private void AddArmorAgility()
        {
            Client.myStats.ArmorAgility.Items += Value;
        }

        private void AddArmorIntelligence()
        {
            Client.myStats.ArmorIntelligence.Items += Value;
        }

        private void AddArmorPercentStrenght()
        {
            Client.myStats.ArmorPercentStrenght.Items += Value;
        }

        private void AddArmorPercentIntelligence()
        {
            Client.myStats.ArmorPercentIntelligence.Items += Value;
        }

        private void AddArmorPercentAgility()
        {
            Client.myStats.ArmorPercentAgility.Items += Value;
        }

        private void AddArmorPercentLuck()
        {
            Client.myStats.ArmorPercentLuck.Items += Value;
        }

        private void AddArmorPercentNeutral()
        {
            Client.myStats.ArmorPercentNeutral.Items += Value;
        }

        private void AddArmorPvpStrenght()
        {
            Client.myStats.ArmorPvpStrenght.Items += Value;
        }

        private void AddArmorPvpNeutral()
        {
            Client.myStats.ArmorPvpNeutral.Items += Value;
        }

        private void AddArmorPvpLuck()
        {
            Client.myStats.ArmorPvpLuck.Items += Value;
        }

        private void AddArmorPvpAgility()
        {
            Client.myStats.ArmorPvpAgility.Items += Value;
        }

        private void AddArmorPvpIntelligence()
        {
            Client.myStats.ArmorPvpIntelligence.Items += Value;
        }

        private void AddArmorPvpStrenghtPercent()
        {
            Client.myStats.ArmorPvpPercentStrenght.Items += Value;
        }

        private void AddArmorPvpIntelligencePercent()
        {
            Client.myStats.ArmorPvpPercentIntelligence.Items += Value;
        }

        private void AddArmorPvpAgilityPercent()
        {
            Client.myStats.ArmorPvpPercentAgility.Items += Value;
        }

        private void AddArmorPvpLuckPercent()
        {
            Client.myStats.ArmorPvpPercentLuck.Items += Value;
        }

        private void AddArmorPvpNeutralPercent()
        {
            Client.myStats.ArmorPvpPercentNeutral.Items += Value;
        }

        private void SubArmorPercentStrenght()
        {
            Client.myStats.ArmorPercentStrenght.Items -= Value;
        }

        private void SubArmorPercentLuck()
        {
            Client.myStats.ArmorPercentLuck.Items -= Value;
        }

        private void SubArmorPercentAgility()
        {
            Client.myStats.ArmorPercentAgility.Items -= Value;
        }

        private void SubArmorPercentNeutral()
        {
            Client.myStats.ArmorPercentNeutral.Items -= Value;
        }

        private void SubArmorPercentIntelligence()
        {
            Client.myStats.ArmorPercentIntelligence.Items -= Value;
        }

        private void SubArmorStrenght()
        {
            Client.myStats.ArmorStrenght.Items -= Value;
        }

        private void SubArmorIntelligence()
        {
            Client.myStats.ArmorIntelligence.Items -= Value;
        }

        private void SubArmorAgility()
        {
            Client.myStats.ArmorAgility.Items -= Value;
        }

        private void SubArmorLuck()
        {
            Client.myStats.ArmorLuck.Items -= Value;
        }

        private void SubArmorNeutral()
        {
            Client.myStats.ArmorNeutral.Items -= Value;
        }

        private void SubArmorPvpStrenghtPercent()
        {
            Client.myStats.ArmorPvpPercentStrenght.Items -= Value;
        }

        private void SubArmorPvpNeutralPercent()
        {
            Client.myStats.ArmorPvpPercentNeutral.Items -= Value;
        }

        private void SubArmorPvpLuckPercent()
        {
            Client.myStats.ArmorPvpPercentLuck.Items -= Value;
        }

        private void SubArmorPvpAgilityPercent()
        {
            Client.myStats.ArmorPvpPercentAgility.Items -= Value;
        }

        private void SubArmorPvpIntelligencePercent()
        {
            Client.myStats.ArmorPvpPercentIntelligence.Items -= Value;
        }

        #endregion
    }
}
