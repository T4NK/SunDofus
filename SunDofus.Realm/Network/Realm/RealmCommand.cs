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

                            realm.Realm.World.Save.ParseSave(Datas.Length > 1 ? Datas[1] : "all");
                            break;

                        case "vita":

                            if (Datas.Length < 2)
                                return;

                            Client.myPlayer.ResetVita(Datas[1]);
                            break;

                        case "item":

                            if (Datas.Length < 2)
                                return;

                            Client.myPlayer.myInventary.AddItem(int.Parse(Datas[1]), false);
                            break;

                        case "teleport":

                            if (Datas.Length < 3)
                                return;

                            Client.myPlayer.TeleportNewMap(int.Parse(Datas[1]), int.Parse(Datas[2]));
                            break;

                        case "exp":

                            if (Datas.Length < 2)
                                return;

                            Client.myPlayer.AddExp(long.Parse(Datas[1]));
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot parse command from <{0}> because : {1}", Client.myIp(), e.ToString()));
            }
        }
    }
}
