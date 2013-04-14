using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Utilities
{
    class Basic
    {
        private static Random _randomizer = new Random();
        public static object ConsoleLocker = new object();

        private static char[] _hash = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        public static string RandomString(int lenght)
        {
            var str = string.Empty;

            for (var i = 1; i <= lenght; i++)
            {
                var randomInt = _randomizer.Next(0, _hash.Length);
                str += _hash[randomInt];
            }

            return str;
        }

        public static long GetActualTime()
        {
            return (long)DateTime.Now.Subtract(DateTime.Now).TotalMilliseconds;
        }

        public static string Encrypt(string password, string key)
        {
            var _Crypted = "1";

            for (var i = 0; i < password.Length; i++)
            {
                var PPass = password[i];
                var PKey = key[i];
                var APass = (int)PPass / 16;
                var AKey = (int)PPass % 16;
                var ANB = (APass + (int)PKey) % _hash.Length;
                var ANB2 = (AKey + (int)PKey) % _hash.Length;

                _Crypted += _hash[ANB];
                _Crypted += _hash[ANB2];

            }

            return _Crypted;
        }
    }
}
