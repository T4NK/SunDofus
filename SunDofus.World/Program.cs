using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus
{
    class Program
    {
        static void Main(string[] args)
        {
            //var date = new DateTime(2013, 3, 2, 12, 00, 00, 00);

            //if (DateTime.Now > date)
            //    Environment.Exit(0);

            try
            {
                Utilities.Config.LoadConfiguration();
                Utilities.Loggers.InitialiseLoggers();

                Console.Title = string.Format("DofusOrigin.Realm ~ {0} | Shaak [c]",
                    Utilities.Config.GetConfig.GetIntElement("ServerId"));

                Database.DatabaseHandler.InitialiseConnection();

                Database.Cache.LevelsCache.LoadLevels();

                Database.Cache.ItemsCache.LoadItems();
                Database.Cache.ItemsCache.LoadItemsSets();
                Database.Cache.ItemsCache.LoadUsablesItems();

                Database.Cache.SpellsCache.LoadSpells();
                Database.Cache.SpellsCache.LoadSpellsToLearn();

                Database.Cache.MonstersCache.LoadMonsters();
                Database.Cache.MonstersCache.LoadMonstersLevels();

                Database.Cache.MapsCache.LoadMaps();
                Database.Cache.TriggersCache.LoadTriggers();

                Database.Cache.NoPlayerCharacterCache.LoadNPCsAnswers();
                Database.Cache.NoPlayerCharacterCache.LoadNPCsQuestions();
                Database.Cache.NoPlayerCharacterCache.LoadNPCs();

                Database.Cache.CharactersCache.LoadCharacters();

                Database.Cache.AuthsCache.ReloadAuths();

                Network.ServersHandler.InitialiseServers();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.ReadLine();
        }
    }
}
