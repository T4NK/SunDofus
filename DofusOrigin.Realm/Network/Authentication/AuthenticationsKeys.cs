using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Network.Authentication
{
    class AuthenticationsKeys
    {
        public static List<AuthenticationsKeys> m_keys = new List<AuthenticationsKeys>();

        private string _key;

        public string Key
        {
            get
            {
                return _key;
            }
        }
        public Database.Models.Clients.AccountModel Infos;

        public AuthenticationsKeys(string packet)
        {
            Infos = new Database.Models.Clients.AccountModel();
            var _datas = packet.Split('|');

            _key = _datas[1];
            Infos.m_id = int.Parse(_datas[2]);
            Infos.m_pseudo = _datas[3];
            Infos.m_question = _datas[4];
            Infos.m_answer = _datas[5];
            Infos.m_level = int.Parse(_datas[6]);
            Infos.m_strcharacters = _datas[7];
            Infos.m_subscription = long.Parse(_datas[8]);
            Infos.m_strgifts = _datas[9];
        }
    }
}
