using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.World
{
    class Save
    {        
        public static void SaveWorld()
        {
            SaveChararacters();
        }

        public static void SaveChararacters()
        {
            Network.ServersHandler.m_authLinks.Send("SSM");

            foreach (var character in Characters.CharactersManager.m_charactersList)
            {
                Database.Cache.CharactersCache.SaveCharacter(character);
                System.Threading.Thread.Sleep(100);
            }

            Network.ServersHandler.m_authLinks.Send("STM");
        }
    }
}
