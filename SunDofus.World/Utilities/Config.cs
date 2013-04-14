using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus.Settings;

namespace SunDofus.Utilities
{
    class Config
    {
        private static Configuration _config;

        public static Configuration GetConfig
        {
            get
            {
                return _config;
            }
        }

        public static void LoadConfiguration()
        {
            if (!System.IO.File.Exists("DofusOrigin.conf"))
                throw new Exception("Configuration file doesn't exist !");

            _config = new Configuration();
            _config.LoadConfiguration("DofusOrigin.conf");
        }
    }
}
