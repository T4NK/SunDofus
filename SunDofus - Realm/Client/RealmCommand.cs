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
                        Realm.World.Save.ParseSave(Datas.Length > 1 ? Datas[1] : "all");
                        break;

                    case "vita":
                        if (Datas.Length < 2)
                            return;

                        Client.m_Player.ResetVita(Datas[1]);
                        break;

                    case "item":
                        if (Datas.Length < 2)
                            return;

                        Client.m_Player.m_Inventary.AddItem(int.Parse(Datas[1]), false);
                        break;

                    case "teleport":
                        if (Datas.Length < 3)
                            return;

                        Client.m_Player.TeleportNewMap(int.Parse(Datas[1]), int.Parse(Datas[2]));
                        break;
                }
            }
        }
    }
}
