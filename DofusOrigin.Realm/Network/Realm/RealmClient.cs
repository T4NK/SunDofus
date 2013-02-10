using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilverSock;
using DofusOrigin.Realm.Characters;
using DofusOrigin.Database.Models.Clients;

namespace DofusOrigin.Network.Realm
{
    class RealmClient : DofusOrigin.Network.TCPClient
    {
        public bool isAuth { get; set; }

        public Character m_player { get; set; }
        public List<Character> m_characters { get; set; }
        public AccountModel m_infos { get; set; }
        public RealmCommand m_commander { get; set; }

        object m_packetLocker { get; set; }
        RealmParser m_parser { get; set; }

        public RealmClient(SilverSocket _socket) :  base(_socket)
        {
            m_packetLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.ReceivedPackets);

            m_characters = new List<DofusOrigin.Realm.Characters.Character>();
            m_commander = new RealmCommand(this);
            m_parser = new RealmParser(this);

            m_player = null;
            isAuth = false;

            Send("HG");
        }

        public void Send(string _message)
        {
            this.SendDatas(_message);
            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), _message));
        }

        public void ParseCharacters()
        {
            foreach (var name in m_infos.m_characters)
            {
                if (!DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.m_name == name))
                {
                    Network.ServersHandler.m_authLinks.Send(string.Format("SDAC|{0}|{1}", m_infos.m_id, name));
                    continue;
                }

                var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.m_name == name);
                m_characters.Add(character);
            }
        }

        public void SendGifts()
        {
            m_infos.ParseGifts();

            foreach (var gift in m_infos.m_gifts)
            {
                if (Database.Cache.ItemsCache.m_itemsList.Any(x => x.m_id == gift.m_itemID) == false)
                    return;

                var item = new DofusOrigin.Realm.Characters.Items.CharacterItem(Database.Cache.ItemsCache.m_itemsList.First(x => x.m_id == gift.m_itemID));

                item.GeneratItem();

                gift.m_item = item;

                this.Send(string.Format("Ag1|{0}|{1}|{2}|{3}|{4}~{5}~{6}~~{7};", gift.m_id, gift.m_title, gift.m_message, (gift.m_image != "" ? gift.m_image : "http://s2.e-monsite.com/2009/12/26/04/167wpr7.png"),
                   Utilities.Basic.DeciToHex(item.m_base.m_id), Utilities.Basic.DeciToHex(item.m_base.m_id), Utilities.Basic.DeciToHex(item.m_quantity), item.EffectsInfos()));
            }
        }

        public void SendConsoleMessage(string _message, int Color = 1)
        {
            Send(string.Format("BAT{0}{1}", Color, _message));
        }

        public void SendMessage(string _message)
        {
            Send(string.Format("cs<font color=\"#FF0000\">{0}</font>", _message));
        }

        void ReceivedPackets(string _datas)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive datas from @<{0}>@ : {1}", myIp(), _datas));

            lock (m_packetLocker)
                m_parser.Parse(_datas);
        }

        void Disconnected()
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed client @<{0}>@ connection !", myIp()));

            if (isAuth == true)
            {
                Network.ServersHandler.m_authLinks.Send(string.Format("SND|{0}", m_infos.m_pseudo));

                if (m_player != null)
                {
                    m_player.GetMap().DelPlayer(m_player);
                    m_player.isConnected = false;

                    if (m_player.m_state.onExchange)
                        DofusOrigin.Realm.Exchanges.ExchangesManager.LeaveExchange(m_player);

                    if (m_player.m_state.onWaitingParty)
                    {
                        try
                        {
                            if (m_player.m_state.receiverInviteParty != -1 || m_player.m_state.senderInviteParty != -1)
                            {
                                var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First
                                    (x => x.m_id == (m_player.m_state.receiverInviteParty != -1 ? m_player.m_state.receiverInviteParty : m_player.m_state.senderInviteParty));
                                if (character.isConnected)
                                {
                                    character.m_state.senderInviteParty = -1;
                                    character.m_state.receiverInviteParty = -1;
                                    character.m_state.onWaitingParty = false;
                                    character.m_networkClient.Send("PR");
                                }

                                m_player.m_state.receiverInviteParty = -1;
                                m_player.m_state.senderInviteParty = -1;
                                m_player.m_state.onWaitingParty = false;
                            }
                        }
                        catch { }
                    }

                    if (m_player.m_state.Party != null)
                        m_player.m_state.Party.LeaveParty(m_player.m_name);

                    if (m_player.m_state.isFollowing)
                    {
                        if(DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.m_state.Followers.Contains(m_player) && x.m_id == m_player.m_state.followingID))
                            DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.m_id == m_player.m_state.followingID).m_state.Followers.Remove(m_player);
                    }

                    if (m_player.m_state.isFollow)
                    {
                        m_player.m_state.Followers.Clear();
                        m_player.m_state.isFollow = false;
                    }
                }
            }
            Network.ServersHandler.m_realmServer.m_clients.Remove(this);
        }
    }
}
