using System;
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
            var str = string.Empty;

            for (var i = 1; i <= lenght; i++)
            {
                var randomInt = Randomizer.Next(0, HASH.Length);
                str += HASH[randomInt];
            }

            return str;
        }

        public static string Encrypt(string Password, string Key)
        {
            var _Crypted = "1";

            for (var i = 0; i < Password.Length; i++)
            {
                var PPass = Password[i];
                var PKey = Key[i];
                var APass = (int)PPass / 16;
                var AKey = (int)PPass % 16;
                var ANB = (APass + (int)PKey) % HASH.Length;
                var ANB2 = (AKey + (int)PKey) % HASH.Length;

                _Crypted += HASH[ANB];
                _Crypted += HASH[ANB2];

            }

            return _Crypted;
        }
    }
}
