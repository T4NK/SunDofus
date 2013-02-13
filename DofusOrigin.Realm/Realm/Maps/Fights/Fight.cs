using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps.Fights
{
    class Fight
    {
        private FightType _type;

        //private Characters.Character _actualPlayer;
        private List<Characters.Character> _players;

        public Fight(Characters.Character player1, Characters.Character player2, FightType type)
        {
            _players = new List<Characters.Character>();
            _players.Add(player1);
            _players.Add(player2);

            _type = type;
        }

        public void AddPlayer(Characters.Character player, int team)
        {
        }

        public enum FightType
        {
            PvP,
            PvM,
            Challenge,
            Collector,
        }
    }
}
