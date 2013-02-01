using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin;
using DofusOrigin.Realm.Characters.Stats;
using DofusOrigin.Realm.Characters.Spells;
using DofusOrigin.Realm.Characters.Items;
using DofusOrigin.Network.Realm;

namespace DofusOrigin.Realm.Characters
{
    class Character
    {
        public string m_name { get; set; }
        public string m_channel { get; set; }

        public int m_id { get; set; }
        public int m_color { get; set; }
        public int m_color2 { get; set; }
        public int m_color3 { get; set; }
        public int m_class { get; set; }
        public int m_sex { get; set; }
        public int m_skin { get; set; }
        public int m_size { get; set; }
        public int m_level { get; set; }
        public int m_mapID { get; set; }
        public int m_mapCell { get; set; }
        public int m_dir { get; set; }
        public int m_charactPoint { get; set; }
        public int m_spellPoint { get; set; }
        public int m_energy { get; set; }
        public int m_maximumLife { get; set; }
        public int m_life { get; set; }
        public int m_pods { get; set; }

        public bool isNewCharacter { get; set; }
        public bool isConnected { get; set; }

        public long m_exp { get; set; }
        public long m_kamas { get; set; }
        public long m_quotaRecruitment { get; set; }
        public long m_quotaTrade { get; set; }

        public Stats.Stats m_stats { get; set; }
        public InventaryItems m_inventary { get; set; }
        public InventarySpells m_spellInventary { get; set; }
        public RealmClient m_networkClient { get; set; }
        public CharacterState m_state { get; set; }

        public Character()
        {
            m_stats = new Stats.Stats();
            m_inventary = new Items.InventaryItems(this);
            m_spellInventary = new Spells.InventarySpells(this);

            m_channel = "*#$p%i:?!";
            m_energy = 10000;
            isConnected = false;

            m_quotaRecruitment = 0;
            m_quotaTrade = 0;
        }

        #region Exp

        public void AddExp(long _exp)
        {
            m_exp += _exp;
            LevelUp();
        }

        void LevelUp()
        {
            if (this.m_level == Database.Cache.LevelsCache.MaxLevel())
                return;

            if (m_exp >= Database.Cache.LevelsCache.ReturnLevel(m_level + 1).m_character)
            {
                while (m_exp >= Database.Cache.LevelsCache.ReturnLevel(m_level + 1).m_character)
                {
                    if (this.m_level == Database.Cache.LevelsCache.MaxLevel())
                        break;

                    m_level++;
                    m_spellPoint++;
                    m_charactPoint += 5;
                }

                m_networkClient.Send(string.Format("AN{0}", m_level));
                m_spellInventary.LearnSpells();
                SendChararacterStats();
            }
        }

        #endregion

        #region ChatSpam

        public long TimeTrade()
        {
            return (long)Math.Ceiling((double)((m_quotaTrade - Environment.TickCount) / 1000));
        }

        public long TimeRecruitment()
        {
            return (long)Math.Ceiling((double)((m_quotaRecruitment - Environment.TickCount) / 1000));
        }

        public bool CanSendinTrade()
        {
            return (TimeTrade() <= 0 ? true : false);
        }

        public bool CanSendinRecruitment()
        {
            return (TimeRecruitment() <= 0 ? true : false);
        }

        public void RefreshTrade()
        {
            m_quotaTrade = Environment.TickCount + Utilities.Config.m_config.GetLongElement("AntiSpamTrade");
        }

        public void RefreshRecruitment()
        {
            m_quotaRecruitment = Environment.TickCount + Utilities.Config.m_config.GetLongElement("AntiSpamRecruitment");
        }

        #endregion

        #region Items

