using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Exchanges
{
    class Exchange
    {
        public Characters.Character player1;
        public Characters.Character player2;

        public Exchange(Characters.Character new1, Characters.Character new2)
        {
            player1 = new1;
            player2 = new2;
        }
    }
}
