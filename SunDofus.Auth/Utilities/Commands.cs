using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace auth.Utilities
{
    class Commands
    {
        public static void ParseCommand(string Command)
        {
            try
            {
                var datas = Command.Split(' ');

                switch (datas[0])
                {
                    case "restart":

                        
                        break;
                }
            }
            catch(Exception e)
            {
                Utilities.Loggers.ErrorsLogger.Write(string.Format("Cannot parse local command because {0}", e.ToString()));
            }
        }
    }
}