        public string GetItemsPos()
        {
            var packet = "";

            if (m_inventary.m_itemsList.Any(x => x.m_position == 1))
                packet += Utilities.Basic.DeciToHex(m_inventary.m_itemsList.First(x => x.m_position == 1).m_base.m_id);

            packet += ",";

            if (m_inventary.m_itemsList.Any(x => x.m_position == 6))
                packet += Utilities.Basic.DeciToHex(m_inventary.m_itemsList.First(x => x.m_position == 6).m_base.m_id);

            packet += ",";

            if (m_inventary.m_itemsList.Any(x => x.m_position == 7))
                packet += Utilities.Basic.DeciToHex(m_inventary.m_itemsList.First(x => x.m_position == 7).m_base.m_id);

            packet += ",";

            if (m_inventary.m_itemsList.Any(x => x.m_position == 8))
                packet += Utilities.Basic.DeciToHex(m_inventary.m_itemsList.First(x => x.m_position == 8).m_base.m_id);

            packet += ",";

            if (m_inventary.m_itemsList.Any(x => x.m_position == 15))
                packet += Utilities.Basic.DeciToHex(m_inventary.m_itemsList.First(x => x.m_position == 15).m_base.m_id);

            packet += ",";

            return packet;
        }

        public string GetItems()
        {
            return string.Join(";", m_inventary.m_itemsList);
        }

        public string GetItemsToSave()
        {
            var packet = "";

            foreach (var myItem in m_inventary.m_itemsList)
                packet += string.Format("{0};", myItem.SaveString());

            if (packet != "")
                return packet.Substring(0, packet.Length - 1);
            else
                return packet;
        }

#endregion

        #region Pattern

        public string PatternList()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(m_id).Append(";");
            builder.Append(m_name).Append(";");
            builder.Append(m_level).Append(";");
            builder.Append(m_skin).Append(";");
            builder.Append(Utilities.Basic.DeciToHex(m_color)).Append(";");
            builder.Append(Utilities.Basic.DeciToHex(m_color2)).Append(";");
            builder.Append(Utilities.Basic.DeciToHex(m_color3)).Append(";");
            builder.Append(GetItemsPos()).Append(";");
            builder.Append("0;").Append(Utilities.Config.m_config.GetIntElement("ServerId")).Append(";;;");

            return builder.ToString();
        }

        public string PatternOnParty()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(m_id).Append(";");
            builder.Append(m_name).Append(";");
            builder.Append(m_skin).Append(";");
            builder.Append(m_color).Append(";");
            builder.Append(m_color2).Append(";");
            builder.Append(m_color3).Append(";");
            builder.Append(GetItemsPos()).Append(";");
            builder.Append(m_life).Append(",").Append(m_maximumLife).Append(";");
            builder.Append(m_level).Append(";");
            builder.Append(m_stats.initiative).Append(";");
            builder.Append(m_stats.prospection).Append(";0");

