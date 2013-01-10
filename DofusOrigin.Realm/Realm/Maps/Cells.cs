using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps
{
    class Cells
    {
        public static int GetCellNum(string _cellChars)
        {

            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            var numChar1 = hash.IndexOf(_cellChars[0]) * hash.Length;
            var numChar2 = hash.IndexOf(_cellChars[1]);

            return numChar1 + numChar2;

        }

        public static bool isValidCell(int _cell, string _path)
        {
            if (_path.Length == 0) return false;
            if ((_path.Length % 3) != 0) return false;

            var lastCell = _cell;

            for (var i = 0; i <= _path.Length - 1; i += 3)
            {
                var actualCell = _path.Substring(i, 3);
                lastCell = GetCellNum(actualCell.Substring(1));

            }

            return true;
        }

        public static string GetDirChar(int _dirNum)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            if (_dirNum >= hash.Length) return "";
            return hash[_dirNum].ToString();
        }
    }
}
