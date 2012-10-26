using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network.Realm
{
    class RealmCommand
    {
        public RealmClient Client;

        public RealmCommand(RealmClient Cli)
        {
            Client = Cli;
        }

        public void ParseCommand(string Args)
        {
            try
            {
                string[] Datas = Args.Split(' ');

                if (Client.myInfos.myLevel > 0)
                {
                    switch (Datas[0])
                    {
                        case "save":

                            ParseCommanSave(Datas);
                            break;

                        case "vita":
                            ParseCommandVita(Datas);
                            break;

                        case "item":
                            ParseCommandItem(Datas);
                            break;

                        case "teleport":
                            ParseCommandTeleport(Datas);
                            break;

                        case "exp":
                            ParseCommandExp(Datas);
                            break;

                        case "help":
                            ParseCommandHelp();
                            break;

                        default:
                            Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                            Client.SendConsoleMessage("Use the command 'Help' for more informations !");
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");

                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot parse command from <{0}> because : {1}", Client.myIp(), e.ToString()));
            }
        }

        #region CommandInfos

        void ParseCommanSave(string[] datas)
        {
            try
            {
                if (datas.Length <= 1)
                {
                    realm.Realm.World.Save.SaveWorld();
                    return;
                }

                switch (datas[1])
                {
                    case "all":
                        realm.Realm.World.Save.SaveWorld();
                        break;

                    case "char":
                        realm.Realm.World.Save.SaveChararacters();
                        break;

                    default:
                        realm.Realm.World.Save.SaveWorld();
                        break;
                }
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        void ParseCommandItem(string[] datas)
        {
            try
            {
                if (datas.Length == 2)
                {
                    Client.myPlayer.myInventary.AddItem(int.Parse(datas[1]), false);
                    Client.SendConsoleMessage("Item Added !");
                }

                else if (datas.Length == 3)
                {
                    Client.myPlayer.myInventary.AddItem(int.Parse(datas[1]), false, int.Parse(datas[2]));
                    Client.SendConsoleMessage("Item Added !");
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !", 0);
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        void ParseCommandExp(string[] datas)
        {
            try
            {
                if (datas.Length == 2)
                {
                    Client.myPlayer.AddExp(long.Parse(datas[1]));
                    Client.SendConsoleMessage("Exp Added !");
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !", 0);
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        
        }

        void ParseCommandTeleport(string[] datas)
        {
            try
            {
                if (datas.Length == 3)
                {
                    Client.myPlayer.TeleportNewMap(int.Parse(datas[1]), int.Parse(datas[2]));
                    Client.SendConsoleMessage("Character Teleported !");
                }

                else if (datas.Length == 4)
                {
                    var myMap = Database.Cache.MapsCache.MapsList.First(x => x.myMap.x == int.Parse(datas[1]) && x.myMap.y == int.Parse(datas[2]));
                    Client.myPlayer.TeleportNewMap(myMap.myMap.myId, int.Parse(datas[3]));
                    Client.SendConsoleMessage("Character Teleported !");
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !", 0);
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        void ParseCommandVita(string[] datas)
        {
            try
            {
                if (datas.Length == 2)
                {
                    Client.myPlayer.ResetVita(datas[1]);
                    Client.SendConsoleMessage("Vita Updated !");
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !", 0);
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        void ParseCommandHelp()
        {
            Client.SendConsoleMessage("Commands avaliables :");
            Client.SendConsoleMessage("save : <Optional all|char>");
            Client.SendConsoleMessage("vita : <number>");
            Client.SendConsoleMessage("teleport : <x|y> <cell> || <mapid> <cell>");
            Client.SendConsoleMessage("exp : <number>");
            Client.SendConsoleMessage("item : <itemid> <Optional 1|2|3|4>");
        }

        #endregion

    }
}
