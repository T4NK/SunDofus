using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DofusOrigin.Interface
{
    public class Logger
    {
        ConsoleColor _color;
        StreamWriter _writer;

        bool _inConsole = false, _inFile = false;
        string _name = "";
        object _locker;

        public Logger(string name,object locker, ConsoleColor color = ConsoleColor.Gray)
        {
            _locker = locker;
            _name = name;
            _color = color;
        }

        public void StartConsoleLogger()
        {
            _inConsole = true;
        }

        public void StartFileLogger()
        {
            _inFile = true;

            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            _writer = new StreamWriter(string.Format("logs/DofusOrigin {0} - {1}.log", _name, 
                DateTime.Now.ToString().Replace("/", "_")));
            _writer.AutoFlush = true;
        }

        public void Write(string _message, bool _line = true)
        {
            if (_inConsole == true)
            {
                lock (_locker)
                {
                    Console.ForegroundColor = _color;

                    Console.Write("{0} >> ", DateTime.Now.ToString());

                    foreach (char c in _message)
                    {
                        if (c == '@')
                        {
                            if (Console.ForegroundColor == ConsoleColor.White)
                                Console.ForegroundColor = _color;
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

            if (_inFile == true)
            {
                _message = _message.Replace("@", "");
                _writer.WriteLine("[{0}] ({1}) : {2}", _name, DateTime.Now.ToString(), _message);
            }
        }
    }
}
