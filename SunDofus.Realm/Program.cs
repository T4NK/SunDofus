using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm
{
    class Program
    {
        public static int m_ServerID = -1;

        static void Main(string[] args)
        {
            Utilities.Config.LoadConfiguration();

            Console.Title = "SunDofus - Realm ~ " + m_ServerID + " | Shaak [c]";

            Database.DatabaseHandler.InitialiseConnection();

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
