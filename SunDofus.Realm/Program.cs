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

            Database.Data.ItemSql.LoadItems();
            Database.Data.ItemSql.LoadItemsSets();
            Database.Data.ItemSql.LoadUsablesItems();

            Database.Data.SpellSql.LoadSpells();
            Database.Data.SpellSql.LoadSpellsToLearn();

            Database.Data.MapSql.LoadMaps();
            Database.Data.TriggerSql.LoadTriggers();

            Database.Data.CharacterSql.LoadCharacters();

            Network.ServersHandler.InitialiseServers();

            while(true)
                Console.ReadLine();
        }
    }
}
