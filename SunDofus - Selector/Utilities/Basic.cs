﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace auth.Utilities
{
    class Basic
    {
        public static Random Randomizer = new Random();
        private static char[] HASH = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        public static string RandomString(int lenght)
        {
            string str = string.Empty;
            for (int i = 1; i <= lenght; i++)
            {
                int randomInt = Randomizer.Next(0, HASH.Length);
                str += HASH[randomInt];
            }
            return str;
        }

        public static string Encrypt(string Password, string Key)
        {
            string _Crypted = "1";

            for (int i = 0; i < Password.Length; i++)
            {
                char PPass = Password[i];
                char PKey = Key[i];

                int APass = (int)PPass / 16;

                int AKey = (int)PPass % 16;

                int ANB = (APass + (int)PKey) % HASH.Length;
                int ANB2 = (AKey + (int)PKey) % HASH.Length;

                _Crypted += HASH[ANB];
                _Crypted += HASH[ANB2];

            }
            return _Crypted;
        }
    }
}
