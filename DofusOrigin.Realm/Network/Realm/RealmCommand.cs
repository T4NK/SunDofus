using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Network.Realm
{
    class RealmCommand
    {
        public RealmClient Client;

        public RealmCommand(RealmClient client)
        {
            Client = client;
        }

        public void ParseCommand(string args)
        {
            try
            {
                Utilities.Loggers.CommandsLogger.Write(string.Format("Command [{0}] by [{1}]", args, Client.Infos.Pseudo));

                var datas = args.Split(' ');

                if (Client.Infos.GMLevel > 0)
                {
                    switch (datas[0])
                    {
                        case "save":
                            ParseCommanSave(datas);
                            break;

                        case "vita":
                            ParseCommandVita(datas);
                            break;

                        case "item":
                            ParseCommandItem(datas);
                            break;

                        case "kamas":
                            ParseCommandKamas(datas);
                            break;

                        case "teleport":
                            ParseCommandTeleport(datas);
                            break;

                        case "exp":
                            ParseCommandExp(datas);
                            break;

                        case "map":
                            ParseMapCommand(datas);
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

        private void ParseCommandKamas(string[] datas)
        {
            try
            {                
                Client.Player.Kamas += int.Parse(datas[1]);
                Client.SendConsoleMessage("Kamas Added", 0);
                Client.Player.SendChararacterStats();
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        private void ParseCommanSave(string[] datas)
        {
            try
            {
                if (datas.Length <= 1)
                {
                    DofusOrigin.Realm.World.Save.SaveWorld();
                    return;
                }

                switch (datas[1])
                {
                    case "all":
                        DofusOrigin.Realm.World.Save.SaveWorld();
                        Client.SendConsoleMessage("World saved !", 0);
                        break;

                    case "char":
                        DofusOrigin.Realm.World.Save.SaveChararacters();
                        Client.SendConsoleMessage("Characters saved !", 0);
                        break;

                    default:
                        DofusOrigin.Realm.World.Save.SaveWorld();
                        Client.SendConsoleMessage("World saved !", 0);
                        break;
                }
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        private void ParseMapCommand(string[] datas)
        {
            try
            {
                switch (datas[1])
                {
                    case "spawnmobs":

                        Client.Player.GetMap().AddMonstersGroup();
                        break;
                }
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        private void ParseCommandItem(string[] datas)
        {
            try
            {
                var item = Database.Cache.ItemsCache.ItemsList.First(x => x.ID == int.Parse(datas[1]));

                if (datas.Length == 2)
                {
                    var newItem = new DofusOrigin.Realm.Characters.Items.CharacterItem(item);
                    newItem.GeneratItem();

                    Client.Player.ItemsInventary.AddItem(newItem, false);
                    Client.SendConsoleMessage("Item Added !", 0);
                }

                else if (datas.Length == 3)
                {
                    var newItem = new DofusOrigin.Realm.Characters.Items.CharacterItem(item);
                    newItem.GeneratItem(int.Parse(datas[2]));

                    Client.Player.ItemsInventary.AddItem(newItem, false);
                    Client.SendConsoleMessage("Item Added !", 0);
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !");
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        private void ParseCommandExp(string[] datas)
        {
            try
            {
                if (datas.Length == 2)
                {
                    Client.Player.AddExp(long.Parse(datas[1]));
                    Client.SendConsoleMessage("Exp Added !", 0);
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !");
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        
        }

        private void ParseCommandTeleport(string[] datas)
        {
            try
            {
                if (datas.Length == 3)
                {
                    Client.Player.TeleportNewMap(int.Parse(datas[1]), int.Parse(datas[2]));
                    Client.SendConsoleMessage("Character Teleported !", 0);
                }

                else if (datas.Length == 4)
                {
                    var myMap = Database.Cache.MapsCache.MapsList.First(x => x.GetModel.PosX == int.Parse(datas[1]) && x.GetModel.PosY == int.Parse(datas[2]));
                    Client.Player.TeleportNewMap(myMap.GetModel.ID, int.Parse(datas[3]));
                    Client.SendConsoleMessage("Character Teleported !", 0);
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !");
            }
            catch(Exception e)
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
                Client.SendConsoleMessage(e.ToString());
            }
        }

        private void ParseCommandVita(string[] datas)
        {
            try
            {
                if (datas.Length == 2)
                {
                    Client.Player.ResetVita(datas[1]);
                    Client.SendConsoleMessage("Vita Updated !", 0);
                }

                else
                    Client.SendConsoleMessage("Invalid Syntax !");
            }
            catch
            {
                Client.SendConsoleMessage("Cannot parse your AdminCommand !");
                Client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        #endregion

    }
}
