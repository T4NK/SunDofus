using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Clients
{
    class AccountModel
    {
        public int m_id { get; set; }
        public int m_level { get; set; }
        public string m_pseudo { get; set; }
        public string m_question { get; set; }
        public string m_answer { get; set; }
        public long m_subscription { get; set; }
        public string m_strcharacters { get; set; }
        public string m_strgifts { get; set; }

        public List<string> m_characters { get; set; }
        public List<GiftModel> m_gifts { get; set; }

        public AccountModel()
        {
            m_characters = new List<string>();
            m_gifts = new List<GiftModel>();

            m_pseudo = "";
            m_question = "";
            m_answer = "";
            m_id = -1;
            m_level = -1;
            m_strcharacters = "";
            m_subscription = 0;
            m_strgifts = "";
        }

        public void ParseCharacters()
        {
            if (m_strcharacters == "") 
                return;

            foreach (var datas in m_strcharacters.Split(','))
            {
                if (!m_characters.Contains(datas))
                    m_characters.Add(datas);
            }
        }

        public void ParseGifts()
        {
            if (m_strgifts == "") 
                return;

            var infos = m_strgifts.Split('+');

            foreach (var datas in infos)
            {
                var giftDatas = datas.Split('~');
                var gift = new GiftModel();

                gift.m_id = int.Parse(giftDatas[0]);
                gift.m_title = giftDatas[1];
                gift.m_message = giftDatas[2];
                gift.m_itemID = int.Parse(giftDatas[3]);

                m_gifts.Add(gift);
            }
        }
    }
}
