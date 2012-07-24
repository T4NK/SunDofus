using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Utils
{
    class Basic
    {
        public static Random _Rando = new Random();

        public static string Vowels = "aeiouy";
        public static string Consonants = "bcdfghjklmnpqrstvwxz";

        public static int Rand(int Min, int Max)
        {
            return _Rando.Next(Min, Max + 1);
        }

        public static string GetVowels()
        {
            return Vowels.Substring((Rand(0, Vowels.Length - 1)),1);
        }

        public static string GetConsonants()
        {
            return Consonants.Substring((Rand(0, Consonants.Length - 1)), 1);
        }

        public static string RandomName()
        {
            string Name = "";
            Name += GetConsonants();
            Name += GetVowels();
            Name += GetVowels();
            Name += GetConsonants();
            Name += GetConsonants();
            Name += GetVowels();

            if (Rand(0, 1) == 0)
            {
                Name += GetConsonants();
                Name += GetVowels();
            }

            return Name;
        }

        public static string DeciToHex(int Deci)
        {
            if (Deci == -1) return "-1";
            else return Deci.ToString("x");
        }
    }
}
