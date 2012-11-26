using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Network.Authentication
{
    class AuthenticationKeys
    {
        //Static
        public static List<AuthenticationKeys> m_keys = new List<AuthenticationKeys>();

        //Object
        public string m_key;
        public Database.Models.Clients.AccountModel m_infos;

        public AuthenticationKeys(string _packet)
        {
            m_key = "";
            m_infos = new Database.Models.Clients.AccountModel();

            var _datas = _packet.Split('|');

            m_key = _datas[1];
            m_infos.m_id = int.Parse(_datas[2]);
            m_infos.m_pseudo = _datas[3];
            m_infos.m_question = _datas[4];
            m_infos.m_answer = _datas[5];
            m_infos.m_level = int.Parse(_datas[6]);
            m_infos.m_strcharacters = _datas[7];
            m_infos.m_subscription = long.Parse(_datas[8]);
            m_infos.m_strgifts = _datas[9];
        }
    }
}
