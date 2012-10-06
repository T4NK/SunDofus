using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Map
{
    class Cells
    {
        public static int GetCellNum(string CellChars)
        {

            string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            int NumChar1 = hash.IndexOf(CellChars[0]) * hash.Length;
            int NumChar2 = hash.IndexOf(CellChars[1]);

            return NumChar1 + NumChar2;

        }

        public static bool isValidCell(Character.Character m_C, string Path)
        {
            if (Path.Length == 0) return false;
            if ((Path.Length % 3) != 0) return false;

            int LastCell = m_C.MapCell;

            for (int i = 0; i <= Path.Length - 1; i += 3)
            {
                string ActualCell = Path.Substring(i, 3);
                LastCell = GetCellNum(ActualCell.Substring(1));

            }

            return true;
        }

        public static string GetDirChar(int DirNum)
        {
            string hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            if (DirNum >= hash.Length) return "";
            return hash[DirNum].ToString();
        }
    }
}
