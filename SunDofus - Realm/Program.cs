﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm
{
    class Program
    {
        public static int m_ServerID = -1;

        public static Network.SelectorLink m_RealmLink;
        public static Network.AuthenticationServer m_AuthServer;

        static void Main(string[] args)
        {
            Config.ConfigurationManager.IniConfig();
            SunDofus.Logger.Debug = Config.ConfigurationManager.GetBool("Debug");
            Console.Title = "SunDofus - Realm ~ " + m_ServerID + " | Shaak [c]  2012";

            Database.SQLManager.Initialise();
            Database.Data.ItemSql.LoadItems();
            Database.Data.ItemSql.LoadItemsSets();
            Database.Data.ItemSql.LoadUsablesItems();

            Database.Data.SpellSql.LoadSpells();
            Database.Data.SpellSql.LoadSpellsToLearn();

            Database.Data.MapSql.LoadMaps();
            Database.Data.TriggerSql.LoadTriggers();

            Database.Data.CharacterSql.LoadCharacters();

            m_AuthServer = new Network.AuthenticationServer();
            m_AuthServer.Start();

            m_RealmLink = new Network.SelectorLink();
            m_RealmLink.Start();

            Console.ReadLine();
        }
    }
}
