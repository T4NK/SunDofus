using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Maps
{
    class MapModel
    {
        public int myId = -1, myDate = -1, myWidth = -1, myHeight = -1;
        public int myCapabilities = -1, myNumgroup = -1, myGroupmaxsize = -1;
        public string myMapData = "", myKey = "", myCells = "", myMonsters = "", myMappos = "";
        public int x = 0, y = 0;

        public void ParsePos()
        {
            var datas = myMappos.Split(',');
            try
            {
                x = int.Parse(datas[0]);
                y = int.Parse(datas[1]);
            }
            catch { }
        }
    }
}
