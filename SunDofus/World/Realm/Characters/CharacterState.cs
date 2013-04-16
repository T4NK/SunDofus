using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Realm.Characters
{
    class CharacterState
    {
        Character Character;

        public CharacterState(Character character)
        {
            Character = character;
            created = false;

            Party = null;
            Followers = new List<Character>();
        }

        public bool created = false;

        public bool onMove = false;
        public bool onExchange = false;
        public bool onExchangePanel = false;
        public bool onExchangeAccepted = false;

        public int moveToCell = -1;
        public int actualNPC = -1;
        public int actualTraided = -1;
        public int actualTraider = -1;
        public int actualPlayerExchange = -1;

        public bool onWaitingParty = false;
        public int senderInviteParty = -1;
        public int receiverInviteParty = -1;

        public bool isFollow = false;
        public bool isFollowing = false;
        public int followingID = -1;

        public bool onDialoging = false;
        public int onDialogingWith = -1;

        public bool isChallengeAsked = false;
        public bool isChallengeAsker = false;
        public int ChallengeAsked = -1;
        public int ChallengeAsker = -1;

        public CharacterParty Party;
        public List<Character> Followers;

        public bool Occuped
        {
            get
            {
                return (onMove || onExchange || onWaitingParty || onDialoging || isChallengeAsked || isChallengeAsker);
            }
        }
    }
}
