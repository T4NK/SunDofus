using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Map
{
    class Map
    {
        public List<Character.Character> myCharacters;
        public List<Database.Models.Maps.TriggerModel> myTriggers;

        public Database.Models.Maps.MapModel myMap;

        public Map(Database.Models.Maps.MapModel Map)
        {
            myMap = Map;

            myCharacters = new List<Character.Character>();
            myTriggers = new List<Database.Models.Maps.TriggerModel>();
        }

        public void Send(string Message)
        {
            foreach (Character.Character m_C in myCharacters)
            {
                m_C.Client.Send(Message);
            }
        }

        public void AddPlayer(Character.Character m_C)
        {
            Send("GM|+" + m_C.PatternDisplayChar());
            myCharacters.Add(m_C);
            m_C.Client.Send("GM" + CharactersPattern());
        }

        public void DelPlayer(Character.Character m_C)
        {
            Send("GM|-" + m_C.ID);
            myCharacters.Remove(m_C);
        }

        public string CharactersPattern()
        {
            string Packet = "|+";

            foreach (Character.Character m_C in myCharacters)
            {
                Packet += "|+" + m_C.PatternDisplayChar();
            }

            return Packet;
        }
    }
}
