using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Realm.World
{
    class Save
    {        
        public static void SaveWorld()
        {
            SaveChararacters();
        }

        public static void SaveChararacters()
        {
            Network.ServersHandler.AuthLinks.Send("SSM");

            foreach (var character in Characters.CharactersManager.CharactersList)
            {
                if (character.isConnected)
                    character.NetworkClient.Send("Im1164");

                Database.Cache.CharactersCache.SaveCharacter(character);
                System.Threading.Thread.Sleep(100);

                if (character.isConnected)
                    character.NetworkClient.Send("Im1165");
            }

            Network.ServersHandler.AuthLinks.Send("STM");
        }
    }
}
