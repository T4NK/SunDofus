using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Realm.Character;

namespace realm.Client
{
    class RealmParser
    {
        public RealmClient Client;

        public RealmParser(RealmClient m_C)
        {
            Client = m_C;
        }

        public void Parse(string Data)
        {
            switch (Client.m_State)
            { 
                case RealmClient.State.Ticket:
                    ParseTicket(Data);
                    break;

                case RealmClient.State.Character:
                    ParseCharacter(Data);
                    break;

                case RealmClient.State.InGame:
                    ParseInGame(Data);
                    break;
            }
        }

        public void ParseTicket(string Data)
        {
            Data = Data.Replace("AT", "");
            foreach (Network.SelectorKeys Key in Network.SelectorKeys.m_Keys)
            {
                if (Key.m_Key == Data)
                {
                    Client.m_Infos = Key.m_Infos;
                    Client.m_Infos.ParseCharacters();
                    Client.ParseCharacters();

                    Client.m_State = RealmClient.State.Character;

                    Program.m_RealmLink.Send("NC|" + Client.m_Infos.Pseudo);
                    Client.Send("ATK0");
                }
            }
        }

        public void ParseCharacter(string Data)
        {
            if (Data.Substring(0, 1) == "A")
            {
                switch (Data.Substring(1, 1))
                {
                    case "A":
                        CreateCharacter(Data);
                        break;

                    case "D":
                        DeleteCharacter(Data);
                        break;

                    case "L":
                        SendCharacterList();
                        break;

                    case "P":
                        Client.Send("APK" + Utils.Basic.RandomName());
                        break;

                    case "S":
                        //SELECT
                        break;

                    case "V":
                        Client.Send("AV0");
                        break;
                }
            }
        }

        public void SendCharacterList()
        {
            long MemberTime = 60 * 60 * 24 * 365;
            string Pack = "ALK" + (MemberTime * 1000) + "|" + Client.m_Infos.CharactersNames.Count;

            if (Client.m_Infos.CharactersNames.Count != 0)
            {
                foreach (Realm.Character.Character m_C in Client.m_Characters)
                {
                    Pack += "|" + m_C.PatternList();
                }
            }
            
            Client.Send(Pack);
        }

        public void CreateCharacter(string Packet)
        {
            string Data = Packet.Substring(2);
            string[] CharData = Data.Split('|');

            if (CharData[0] != "" | CharactersManager.ExistsName(CharData[0]))
            {
                Character m_Character = new Character();
                m_Character.ID = 1;
                m_Character.Name = CharData[0];
                m_Character.Level = 1;
                m_Character.Class = int.Parse(CharData[1]);
                m_Character.Sex = int.Parse(CharData[2]);
                m_Character.Skin = int.Parse(m_Character.Class + "" + m_Character.Sex);
                m_Character.Size = 100;
                m_Character.Color = int.Parse(CharData[3]);
                m_Character.Color2 = int.Parse(CharData[4]);
                m_Character.Color3 = int.Parse(CharData[5]);
                m_Character.NewCharacter = true;

                if (m_Character.Class < 1 | m_Character.Class > 12 | m_Character.Sex < 0 | m_Character.Sex > 1)
                {
                    Client.Send("AAE");
                    return;
                }

                CharactersManager.ListOfCharacters.Add(m_Character);
                Client.m_Characters.Add(m_Character);

                Program.m_RealmLink.Send("NCHAR|" + Client.m_Infos.Id + "|" + Client.m_Infos.AddNewCharacterToAccount(m_Character.Name));
                Database.Data.CharacterSql.CreateCharacter(m_Character);

                Client.Send("AAK");
                SendCharacterList();
            }
            else
            {
                Client.Send("AAE");
            }
        }

        public void DeleteCharacter(string Packet)
        {

        }

        public void ParseInGame(string Data)
        { 

        }
    }
}
