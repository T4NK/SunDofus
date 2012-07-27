using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunDofus;

namespace realm.Realm.Character
{
    class Character
    {
        public string Name = "";
        public int ID, Color, Color2, Color3, Class, Sex, Skin, Size, Level, MapID, MapCell, Dir = -1;
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

        public string PatternSelect()
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

        public void LoadMap()
        {
            Map.Map m_M = Database.Data.MapSql.ListOfMaps.First(x => x.id == this.MapID);
            if (m_M == null) return;

            if (m_M.key == "")
            {
                Client.Send("GDM|" + m_M.id + "|" + m_M.date);
            }
            else
            {
                Client.Send("GDM|" + m_M.id + "|" + m_M.date + "|" + m_M.key);
            }
        }

        public Map.Map GetMap()
        {
            return Database.Data.MapSql.ListOfMaps.First(x => x.id == this.MapID);
        }

        public string PatternDisplayChar()
        {
            StringBuilder Builder = new StringBuilder();

            Builder.Append(MapCell + ";");
            Builder.Append(Dir + ";0;");
            Builder.Append(ID + ";");
            Builder.Append(Name + ";");
            Builder.Append(Class + ";");
            Builder.Append(Skin + "^" + Size + ";");
            Builder.Append(Sex + ";0,0,0," + (Level + ID) + ";"); // Sex + Alignment
            Builder.Append(SunDofus.Basic.DeciToHex(Color) + ";");
            Builder.Append(SunDofus.Basic.DeciToHex(Color2) + ";");
            Builder.Append(SunDofus.Basic.DeciToHex(Color3) + ";");
            Builder.Append(",,,,,;"); // Items
            Builder.Append("0;"); //Aura
            Builder.Append(";;");
            Builder.Append(";"); // Guild
            Builder.Append(";0;");
            Builder.Append(";"); // Mount

            return Builder.ToString();
        }
    }
}
