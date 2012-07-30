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
        public int Life = 0;

        public int Pods = 0;

        public Stats.Stats m_Stats = new Stats.Stats();
        public Items.InventaryItems m_Items;

        public Client.RealmClient Client;
        public CharacterState State;

        public string Channel = "*#$p%i:?!";

        public Character()
        {
            m_Items = new Items.InventaryItems(this);
        }

        #region Items

        public string GetItemsPos()
        {
            string m = "";

            if (m_Items.ItemsList.Any(x => x.Position == 1))
                m += SunDofus.Basic.DeciToHex(m_Items.ItemsList.First(x => x.Position == 1).BaseItem.ID);
            m += ",";

            if (m_Items.ItemsList.Any(x => x.Position == 6))
                m += SunDofus.Basic.DeciToHex(m_Items.ItemsList.First(x => x.Position == 6).BaseItem.ID);
            m += ",";

            if (m_Items.ItemsList.Any(x => x.Position == 7))
                m += SunDofus.Basic.DeciToHex(m_Items.ItemsList.First(x => x.Position == 7).BaseItem.ID);
            m += ",";

            if (m_Items.ItemsList.Any(x => x.Position == 8))
                m += SunDofus.Basic.DeciToHex(m_Items.ItemsList.First(x => x.Position == 8).BaseItem.ID);
            m += ",";

            if (m_Items.ItemsList.Any(x => x.Position == 15))
                m += SunDofus.Basic.DeciToHex(m_Items.ItemsList.First(x => x.Position == 15).BaseItem.ID);
            m += ",";

            return m;
        }

        public string GetItems()
        {
            string m = "";

            foreach (Items.Item m_I in m_Items.ItemsList)
            {
                m += m_I.ToString() + ";";
            }

            if (m != "")
                return m.Substring(0, m.Length - 1);
            else
                return m;
        }

        public string GetItemsToSave()
        {
            string m = "";

            foreach (Items.Item m_I in m_Items.ItemsList)
            {
                m += m_I.SaveString() + ";";
            }

            if (m != "")
                return m.Substring(0, m.Length - 1);
            else
                return m;
        }

