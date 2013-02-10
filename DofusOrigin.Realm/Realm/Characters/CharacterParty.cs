using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Characters
{
    class CharacterParty
    {
        public Dictionary<Character, int> Members;

        private string _ownerName = "";
        private int _ownerID = -1;

        public CharacterParty(Character leader)
        {
            Members = new Dictionary<Character, int>();
            Members.Add(leader, 1);

            _ownerID = leader.m_id;
            _ownerName = leader.m_name;
        }

        public void AddMember(Character member)
        {
            Members.Add(member, 0);
            member.m_state.Party = this;

            if (Members.Count == 2)
            {
                Send(string.Format("PCK{0}", _ownerName));
                Send(string.Format("PL{0}", _ownerID));
                Send(string.Format("PM{0}", PartyPattern()));
            }
            else
            {
                member.m_networkClient.Send(string.Format("PCK{0}", _ownerName));
                member.m_networkClient.Send(string.Format("PL{0}", _ownerID));
                member.m_networkClient.Send(string.Format("PM{0}", PartyPattern()));

                foreach (var character in Members.Keys.ToList().Where(x => x != member))
                    character.m_networkClient.Send(string.Format("PM{0}", character.PatternOnParty()));
            }
        }

        public void LeaveParty(string name, string kicker = "")
        {
            try
            {
                var character = Members.Keys.ToList().First(x => x.m_name == name);
                character.m_state.Party = null;

                Members.Remove(character);

                Send(string.Format("PM-{0}", character.m_id));

                if (character.m_state.isFollow)
                {
                    character.m_state.Followers.Clear();
                    character.m_state.isFollow = false;
                }

                if(character.isConnected)
                    character.m_networkClient.Send(string.Format("PV{0}", kicker));

                if (Members.Count == 1)
                {
                    var last = Members.Keys.ToList()[0];
                    last.m_state.Party = null;

                    Members.Remove(last);

                    if (last.isConnected)
                        last.m_networkClient.Send(string.Format("PV{0}", kicker));
                }
                else if (_ownerID == character.m_id)
                    GetNewLeader();
            }
            catch { }
        }

        private void Send(string text)
        {
            try
            {
                foreach (var character in Members.Keys)
                    character.m_networkClient.Send(text);
            }
            catch { }
        }

        private void GetNewLeader()
        {
            var character = Members.Keys.ToList()[0];
            Members[character] = 1;

            _ownerID = character.m_id;
            _ownerName = character.m_name;

            Send(string.Format("PL{0}", _ownerID));
        }

        private string PartyPattern()
        {
            var packet = "+";
            Members.Keys.ToList().ForEach(x => packet += string.Format("{0}|", x.PatternOnParty()));
            return packet.Substring(0, packet.Length -1);
        }
    }
}