            return builder.ToString();
        }

        public string PatternSelect()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("|").Append(m_id).Append("|");
            builder.Append(m_name).Append("|");
            builder.Append(m_level).Append("|");
            builder.Append(m_class).Append("|");
            builder.Append(m_skin).Append("|");
            builder.Append(Utilities.Basic.DeciToHex(m_color)).Append("|");
            builder.Append(Utilities.Basic.DeciToHex(m_color2)).Append("|");
            builder.Append(Utilities.Basic.DeciToHex(m_color3)).Append("||");
            builder.Append(GetItems()).Append("|");

            return builder.ToString();
        }

        public string PatternDisplayChar()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(m_mapCell).Append(";");
            builder.Append(m_dir).Append(";0;");
            builder.Append(m_id).Append(";");
            builder.Append(m_name).Append(";");
            builder.Append(m_class).Append(";");
            builder.Append(m_skin).Append("^").Append(m_size).Append(";");
            builder.Append(m_sex).Append(";0,0,0,").Append(m_level + m_id).Append(";"); // Sex + Alignment
            builder.Append(Utilities.Basic.DeciToHex(m_color)).Append(";");
            builder.Append(Utilities.Basic.DeciToHex(m_color2)).Append(";");
            builder.Append(Utilities.Basic.DeciToHex(m_color3)).Append(";");
            builder.Append(GetItemsPos()).Append(";"); // Items
            builder.Append("0;"); //Aura
            builder.Append(";;");
            builder.Append(";"); // Guild
            builder.Append(";0;");
            builder.Append(";"); // Mount

            return builder.ToString();
        }

        #endregion

        #region Map

        public void LoadMap()
        {
            if (Database.Cache.MapsCache.m_mapsList.Any(x => x.m_map.m_id == this.m_mapID))
            {
                var map = Database.Cache.MapsCache.m_mapsList.First(x => x.m_map.m_id == this.m_mapID);

                m_networkClient.Send(string.Format("GDM|{0}|0{1}|{2}", map.m_map.m_id, map.m_map.m_date, map.m_map.m_key));

                if (this.m_state.isFollow)
                {
                    foreach (var character in this.m_state.followers)
                        character.m_networkClient.Send(string.Format("IC{0}|{1}", GetMap().m_map.m_PosX, GetMap().m_map.m_PosY));
                }
            }
        }

        public void TeleportNewMap(int _mapID, int _cell)
        {
            m_networkClient.Send(string.Format("GA;2;{0};", m_id));

            GetMap().DelPlayer(this);
            var map = Database.Cache.MapsCache.m_mapsList.First(x => x.m_map.m_id == _mapID);

            m_mapID = map.m_map.m_id;
            m_mapCell = _cell;

            LoadMap();
        }

        public Maps.Map GetMap()
        {
            return Database.Cache.MapsCache.m_mapsList.First(x => x.m_map.m_id == this.m_mapID);
        }

        #endregion

        #region Stats

        public void SendChararacterStats()
        {
            UpdateStats();
            m_networkClient.Send(string.Format("As{0}", this.ToString()));
        }

        public void SendPods()
        {
            m_networkClient.Send(string.Format("Ow{0}|{1}", m_pods, m_stats.maxPods.Total()));
        }

        public void ResetBonus()
        {
            m_stats.life.m_boosts = 0;
            m_stats.wisdom.m_boosts = 0;
            m_stats.strenght.m_boosts = 0;
            m_stats.intelligence.m_boosts = 0;
            m_stats.luck.m_boosts = 0;
            m_stats.agility.m_boosts = 0;

            m_stats.initiative.m_boosts = 0;
            m_stats.prospection.m_boosts = 0;
            m_stats.PO.m_boosts = 0;
            m_stats.PA.m_boosts = 0;
            m_stats.PM.m_boosts = 0;
            m_stats.maxMonsters.m_boosts = 0;
            m_stats.maxPods.m_boosts = 0;

            m_stats.bonusDamage.m_boosts = 0;
            m_stats.returnDamage.m_boosts = 0;
            m_stats.bonusDamagePercent.m_boosts = 0;
            m_stats.bonusDamagePhysic.m_boosts = 0;
            m_stats.bonusDamageMagic.m_boosts = 0;
            m_stats.bonusHeal.m_boosts = 0;
            m_stats.bonusDamageTrap.m_boosts = 0;
            m_stats.bonusDamageTrapPercent.m_boosts = 0;
            m_stats.bonusCritical.m_boosts = 0;
            m_stats.bonusFail.m_boosts = 0;

            m_stats.armorNeutral.m_boosts = 0;
            m_stats.armorPercentNeutral.m_boosts = 0;
            m_stats.armorPvpNeutral.m_boosts = 0;
            m_stats.armorPvpPercentNeutral.m_boosts = 0;

            m_stats.armorIntelligence.m_boosts = 0;
            m_stats.armorPercentIntelligence.m_boosts = 0;
            m_stats.armorPvpIntelligence.m_boosts = 0;
            m_stats.armorPvpPercentIntelligence.m_boosts = 0;

            m_stats.armorStrenght.m_boosts = 0;
            m_stats.armorPercentStrenght.m_boosts = 0;
            m_stats.armorPvpStrenght.m_boosts = 0;
            m_stats.armorPvpPercentStrenght.m_boosts = 0;

            m_stats.armorLuck.m_boosts = 0;
            m_stats.armorPercentLuck.m_boosts = 0;
            m_stats.armorPvpLuck.m_boosts = 0;
            m_stats.armorPvpPercentLuck.m_boosts = 0;

            m_stats.armorAgility.m_boosts = 0;
            m_stats.armorPercentAgility.m_boosts = 0;
            m_stats.armorPvpAgility.m_boosts = 0;
            m_stats.armorPvpPercentAgility.m_boosts = 0;
        }

        public void ResetItemsStats()
        {
            m_stats.life.m_items = 0;
            m_stats.wisdom.m_items = 0;
            m_stats.strenght.m_items = 0;
            m_stats.intelligence.m_items = 0;
            m_stats.luck.m_items = 0;
            m_stats.agility.m_items = 0;

            m_stats.initiative.m_items = 0;
            m_stats.prospection.m_items = 0;
            m_stats.PO.m_items = 0;
            m_stats.PA.m_items = 0;
            m_stats.PM.m_items = 0;
            m_stats.maxMonsters.m_items = 0;
            m_stats.maxPods.m_items = 0;

            m_stats.dodgePA.m_items = 0;
            m_stats.dodgePM.m_items = 0;

            m_stats.bonusDamage.m_items = 0;
            m_stats.returnDamage.m_items = 0;
            m_stats.bonusDamagePercent.m_items = 0;
            m_stats.bonusDamagePhysic.m_items = 0;
            m_stats.bonusDamageMagic.m_items = 0;
            m_stats.bonusHeal.m_items = 0;
            m_stats.bonusDamageTrap.m_items = 0;
            m_stats.bonusDamageTrapPercent.m_items = 0;
            m_stats.bonusCritical.m_items = 0;
            m_stats.bonusFail.m_items = 0;

            m_stats.armorNeutral.m_items = 0;
            m_stats.armorPercentNeutral.m_items = 0;
            m_stats.armorPvpNeutral.m_items = 0;
            m_stats.armorPvpPercentNeutral.m_items = 0;

            m_stats.armorIntelligence.m_items = 0;
            m_stats.armorPercentIntelligence.m_items = 0;
            m_stats.armorPvpIntelligence.m_items = 0;
            m_stats.armorPvpPercentIntelligence.m_items = 0;

            m_stats.armorStrenght.m_items = 0;
            m_stats.armorPercentStrenght.m_items = 0;
            m_stats.armorPvpStrenght.m_items = 0;
            m_stats.armorPvpPercentStrenght.m_items = 0;

            m_stats.armorLuck.m_items = 0;
            m_stats.armorPercentLuck.m_items = 0;
            m_stats.armorPvpLuck.m_items = 0;
            m_stats.armorPvpPercentLuck.m_items = 0;

            m_stats.armorAgility.m_items = 0;
            m_stats.armorPercentAgility.m_items = 0;
            m_stats.armorPvpAgility.m_items = 0;
            m_stats.armorPvpPercentAgility.m_items = 0;
        }

        public void ResetDons()
        {
            m_stats.life.m_dons = 0;
            m_stats.wisdom.m_dons = 0;
            m_stats.strenght.m_dons = 0;
            m_stats.intelligence.m_dons = 0;
            m_stats.luck.m_dons = 0;
            m_stats.agility.m_dons = 0;

            m_stats.initiative.m_dons = 0;
            m_stats.prospection.m_dons = 0;
            m_stats.PO.m_dons = 0;
            m_stats.PA.m_dons = 0;
            m_stats.PM.m_dons = 0;
            m_stats.maxMonsters.m_dons = 0;
            m_stats.maxPods.m_dons = 0;

            m_stats.bonusDamage.m_dons = 0;
            m_stats.returnDamage.m_dons = 0;
            m_stats.bonusDamagePercent.m_dons = 0;
            m_stats.bonusDamagePhysic.m_dons = 0;
            m_stats.bonusDamageMagic.m_dons = 0;
            m_stats.bonusHeal.m_dons = 0;
            m_stats.bonusDamageTrap.m_dons = 0;
            m_stats.bonusDamageTrapPercent.m_dons = 0;
            m_stats.bonusCritical.m_dons = 0;
            m_stats.bonusFail.m_dons = 0;

            m_stats.armorNeutral.m_dons = 0;
            m_stats.armorPercentNeutral.m_dons = 0;
            m_stats.armorPvpNeutral.m_dons = 0;
            m_stats.armorPvpPercentNeutral.m_dons = 0;

            m_stats.armorIntelligence.m_dons = 0;
            m_stats.armorPercentIntelligence.m_dons = 0;
            m_stats.armorPvpIntelligence.m_dons = 0;
            m_stats.armorPvpPercentIntelligence.m_dons = 0;

            m_stats.armorStrenght.m_dons = 0;
            m_stats.armorPercentStrenght.m_dons = 0;
            m_stats.armorPvpStrenght.m_dons = 0;
            m_stats.armorPvpPercentStrenght.m_dons = 0;

            m_stats.armorLuck.m_dons = 0;
            m_stats.armorPercentLuck.m_dons = 0;
            m_stats.armorPvpLuck.m_dons = 0;
            m_stats.armorPvpPercentLuck.m_dons = 0;

            m_stats.armorAgility.m_dons = 0;
            m_stats.armorPercentAgility.m_dons = 0;
            m_stats.armorPvpAgility.m_dons = 0;
            m_stats.armorPvpPercentAgility.m_dons = 0;
        }

        public void ResetStats()
        {
            m_stats.life.m_bases = 0;
            m_stats.wisdom.m_bases = 0;
            m_stats.strenght.m_bases = 0;
            m_stats.intelligence.m_bases = 0;
            m_stats.luck.m_bases = 0;
            m_stats.agility.m_bases = 0;

            m_stats.initiative.m_bases = 0;
            m_stats.prospection.m_bases = 0;
            m_stats.PO.m_bases = 0;
            m_stats.PA.m_bases = 0;
            m_stats.PM.m_bases = 0;
            m_stats.maxMonsters.m_bases = 0;
            m_stats.maxPods.m_bases = 0;

            m_stats.bonusDamage.m_bases = 0;
            m_stats.returnDamage.m_bases = 0;
            m_stats.bonusDamagePercent.m_bases = 0;
            m_stats.bonusDamagePhysic.m_bases = 0;
            m_stats.bonusDamageMagic.m_bases = 0;
            m_stats.bonusHeal.m_bases = 0;
            m_stats.bonusDamageTrap.m_bases = 0;
            m_stats.bonusDamageTrapPercent.m_bases = 0;
            m_stats.bonusCritical.m_bases = 0;
            m_stats.bonusFail.m_bases = 0;

            m_stats.armorNeutral.m_bases = 0;
            m_stats.armorPercentNeutral.m_bases = 0;
            m_stats.armorPvpNeutral.m_bases = 0;
            m_stats.armorPvpPercentNeutral.m_bases = 0;

            m_stats.armorIntelligence.m_bases = 0;
            m_stats.armorPercentIntelligence.m_bases = 0;
            m_stats.armorPvpIntelligence.m_bases = 0;
            m_stats.armorPvpPercentIntelligence.m_bases = 0;

            m_stats.armorStrenght.m_bases = 0;
            m_stats.armorPercentStrenght.m_bases = 0;
            m_stats.armorPvpStrenght.m_bases = 0;
            m_stats.armorPvpPercentStrenght.m_bases = 0;

            m_stats.armorLuck.m_bases = 0;
            m_stats.armorPercentLuck.m_bases = 0;
            m_stats.armorPvpLuck.m_bases = 0;
            m_stats.armorPvpPercentLuck.m_bases = 0;

            m_stats.armorAgility.m_bases = 0;
            m_stats.armorPercentAgility.m_bases = 0;
            m_stats.armorPvpAgility.m_bases = 0;
            m_stats.armorPvpPercentAgility.m_bases = 0;
        }

        public void AddLife(int _life)
        {
            if (m_life == m_maximumLife)
                m_networkClient.SendMessage("Vous avez déjà un nombre maximum de point de vie !");

            else if ((m_life + _life) > m_maximumLife)
            {
                m_networkClient.SendMessage("Vous venez de récupérer '" + (m_maximumLife - m_life) + "' de vie !");
                m_life = m_maximumLife;
            }
            else
            {
                m_networkClient.SendMessage("Vous venez de récupérer '" + _life + "' de vie !");
                m_life += _life;
            }
        }

        public void UpdateStats()
        {
            var dif = 0;

            if (m_life < m_maximumLife)
                dif = m_maximumLife - m_life;

            m_maximumLife = m_stats.life.Total() + (m_networkClient.m_player.m_level * 5) + 55;

            if (dif <= 0)
                m_life = m_maximumLife;
            else
                m_life = (m_maximumLife - dif);

            m_stats.PA.m_bases = (m_level >= 100 ? 7 : 6);
            m_stats.PM.m_bases = 3;

            m_stats.dodgePA.m_bases = 0;
            m_stats.dodgePM.m_bases = 0;
            m_stats.prospection.m_bases = 0;
            m_stats.initiative.m_bases = 0;
            m_stats.maxPods.m_bases = 1000;

            m_stats.dodgePA.m_bases = (m_stats.wisdom.m_bases / 4);
            m_stats.dodgePM.m_bases = (m_stats.wisdom.m_bases / 4);
            m_stats.dodgePA.m_items = (m_stats.wisdom.m_items / 4);
            m_stats.dodgePM.m_items = (m_stats.wisdom.m_items / 4);

            m_stats.prospection.m_bases = (m_stats.luck.Total() / 10) + 100;
            if (m_class == 3) m_stats.prospection.m_bases += 20;
            m_stats.initiative.m_bases = (m_maximumLife / 4 + m_stats.initiative.Total()) * (m_life / m_maximumLife);
        }

        public void ResetVita(string _datas)
        {
            if (_datas == "full")
            {
                m_life = m_maximumLife;
                SendChararacterStats();
            }
            else
            {
                m_life = (m_maximumLife / (int.Parse(_datas) / 100));
                SendChararacterStats();
            }
        }

        public string SqlStats()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(m_charactPoint).Append("|");
            builder.Append(m_spellPoint).Append("|");
            builder.Append(m_kamas).Append("|");
            builder.Append(m_stats.life.m_bases).Append("|");
            builder.Append(m_stats.wisdom.m_bases).Append("|");
            builder.Append(m_stats.strenght.m_bases).Append("|");
            builder.Append(m_stats.intelligence.m_bases).Append("|");
            builder.Append(m_stats.luck.m_bases).Append("|");
            builder.Append(m_stats.agility.m_bases);

            return builder.ToString();
        }

        public void ParseStats(string _args)
        {
            if (_args == "") 
                return;

            string[] Data = _args.Split('|');
            m_charactPoint = int.Parse(Data[0]);
            m_spellPoint = int.Parse(Data[1]);
            m_kamas = long.Parse(Data[2]);
            m_stats.life.m_bases = int.Parse(Data[3]);
            m_stats.wisdom.m_bases = int.Parse(Data[4]);
            m_stats.strenght.m_bases = int.Parse(Data[5]);
            m_stats.intelligence.m_bases = int.Parse(Data[6]);
            m_stats.luck.m_bases = int.Parse(Data[7]);
            m_stats.agility.m_bases = int.Parse(Data[8]);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(m_exp).Append(",");
            builder.Append(Database.Cache.LevelsCache.ReturnLevel(m_level).m_character).Append(",");
            builder.Append(Database.Cache.LevelsCache.ReturnLevel(m_level + 1).m_character).Append("|");
            builder.Append(m_kamas).Append("|");
            builder.Append(m_charactPoint).Append("|");
            builder.Append(m_spellPoint).Append("|");
            builder.Append("0~2,0,0,0,0,0|"); // Alignement
            builder.Append(m_life).Append(",");
            builder.Append(m_maximumLife).Append("|");
            builder.Append(m_energy).Append(",10000|");
            builder.Append(m_stats.initiative.Total()).Append("|");
            builder.Append(m_stats.prospection.Total()).Append("|");

            builder.Append(m_stats.PA.ToString()).Append("|");
            builder.Append(m_stats.PM.ToString()).Append("|");
            builder.Append(m_stats.strenght.ToString()).Append("|");
            builder.Append(m_stats.life.ToString()).Append("|");
            builder.Append(m_stats.wisdom.ToString()).Append("|");
            builder.Append(m_stats.luck.ToString()).Append("|");
            builder.Append(m_stats.agility.ToString()).Append("|");
            builder.Append(m_stats.intelligence.ToString()).Append("|");
            builder.Append(m_stats.PO.ToString()).Append("|");
            builder.Append(m_stats.maxMonsters.ToString()).Append("|");
            builder.Append(m_stats.bonusDamage.ToString()).Append("|");
            builder.Append(m_stats.bonusDamagePhysic.ToString()).Append("|");
            builder.Append(m_stats.bonusDamageMagic.ToString()).Append("|");
            builder.Append(m_stats.bonusDamagePercent.ToString()).Append("|");
            builder.Append(m_stats.bonusHeal.ToString()).Append("|");
            builder.Append(m_stats.bonusDamageTrap.ToString()).Append("|");
            builder.Append(m_stats.bonusDamageTrapPercent.ToString()).Append("|");
            builder.Append(m_stats.returnDamage.ToString()).Append("|");
            builder.Append(m_stats.bonusCritical.ToString()).Append("|");
            builder.Append(m_stats.bonusFail.ToString()).Append("|");
            builder.Append(m_stats.dodgePA.ToString()).Append("|");
            builder.Append(m_stats.dodgePM.ToString()).Append("|");

            builder.Append(m_stats.armorNeutral.ToString()).Append("|");
            builder.Append(m_stats.armorPercentNeutral.ToString()).Append("|");
            builder.Append(m_stats.armorPvpNeutral.ToString()).Append("|");
            builder.Append(m_stats.armorPvpPercentNeutral.ToString()).Append("|");

            builder.Append(m_stats.armorStrenght.ToString()).Append("|");
            builder.Append(m_stats.armorPercentStrenght.ToString()).Append("|");
            builder.Append(m_stats.armorPvpStrenght.ToString()).Append("|");
            builder.Append(m_stats.armorPvpPercentNeutral.ToString()).Append("|");

            builder.Append(m_stats.armorLuck.ToString()).Append("|");
            builder.Append(m_stats.armorPercentLuck.ToString()).Append("|");
            builder.Append(m_stats.armorPvpLuck.ToString()).Append("|");
            builder.Append(m_stats.armorPvpPercentNeutral.ToString()).Append("|");

            builder.Append(m_stats.armorAgility.ToString()).Append("|");
            builder.Append(m_stats.armorPercentAgility.ToString()).Append("|");
            builder.Append(m_stats.armorPvpAgility.ToString()).Append("|");
            builder.Append(m_stats.armorPvpPercentNeutral.ToString()).Append("|");

            builder.Append(m_stats.armorIntelligence.ToString()).Append("|");
            builder.Append(m_stats.armorPercentIntelligence.ToString()).Append("|");
            builder.Append(m_stats.armorPvpIntelligence.ToString()).Append("|");
            builder.Append(m_stats.armorPvpPercentNeutral.ToString()).Append("|");

            builder.Append("1");

            return builder.ToString();
        }

        #endregion
    }
}
