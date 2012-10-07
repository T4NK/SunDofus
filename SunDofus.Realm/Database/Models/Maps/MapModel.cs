using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Maps
{
    class MapModel
    {
        public int id = -1, date = -1, width = -1, height = -1;
        public int capabilities = -1, numgroup = -1, groupmaxsize = -1;
        public string MapData = "", key = "", cells = "", monsters = "", mappos = "";        
    }
}
