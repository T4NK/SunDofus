using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace realm.Config
{
    class ConfigurationManager
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>();
        public static bool Debug = false;

        public static void LoadConfiguration()
        {
            try
            {
                StreamReader Reader = new StreamReader("Config/Config.txt");
                while (!Reader.EndOfStream)
                {
                    string Line = Reader.ReadLine();
                    if (!Line.Contains("=")) continue;
                    Line = Line.Replace(" ", "");
                    string[] Infos = Line.Split('=');
                    Values.Add(Infos[0], Infos[1]);
                }
                Utils.Logger.Status("Configuration loaded ! '" + Values.Count + "' paramaters loaded !");

                Debug = GetBool("Debug");
                Program.m_ServerID = GetInt("Server_ID");

            }
            catch (Exception e)
            {
                Utils.Logger.Error(e);
            }
        }

        public static string GetString(string M)
        {
            return Values[M];
        }

        public static int GetInt(string I)
        {
            return int.Parse(Values[I]);
        }

        public static bool GetBool(string B)
        {
            return bool.Parse(Values[B]);
        }
    }
}
