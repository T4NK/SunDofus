using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilities.Config.LoadConfiguration();
            Utilities.Loggers.InitialiseLoggers();

            Console.Title = string.Format("SunDofus.Realm ~ {0} | Shaak [c]",
                Utilities.Config.m_config.GetIntElement("ServerId"));

            Database.DatabaseHandler.InitialiseConnection();

            Database.Cache.LevelsCache.LoadLevels();

            Database.Cache.ItemsCache.LoadItems();
            Database.Cache.ItemsCache.LoadItemsSets();
            Database.Cache.ItemsCache.LoadUsablesItems();

            Database.Cache.SpellsCache.LoadSpells();
            Database.Cache.SpellsCache.LoadSpellsToLearn();

            Database.Cache.MapsCache.LoadMaps();
            Database.Cache.TriggersCache.LoadTriggers();

            Database.Cache.CharactersCache.LoadCharacters();

            Network.ServersHandler.InitialiseServers();

            while(true)
                Console.ReadLine();
        }
    }
}
