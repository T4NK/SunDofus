using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.World
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

            lock (Characters.CharactersManager.CharactersList)
            {
                foreach (var character in Characters.CharactersManager.CharactersList)
                {
                    if (character.isConnected)
                        character.m_networkClient.Send("Im1164");

                    Database.Cache.CharactersCache.SaveCharacter(character);
                    System.Threading.Thread.Sleep(100);

                    if (character.isConnected)
                        character.m_networkClient.Send("Im1165");
                }
            }

            Network.ServersHandler.m_authLinks.Send("STM");
        }
    }
}
