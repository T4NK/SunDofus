using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus
{
    public class Basic
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

        public static Random _Rando = new Random();

        public static string Vowels = "aeiouy";
        public static string Consonants = "bcdfghjklmnpqrstvwxz";

        public static int Rand(int Min, int Max)
        {
            return _Rando.Next(Min, Max + 1);
        }

        public static string GetVowels()
        {
            return Vowels.Substring((Rand(0, Vowels.Length - 1)), 1);
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

        public static int HexToDeci(string Hex)
        {
            if (Hex == "-1" | Hex == "") return -1;
            return Convert.ToInt32(Hex, 16);
        }

        public static string GetActuelTime()
        {
            return (DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000) + DateTime.Now.Millisecond.ToString();
        }

        public static string GetDofusDate()
        {
            return "BD" + (DateTime.Now.Year - 1370).ToString() + "|" + (DateTime.Now.Month - 1) + "|" + (DateTime.Now.Day);
        }

        public static int GetRandomJet(string Jet)
        {
            if (Jet.Length > 3)
            {
                int Damage = 0;
                int DS = int.Parse(Jet.Split('d')[0]);
                int Faces = int.Parse(Jet.Split('d')[1].Split('+')[0]);
                int Fixe = int.Parse(Jet.Split('d')[1].Split('+')[1]);

                if (DS != 0)
                {
                    for (int i = 1; i <= DS; i++)
                    {
                        Damage += Rand(1, Faces);
                    }
                }

                return (Damage + Fixe);
            }
            else
                return 0;
        }
    }
}
