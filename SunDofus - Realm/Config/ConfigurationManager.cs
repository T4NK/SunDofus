using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SunDofus;

namespace realm.Config
{
    class ConfigurationManager
    {
        static Configuration Config;
        public static bool Subscription = false;

        public static void IniConfig()
        {
            Config = new Configuration("Config/Config.txt");
            Program.m_ServerID = Config.GetInt("Server_ID");
            Subscription = Config.GetBool("Subscription");
        }

        public static string GetString(string M)
        {
            return Config.GetString(M);
        }

        public static int GetInt(string I)
        {
            return Config.GetInt(I);
        }

        public static bool GetBool(string B)
        {
            return Config.GetBool(B);
        }
    }
}
