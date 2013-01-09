using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DofusOrigin.Interface
{
    public class Logger
    {
        ConsoleColor my_color;
        StreamWriter m_writer;

        bool inConsole = false, inFile = false;
        string m_name = "";
        object m_locker;

        public Logger(string _name,object _locker, ConsoleColor _color = ConsoleColor.Gray)
        {
            m_locker = _locker;
            m_name = _name;
            my_color = _color;
        }

        public void StartConsoleLogger()
        {
            inConsole = true;
        }

        public void StartFileLogger()
        {
            inFile = true;

            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            m_writer = new StreamWriter(string.Format("logs/SunDofus {0} - {1}.log", m_name, 
                DateTime.Now.ToString().Replace("/", "_").Split(' ')[0]));
            m_writer.AutoFlush = true;
        }

        public void Write(string _message, bool _line = true)
        {
            if (inConsole == true)
            {
                lock (m_locker)
                {
                    Console.ForegroundColor = my_color;

                    Console.Write(">> ");

                    foreach (char c in _message)
                    {
                        if (c == '@')
                        {
                            if (Console.ForegroundColor == ConsoleColor.White)
                                Console.ForegroundColor = my_color;
                            else
                                Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.Write(c);
                        }
                    }

                    if (_line == true)
                        Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

            if (inFile == true)
            {
                _message = _message.Replace("@", "");
                m_writer.WriteLine("[{0}] ({1}) : {2}", m_name, DateTime.Now.ToString(), _message);
            }
        }
    }
}
