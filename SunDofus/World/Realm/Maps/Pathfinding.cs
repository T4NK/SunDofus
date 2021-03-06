﻿/*
 * ORIGINAL CLASS BY NIGHTWOLF FROM THE SNOWING's PROJECT ! All rights reserved !
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.World.Realm.Maps
{
    class Pathfinding
    {
        private string _strPath;
        private int _startCell;
        private int _startDir;

        private int _destination;

        public int Destination
        {
            get
            {
                return _destination;
            }
        }

        private int _direction;

        public int Direction
        {
            get
            {
                return _direction;
            }
        }

        private Map _map;

        public Pathfinding(string path, Map map, int startCell, int startDir)
        {
            _strPath = path;
            _map = map;
            _startCell = startCell;
            _startDir = startDir;
        }

        public void UpdatePath(string path)
        {
            _strPath = path;
        }

        public string GetStartPath
        {
            get
            {
                return GetDirChar(_startDir) + GetCellChars(_startCell);
            }
        }

        public int GetCaseIDFromDirection(int caseID, char direction, bool fight)
        {
            switch (direction)
            {
                case 'a':
                    return fight ? -1 : caseID + 1;
                case 'b':
                    return caseID + _map.GetModel.Width;
                case 'c':
                    return fight ? -1 : caseID + (_map.GetModel.Width * 2 - 1);
                case 'd':
                    return caseID + (_map.GetModel.Width - 1);
                case 'e':
                    return fight ? -1 : caseID - 1;
                case 'f':
                    return caseID - _map.GetModel.Width;
                case 'g':
                    return fight ? -1 : caseID - (_map.GetModel.Width * 2 - 1);
                case 'h':
                    return caseID - _map.GetModel.Width + 1;
            }

            return -1; 
        }

        public static int GetCellNum(string cellChars)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            var numChar1 = hash.IndexOf(cellChars[0]) * hash.Length;
            var numChar2 = hash.IndexOf(cellChars[1]);

            return numChar1 + numChar2;
        }

        public static string GetCellChars(int cellNum)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            var charCode2 = (cellNum % hash.Length);
            var charCode1 = (cellNum - charCode2) / hash.Length;

            return hash[charCode1].ToString() + hash[charCode2].ToString();
        }

        public static string GetDirChar(int dirNum)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

            if (dirNum >= hash.Length)
                return "";

            return hash[dirNum].ToString();
        }

        public static int GetDirNum(string dirChar)
        {
            var hash = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            return hash.IndexOf(dirChar);
        }

        public bool InLine(int cell1, int cell2)
        {
            var isX = GetCellXCoord(cell1) == GetCellXCoord(cell2);
            var isY = GetCellYCoord(cell1) == GetCellYCoord(cell2);

            return isX || isY;
        }

        public int NextCell(int cell, int dir)
        {
            switch (dir)
            {
                case 0:
                    return cell + 1;

                case 1:
                    return cell + _map.GetModel.Width;

                case 2:
                    return cell + (_map.GetModel.Width * 2) - 1;

                case 3:
                    return cell + _map.GetModel.Width - 1;

                case 4:
                    return cell - 1;

                case 5:
                    return cell - _map.GetModel.Width;

                case 6:
                    return cell - (_map.GetModel.Width * 2) + 1;

                case 7:
                    return cell - _map.GetModel.Width + 1;

            }

            return -1;
        }

        public string RemakeLine(int lastCell, string cell, int finalCell)
        {
            var direction = GetDirNum(cell[0].ToString());
            var toCell = GetCellNum(cell.Substring(1));
            var lenght = 0;

            if (InLine(lastCell, toCell))
                lenght = GetEstimateDistanceBetween(lastCell, toCell);
            else
                lenght = int.Parse(Math.Truncate((GetEstimateDistanceBetween(lastCell, toCell) / 1.4)).ToString());

            var backCell = lastCell;
            var actuelCell = lastCell;

            for (var i = 1; i <= lenght; i++)
            {
                actuelCell = NextCell(actuelCell, direction);
                backCell = actuelCell;
            }

            return cell + ",1";
        }

        public string RemakePath()
        {
            var newPath = "";
            var newCell = GetCellNum(_strPath.Substring(_strPath.Length - 2, 2));
            var lastCell = _startCell;

            for (var i = 0; i <= _strPath.Length - 1; i += 3)
            {
                var actualCell = _strPath.Substring(i, 3);
                var lineData = RemakeLine(lastCell, actualCell, newCell).Split(',');
                newPath += lineData[0];

                if (lineData[1] == null)
                    return newPath;

                lastCell = GetCellNum(actualCell.Substring(1));
            }

            _destination = GetCellNum(_strPath.Substring(_strPath.Length - 2, 2));
            _direction = GetDirNum(_strPath.Substring(_strPath.Length - 3, 1));
 
            return newPath;
        }

        public int GetDistanceBetween(int id1, int id2)
        {
            if (id1 == id2 || _map == null) 
                return 0;
            
            var diffX = Math.Abs(GetCellXCoord(id1) - GetCellXCoord(id2));
            var diffY = Math.Abs(GetCellYCoord(id1) - GetCellYCoord(id2));

            return (diffX + diffY);
        }

        public int GetEstimateDistanceBetween(int id1, int id2)
        {
            if (id1 == id2 || _map == null)
                return 0;

            var diffX = Math.Abs(GetCellXCoord(id1) - GetCellXCoord(id2));
            var diffY = Math.Abs(GetCellYCoord(id1) - GetCellYCoord(id2));

            return int.Parse(Math.Truncate(Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2))).ToString());
        }

        public int GetCellXCoord(int cellid)
        {
            var width = _map.GetModel.Width;
            return ((cellid - (width - 1) * GetCellYCoord(cellid)) / width);
        }

        public int GetCellYCoord(int cellid)
        {
            var width = _map.GetModel.Width;
            var loc5 = (int)(cellid / ((width * 2) - 1));
            var loc6 = cellid - loc5 * ((width * 2) - 1);
            var loc7 = loc6 % width;

            return (loc5 - loc7);
        }

        public static bool isValidCell(int cell, string path)
        {
            if (path.Length == 0 || (path.Length % 3) != 0) 
                return false;

            var lastCell = cell;

            for (var i = 0; i <= path.Length - 1; i += 3)
            {
                var actualCell = path.Substring(i, 3);
                lastCell = GetCellNum(actualCell.Substring(1));

            }

            return true;
        }
    }
}
