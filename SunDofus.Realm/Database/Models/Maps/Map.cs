using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Map
{
    class Map
    {
        public List<Character.Character> m_Characters;
        public List<Trigger> m_Triggers;

        public int id, date, width, height, capabilities, numgroup, groupmaxsize = -1;
        public string MapData, key, cells, monsters, mappos = "";

        public Map()
        {
            m_Characters = new List<Character.Character>();
            m_Triggers = new List<Trigger>();
        }

        public void Send(string Message)
        {
            foreach (Character.Character m_C in m_Characters)
            {
                m_C.Client.Send(Message);
            }
        }

        public void AddPlayer(Character.Character m_C)
        {
            Send("GM|+" + m_C.PatternDisplayChar());
            m_Characters.Add(m_C);
            m_C.Client.Send("GM" + CharactersPattern());
        }

        public void DelPlayer(Character.Character m_C)
        {
            Send("GM|-" + m_C.ID);
            m_Characters.Remove(m_C);
        }

        public string CharactersPattern()
        {
            string Packet = "|+";

            foreach (Character.Character m_C in m_Characters)
            {
                Packet += "|+" + m_C.PatternDisplayChar();
            }

            return Packet;
        }
    }
}
