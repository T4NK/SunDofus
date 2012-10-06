﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SunDofus
{
    public class Logger
    {
        ConsoleColor myColor;
        bool inConsole = false, inFile = false;
        StreamWriter myWriter;
        string myName = "";

        public Logger(string newName, ConsoleColor newColor = ConsoleColor.Gray)
        {
            myName = newName;
            myColor = newColor;
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

            myWriter = new StreamWriter("logs/SunDofus [" + myName + "].log");
            myWriter.AutoFlush = true;
        }

        public void Write(string Message, bool Line = true)
        {
            if (inConsole == true)
            {
                Console.ForegroundColor = myColor;

                Console.Write(">> ");

                foreach (char c in Message)
                {
                    if (c == '@')
                    {
                        if (Console.ForegroundColor == ConsoleColor.White)
                            Console.ForegroundColor = myColor;
                        else
                            Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }

                if (Line == true)
                    Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Gray;
            }

            if (inFile == true)
            {
                Message = Message.Replace("@", "");
                myWriter.WriteLine("[" + myName + "] (" + DateTime.Now.ToString() + ") : " + Message);
            }
        }
    }
}
