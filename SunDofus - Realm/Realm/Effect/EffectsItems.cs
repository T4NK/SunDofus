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
            return SunDofus.Basic.DeciToHex(ID) + "#" + (Value <= 0 ? "" : SunDofus.Basic.DeciToHex(Value)) + 
                "#" + (Value2 <= 0 ? "" : SunDofus.Basic.DeciToHex(Value2)) + "#" + (Value3 <= 0 ? "0" : SunDofus.Basic.DeciToHex(Value3)) + "#" + Effect;
        }

        public string SetString()
        {
            return SunDofus.Basic.DeciToHex(ID) + "#" + (Value <= 0 ? "" : SunDofus.Basic.DeciToHex(Value)) +
                "#" + (Value2 <= 0 ? "" : SunDofus.Basic.DeciToHex(Value2));
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

                    /*TODO :: Différencier les Dommages pièges et les %dommages */
                case 225: // + Dommage Piege (%?)
                    AddTrapDamage();
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

        #endregion
    }
}
