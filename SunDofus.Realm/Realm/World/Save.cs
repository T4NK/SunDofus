using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.World
{
    class Save
    {
        public static void ParseSave(string Data)
        {
            switch (Data)
            {
                case "all":
                    SaveWorld();
                    break;

                case "char":
                    SaveChar();
                    break;
            }
        }
        
        public static void SaveWorld()
        {

        }

        public static void SaveChar()
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
