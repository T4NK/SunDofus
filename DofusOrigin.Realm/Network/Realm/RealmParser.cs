using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DofusOrigin.Realm.Characters;
using DofusOrigin.Realm.Maps;
using DofusOrigin.Realm.Characters.Stats;
using DofusOrigin.Realm;
using DofusOrigin.Realm.World;

namespace DofusOrigin.Network.Realm
{
    class RealmParser
    {
        public RealmClient m_client;

        delegate void Packets(string _string);
        Dictionary<string, Packets> m_packets;

        public RealmParser(RealmClient _client)
        {
            m_client = _client;

            m_packets = new Dictionary<string, Packets>();
            RegisterPackets();
        }

        void RegisterPackets()
        {
            m_packets["AA"] = CreateCharacter;
            m_packets["AB"] = StatsBoosts;
            m_packets["AD"] = DeleteCharacter;
            m_packets["Ag"] = SendGifts;
            m_packets["AG"] = AcceptGift;
            m_packets["AL"] = SendCharacterList;
            m_packets["AP"] = SendRandomName;
            m_packets["AS"] = SelectCharacter;
            m_packets["AT"] = ParseTicket;
            m_packets["AV"] = SendCommunauty;
            m_packets["BA"] = ParseConsoleMessage;
            m_packets["BD"] = SendDate;
            m_packets["BM"] = ParseChatMessage;
            m_packets["cC"] = ChangeChannel;
            m_packets["EB"] = BuyFromNPC;
            m_packets["ER"] = RequestExchange;
            m_packets["ES"] = SellFromNPC;
            m_packets["EV"] = CancelExchange;
            m_packets["GA"] = GameAction;
            m_packets["GC"] = CreateGame;
            m_packets["GI"] = GameInformations;
            m_packets["GK"] = EndAction;
            m_packets["Od"] = DeleteItem;
            m_packets["OM"] = MoveItem;
            m_packets["OU"] = UseItem;
            m_packets["SB"] = SpellBoost;
            m_packets["SM"] = SpellMove;
        }

        public void Parse(string _datas)
        {
            if (_datas == "ping")
                m_client.Send("pong");
            else if (_datas == "qping")
                m_client.Send("qpong");

            if (_datas.Length < 2) 
                return;

            string header = _datas.Substring(0, 2);

            if (!m_packets.ContainsKey(header))
            {
                m_client.Send("BN");
                return;
            }

            m_packets[header](_datas.Substring(2));
        }

        #region Ticket

        public void ParseTicket(string _datas)
        {
            if (Network.Authentication.AuthenticationsKeys.m_keys.Any(x => x.m_key == _datas))
            {
                var key = Network.Authentication.AuthenticationsKeys.m_keys.First(x => x.m_key == _datas);

                m_client.m_infos = key.m_infos;
                m_client.m_infos.ParseCharacters();
                m_client.ParseCharacters();

                m_client.isAuth = true;

                Network.Authentication.AuthenticationsKeys.m_keys.Remove(key);

                Network.ServersHandler.m_authLinks.Send(string.Format("SNC|{0}", m_client.m_infos.m_pseudo));
                ServersHandler.m_realmServer.m_pseudoClients.Add(m_client.m_infos.m_pseudo);

                m_client.Send("ATK0");
            }
            else
                m_client.Send("ATE");
        }

        #endregion
        
        #region Character

        public void SendRandomName(string _datas)
        {
            m_client.Send(string.Format("APK{0}", Utilities.Basic.RandomName()));
        }

        public void SendCommunauty(string _datas)
        {
            m_client.Send(string.Format("AV{0}", Utilities.Config.m_config.GetIntElement("ServerCom")));
        }

        public void SendCharacterList(string _datas)
        {
            string packet = string.Format("ALK{0}|{1}", m_client.m_infos.m_subscription, m_client.m_infos.m_characters.Count);

            if (m_client.m_infos.m_characters.Count != 0)
            {
                foreach (DofusOrigin.Realm.Characters.Character m_C in m_client.m_characters)
                    packet += string.Format("|{0}", m_C.PatternList());
            }

            m_client.Send(packet);
        }

