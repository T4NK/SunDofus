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
            string[] Data = Args.Split(' ');

            if (Client.m_Infos.Level > 0)
            {
                switch (Data[0])
                {
                    case "save":
                        Realm.World.Save.ParseSave(Data[1]);
                        break;

                    case "vita":
                        Client.m_Player.ResetVita(Data[1]);
                        break;

                    case "item":
                        Client.m_Player.m_Inventary.AddItem(int.Parse(Data[1]));
                        break;

                    case "teleport":
                        Client.m_Player.TeleportNewMap(int.Parse(Data[1]), int.Parse(Data[2]));
                        break;
                }
            }
        }
    }
}
