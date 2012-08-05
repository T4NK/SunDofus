using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Client
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
            string[] Datas = Args.Split(' ');

            if (Client.m_Infos.Level > 0)
            {
                switch (Datas[0])
                {
                    case "save":
                        Realm.World.Save.ParseSave(Datas[1]);
                        break;

                    case "vita":
                        Client.m_Player.ResetVita(Datas[1]);
                        break;

                    case "item":
                        Client.m_Player.m_Inventary.AddItem(int.Parse(Datas[1]));
                        break;

                    case "teleport":
                        Client.m_Player.TeleportNewMap(int.Parse(Datas[1]), int.Parse(Datas[2]));
                        break;
                }
            }
        }
    }
}
