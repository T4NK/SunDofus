using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace realm.Realm.Character
{
    class Character
    {
        public string m_Name = "";
        public int ID, Color, Color2, Color3, Class, Sex, Skin, Size, Level, MapID, MapCell, Dir = -1;
        public bool NewCharacter, isConnected = false;

        public int Exp = 0;
        public int Kamas = 0;
        public int CharactPoint = 0;
        public int SpellPoint = 0;
        public int Energy = 10000;

        public int MaximumLife = 55;
        public int Life = 55;

        public int Pods = 0;

        public Stats.Stats m_Stats = new Stats.Stats();

        public Client.RealmClient Client;
        public CharacterState State;

        public string Channel = "*#$p%i:?!";

        #region Pattern

        public string PatternList()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(ID + ";");
            Builder.Append(m_Name + ";");
            Builder.Append(Level + ";"); // Level
            Builder.Append(Skin + ";");
            Builder.Append(Basic.DeciToHex(Color) + ";");
            Builder.Append(Basic.DeciToHex(Color2) + ";");
            Builder.Append(Basic.DeciToHex(Color3) + ";");
            Builder.Append(",,,,,;"); // Items
            Builder.Append("0;" + Program.m_ServerID + ";;;");

            return Builder.ToString();
        }

        public string PatternSelect()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("|" + ID + "|");
            Builder.Append(m_Name + "|");
            Builder.Append(Level + "|"); // Level
            Builder.Append(Class + "|");
            Builder.Append(Skin + "|");
            Builder.Append(Basic.DeciToHex(Color) + "|");
            Builder.Append(Basic.DeciToHex(Color2) + "|");
            Builder.Append(Basic.DeciToHex(Color3) + "|");
            Builder.Append(""); // Items

            return Builder.ToString();
        }

        public string PatternDisplayChar()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(MapCell + ";");
            Builder.Append(Dir + ";0;");
            Builder.Append(ID + ";");
            Builder.Append(m_Name + ";");
            Builder.Append(Class + ";");
            Builder.Append(Skin + "^" + Size + ";");
            Builder.Append(Sex + ";0,0,0," + (Level + ID) + ";"); // Sex + Alignment
            Builder.Append(SunDofus.Basic.DeciToHex(Color) + ";");
            Builder.Append(SunDofus.Basic.DeciToHex(Color2) + ";");
            Builder.Append(SunDofus.Basic.DeciToHex(Color3) + ";");
            Builder.Append(",,,,,;"); // Items
            Builder.Append("0;"); //Aura
            Builder.Append(";;");
            Builder.Append(";"); // Guild
            Builder.Append(";0;");
            Builder.Append(";"); // Mount

            return Builder.ToString();
        }

        #endregion

        #region Map

        public void LoadMap()
        {
            Map.Map m_M = Database.Data.MapSql.MapsList.First(x => x.id == this.MapID);
            if (m_M == null) return;

            if (m_M.key == "")
            {
                Client.Send("GDM|" + m_M.id + "|" + m_M.date);
            }
            else
            {
                Client.Send("GDM|" + m_M.id + "|" + m_M.date + "|" + m_M.key);
            }
        }

        public void TeleportNewMap(int m_MID, int m_C)
        {
            Client.Send("GA;2;" + ID + ";");

            GetMap().DelPlayer(this);
            Map.Map m_M = Database.Data.MapSql.MapsList.First(x => x.id == m_MID);

            MapID = m_M.id;
            MapCell = m_C;

            LoadMap();
        }

        public Map.Map GetMap()
        {
            return Database.Data.MapSql.MapsList.First(x => x.id == this.MapID);
        }

        #endregion

        #region Stats

        public void SendCharStats()
        {
            UpdateStats();
            Client.Send("As" + this.ToString());
        }

        public void SendPods()
        {
            Client.Send("Ow" + Pods + "|" + m_Stats.MaxPods.Total());
        }

        public void ResetBonus()
        {
            m_Stats.Vitalite.Boosts = 0;
            m_Stats.Sagesse.Boosts = 0;
            m_Stats.Force.Boosts = 0;
            m_Stats.Intelligence.Boosts = 0;
            m_Stats.Chance.Boosts = 0;
            m_Stats.Agilite.Boosts = 0;
        }

        public void ResetItemsStats()
        {
            m_Stats.Vitalite.Items = 0;
            m_Stats.Sagesse.Items = 0;
            m_Stats.Force.Items = 0;
            m_Stats.Intelligence.Items = 0;
            m_Stats.Chance.Items = 0;
            m_Stats.Agilite.Items = 0;
        }

        public void ResetDons()
        {
            m_Stats.Vitalite.Dons = 0;
            m_Stats.Sagesse.Dons = 0;
            m_Stats.Force.Dons = 0;
            m_Stats.Intelligence.Dons = 0;
            m_Stats.Chance.Dons = 0;
            m_Stats.Agilite.Dons = 0;
        }

        public void ResetStats()
        {
            m_Stats.Vitalite.Bases = 0;
            m_Stats.Sagesse.Bases = 0;
            m_Stats.Force.Bases = 0;
            m_Stats.Intelligence.Bases = 0;
            m_Stats.Chance.Bases = 0;
            m_Stats.Agilite.Bases = 0;
        }

        public void UpdateStats()
        {
            m_Stats.DodgePA.Bases = 0;
            m_Stats.DodgePM.Bases = 0;
            m_Stats.Prospection.Bases = 0;
            m_Stats.Initiative.Bases = 0;
            m_Stats.MaxPods.Bases = 1000;

            m_Stats.DodgePA.Bases = (m_Stats.Sagesse.Total() / 4);
            m_Stats.DodgePM.Bases = (m_Stats.Sagesse.Total() / 4);
            m_Stats.Prospection.Bases = (m_Stats.Chance.Total() / 4);
            m_Stats.Initiative.Bases = (MaximumLife / 4 + m_Stats.Initiative.Total()) * (Life / MaximumLife);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(Exp).Append(",");
            Builder.Append("0,1500|"); // Last MaxExpLevel , This MaxExpLevel
            Builder.Append(Kamas).Append("|");
            Builder.Append(CharactPoint).Append("|");
            Builder.Append(SpellPoint).Append("|");
            Builder.Append("0~2,0,0,0,0,0|"); // Alignement
            Builder.Append(Life).Append(",");
            Builder.Append(MaximumLife).Append("|");
            Builder.Append(Energy).Append(",10000|");
            Builder.Append(m_Stats.Initiative.Total()).Append("|");
            Builder.Append(m_Stats.Prospection.Total()).Append("|");

            Builder.Append(m_Stats.PA.ToString()).Append("|");
            Builder.Append(m_Stats.PM.ToString()).Append("|");
            Builder.Append(m_Stats.Force.ToString()).Append("|");
            Builder.Append(m_Stats.Vitalite.ToString()).Append("|");
            Builder.Append(m_Stats.Sagesse.ToString()).Append("|");
            Builder.Append(m_Stats.Chance.ToString()).Append("|");
            Builder.Append(m_Stats.Agilite.ToString()).Append("|");
            Builder.Append(m_Stats.Intelligence.ToString()).Append("|");
            Builder.Append(m_Stats.PO.ToString()).Append("|");
            Builder.Append(m_Stats.MaxMonsters.ToString()).Append("|");
            Builder.Append(m_Stats.BonusDamage.ToString()).Append("|");
            Builder.Append(m_Stats.BonusDamagePhysic.ToString()).Append("|");
            Builder.Append(m_Stats.BonusDamageMagic.ToString()).Append("|");
            Builder.Append(m_Stats.BonusDamagePercent.ToString()).Append("|");
            Builder.Append(m_Stats.BonusHeal.ToString()).Append("|");
            Builder.Append(m_Stats.BonusDamageTrap.ToString()).Append("|");
            Builder.Append(m_Stats.BonusDamageTrapPercent.ToString()).Append("|");
            Builder.Append(m_Stats.ReturnDamage.ToString()).Append("|");
            Builder.Append(m_Stats.BonusCritical.ToString()).Append("|");
            Builder.Append(m_Stats.BonusFail.ToString()).Append("|");
            Builder.Append(m_Stats.DodgePA.ToString()).Append("|");
            Builder.Append(m_Stats.DodgePM.ToString()).Append("|");
            Builder.Append("0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|1"); // Resist

            return Builder.ToString();
        }

        #endregion
    }
}
