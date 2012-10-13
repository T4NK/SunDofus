using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace realm.Realm.Character
{
    class Character
    {
        public string myName = "";
        public int ID, Color, Color2, Color3, Class, Sex, Skin, Size, Level, MapID, MapCell, Dir = -1;
        public bool NewCharacter, isConnected = false;

        public long Exp = 0;
        public long Kamas = 0;
        public int CharactPoint = 0;
        public int SpellPoint = 0;
        public int Energy = 10000;

        public int MaximumLife = 55;
        public int Life = 0;

        public int Pods = 0;

        public Stats.Stats myStats;
        public Items.InventaryItems myInventary;
        public Spells.InventarySpells mySpellInventary;

        public Network.Realm.RealmClient Client;
        public CharacterState State;

        public string Channel = "*#$p%i:?!";

        public Character()
        {
            myStats = new Stats.Stats();
            myInventary = new Items.InventaryItems(this);
            mySpellInventary = new Spells.InventarySpells(this);
        }

        #region Exp

        public void AddExp(long _Exp)
        {
            Exp += _Exp;
            LevelUp();
        }

        void LevelUp()
        {
            if (Exp >= Database.Cache.LevelsCache.ReturnLevel(Level + 1).Character)
            {
                while (Exp >= Database.Cache.LevelsCache.ReturnLevel(Level + 1).Character)
                {
                    Level++;
                    SpellPoint++;
                    CharactPoint += 5;
                }

                Client.Send(string.Format("AN{0}", Level));
                mySpellInventary.LearnSpells();
                SendCharStats();
            }
        }

        #endregion

        #region Items

        public string GetItemsPos()
        {
            var myPacket = "";

            if (myInventary.ItemsList.Any(x => x.myPosition == 1))
                myPacket += Utilities.Basic.DeciToHex(myInventary.ItemsList.First(x => x.myPosition == 1).myBaseItem.myID);

            myPacket += ",";

            if (myInventary.ItemsList.Any(x => x.myPosition == 6))
                myPacket += Utilities.Basic.DeciToHex(myInventary.ItemsList.First(x => x.myPosition == 6).myBaseItem.myID);

            myPacket += ",";

            if (myInventary.ItemsList.Any(x => x.myPosition == 7))
                myPacket += Utilities.Basic.DeciToHex(myInventary.ItemsList.First(x => x.myPosition == 7).myBaseItem.myID);

            myPacket += ",";

            if (myInventary.ItemsList.Any(x => x.myPosition == 8))
                myPacket += Utilities.Basic.DeciToHex(myInventary.ItemsList.First(x => x.myPosition == 8).myBaseItem.myID);

            myPacket += ",";

            if (myInventary.ItemsList.Any(x => x.myPosition == 15))
                myPacket += Utilities.Basic.DeciToHex(myInventary.ItemsList.First(x => x.myPosition == 15).myBaseItem.myID);

            myPacket += ",";

            return myPacket;
        }

        public string GetItems()
        {
            var myPacket = "";

            foreach (var myItem in myInventary.ItemsList)
                myPacket += string.Format("{0};", myItem.ToString());

            if (myPacket != "")
                return myPacket.Substring(0, myPacket.Length - 1);
            else
                return myPacket;
        }

        public string GetItemsToSave()
        {
            var myPacket = "";

            foreach (var myItem in myInventary.ItemsList)
                myPacket += string.Format("{0};", myItem.SaveString());

            if (myPacket != "")
                return myPacket.Substring(0, myPacket.Length - 1);
            else
                return myPacket;
        }

#endregion

        #region Pattern

        public string PatternList()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(ID).Append(";");
            Builder.Append(myName).Append(";");
            Builder.Append(Level).Append(";");
            Builder.Append(Skin).Append(";");
            Builder.Append(Utilities.Basic.DeciToHex(Color)).Append(";");
            Builder.Append(Utilities.Basic.DeciToHex(Color2)).Append(";");
            Builder.Append(Utilities.Basic.DeciToHex(Color3)).Append(";");
            Builder.Append(GetItemsPos()).Append(";");
            Builder.Append("0;").Append(Utilities.Config.myConfig.GetIntElement("ServerId")).Append(";;;");

            return Builder.ToString();
        }

        public string PatternSelect()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("|").Append(ID).Append("|");
            Builder.Append(myName).Append("|");
            Builder.Append(Level).Append("|");
            Builder.Append(Class).Append("|");
            Builder.Append(Skin).Append("|");
            Builder.Append(Utilities.Basic.DeciToHex(Color)).Append("|");
            Builder.Append(Utilities.Basic.DeciToHex(Color2)).Append("|");
            Builder.Append(Utilities.Basic.DeciToHex(Color3)).Append("||");
            Builder.Append(GetItems()).Append("|");

            return Builder.ToString();
        }

        public string PatternDisplayChar()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(MapCell).Append(";");
            Builder.Append(Dir).Append(";0;");
            Builder.Append(ID).Append(";");
            Builder.Append(myName).Append(";");
            Builder.Append(Class).Append(";");
            Builder.Append(Skin).Append("^").Append(Size).Append(";");
            Builder.Append(Sex).Append(";0,0,0,").Append(Level + ID).Append(";"); // Sex + Alignment
            Builder.Append(Utilities.Basic.DeciToHex(Color)).Append(";");
            Builder.Append(Utilities.Basic.DeciToHex(Color2)).Append(";");
            Builder.Append(Utilities.Basic.DeciToHex(Color3)).Append(";");
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
            if (Database.Cache.MapsCache.MapsList.Any(x => x.myMap.myId == this.MapID))
            {
                var myMap = Database.Cache.MapsCache.MapsList.First(x => x.myMap.myId == this.MapID);

                Client.Send(string.Format("GDM|{0}|0{1}|{2}", myMap.myMap.myId, myMap.myMap.myDate, myMap.myMap.myKey));
            }
        }

        public void TeleportNewMap(int myMapID, int myCharacter)
        {
            Client.Send(string.Format("GA;2;{0};", ID));

            GetMap().DelPlayer(this);
            var m_M = Database.Cache.MapsCache.MapsList.First(x => x.myMap.myId == myMapID);

            MapID = m_M.myMap.myId;
            MapCell = myCharacter;

            LoadMap();
        }

        public Map.Map GetMap()
        {
            return Database.Cache.MapsCache.MapsList.First(x => x.myMap.myId == this.MapID);
        }

        #endregion

        #region Stats

        public void SendCharStats()
        {
            UpdateStats();
            Client.Send(string.Format("As{0}", this.ToString()));
        }

        public void SendPods()
        {
            Client.Send(string.Format("Ow{0}|{1}", Pods, myStats.MaxPods.Total()));
        }

        public void ResetBonus()
        {
            myStats.Life.Boosts = 0;
            myStats.Wisdom.Boosts = 0;
            myStats.Strenght.Boosts = 0;
            myStats.Intelligence.Boosts = 0;
            myStats.Luck.Boosts = 0;
            myStats.Agility.Boosts = 0;

            myStats.Initiative.Boosts = 0;
            myStats.Prospection.Boosts = 0;
            myStats.PO.Boosts = 0;
            myStats.PA.Boosts = 0;
            myStats.PM.Boosts = 0;
            myStats.MaxMonsters.Boosts = 0;
            myStats.MaxPods.Boosts = 0;

            myStats.BonusDamage.Boosts = 0;
            myStats.ReturnDamage.Boosts = 0;
            myStats.BonusDamagePercent.Boosts = 0;
            myStats.BonusDamagePhysic.Boosts = 0;
            myStats.BonusDamageMagic.Boosts = 0;
            myStats.BonusHeal.Boosts = 0;
            myStats.BonusDamageTrap.Boosts = 0;
            myStats.BonusDamageTrapPercent.Boosts = 0;
            myStats.BonusCritical.Boosts = 0;
            myStats.BonusFail.Boosts = 0;

            myStats.ArmorNeutral.Boosts = 0;
            myStats.ArmorPercentNeutral.Boosts = 0;
            myStats.ArmorPvpNeutral.Boosts = 0;
            myStats.ArmorPvpPercentNeutral.Boosts = 0;

            myStats.ArmorIntelligence.Boosts = 0;
            myStats.ArmorPercentIntelligence.Boosts = 0;
            myStats.ArmorPvpIntelligence.Boosts = 0;
            myStats.ArmorPvpPercentIntelligence.Boosts = 0;

            myStats.ArmorStrenght.Boosts = 0;
            myStats.ArmorPercentStrenght.Boosts = 0;
            myStats.ArmorPvpStrenght.Boosts = 0;
            myStats.ArmorPvpPercentStrenght.Boosts = 0;

            myStats.ArmorLuck.Boosts = 0;
            myStats.ArmorPercentLuck.Boosts = 0;
            myStats.ArmorPvpLuck.Boosts = 0;
            myStats.ArmorPvpPercentLuck.Boosts = 0;

            myStats.ArmorAgility.Boosts = 0;
            myStats.ArmorPercentAgility.Boosts = 0;
            myStats.ArmorPvpAgility.Boosts = 0;
            myStats.ArmorPvpPercentAgility.Boosts = 0;
        }

        public void ResetItemsStats()
        {
            myStats.Life.Items = 0;
            myStats.Wisdom.Items = 0;
            myStats.Strenght.Items = 0;
            myStats.Intelligence.Items = 0;
            myStats.Luck.Items = 0;
            myStats.Agility.Items = 0;

            myStats.Initiative.Items = 0;
            myStats.Prospection.Items = 0;
            myStats.PO.Items = 0;
            myStats.PA.Items = 0;
            myStats.PM.Items = 0;
            myStats.MaxMonsters.Items = 0;
            myStats.MaxPods.Items = 0;

            myStats.DodgePA.Items = 0;
            myStats.DodgePM.Items = 0;

            myStats.BonusDamage.Items = 0;
            myStats.ReturnDamage.Items = 0;
            myStats.BonusDamagePercent.Items = 0;
            myStats.BonusDamagePhysic.Items = 0;
            myStats.BonusDamageMagic.Items = 0;
            myStats.BonusHeal.Items = 0;
            myStats.BonusDamageTrap.Items = 0;
            myStats.BonusDamageTrapPercent.Items = 0;
            myStats.BonusCritical.Items = 0;
            myStats.BonusFail.Items = 0;

            myStats.ArmorNeutral.Items = 0;
            myStats.ArmorPercentNeutral.Items = 0;
            myStats.ArmorPvpNeutral.Items = 0;
            myStats.ArmorPvpPercentNeutral.Items = 0;

            myStats.ArmorIntelligence.Items = 0;
            myStats.ArmorPercentIntelligence.Items = 0;
            myStats.ArmorPvpIntelligence.Items = 0;
            myStats.ArmorPvpPercentIntelligence.Items = 0;

            myStats.ArmorStrenght.Items = 0;
            myStats.ArmorPercentStrenght.Items = 0;
            myStats.ArmorPvpStrenght.Items = 0;
            myStats.ArmorPvpPercentStrenght.Items = 0;

            myStats.ArmorLuck.Items = 0;
            myStats.ArmorPercentLuck.Items = 0;
            myStats.ArmorPvpLuck.Items = 0;
            myStats.ArmorPvpPercentLuck.Items = 0;

            myStats.ArmorAgility.Items = 0;
            myStats.ArmorPercentAgility.Items = 0;
            myStats.ArmorPvpAgility.Items = 0;
            myStats.ArmorPvpPercentAgility.Items = 0;
        }

        public void ResetDons()
        {
            myStats.Life.Dons = 0;
            myStats.Wisdom.Dons = 0;
            myStats.Strenght.Dons = 0;
            myStats.Intelligence.Dons = 0;
            myStats.Luck.Dons = 0;
            myStats.Agility.Dons = 0;

            myStats.Initiative.Dons = 0;
            myStats.Prospection.Dons = 0;
            myStats.PO.Dons = 0;
            myStats.PA.Dons = 0;
            myStats.PM.Dons = 0;
            myStats.MaxMonsters.Dons = 0;
            myStats.MaxPods.Dons = 0;

            myStats.BonusDamage.Dons = 0;
            myStats.ReturnDamage.Dons = 0;
            myStats.BonusDamagePercent.Dons = 0;
            myStats.BonusDamagePhysic.Dons = 0;
            myStats.BonusDamageMagic.Dons = 0;
            myStats.BonusHeal.Dons = 0;
            myStats.BonusDamageTrap.Dons = 0;
            myStats.BonusDamageTrapPercent.Dons = 0;
            myStats.BonusCritical.Dons = 0;
            myStats.BonusFail.Dons = 0;

            myStats.ArmorNeutral.Dons = 0;
            myStats.ArmorPercentNeutral.Dons = 0;
            myStats.ArmorPvpNeutral.Dons = 0;
            myStats.ArmorPvpPercentNeutral.Dons = 0;

            myStats.ArmorIntelligence.Dons = 0;
            myStats.ArmorPercentIntelligence.Dons = 0;
            myStats.ArmorPvpIntelligence.Dons = 0;
            myStats.ArmorPvpPercentIntelligence.Dons = 0;

            myStats.ArmorStrenght.Dons = 0;
            myStats.ArmorPercentStrenght.Dons = 0;
            myStats.ArmorPvpStrenght.Dons = 0;
            myStats.ArmorPvpPercentStrenght.Dons = 0;

            myStats.ArmorLuck.Dons = 0;
            myStats.ArmorPercentLuck.Dons = 0;
            myStats.ArmorPvpLuck.Dons = 0;
            myStats.ArmorPvpPercentLuck.Dons = 0;

            myStats.ArmorAgility.Dons = 0;
            myStats.ArmorPercentAgility.Dons = 0;
            myStats.ArmorPvpAgility.Dons = 0;
            myStats.ArmorPvpPercentAgility.Dons = 0;
        }

        public void ResetStats()
        {
            myStats.Life.Bases = 0;
            myStats.Wisdom.Bases = 0;
            myStats.Strenght.Bases = 0;
            myStats.Intelligence.Bases = 0;
            myStats.Luck.Bases = 0;
            myStats.Agility.Bases = 0;

            myStats.Initiative.Bases = 0;
            myStats.Prospection.Bases = 0;
            myStats.PO.Bases = 0;
            myStats.PA.Bases = 0;
            myStats.PM.Bases = 0;
            myStats.MaxMonsters.Bases = 0;
            myStats.MaxPods.Bases = 0;

            myStats.BonusDamage.Bases = 0;
            myStats.ReturnDamage.Bases = 0;
            myStats.BonusDamagePercent.Bases = 0;
            myStats.BonusDamagePhysic.Bases = 0;
            myStats.BonusDamageMagic.Bases = 0;
            myStats.BonusHeal.Bases = 0;
            myStats.BonusDamageTrap.Bases = 0;
            myStats.BonusDamageTrapPercent.Bases = 0;
            myStats.BonusCritical.Bases = 0;
            myStats.BonusFail.Bases = 0;

            myStats.ArmorNeutral.Bases = 0;
            myStats.ArmorPercentNeutral.Bases = 0;
            myStats.ArmorPvpNeutral.Bases = 0;
            myStats.ArmorPvpPercentNeutral.Bases = 0;

            myStats.ArmorIntelligence.Bases = 0;
            myStats.ArmorPercentIntelligence.Bases = 0;
            myStats.ArmorPvpIntelligence.Bases = 0;
            myStats.ArmorPvpPercentIntelligence.Bases = 0;

            myStats.ArmorStrenght.Bases = 0;
            myStats.ArmorPercentStrenght.Bases = 0;
            myStats.ArmorPvpStrenght.Bases = 0;
            myStats.ArmorPvpPercentStrenght.Bases = 0;

            myStats.ArmorLuck.Bases = 0;
            myStats.ArmorPercentLuck.Bases = 0;
            myStats.ArmorPvpLuck.Bases = 0;
            myStats.ArmorPvpPercentLuck.Bases = 0;

            myStats.ArmorAgility.Bases = 0;
            myStats.ArmorPercentAgility.Bases = 0;
            myStats.ArmorPvpAgility.Bases = 0;
            myStats.ArmorPvpPercentAgility.Bases = 0;
        }

        public void AddLife(int NewLife)
        {
            if (Life == MaximumLife)
                Client.SendMessage("Vous avez déjà un nombre maximum de point de vie !");

            else if ((Life + NewLife) > MaximumLife)
            {
                Client.SendMessage("Vous venez de récupérer '" + (MaximumLife - Life) + "' de vie !");
                Life = MaximumLife;
            }
            else
            {
                Client.SendMessage("Vous venez de récupérer '" + NewLife + "' de vie !");
                Life += NewLife;
            }
        }

        public void UpdateStats()
        {
            var Dif = 0;

            if (Life < MaximumLife)
                Dif = MaximumLife - Life;

            MaximumLife = myStats.Life.Total() + (Client.myPlayer.Level * 5) + 55;

            if (Dif <= 0)
                Life = MaximumLife;
            else
                Life = (MaximumLife - Dif);

            myStats.PA.Bases = (Level >= 100 ? 7 : 6);
            myStats.PM.Bases = 3;

            myStats.DodgePA.Bases = 0;
            myStats.DodgePM.Bases = 0;
            myStats.Prospection.Bases = 0;
            myStats.Initiative.Bases = 0;
            myStats.MaxPods.Bases = 1000;

            myStats.DodgePA.Bases = (myStats.Wisdom.Bases / 4);
            myStats.DodgePM.Bases = (myStats.Wisdom.Bases / 4);
            myStats.DodgePA.Items = (myStats.Wisdom.Items / 4);
            myStats.DodgePM.Items = (myStats.Wisdom.Items / 4);

            myStats.Prospection.Bases = (myStats.Luck.Total() / 10) + 100;
            if (Class == 3) myStats.Prospection.Bases += 20;
            myStats.Initiative.Bases = (MaximumLife / 4 + myStats.Initiative.Total()) * (Life / MaximumLife);
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
            Builder.Append(myStats.Life.Bases).Append("|");
            Builder.Append(myStats.Wisdom.Bases).Append("|");
            Builder.Append(myStats.Strenght.Bases).Append("|");
            Builder.Append(myStats.Intelligence.Bases).Append("|");
            Builder.Append(myStats.Luck.Bases).Append("|");
            Builder.Append(myStats.Agility.Bases);

            return Builder.ToString();
        }

        public void ParseStats(string Args)
        {
            if (Args == "") return;

            string[] Data = Args.Split('|');
            CharactPoint = int.Parse(Data[0]);
            SpellPoint = int.Parse(Data[1]);
            myStats.Life.Bases = int.Parse(Data[2]);
            myStats.Wisdom.Bases = int.Parse(Data[3]);
            myStats.Strenght.Bases = int.Parse(Data[4]);
            myStats.Intelligence.Bases = int.Parse(Data[5]);
            myStats.Luck.Bases = int.Parse(Data[6]);
            myStats.Agility.Bases = int.Parse(Data[7]);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(Exp).Append(",");
            Builder.Append(Database.Cache.LevelsCache.ReturnLevel(Level).Character).Append(",");
            Builder.Append(Database.Cache.LevelsCache.ReturnLevel(Level + 1).Character).Append("|");
            Builder.Append(Kamas).Append("|");
            Builder.Append(CharactPoint).Append("|");
            Builder.Append(SpellPoint).Append("|");
            Builder.Append("0~2,0,0,0,0,0|"); // Alignement
            Builder.Append(Life).Append(",");
            Builder.Append(MaximumLife).Append("|");
            Builder.Append(Energy).Append(",10000|");
            Builder.Append(myStats.Initiative.Total()).Append("|");
            Builder.Append(myStats.Prospection.Total()).Append("|");

            Builder.Append(myStats.PA.ToString()).Append("|");
            Builder.Append(myStats.PM.ToString()).Append("|");
            Builder.Append(myStats.Strenght.ToString()).Append("|");
            Builder.Append(myStats.Life.ToString()).Append("|");
            Builder.Append(myStats.Wisdom.ToString()).Append("|");
            Builder.Append(myStats.Luck.ToString()).Append("|");
            Builder.Append(myStats.Agility.ToString()).Append("|");
            Builder.Append(myStats.Intelligence.ToString()).Append("|");
            Builder.Append(myStats.PO.ToString()).Append("|");
            Builder.Append(myStats.MaxMonsters.ToString()).Append("|");
            Builder.Append(myStats.BonusDamage.ToString()).Append("|");
            Builder.Append(myStats.BonusDamagePhysic.ToString()).Append("|");
            Builder.Append(myStats.BonusDamageMagic.ToString()).Append("|");
            Builder.Append(myStats.BonusDamagePercent.ToString()).Append("|");
            Builder.Append(myStats.BonusHeal.ToString()).Append("|");
            Builder.Append(myStats.BonusDamageTrap.ToString()).Append("|");
            Builder.Append(myStats.BonusDamageTrapPercent.ToString()).Append("|");
            Builder.Append(myStats.ReturnDamage.ToString()).Append("|");
            Builder.Append(myStats.BonusCritical.ToString()).Append("|");
            Builder.Append(myStats.BonusFail.ToString()).Append("|");
            Builder.Append(myStats.DodgePA.ToString()).Append("|");
            Builder.Append(myStats.DodgePM.ToString()).Append("|");

            Builder.Append(myStats.ArmorNeutral.ToString()).Append("|");
            Builder.Append(myStats.ArmorPercentNeutral.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpNeutral.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpPercentNeutral.ToString()).Append("|");

            Builder.Append(myStats.ArmorStrenght.ToString()).Append("|");
            Builder.Append(myStats.ArmorPercentStrenght.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpStrenght.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpPercentNeutral.ToString()).Append("|");

            Builder.Append(myStats.ArmorLuck.ToString()).Append("|");
            Builder.Append(myStats.ArmorPercentLuck.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpLuck.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpPercentNeutral.ToString()).Append("|");

            Builder.Append(myStats.ArmorAgility.ToString()).Append("|");
            Builder.Append(myStats.ArmorPercentAgility.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpAgility.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpPercentNeutral.ToString()).Append("|");

            Builder.Append(myStats.ArmorIntelligence.ToString()).Append("|");
            Builder.Append(myStats.ArmorPercentIntelligence.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpIntelligence.ToString()).Append("|");
            Builder.Append(myStats.ArmorPvpPercentNeutral.ToString()).Append("|");

            Builder.Append("1");

            return Builder.ToString();
        }

        #endregion
    }
}
