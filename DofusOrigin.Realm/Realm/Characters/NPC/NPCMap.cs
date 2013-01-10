using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DofusOrigin.Realm.Characters.NPC
{
    class NPCMap
    {
        public Database.Models.NPC.NoPlayerCharacterModel m_model;

        public int m_idOnMap { get; set; }

        public int m_mapid { get; set; }
        public int m_cellid { get; set; }
        public int m_dir { get; set; }

        public bool mustMove { get; set; }

        private Timer m_movements { get; set; }


        public NPCMap(Database.Models.NPC.NoPlayerCharacterModel _model)
        {
            m_model = _model;
        }

        public void StartMove()
        {
            if (mustMove == false)
                return;

            m_movements = new Timer();
            m_movements.Enabled = true;
            m_movements.Interval = Utilities.Basic.Rand(1000, 5000);
            m_movements.Elapsed += new ElapsedEventHandler(this.Move);
        }

        public string PatternOnMap()
        {
            var builder = new StringBuilder();
            {
                builder.Append(m_cellid).Append(";").Append(m_dir).Append(";0;");
                builder.Append(m_idOnMap).Append(";");
                builder.Append(m_model.m_id).Append(";-4;");
                builder.Append(m_model.m_gfxid).Append("^").Append(m_model.m_size).Append(";");
                builder.Append(m_model.m_sex).Append(";").Append(Utilities.Basic.DeciToHex(m_model.m_color)).Append(";");
                builder.Append(Utilities.Basic.DeciToHex(m_model.m_color2)).Append(";").Append(Utilities.Basic.DeciToHex(m_model.m_color3)).Append(";");
                builder.Append(m_model.m_items).Append(";;");
            }

            return builder.ToString();
        }

        private void Move(object e, EventArgs e2)
        {
            m_movements.Interval = Utilities.Basic.Rand(1000, 5000);
            var map = Database.Cache.MapsCache.m_mapsList.First(x => x.m_map.m_id == m_mapid);

            var path = new Realm.Maps.Pathfinding("", map, m_cellid, m_dir);
            var newDir = Utilities.Basic.Rand(0, 7);
            var newCell = path.NextCell(m_cellid, newDir);

            if (newCell <= 0)
                return;

            path.UpdatePath(Realm.Maps.Pathfinding.GetDirChar(m_dir) + Realm.Maps.Pathfinding.GetCellChars(m_cellid) + Realm.Maps.Pathfinding.GetDirChar(newDir) + 
                Realm.Maps.Pathfinding.GetCellChars(newCell));            

            var startpath = path.GetStartPath;
            var cellpath = path.RemakePath();

            if(!Realm.Maps.Cells.isValidCell(m_cellid, cellpath))
                return;

            if (cellpath != "")
            {
                m_cellid = path.m_destination;
                m_dir = path.m_newDirection;

                var packet = string.Format("GA0;1;{0};{1}", m_idOnMap, startpath + cellpath);

                map.Send(packet);
            }
        }
    }
}
