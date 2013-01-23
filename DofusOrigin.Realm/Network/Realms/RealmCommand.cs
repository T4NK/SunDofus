using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Network.Realms
{
    class RealmCommand
    {
        public RealmClient m_client;

        public RealmCommand(RealmClient _client)
        {
            m_client = _client;
        }

        public void ParseCommand(string _args)
        {
            try
            {
                var datas = _args.Split(' ');

                if (m_client.m_infos.m_level > 0)
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
                            m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                            m_client.SendConsoleMessage("Use the command 'Help' for more informations !");
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                m_client.SendConsoleMessage("Use the command 'Help' for more informations !");

                Utilities.Loggers.m_errorsLogger.Write(string.Format("Cannot parse command from <{0}> because : {1}", m_client.myIp(), e.ToString()));
            }
        }

        #region CommandInfos

        void ParseCommanSave(string[] _datas)
        {
            try
            {
                if (_datas.Length <= 1)
                {
                    DofusOrigin.Realm.World.Save.SaveWorld();
                    return;
                }

                switch (_datas[1])
                {
                    case "all":
                        DofusOrigin.Realm.World.Save.SaveWorld();
                        m_client.SendConsoleMessage("World saved !", 0);
                        break;

                    case "char":
                        DofusOrigin.Realm.World.Save.SaveChararacters();
                        m_client.SendConsoleMessage("Characters saved !", 0);
                        break;

                    default:
                        DofusOrigin.Realm.World.Save.SaveWorld();
                        m_client.SendConsoleMessage("World saved !", 0);
                        break;
                }
            }
            catch
            {
                m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                m_client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        void ParseMapCommand(string[] _datas)
        {
            try
            {
                switch (_datas[1])
                {
                    case "spawnmobs":

                        m_client.m_player.GetMap().AddMonstersGroup();
                        break;
                }
            }
            catch
            {
                m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                m_client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        void ParseCommandItem(string[] _datas)
        {
            try
            {
                if (_datas.Length == 2)
                {
                    m_client.m_player.m_inventary.AddItem(int.Parse(_datas[1]), false);
                    m_client.SendConsoleMessage("Item Added !", 0);
                }

                else if (_datas.Length == 3)
                {
                    m_client.m_player.m_inventary.AddItem(int.Parse(_datas[1]), false, int.Parse(_datas[2]));
                    m_client.SendConsoleMessage("Item Added !", 0);
                }

                else
                    m_client.SendConsoleMessage("Invalid Syntax !");
            }
            catch
            {
                m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                m_client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        void ParseCommandExp(string[] _datas)
        {
            try
            {
                if (_datas.Length == 2)
                {
                    m_client.m_player.AddExp(long.Parse(_datas[1]));
                    m_client.SendConsoleMessage("Exp Added !", 0);
                }

                else
                    m_client.SendConsoleMessage("Invalid Syntax !");
            }
            catch
            {
                m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                m_client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        
        }

        void ParseCommandTeleport(string[] _datas)
        {
            try
            {
                if (_datas.Length == 3)
                {
                    m_client.m_player.TeleportNewMap(int.Parse(_datas[1]), int.Parse(_datas[2]));
                    m_client.SendConsoleMessage("Character Teleported !", 0);
                }

                else if (_datas.Length == 4)
                {
                    var myMap = Database.Cache.MapsCache.m_mapsList.First(x => x.m_map.m_PosX == int.Parse(_datas[1]) && x.m_map.m_PosY == int.Parse(_datas[2]));
                    m_client.m_player.TeleportNewMap(myMap.m_map.m_id, int.Parse(_datas[3]));
                    m_client.SendConsoleMessage("Character Teleported !", 0);
                }

                else
                    m_client.SendConsoleMessage("Invalid Syntax !");
            }
            catch(Exception e)
            {
                m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                m_client.SendConsoleMessage("Use the command 'Help' for more informations !");
                m_client.SendConsoleMessage(e.ToString());
            }
        }

        void ParseCommandVita(string[] _datas)
        {
            try
            {
                if (_datas.Length == 2)
                {
                    m_client.m_player.ResetVita(_datas[1]);
                    m_client.SendConsoleMessage("Vita Updated !", 0);
                }

                else
                    m_client.SendConsoleMessage("Invalid Syntax !");
            }
            catch
            {
                m_client.SendConsoleMessage("Cannot parse your AdminCommand !");
                m_client.SendConsoleMessage("Use the command 'Help' for more informations !");
            }
        }

        #endregion

    }
}
