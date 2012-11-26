using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Maps
{
    class Map
    {
        public List<Characters.Character> m_characters { get; set; }
        public List<Database.Models.Maps.TriggerModel> m_triggers { get; set; }
        public Database.Models.Maps.MapModel m_map { get; set; }

        public Map(Database.Models.Maps.MapModel _map)
        {
            m_map = _map;

            m_characters = new List<Characters.Character>();
            m_triggers = new List<Database.Models.Maps.TriggerModel>();
        }

        public void Send(string _message)
        {
            foreach (var character in m_characters)
            {
                character.m_networkClient.Send(_message);
            }
        }

        public void AddPlayer(Characters.Character _character)
        {
            Send(string.Format("GM|+{0}", _character.PatternDisplayChar()));
            m_characters.Add(_character);
            _character.m_networkClient.Send(string.Format("GM{0}", CharactersPattern()));
        }

        public void DelPlayer(Characters.Character _character)
        {
            Send(string.Format("GM|-{0}", _character.m_id));
            m_characters.Remove(_character);
        }

        public string CharactersPattern()
        {
            var packet = "|+";

            foreach (var character in m_characters)
                packet += string.Format("|+{0}", character.PatternDisplayChar());

            return packet;
        }
    }
}
