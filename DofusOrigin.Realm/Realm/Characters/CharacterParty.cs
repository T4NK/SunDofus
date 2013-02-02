using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters
{
    class CharacterParty
    {
        public Dictionary<Character, int> myMembers;
        private string ownerName = "";
        private int ownerID = -1;

        public CharacterParty(Character mener)
        {
            myMembers = new Dictionary<Character, int>();
            myMembers.Add(mener, 1);
        }

        public void AddMember(Character newMember)
        {
            myMembers.Add(newMember, 0);
            newMember.m_state.myParty = this;

            if (myMembers.Count == 2)
            {
                Send(string.Format("PCK{0}", ownerName));
                Send(string.Format("PL{0}", ownerID));
                Send(string.Format("PM{0}", PartyPattern()));
            }
            else
            {
                newMember.m_networkClient.Send(string.Format("PCK{0}", ownerName));
                newMember.m_networkClient.Send(string.Format("PL{0}", ownerID));
                newMember.m_networkClient.Send(string.Format("PM{0}", PartyPattern()));

                foreach (var character in myMembers.Keys.ToList().Where(x => x != newMember))
                    character.m_networkClient.Send(string.Format("PM+{0}", character.PatternOnParty()));
            }
        }

        public void LeaveParty(string name, string kicker = "")
        {
            try
            {
                var character = myMembers.Keys.ToList().First(x => x.m_name == name);
                character.m_state.myParty = null;

                myMembers.Remove(character);

                Send(string.Format("PM-{0}", character.m_id));

                if (character.m_state.isFollow)
                {
                    character.m_state.followers.Clear();
                    character.m_state.isFollow = false;
                }

                if(character.isConnected)
                    character.m_networkClient.Send(string.Format("PV{0}", kicker));

                if (myMembers.Count == 1)
                {
                    var last = myMembers.Keys.ToList()[0];
                    last.m_state.myParty = null;

                    myMembers.Remove(last);

                    if (last.isConnected)
                        last.m_networkClient.Send(string.Format("PV{0}", kicker));
                }
                else if (ownerID == character.m_id)
                    GetNewLeader();
            }
            catch { }
        }

        private void Send(string text)
        {
            try
            {
                foreach (var character in myMembers.Keys)
                    character.m_networkClient.Send(text);
            }
            catch { }
        }

        private void GetNewLeader()
        {
            var character = myMembers.Keys.ToList()[0];
            myMembers[character] = 1;

            ownerID = character.m_id;
            ownerName = character.m_name;

            Send(string.Format("PL{0}", ownerID));
        }

        private string PartyPattern()
        {
            var packet = "+";
            myMembers.Keys.ToList().ForEach(x => packet += string.Format("{0}|", x.PatternOnParty()));
            return packet.Substring(0, packet.Length -1);
        }
    }
}
