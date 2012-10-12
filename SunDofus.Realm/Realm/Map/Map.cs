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
            foreach (var myCharacter in myCharacters)
            {
                myCharacter.Client.Send(Message);
            }
        }

        public void AddPlayer(Character.Character myCharacter)
        {
            Send(string.Format("GM|+{0}", myCharacter.PatternDisplayChar()));
            myCharacters.Add(myCharacter);
            myCharacter.Client.Send(string.Format("GM{0}", CharactersPattern()));
        }

        public void DelPlayer(Character.Character myCharacter)
        {
            Send(string.Format("GM|-{0}", myCharacter.ID));
            myCharacters.Remove(myCharacter);
        }

        public string CharactersPattern()
        {
            var Packet = "|+";

            foreach (var myCharacter in myCharacters)
                Packet += string.Format("|+{0}", myCharacter.PatternDisplayChar());

            return Packet;
        }
    }
}
