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
            Network.ServersHandler.myAuthLink.Send("SSM");

            foreach (var myCharacter in Character.CharactersManager.CharactersList)
            {
                Database.Cache.CharactersCache.SaveCharacter(myCharacter);
                System.Threading.Thread.Sleep(100);
            }

            Network.ServersHandler.myAuthLink.Send("STM");
        }
    }
}
