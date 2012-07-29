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

        public void ParseCommand(string Command, string Args)
        {
            if (Client.m_Infos.Level > 0)
            {
                switch (Command)
                {
                    case "save":
                        Realm.World.Save.ParseSave(Args);
                        break;

                    case "vita":
                        Client.m_Player.ResetVita(Args);
                        break;
                }
            }
        }
    }
}