#endregion

        #region Pattern

        public string PatternList()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(ID).Append(";");
            Builder.Append(m_Name).Append(";");
            Builder.Append(Level).Append(";");
            Builder.Append(Skin).Append(";");
            Builder.Append(Basic.DeciToHex(Color)).Append(";");
            Builder.Append(Basic.DeciToHex(Color2)).Append(";");
            Builder.Append(Basic.DeciToHex(Color3)).Append(";");
            Builder.Append(GetItemsPos()).Append(";");
            Builder.Append("0;").Append(Program.m_ServerID).Append(";;;");

            return Builder.ToString();
        }

        public string PatternSelect()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("|").Append(ID).Append("|");
            Builder.Append(m_Name).Append("|");
            Builder.Append(Level).Append("|");
            Builder.Append(Class).Append("|");
            Builder.Append(Skin).Append("|");
            Builder.Append(Basic.DeciToHex(Color)).Append("|");
            Builder.Append(Basic.DeciToHex(Color2)).Append("|");
            Builder.Append(Basic.DeciToHex(Color3)).Append("||");
            Builder.Append(GetItems()).Append("|");

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
            Builder.Append(GetItemsPos()).Append(";"); // Items
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
            m_Stats.Life.Boosts = 0;
            m_Stats.Wisdom.Boosts = 0;
            m_Stats.Strenght.Boosts = 0;
            m_Stats.Intelligence.Boosts = 0;
            m_Stats.Luck.Boosts = 0;
            m_Stats.Agility.Boosts = 0;
        }

        public void ResetItemsStats()
        {
            m_Stats.Life.Items = 0;
            m_Stats.Wisdom.Items = 0;
            m_Stats.Strenght.Items = 0;
            m_Stats.Intelligence.Items = 0;
            m_Stats.Luck.Items = 0;
            m_Stats.Agility.Items = 0;
        }

        public void ResetDons()
        {
            m_Stats.Life.Dons = 0;
            m_Stats.Wisdom.Dons = 0;
            m_Stats.Strenght.Dons = 0;
            m_Stats.Intelligence.Dons = 0;
            m_Stats.Luck.Dons = 0;
            m_Stats.Agility.Dons = 0;
        }

        public void ResetStats()
        {
            m_Stats.Life.Bases = 0;
            m_Stats.Wisdom.Bases = 0;
            m_Stats.Strenght.Bases = 0;
            m_Stats.Intelligence.Bases = 0;
            m_Stats.Luck.Bases = 0;
            m_Stats.Agility.Bases = 0;
        }

        public void UpdateStats()
        {
            MaximumLife = m_Stats.Life.Total() + (Client.m_Player.Level * 5) + 55;

            m_Stats.PA.Bases = (Level >= 100 ? 7 : 6);
            m_Stats.PM.Bases = 3;

            m_Stats.DodgePA.Bases = 0;
            m_Stats.DodgePM.Bases = 0;
            m_Stats.Prospection.Bases = 0;
            m_Stats.Initiative.Bases = 0;
            m_Stats.MaxPods.Bases = 1000;

            m_Stats.DodgePA.Bases = (m_Stats.Wisdom.Total() / 4);
            m_Stats.DodgePM.Bases = (m_Stats.Wisdom.Total() / 4);
            m_Stats.Prospection.Bases = (m_Stats.Luck.Total() / 10) + 100;
            if (Class == 3) m_Stats.Prospection.Bases += 20;
            m_Stats.Initiative.Bases = (MaximumLife / 4 + m_Stats.Initiative.Total()) * (Life / MaximumLife);
        }

        public void ResetVita(string Data)
        {
            if (Data == "full")
            {
                Life = MaximumLife;
                SendCharStats();
            }
            else
            {
                Life = (MaximumLife / (int.Parse(Data) / 100));
                SendCharStats();
            }
        }

        public string SqlStats()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(CharactPoint).Append("|");
            Builder.Append(SpellPoint).Append("|");
            Builder.Append(m_Stats.Life.Bases).Append("|");
            Builder.Append(m_Stats.Wisdom.Bases).Append("|");
            Builder.Append(m_Stats.Strenght.Bases).Append("|");
            Builder.Append(m_Stats.Intelligence.Bases).Append("|");
            Builder.Append(m_Stats.Luck.Bases).Append("|");
            Builder.Append(m_Stats.Agility.Bases);

            return Builder.ToString();
        }

        public void ParseStats(string Args)
        {
            if (Args == "") return;
            string[] Data = Args.Split('|');
            CharactPoint = int.Parse(Data[0]);
            SpellPoint = int.Parse(Data[1]);
            m_Stats.Life.Bases = int.Parse(Data[2]);
            m_Stats.Wisdom.Bases = int.Parse(Data[3]);
            m_Stats.Strenght.Bases = int.Parse(Data[4]);
            m_Stats.Intelligence.Bases = int.Parse(Data[5]);
            m_Stats.Luck.Bases = int.Parse(Data[6]);
            m_Stats.Agility.Bases = int.Parse(Data[7]);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(Exp).Append(",");
            Builder.Append("0,255|"); // Last MaxExpLevel , This MaxExpLevel
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
            Builder.Append(m_Stats.Strenght.ToString()).Append("|");
            Builder.Append(m_Stats.Life.ToString()).Append("|");
            Builder.Append(m_Stats.Wisdom.ToString()).Append("|");
            Builder.Append(m_Stats.Luck.ToString()).Append("|");
            Builder.Append(m_Stats.Agility.ToString()).Append("|");
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
            Builder.Append("0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|0,0,0,0|1"); // Resist

            return Builder.ToString();
        }

        #endregion
    }
}