        public void CreateCharacter(string _datas)
        {
            try
            {
                var characterDatas = _datas.Split('|');

                if (characterDatas[0] != "" | CharactersManager.ExistsName(characterDatas[0]) == false)
                {
                    var character = new Character();

                    character.m_id = Database.Cache.CharactersCache.GetNewID();
                    character.m_name = characterDatas[0];
                    character.m_level = Utilities.Config.m_config.GetIntElement("StartLevel");
                    character.m_class = int.Parse(characterDatas[1]);
                    character.m_sex = int.Parse(characterDatas[2]);
                    character.m_skin = int.Parse(character.m_class + "" + character.m_sex);
                    character.m_size = 100;
                    character.m_color = int.Parse(characterDatas[3]);
                    character.m_color2 = int.Parse(characterDatas[4]);
                    character.m_color3 = int.Parse(characterDatas[5]);

                    switch (character.m_class)
                    {
                        case 1:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Feca");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Feca");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Feca");
                            break;
                        case 2:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Osa");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Osa");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Osa");
                            break;
                        case 3:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Enu");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Enu");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Enu");
                            break;
                        case 4:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Sram");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Sram");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Sram");
                            break;
                        case 5:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Xel");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Xel");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Xel");
                            break;
                        case 6:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Eca");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Eca");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Eca");
                            break;
                        case 7:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Eni");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Eni");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Eni");
                            break;
                        case 8:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Iop");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Iop");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Iop");
                            break;
                        case 9:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Cra");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Cra");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Cra");
                            break;
                        case 10:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Sadi");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Sadi");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Sadi");
                            break;
                        case 11:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Sacri");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Sacri");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Sacri");
                            break;
                        case 12:
                            character.m_mapID = Utilities.Config.m_config.GetIntElement("StartMap_Panda");
                            character.m_mapCell = Utilities.Config.m_config.GetIntElement("StartCell_Panda");
                            character.m_dir = Utilities.Config.m_config.GetIntElement("StartDir_Panda");
                            break;
                    }

                    character.m_charactPoint = (character.m_level - 1) * 5;
                    character.m_spellPoint = (character.m_level - 1);
                    character.m_exp = Database.Cache.LevelsCache.ReturnLevel(character.m_level).m_character;
                    character.m_kamas = (long)Utilities.Config.m_config.GetIntElement("StartKamas");


                    character.isNewCharacter = true;

                    if (character.m_class < 1 | character.m_class > 12 | character.m_sex < 0 | character.m_sex > 1)
                    {
                        m_client.Send("AAE");
                        return;
                    }

                    character.m_spellInventary.LearnSpells();

                    Database.Cache.CharactersCache.CreateCharacter(character);
                    CharactersManager.m_charactersList.Add(character);
                    m_client.m_characters.Add(character);

                    Network.ServersHandler.m_authLinks.Send(string.Format("SNAC|{0}|{1}", m_client.m_infos.m_id, character.m_name));

                    m_client.Send("TB");
                    m_client.Send("AAK");
                    SendCharacterList("");
                }
                else
                {
                    m_client.Send("AAE");
                }
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(e.ToString());
            }
        }

        public void DeleteCharacter(string _datas)
        {
            var id = int.Parse(_datas.Split('|')[0]);

            if (!CharactersManager.m_charactersList.Any(x => x.m_id == id))
                return;

            var character = CharactersManager.m_charactersList.First(x => x.m_id == id);

            if (_datas.Split('|')[1] != m_client.m_infos.m_answer && character.m_level >= 20)
            {
                m_client.Send("ADE");
                return;
            }

            CharactersManager.m_charactersList.Remove(character);
            m_client.m_characters.Remove(character);

            Network.ServersHandler.m_authLinks.Send(string.Format("SDAC|{0}|{1}", m_client.m_infos.m_id, character.m_name));
            Database.Cache.CharactersCache.DeleteCharacter(character.m_name);

            SendCharacterList("");
        }

        public void SelectCharacter(string _datas)
        {
            var id = int.Parse(_datas);

            if (!CharactersManager.m_charactersList.Any(x => x.m_id == id))
                return;

            var character = CharactersManager.m_charactersList.First(x => x.m_id == id);

            if (m_client.m_characters.Contains(character))
            {
                m_client.m_player = character;
                m_client.m_player.m_state = new CharacterState(m_client.m_player);
                m_client.m_player.m_networkClient = m_client;

                m_client.m_player.isConnected = true;

                m_client.Send(string.Format("ASK{0}", m_client.m_player.PatternSelect()));
            }
            else
                m_client.Send("ASE");
        }

        #endregion

        #region Gift

        public void SendGifts(string _datas)
        {
            m_client.SendGifts();
        }

        public void AcceptGift(string _datas)
        {
            try
            {
                var infos = _datas.Split('|');

                if (m_client.m_characters.Any(x => x.m_id == int.Parse(infos[1])))
                {
                    if (m_client.m_infos.m_gifts.Any(x => x.m_id == int.Parse(infos[0])))
                    {
                        var myGift = m_client.m_infos.m_gifts.First(e => e.m_id == int.Parse(infos[0]));
                        m_client.m_characters.First(x => x.m_id == int.Parse(infos[1])).m_inventary.AddItem(myGift.m_item, true);

                        m_client.Send("AG0");
                        Network.ServersHandler.m_authLinks.Send(string.Format("SNDG|{0}|{1}", myGift.m_id, m_client.m_infos.m_id));
                        m_client.m_infos.m_gifts.Remove(myGift);

                    }
                    else
                        m_client.Send("AGE");
                }
                else
                    m_client.Send("AGE");
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(e.ToString());
            }
        }

        #endregion

        #region Realm

        void SendDate(string _datas)
        {
            m_client.Send(string.Format("BD{0}", Utilities.Basic.GetDofusDate()));
        }

        public void CreateGame(string _datas)
        {
            m_client.Send(string.Format("GCK|1|{0}", m_client.m_player.m_name));
            m_client.Send("AR6bk");

            m_client.Send("cC+*#$p%i:?!");
            m_client.Send("SLo+");
            m_client.m_player.m_spellInventary.SendAllSpells();
            m_client.Send(string.Format("BT{0}", Utilities.Basic.GetActuelTime()));

            if (m_client.m_player.m_life == 0)
            {
                m_client.m_player.UpdateStats();
                m_client.m_player.m_life = m_client.m_player.m_maximumLife;
            }

            m_client.m_player.m_inventary.RefreshBonus();
            m_client.m_player.SendPods();
            m_client.m_player.SendChararacterStats();

            m_client.m_player.LoadMap();
        }

        public void ChangeChannel(string _channel)
        {
            var Add = false;

            if (_channel.Contains("+"))
            {
                Add = true;
                _channel = _channel.Replace("+", "");
            }

            else if (_channel.Contains("-")) 
                _channel = _channel.Replace("-", "");
            else 
                return;

            if (Add == true)
            {
                if(!m_client.m_player.m_channel.Contains(_channel)) m_client.m_player.m_channel = m_client.m_player.m_channel + _channel;
                m_client.Send("cC+" + _channel);
            }
            else
            {
                m_client.m_player.m_channel = m_client.m_player.m_channel.Replace(_channel, "");
                m_client.Send("cC-" + _channel);
            }
        }

        public void ParseChatMessage(string _datas)
        {
            var datas = _datas.Split('|');

            var channel = datas[0];
            var message = datas[1];

            try
            {
                switch (channel)
                {
                    case "*":
                        Chat.SendGeneralMessage(m_client, message);
                        break;

                    case "$":
                        //PartyMessage
                        break;

                    case "%":
                        //GuildMessage
                        break;

                    case "#":
                        //TeamMessage
                        break;

                    case "?":
                        Chat.SendRecruitmentMessage(m_client, message);
                        break;

                    case "!":
                        //AlignmentMessage
                        break;

                    case ":":
                        Chat.SendTradeMessage(m_client, message);
                        break;

                    case "@":
                        //AdminMessage
                        break;

                    case "¤":
                        //No idea
                        break;

                    default:
                        if (channel.Length > 1)
                            Chat.SendPrivateMessage(m_client, channel, message);
                        break;
                }
            }
            catch { }
        }

        public void ParseConsoleMessage(string _datas)
        {
            m_client.m_commander.ParseCommand(_datas);
        }

        public void GameInformations(string _datas)
        {
            m_client.m_player.GetMap().AddPlayer(m_client.m_player);
            m_client.Send("GDK");
            m_client.Send("fC0"); //Fight
        }

        public void GameAction(string _datas)
        {
            var packet = int.Parse(_datas.Substring(0, 3));

            switch (packet)
            {
                case 1:
                    GameMove(_datas);
                    break;
            }
        }

        public void GameMove(string _datas)
        {
            var packet = _datas.Substring(3);

            if (!Pathfinding.isValidCell(m_client.m_player.m_mapCell, packet))
            {
                m_client.Send("GA;0");
                return;
            }

            var path = new Pathfinding(packet, m_client.m_player.GetMap(), m_client.m_player.m_mapCell, m_client.m_player.m_dir);
            var newPath = path.RemakePath();

            newPath = path.GetStartPath + newPath;

            if (!m_client.m_player.GetMap().m_rushablesCells.Contains(path.m_destination))
            {
                m_client.Send("GA;0");
                return;
            }

            m_client.m_player.m_dir = path.m_newDirection;
            m_client.m_player.m_state.moveToCell = path.m_destination;
            m_client.m_player.m_state.onMove = true;

            m_client.m_player.GetMap().Send(string.Format("GA0;1;{0};{1}", m_client.m_player.m_id, newPath));
        }

        public void EndAction(string _datas)
        {
            switch(_datas.Substring(0,1))
            {
                case "K":

                    if (m_client.m_player.m_state.onMove == true)
                    {
                        m_client.m_player.m_state.onMove = false;
                        m_client.m_player.m_mapCell = m_client.m_player.m_state.moveToCell;
                        m_client.m_player.m_state.moveToCell = -1;
                        m_client.Send("BN");

                        if (m_client.m_player.GetMap().m_triggers.Any(x => x.m_cellID == m_client.m_player.m_mapCell))
                        {
                            var trigger = m_client.m_player.GetMap().m_triggers.First(x => x.m_cellID == m_client.m_player.m_mapCell);

                            if (DofusOrigin.Realm.World.ConditionsHandler.HasCondition(m_client, trigger.m_conds))
                                DofusOrigin.Realm.Effects.EffectAction.ParseEffect(m_client.m_player,trigger.m_actionID, trigger.m_args);
                            else
                                m_client.SendMessage("Vous ne possédez pas les conditions nécessaires pour cette action !");
                        }
                    }

                    break;

                case "E":

                    int cell = int.Parse(_datas.Split('|')[1]);
                    m_client.m_player.m_state.onMove = false;
                    m_client.m_player.m_mapCell = cell;

                    break;
            }
        }

        #region Items

        public void DeleteItem(string _datas)
        {
            try
            {
                var allDatas = _datas.Split('|');

                if (int.Parse(allDatas[1]) <= 0) 
                    return;

                m_client.m_player.m_inventary.DeleteItem(int.Parse(allDatas[0]), int.Parse(allDatas[1]));
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot delete item from <{0}> because : {1}", m_client.myIp(), e.ToString()));
            }
        }

        public void MoveItem(string _datas)
        {
            try
            {
                var allDatas = _datas.Split('|');
                m_client.m_player.m_inventary.MoveItem(int.Parse(allDatas[0]), int.Parse(allDatas[1]), (allDatas.Length >= 3 ? int.Parse(allDatas[2]) : 1));
            }
            catch (Exception e)
            {
                Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot move item from <{0}> because : {1}", m_client.myIp(), e.ToString()));
            }
        }

        public void UseItem(string _datas)
        {
            m_client.m_player.m_inventary.UseItem(_datas);
        }

        #endregion

        #region StatsBoosts

        public void StatsBoosts(string _datas)
        {
            var caract = int.Parse(_datas);
            var count = 0;

            switch (caract)
            {
                case 11:

                    if (m_client.m_player.m_charactPoint < 1) 
                        return;

                    if (m_client.m_player.m_class == 11)
                    {
                        m_client.m_player.m_stats.life.m_bases += 2;
                        m_client.m_player.m_life += 2;
                    }
                    else
                    {
                        m_client.m_player.m_stats.life.m_bases += 1;
                        m_client.m_player.m_life += 1;
                    }

                    m_client.m_player.m_charactPoint -= 1;
                    m_client.m_player.SendChararacterStats();

                    break;

                case 12:

                    if (m_client.m_player.m_charactPoint < 3) 
                        return;

                    m_client.m_player.m_stats.wisdom.m_bases += 1;
                    m_client.m_player.m_charactPoint -= 3;
                    m_client.m_player.SendChararacterStats();

                    break;

                case 10:

                    if (m_client.m_player.m_class == 1 | m_client.m_player.m_class == 7 | m_client.m_player.m_class == 2 | m_client.m_player.m_class == 5)
                    {
                        if (m_client.m_player.m_stats.strenght.m_bases < 51) count = 2;
                        if (m_client.m_player.m_stats.strenght.m_bases > 50) count = 3;
                        if (m_client.m_player.m_stats.strenght.m_bases > 150) count = 4;
                        if (m_client.m_player.m_stats.strenght.m_bases > 250) count = 5;
                    }

                    else if (m_client.m_player.m_class == 3 | m_client.m_player.m_class == 9)
                    {
                        if (m_client.m_player.m_stats.strenght.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.strenght.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.strenght.m_bases > 150) count = 3;
                        if (m_client.m_player.m_stats.strenght.m_bases > 250) count = 4;
                        if (m_client.m_player.m_stats.strenght.m_bases > 350) count = 5;
                    }

                    else if (m_client.m_player.m_class == 4 | m_client.m_player.m_class == 6 | m_client.m_player.m_class == 8 | m_client.m_player.m_class == 10)
                    {
                        if (m_client.m_player.m_stats.strenght.m_bases < 101) count = 1;
                        if (m_client.m_player.m_stats.strenght.m_bases > 100) count = 2;
                        if (m_client.m_player.m_stats.strenght.m_bases > 200) count = 3;
                        if (m_client.m_player.m_stats.strenght.m_bases > 300) count = 4;
                        if (m_client.m_player.m_stats.strenght.m_bases > 400) count = 5;
                    }

                    else if (m_client.m_player.m_class == 11)
                    {
                        count = 3;
                    }

                    else if (m_client.m_player.m_class == 12)
                    {
                        if (m_client.m_player.m_stats.strenght.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.strenght.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.strenght.m_bases > 200) count = 3;
                    }

                    if (m_client.m_player.m_charactPoint >= count)
                    {
                        m_client.m_player.m_stats.strenght.m_bases += 1;
                        m_client.m_player.m_charactPoint -= count;
                        m_client.m_player.SendChararacterStats();
                    }
                    else
                        m_client.Send("ABE");

                    break;

                case 15:

                    if (m_client.m_player.m_class == 1 | m_client.m_player.m_class == 2 | m_client.m_player.m_class == 5 | m_client.m_player.m_class == 7 | m_client.m_player.m_class == 10)
                    {
                        if (m_client.m_player.m_stats.intelligence.m_bases < 101) count = 1;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 100) count = 2;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 200) count = 3;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 300) count = 4;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 400) count = 5;
                    }

                    else if (m_client.m_player.m_class == 3)
                    {
                        if (m_client.m_player.m_stats.intelligence.m_bases < 21) count = 1;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 20) count = 2;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 60) count = 3;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 100) count = 4;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 140) count = 5;
                    }

                    else if (m_client.m_player.m_class == 4)
                    {
                        if (m_client.m_player.m_stats.intelligence.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 150) count = 3;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 250) count = 4;
                    }

                    else if (m_client.m_player.m_class == 6 | m_client.m_player.m_class == 8)
                    {
                        if (m_client.m_player.m_stats.intelligence.m_bases < 21) count = 1;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 20) count = 2;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 40) count = 3;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 60) count = 4;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 80) count = 5;
                    }

                    else if (m_client.m_player.m_class == 9)
                    {
                        if (m_client.m_player.m_stats.intelligence.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 150) count = 3;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 250) count = 4;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 350) count = 5;
                    }

                    else if (m_client.m_player.m_class == 11)
                    {
                        count = 3;
                    }

                    else if (m_client.m_player.m_class == 12)
                    {
                        if (m_client.m_player.m_stats.intelligence.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.intelligence.m_bases > 200) count = 3;
                    }

                    if (m_client.m_player.m_charactPoint >= count)
                    {
                        m_client.m_player.m_stats.intelligence.m_bases += 1;
                        m_client.m_player.m_charactPoint -= count;
                        m_client.m_player.SendChararacterStats();
                    }
                    else
                        m_client.Send("ABE");

                    break;

                case 13:

                    if (m_client.m_player.m_class == 1 | m_client.m_player.m_class == 4 | m_client.m_player.m_class == 5
                        | m_client.m_player.m_class == 6 | m_client.m_player.m_class == 7 | m_client.m_player.m_class == 8 | m_client.m_player.m_class == 9)
                    {
                        if (m_client.m_player.m_stats.luck.m_bases < 21) count = 1;
                        if (m_client.m_player.m_stats.luck.m_bases > 20) count = 2;
                        if (m_client.m_player.m_stats.luck.m_bases > 40) count = 3;
                        if (m_client.m_player.m_stats.luck.m_bases > 60) count = 4;
                        if (m_client.m_player.m_stats.luck.m_bases > 80) count = 5;
                    }

                    else if (m_client.m_player.m_class == 2 | m_client.m_player.m_class == 10)
                    {
                        if (m_client.m_player.m_stats.luck.m_bases < 101) count = 1;
                        if (m_client.m_player.m_stats.luck.m_bases > 100) count = 2;
                        if (m_client.m_player.m_stats.luck.m_bases > 200) count = 3;
                        if (m_client.m_player.m_stats.luck.m_bases > 300) count = 4;
                        if (m_client.m_player.m_stats.luck.m_bases > 400) count = 5;
                    }

                    else if (m_client.m_player.m_class == 3)
                    {
                        if (m_client.m_player.m_stats.luck.m_bases < 101) count = 1;
                        if (m_client.m_player.m_stats.luck.m_bases > 100) count = 2;
                        if (m_client.m_player.m_stats.luck.m_bases > 150) count = 3;
                        if (m_client.m_player.m_stats.luck.m_bases > 230) count = 4;
                        if (m_client.m_player.m_stats.luck.m_bases > 330) count = 5;
                    }

                    else if (m_client.m_player.m_class == 11)
                    {
                        count = 3;
                    }

                    else if (m_client.m_player.m_class == 12)
                    {
                        if (m_client.m_player.m_stats.luck.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.luck.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.luck.m_bases > 200) count = 3;
                    }

                    if (m_client.m_player.m_charactPoint >= count)
                    {
                        m_client.m_player.m_stats.luck.m_bases += 1;
                        m_client.m_player.m_charactPoint -= count;
                        m_client.m_player.SendChararacterStats();
                    }
                    else
                        m_client.Send("ABE");

                    break;

                case 14:

                    if (m_client.m_player.m_class == 1 | m_client.m_player.m_class == 2 | m_client.m_player.m_class == 3 | m_client.m_player.m_class == 5
                        | m_client.m_player.m_class == 7 | m_client.m_player.m_class == 8 | m_client.m_player.m_class == 10)
                    {
                        if (m_client.m_player.m_stats.agility.m_bases < 21) count = 1;
                        if (m_client.m_player.m_stats.agility.m_bases > 20) count = 2;
                        if (m_client.m_player.m_stats.agility.m_bases > 40) count = 3;
                        if (m_client.m_player.m_stats.agility.m_bases > 60) count = 4;
                        if (m_client.m_player.m_stats.agility.m_bases > 80) count = 5;
                    }

                    else if (m_client.m_player.m_class == 4)
                    {
                        if (m_client.m_player.m_stats.agility.m_bases < 101) count = 1;
                        if (m_client.m_player.m_stats.agility.m_bases > 100) count = 2;
                        if (m_client.m_player.m_stats.agility.m_bases > 200) count = 3;
                        if (m_client.m_player.m_stats.agility.m_bases > 300) count = 4;
                        if (m_client.m_player.m_stats.agility.m_bases > 400) count = 5;
                    }

                    else if (m_client.m_player.m_class == 6 | m_client.m_player.m_class == 9)
                    {
                        if (m_client.m_player.m_stats.agility.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.agility.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.agility.m_bases > 100) count = 3;
                        if (m_client.m_player.m_stats.agility.m_bases > 150) count = 4;
                        if (m_client.m_player.m_stats.agility.m_bases > 200) count = 5;
                    }

                    else if (m_client.m_player.m_class == 11)
                    {
                        count = 3;
                    }

                    else if (m_client.m_player.m_class == 12)
                    {
                        if (m_client.m_player.m_stats.agility.m_bases < 51) count = 1;
                        if (m_client.m_player.m_stats.agility.m_bases > 50) count = 2;
                        if (m_client.m_player.m_stats.agility.m_bases > 200) count = 3;
                    }

                    if (m_client.m_player.m_charactPoint >= count)
                    {
                        m_client.m_player.m_stats.agility.m_bases += 1;
                        m_client.m_player.m_charactPoint -= count;
                        m_client.m_player.SendChararacterStats();
                    }
                    else
                        m_client.Send("ABE");

                    break;
            }
        }

        #endregion

        #region Spells

        void SpellBoost(string _datas)
        {
            try
            {
                var spellID = int.Parse(_datas);

                if (!m_client.m_player.m_spellInventary.m_spells.Any(x => x.m_id == spellID))
                {
                    m_client.Send("SUE");
                    return;
                }

                var level = m_client.m_player.m_spellInventary.m_spells.First(x => x.m_id == spellID).m_level;

                if (m_client.m_player.m_spellPoint < level || level >= 6)
                {
                    m_client.Send("SUE");
                    return;
                }

                m_client.m_player.m_spellPoint -= level;
                
                m_client.m_player.m_spellInventary.m_spells.First(x => x.m_id == spellID).m_level++;

                m_client.Send(string.Format("SUK{0}~{1}", spellID, level + 1));
                m_client.m_player.SendChararacterStats();
            }
            catch { }
        }

        void SpellMove(string _datas)
        {
            try
            {
                m_client.Send("BN");

                var datas = _datas.Split('|');
                var spellID = int.Parse(datas[0]);
                var newPos = int.Parse(datas[1]);

                if (!m_client.m_player.m_spellInventary.m_spells.Any(x => x.m_id == spellID))
                    return;

                if (m_client.m_player.m_spellInventary.m_spells.Any(x => x.m_position == newPos))
                {
                    m_client.m_player.m_spellInventary.m_spells.First(x => x.m_position == newPos).m_position = 25;
                    m_client.m_player.m_spellInventary.m_spells.First(x => x.m_id == spellID).m_position = newPos;
                }
                else
                    m_client.m_player.m_spellInventary.m_spells.First(x => x.m_id == spellID).m_position = newPos;
            }
            catch { }
        }

        #endregion

        #region Exchange

        private void RequestExchange(string _datas)
        {
            try
            {
                if (m_client.m_player == null || m_client.m_player.m_state.Occuped)
                {
                    m_client.Send("BN");
                    return;
                }

                var packet = _datas.Split('|');

                switch (int.Parse(packet[0]))
                {
                    case 0://NPC BUY/SELL

                        var NPC = m_client.m_player.GetMap().m_npcs.First(x => x.m_idOnMap == int.Parse(packet[1]));
                        if (NPC.m_model.m_sellingList.Count == 0)
                        {
                            m_client.Send("BN");
                            return;
                        }

                        m_client.m_player.m_state.onExchange = true;
                        m_client.m_player.m_state.actualNPC = NPC.m_idOnMap;

                        m_client.Send(string.Format("ECK0|{0}", NPC.m_idOnMap));

                        var newPacket = "EL";
                        foreach (var i in NPC.m_model.m_sellingList)
                        {
                            var item = Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == i);

                            newPacket += string.Format("{0};{1}|", i, item.EffectInfos());
                        }

                        m_client.Send(newPacket.Substring(0, newPacket.Length - 1));

                        break;
                }
            }
            catch { }
        }

        private void CancelExchange(string t)
        {
            m_client.Send("EV");
            m_client.m_player.m_state.onExchange = false;
        }

        private void BuyFromNPC(string packet)
        {
            try
            {
                if (!m_client.m_player.m_state.onExchange)
                {
                    m_client.Send("OBE");
                    return;
                }

                var datas = packet.Split('|');
                var itemID = int.Parse(datas[0]);
                var quantity = int.Parse(datas[1]);

                var item = Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == itemID);
                var NPC = m_client.m_player.GetMap().m_npcs.First(x => x.m_idOnMap == m_client.m_player.m_state.actualNPC);

                if (quantity <= 0 || !NPC.m_model.m_sellingList.Contains(itemID))
                {
                    m_client.Send("OBE");
                    return;
                }

                var price = item.m_price * quantity;

                if (m_client.m_player.m_kamas >= price)
                {
                    var newItem = new DofusOrigin.Realm.Characters.Items.CharacterItem(item);
                    newItem.GeneratItem(4);
                    newItem.m_quantity = quantity;


                    m_client.m_player.m_kamas -= price;
                    m_client.Send("EBK");
                    m_client.m_player.m_inventary.AddItem(newItem, false);
                }
                else
                    m_client.Send("OBE");
            }
            catch { }
        }

        private void SellFromNPC(string _datas)
        {
            try
            {
                if (!m_client.m_player.m_state.Occuped)
                {
                    m_client.Send("OSE");
                    return;
                }

                var packet = _datas.Split('|');
                var itemID = int.Parse(packet[0]);
                var quantity = int.Parse(packet[1]);

                if (!m_client.m_player.m_inventary.m_itemsList.Any(x => x.m_id == itemID) || quantity <= 0)
                {
                    m_client.Send("OSE");
                    return;
                }

                var item = m_client.m_player.m_inventary.m_itemsList.First(x => x.m_id == itemID);

                if (item.m_quantity < quantity)
                    quantity = item.m_quantity;

                var price = Math.Floor((double)item.m_base.m_price / 10) * quantity;

                if (price < 1)
                    price = 1;

                m_client.m_player.m_kamas += (int)price;
                m_client.m_player.m_inventary.DeleteItem(item.m_id, quantity);
                m_client.Send("ESK");
            }
            catch { }
        }

        #endregion

        #endregion
    }
}
