using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Utilities;
using SunDofus.Auth.Entities;
using SunDofus.Auth.Entities.Requests;
using SunDofus.World.Network;

namespace SunDofus
{
    class Program
    {
        static void Main(string[] args)
        {
            Basic.Uptime = Environment.TickCount;
            Console.Title = "SunDofus";

            Config.LoadConfiguration();
            Loggers.InitializeLoggers();

            if (Config.GetBoolElement("Realm"))
            {
                try
                {
                    Auth.Network.ServersHandler.InitialiseServers();
                    Auth.Entities.DatabaseProvider.InitializeConnection();

                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
            }

            if (Config.GetBoolElement("World"))
            {
                try
                {
                    Console.Title = string.Format("{0} | Server '{1}'", Console.Title, Config.GetIntElement("ServerID"));

                    World.Entities.DatabaseHandler.InitialiseConnection();

                    World.Entities.Cache.LevelsCache.LoadLevels();

                    World.Entities.Cache.ItemsCache.LoadItems();
                    World.Entities.Cache.ItemsCache.LoadItemsSets();
                    World.Entities.Cache.ItemsCache.LoadUsablesItems();

                    World.Entities.Cache.SpellsCache.LoadSpells();
                    World.Entities.Cache.SpellsCache.LoadSpellsToLearn();

                    World.Entities.Cache.MonstersCache.LoadMonsters();
                    World.Entities.Cache.MonstersCache.LoadMonstersLevels();

                    World.Entities.Cache.MapsCache.LoadMaps();
                    World.Entities.Cache.TriggersCache.LoadTriggers();

                    World.Entities.Cache.NoPlayerCharacterCache.LoadNPCsAnswers();
                    World.Entities.Cache.NoPlayerCharacterCache.LoadNPCsQuestions();
                    World.Entities.Cache.NoPlayerCharacterCache.LoadNPCs();

                    World.Entities.Cache.CharactersCache.LoadCharacters();

                    World.Entities.Cache.AuthsCache.ReloadAuths();

                    World.Network.ServersHandler.InitialiseServers();
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
            }

            Loggers.InfosLogger.Write(string.Format("Started in '{0}'ms !", Basic.Uptime));
            Console.ReadLine();
        }
    }
}
