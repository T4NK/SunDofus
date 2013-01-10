using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Levels
{
    class LevelModel
    {
        public int m_id { get; set; }

        public long m_character { get; set; }
        public long m_job { get; set; }
        public long m_alignment { get; set; }
        public long m_guild { get; set; }
        public long m_mount { get; set; }

        public LevelModel(long _max = 0)
        {
            m_character = _max;
            m_job = _max;
            m_mount = _max;
            m_alignment = _max;
            m_guild = _max;
        }
    }
}
