using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selector.Utils
{
    class Logger
    {
        public static void Write() { Console.WriteLine(); }
        public static void Write(string M) { Console.WriteLine(M); }

        public static void Write(string M, ConsoleColor C)
        {
            Console.ForegroundColor = C;
            Write(M);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Write(string M, ConsoleColor C, bool L)
        {
            if (L == true)
            {
                Write(M, C);
            }
            else
            {
                Console.ForegroundColor = C;
                Console.Write(M);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void Status(string S)
        {
            Write("Status >> ", ConsoleColor.Green, false);
            Write(S);
        }

        public static void Infos(string I)
        {
            Write("Infos >> ", ConsoleColor.White, false);
            Write(I);
        }

        public static void Packets(string P)
        {
            if (Config.ConfigurationManager.Debug == true)
            {
                Write("Packets >> ", ConsoleColor.Magenta, false);
                Write(P);
            }
        }

        public static void Error(string E) { Write("Error >> " + E, ConsoleColor.Yellow); }
        public static void Error(Exception E) { Error(E.ToString()); }
    }
}
