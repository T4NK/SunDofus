using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace realm.Realm.Character
{
    class Character
    {
        public int ID = -1;
        public string Name = "";
        public int Color, Color2, Color3 = -1;
        public int Class = -1;
        public int Sex = -1;
        public int Skin = -1;
        public int Size = -1;
        public int Level = -1;
        public bool NewCharacter = false;

        public Client.RealmClient Client;
        public CharacterState State;

        public string Channel = "*#$p%i:?!";

        public string PatternList()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(ID + ";");
            Builder.Append(Name + ";");
            Builder.Append(Level + ";"); // Level
            Builder.Append(Skin + ";");
            Builder.Append(Basic.DeciToHex(Color) + ";");
            Builder.Append(Basic.DeciToHex(Color2) + ";");
            Builder.Append(Basic.DeciToHex(Color3) + ";");
            Builder.Append(",,,,,;"); // Items
            Builder.Append("0;" + Program.m_ServerID + ";;;");

            return Builder.ToString();
        }

        public string PatterSelect()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append("|" + ID + "|");
            Builder.Append(Name + "|");
            Builder.Append(Level + "|"); // Level
            Builder.Append(Class + "|");
            Builder.Append(Skin + "|");
            Builder.Append(Basic.DeciToHex(Color) + "|");
            Builder.Append(Basic.DeciToHex(Color2) + "|");
            Builder.Append(Basic.DeciToHex(Color3) + "|");
            Builder.Append(""); // Items

            return Builder.ToString();
        }

        public void ChangeChannel(string Chanel, bool Add)
        {
            if (Add == true)
            {
                if (Channel.Contains(Chanel))
                {
                    Client.Send("cC" + Channel);
                    return;
                }
                Channel = Channel + "" + Chanel;
                Client.Send("cC+" + Chanel);
            }
            else
            {
                Channel = Channel.Replace(Chanel, "");
                Client.Send("cC-" + Chanel);
            }
        }
    }
}
