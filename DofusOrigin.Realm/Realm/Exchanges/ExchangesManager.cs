using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Exchanges
{
    class ExchangesManager
    {
        public static List<Exchange> Exchanges = new List<Exchange>();

        public static void AddExchange(Characters.Character traider, Characters.Character traided)
        {
            lock(Exchanges)
                Exchanges.Add(new Exchange(traider, traided));

            traider.m_state.onExchangePanel = true;
            traided.m_state.onExchangePanel = true;

            traider.m_state.actualPlayerExchange = traided.m_id;
            traided.m_state.actualPlayerExchange = traider.m_id;

            traider.m_networkClient.Send("ECK1");
            traided.m_networkClient.Send("ECK1");
        }

        public static void LeaveExchange(Characters.Character canceler, bool must = true)
        {
            try
            {
                if (canceler.m_state.actualNPC != -1)
                {
                    canceler.m_state.actualNPC = -1;
                    canceler.m_state.onExchange = false;
                }

                if (canceler.m_state.actualTraided != -1)
                {
                    lock (DofusOrigin.Realm.Characters.CharactersManager.CharactersList)
                    {
                        if (DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.m_id == canceler.m_state.actualTraided))
                        {
                            var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.m_id == canceler.m_state.actualTraided);

                            if (character.isConnected == true && must)
                                character.m_networkClient.Send("EV");

                            canceler.m_state.actualTraided = -1;
                            canceler.m_state.actualPlayerExchange = -1;
                            canceler.m_state.onExchange = false;
                            canceler.m_state.onExchangePanel = false;
                            canceler.m_state.onExchangeAccepted = false;

                            character.m_state.actualTraider = -1;
                            character.m_state.actualPlayerExchange = -1;
                            character.m_state.onExchange = false;
                            character.m_state.onExchangePanel = false;
                            character.m_state.onExchangeAccepted = false;

                            lock (Exchanges)
                            {
                                if (Exchanges.Any(x => (x.player1 == canceler && x.player2 == character) || (x.player2 == canceler && x.player1 == character)))
                                    Exchanges.Remove(Exchanges.First(x => (x.player1 == canceler && x.player2 == character) || (x.player2 == canceler && x.player1 == character)));
                            }
                        }
                    }
                }

                if (canceler.m_state.actualTraider != -1)
                {
                    lock (DofusOrigin.Realm.Characters.CharactersManager.CharactersList)
                    {
                        if (DofusOrigin.Realm.Characters.CharactersManager.CharactersList.Any(x => x.m_id == canceler.m_state.actualTraider))
                        {
                            var character = DofusOrigin.Realm.Characters.CharactersManager.CharactersList.First(x => x.m_id == canceler.m_state.actualTraider);

                            if (character.isConnected == true && must)
                                character.m_networkClient.Send("EV");

                            canceler.m_state.actualTraider = -1;
                            canceler.m_state.actualPlayerExchange = -1;
                            canceler.m_state.onExchange = false;
                            canceler.m_state.onExchangePanel = false;
                            canceler.m_state.onExchangeAccepted = false;

                            character.m_state.actualTraided = -1;
                            character.m_state.actualPlayerExchange = -1;
                            character.m_state.onExchange = false;
                            character.m_state.onExchangePanel = false;
                            character.m_state.onExchangeAccepted = false;

                            lock (Exchanges)
                            {
                                if (Exchanges.Any(x => (x.player1 == canceler && x.player2 == character) || (x.player2 == canceler && x.player1 == character)))
                                    Exchanges.Remove(Exchanges.First(x => (x.player1 == canceler && x.player2 == character) || (x.player2 == canceler && x.player1 == character)));
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}
