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
        public bool isAuth;

        public Character Player;
        public List<Character> Characters;
        public AccountModel Infos;
        public RealmCommand Commander;

        private object _packetLocker;
        private RealmParser _parser;

        public RealmClient(SilverSocket socket) :  base(socket)
        {
            _packetLocker = new object();

            this.DisconnectedSocket += new DisconnectedSocketHandler(this.Disconnected);
            this.ReceivedDatas += new ReceiveDatasHandler(this.ReceivedPackets);

            Characters = new List<DofusOrigin.Realm.Characters.Character>();
            Commander = new RealmCommand(this);
            _parser = new RealmParser(this);

            Player = null;
            isAuth = false;

            Send("HG");
        }

        public void Send(string message)
        {
            lock(_packetLocker)
                this.SendDatas(message);

            Utilities.Loggers.InfosLogger.Write(string.Format("Sent to @<{0}>@ : {1}", myIp(), message));
        }

        public void ParseCharacters()
        {
            foreach (var name in Infos.Characters)
            {
                if (!DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.Name == name))
                {
                    Network.ServersHandler.AuthLinks.Send(string.Format("SDAC|{0}|{1}", Infos.ID, name));
                    continue;
                }

                var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.Name == name);
                Characters.Add(character);
            }
        }

        public void SendGifts()
        {
            Infos.ParseGifts();

            foreach (var gift in Infos.Gifts)
            {
                if (Database.Cache.ItemsCache.ItemsList.Any(x => x.ID == gift.ItemID) == false)
                    return;

                var item = new DofusOrigin.Realm.Characters.Items.CharacterItem(Database.Cache.ItemsCache.ItemsList.First(x => x.ID == gift.ItemID));

                item.GeneratItem();

                gift.Item = item;

                this.Send(string.Format("Ag1|{0}|{1}|{2}|{3}|{4}~{5}~{6}~~{7};", gift.ID, gift.Title, gift.Message, (gift.Image != "" ? gift.Image : "http://s2.e-monsite.com/2009/12/26/04/167wpr7.png"),
                   Utilities.Basic.DeciToHex(item.Model.ID), Utilities.Basic.DeciToHex(item.Model.ID), Utilities.Basic.DeciToHex(item.Quantity), item.EffectsInfos()));
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

        private void ReceivedPackets(string _datas)
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("Receive datas from @<{0}>@ : {1}", myIp(), _datas));

            lock (_packetLocker)
                _parser.Parse(_datas);
        }

        private void Disconnected()
        {
            Utilities.Loggers.InfosLogger.Write(string.Format("New closed client @<{0}>@ connection !", myIp()));

            if (isAuth == true)
            {
                Network.ServersHandler.AuthLinks.Send(string.Format("SND|{0}", Infos.Pseudo));

                if (Player != null)
                {
                    Player.GetMap().DelPlayer(Player);
                    Player.isConnected = false;

                    if (Player.State.onExchange)
                        DofusOrigin.Realm.Exchanges.ExchangesManager.LeaveExchange(Player);

                    if (Player.State.onWaitingParty)
                    {
                        if (Player.State.receiverInviteParty != -1 || Player.State.senderInviteParty != -1)
                        {
                            if (DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any
                                (x => x.ID == (Player.State.receiverInviteParty != -1 ? Player.State.receiverInviteParty : Player.State.senderInviteParty)))
                            {

                                var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First
                                    (x => x.ID == (Player.State.receiverInviteParty != -1 ? Player.State.receiverInviteParty : Player.State.senderInviteParty));
                                if (character.isConnected)
                                {
                                    character.State.senderInviteParty = -1;
                                    character.State.receiverInviteParty = -1;
                                    character.State.onWaitingParty = false;
                                    character.NetworkClient.Send("PR");
                                }

                                Player.State.receiverInviteParty = -1;
                                Player.State.senderInviteParty = -1;
                                Player.State.onWaitingParty = false;
                            }
                        }
                    }

                    if (Player.State.Party != null)
                        Player.State.Party.LeaveParty(Player.Name);

                    if (Player.State.isFollowing)
                    {
                        if(DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.State.Followers.Contains(Player) && x.ID == Player.State.followingID))
                            DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.ID == Player.State.followingID).State.Followers.Remove(Player);
                    }

                    if (Player.State.isFollow)
                    {
                        Player.State.Followers.Clear();
                        Player.State.isFollow = false;
                    }

                    if (Player.State.isChallengeAsked)
                    {
                        if (DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.State.ChallengeAsked == Player.ID))
                        {
                            var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.State.ChallengeAsked == Player.ID);

                            Player.State.ChallengeAsker = -1;
                            Player.State.isChallengeAsked = false;

                            character.State.ChallengeAsked = -1;
                            character.State.isChallengeAsker = false;

                            character.NetworkClient.Send(string.Format("GA;902;{0};{1}", character.ID, Player.ID));
                        }
                    }

                    if (Player.State.isChallengeAsker)
                    {
                        if (DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.State.ChallengeAsker == Player.ID))
                        {
                            var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.State.ChallengeAsker == Player.ID);

                            Player.State.ChallengeAsked = -1;
                            Player.State.isChallengeAsker = false;

                            character.State.ChallengeAsker = -1;
                            character.State.isChallengeAsked = false;

                            character.NetworkClient.Send(string.Format("GA;902;{0};{1}", character.ID, Player.ID));
                        }
                    }
                }
            }

            if (isAuth)
            {
                lock (ServersHandler.RealmServer.PseudoClients)
                    ServersHandler.RealmServer.PseudoClients.Remove(Infos.Pseudo);
            }

            lock(Network.ServersHandler.RealmServer.Clients)
                Network.ServersHandler.RealmServer.Clients.Remove(this);
        }
    }
}
