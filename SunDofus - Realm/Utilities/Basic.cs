using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Utilities
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
