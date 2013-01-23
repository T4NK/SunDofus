/*
 * ORIGINAL CLASS BY NIGHTWOLF FROM THE SNOWING's PROJECT ! All rights reserved !
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Maps
{
    class Pathfinding
    {
        private string m_strPath;
        private int m_startCell;
        private int m_startDir;

        public int m_destination;
        public int m_newDirection;

        private Map m_map;

        public Pathfinding(string _path, Map _map, int _startCell, int _startDir)
        {
            m_strPath = _path;
            m_map = _map;
            m_startCell = _startCell;
            m_startDir = _startDir;
        }

        public void UpdatePath(string _path)
        {
            m_strPath = _path;
        }

        public string GetStartPath
        {
            get
            {
                return GetDirChar(m_startDir) + GetCellChars(m_startCell);
            }
        }

        public int GetCaseIDFromDirection(int _caseID, char _direction, bool _fight)
        {
            switch (_direction)
            {
                case 'a':
                    return _fight ? -1 : _caseID + 1;
                case 'b':
                    return _caseID + m_map.m_map.m_width;
                case 'c':
                    return _fight ? -1 : _caseID + (m_map.m_map.m_width * 2 - 1);
                case 'd':
                    return _caseID + (m_map.m_map.m_width - 1);
                case 'e':
                    return _fight ? -1 : _caseID - 1;
                case 'f':
                    return _caseID - m_map.m_map.m_width;
                case 'g':
                    return _fight ? -1 : _caseID - (m_map.m_map.m_width * 2 - 1);
                case 'h':
                    return _caseID - m_map.m_map.m_width + 1;
            }

            return -1; 
        }

        public static int GetCellNum(string _cellChars)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            var numChar1 = hash.IndexOf(_cellChars[0]) * hash.Length;
            var numChar2 = hash.IndexOf(_cellChars[1]);

            return numChar1 + numChar2;
        }

        public static string GetCellChars(int _cellNum)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            var charCode2 = (_cellNum % hash.Length);
            var charCode1 = (_cellNum - charCode2) / hash.Length;

            return hash[charCode1].ToString() + hash[charCode2].ToString();
        }

        public static string GetDirChar(int _dirNum)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            if (_dirNum >= hash.Length)
                return "";

            return hash[_dirNum].ToString();
        }

        public static int GetDirNum(string _dirChar)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            return hash.IndexOf(_dirChar);
        }

        public bool InLine(int _cell1, int _cell2)
        {
            var isX = GetCellXCoord(_cell1) == GetCellXCoord(_cell2);
            var isY = GetCellYCoord(_cell1) == GetCellYCoord(_cell2);

            return isX || isY;
        }

        public int NextCell(int _cell, int _dir)
        {
            switch (_dir)
            {
                case 0:
                    return _cell + 1;

                case 1:
                    return _cell + m_map.m_map.m_width;

                case 2:
                    return _cell + (m_map.m_map.m_width * 2) - 1;

                case 3:
                    return _cell + m_map.m_map.m_width - 1;

                case 4:
                    return _cell - 1;

                case 5:
                    return _cell - m_map.m_map.m_width;

                case 6:
                    return _cell - (m_map.m_map.m_width * 2) + 1;

                case 7:
                    return _cell - m_map.m_map.m_width + 1;

            }

            return -1;
        }

        public string RemakeLine(int _lastCell, string _cell, int _finalCell)
        {
            var direction = GetDirNum(_cell[0].ToString());
            var toCell = GetCellNum(_cell.Substring(1));
            var lenght = 0;

            if (InLine(_lastCell, toCell))
                lenght = GetEstimateDistanceBetween(_lastCell, toCell);
            else
                lenght = int.Parse(Math.Truncate((GetEstimateDistanceBetween(_lastCell, toCell) / 1.4)).ToString());

            var backCell = _lastCell;
            var actuelCell = _lastCell;

            for (var i = 1; i <= lenght; i++)
            {
                actuelCell = NextCell(actuelCell, direction);
                backCell = actuelCell;
            }

            return _cell + ",1";
        }

        public string RemakePath()
        {
            var newPath = "";
            var newCell = GetCellNum(m_strPath.Substring(m_strPath.Length - 2, 2));
            var lastCell = m_startCell;

            for (var i = 0; i <= m_strPath.Length - 1; i += 3)
            {
                var actualCell = m_strPath.Substring(i, 3);
                var lineData = RemakeLine(lastCell, actualCell, newCell).Split(',');
                newPath += lineData[0];

                if (lineData[1] == null)
                    return newPath;

                lastCell = GetCellNum(actualCell.Substring(1));
            }

            m_destination = GetCellNum(m_strPath.Substring(m_strPath.Length - 2, 2));
            m_newDirection = GetDirNum(m_strPath.Substring(m_strPath.Length - 3, 1));

            return newPath;
        }

        public int GetDistanceBetween(int _id1, int _id2)
        {
            if (_id1 == _id2) return 0;
            if (m_map == null) return 0;

            var diffX = Math.Abs(GetCellXCoord(_id1) - GetCellXCoord(_id2));
            var diffY = Math.Abs(GetCellYCoord(_id1) - GetCellYCoord(_id2));

            return (diffX + diffY);
        }

        public int GetEstimateDistanceBetween(int _id1, int _id2)
        {
            if (_id1 == _id2) return 0;
            if (m_map == null) return 0;

            var diffX = Math.Abs(GetCellXCoord(_id1) - GetCellXCoord(_id2));
            var diffY = Math.Abs(GetCellYCoord(_id1) - GetCellYCoord(_id2));

            return int.Parse(Math.Truncate(Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2))).ToString());
        }

        public int GetCellXCoord(int _cellid)
        {
            var width = m_map.m_map.m_width;
            return ((_cellid - (width - 1) * GetCellYCoord(_cellid)) / width);
        }

        public int GetCellYCoord(int _cellid)
        {
            var width = m_map.m_map.m_width;
            var loc5 = (int)(_cellid / ((width * 2) - 1));
            var loc6 = _cellid - loc5 * ((width * 2) - 1);
            var loc7 = loc6 % width;

            return (loc5 - loc7);
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
    }
}
