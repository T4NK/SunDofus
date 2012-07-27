using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SunDofus
{
    public class Configuration
    {
        StreamReader Reader;
        Dictionary<string, string> Values;
        public bool isConfigurationLoaded = false;

        public Configuration(string FileName)
        {
            try
            {
                Values = new Dictionary<string, string>();
                Reader = new StreamReader(FileName);

                while (!Reader.EndOfStream)
                {
                    string Line = Reader.ReadLine();
                    if (!Line.Contains("=")) continue;
                    Line = Line.Replace(" ", "");
                    string[] Infos = Line.Split('=');
                    Values.Add(Infos[0], Infos[1]);
                }

                Reader.Close();
                isConfigurationLoaded = true;
                Logger.Status("Configuration loaded : '" + Values.Count + "' params loaded !");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public string GetString(string M)
        {
            if (!isConfigurationLoaded) return "";
            return Values[M];
        }

        public int GetInt(string I)
        {
            if (!isConfigurationLoaded) return -1;
            return int.Parse(Values[I]);
        }

        public bool GetBool(string B)
        {
            if (!isConfigurationLoaded) return false;
            return bool.Parse(Values[B]);
        }
    }
}
