using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Utilities
{
    class Basic
    {
        public static Random m_rando = new Random();
        public static object m_locker = new object();

        public static string m_vowels = "aeiouy";
        public static string m_consonants = "bcdfghjklmnpqrstvwxz";

        public static int Rand(int _min, int _max)
        {
            return m_rando.Next(_min, _max + 1);
        }

        public static string GetVowels()
        {
            return m_vowels.Substring((Rand(0, m_vowels.Length - 1)), 1);
        }

        public static string GetConsonants()
        {
            return m_consonants.Substring((Rand(0, m_consonants.Length - 1)), 1);
        }

        public static string RandomName()
        {
            var name = "";
            name += GetConsonants();
            name += GetVowels();
            name += GetVowels();
            name += GetConsonants();
            name += GetConsonants();
            name += GetVowels();

            if (Rand(0, 1) == 0)
            {
                name += GetConsonants();
                name += GetVowels();
            }

            return name;
        }

        public static string DeciToHex(int _deci)
        {
            if (_deci == -1) return "-1";
            else return _deci.ToString("x");
        }

        public static int HexToDeci(string _hex)
        {
            if (_hex == "-1" | _hex == "") return -1;
            return Convert.ToInt32(_hex, 16);
        }

        public static string GetActuelTime()
        {
            return string.Format("{0}{1}{2}{3}", (DateTime.Now.Hour * 3600000), (DateTime.Now.Minute * 60000),
                (DateTime.Now.Second * 1000), DateTime.Now.Millisecond.ToString());
        }

        public static string GetDofusDate()
        {
            return string.Format("BD{0}|{1}|{2}", (DateTime.Now.Year - 1370).ToString(), (DateTime.Now.Month - 1), (DateTime.Now.Day));
        }

        public static int GetRandomJet(string _jetStr, int _jet = 3)
        {
            if (_jetStr.Length > 3)
            {
                var Damage = 0;
                var DS = int.Parse(_jetStr.Split('d')[0]);
                var Faces = int.Parse(_jetStr.Split('d')[1].Split('+')[0]);
                var Fixe = int.Parse(_jetStr.Split('d')[1].Split('+')[1]);

                if (DS != 0)
                {
                    for (var i = 1; i <= DS; i++)
                    {
                        if (_jet == 1)
                            Damage += Faces;
                        else if (_jet == 2)
                            Damage += 1;
                        else if (_jet == 3)
                            Damage += (int)Math.Ceiling((double)(Faces / 2));
                        else
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
