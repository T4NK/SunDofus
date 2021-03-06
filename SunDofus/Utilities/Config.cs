﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SunDofus.Utilities
{
    class Config
    {
        private static Configuration _config;

        public static void LoadConfiguration()
        {
            if (!File.Exists("config.txt"))
                throw new Exception("Unable to find the file 'config.txt' !");

            _config = new Configuration();
            _config.LoadConfiguration();
        }

        public static int GetIntElement(string value)
        {
            return int.Parse(_config.Values[value.ToUpper()]);
        }
        public static string GetStringElement(string value)
        {
            return _config.Values[value.ToUpper()];
        }
        public static bool GetBoolElement(string value)
        {
            return bool.Parse(_config.Values[value.ToUpper()]);
        }
        public static long GetLongElement(string value)
        {
            return long.Parse(_config.Values[value.ToUpper()]);
        }

        private class Configuration
        {
            public Dictionary<string, string> Values;

            public void LoadConfiguration()
            {
                Values = new Dictionary<string, string>();

                var reader = new StreamReader("config.txt", Encoding.Default);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line.StartsWith("$"))
                    {
                        var lineInfos = line.Substring(1, (line.Length - 2)).Split(' ');

                        if (!Values.ContainsKey(lineInfos[0].ToUpper()))
                            Values.Add(lineInfos[0].ToUpper(), lineInfos[1]);
                    }
                }

                reader.Close();
            }
        }
    }
}
