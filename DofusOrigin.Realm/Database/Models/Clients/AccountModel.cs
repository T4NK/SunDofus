using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Database.Models.Clients
{
    class AccountModel
    {
        public int ID;
        public int GMLevel;
        public string Pseudo;
        public string Question;
        public string Answer;
        public long Subscription;
        public string Strcharacters;
        public string Strgifts;

        public List<string> Characters;
        public List<GiftModel> Gifts;

        public AccountModel()
        {
            Characters = new List<string>();
            Gifts = new List<GiftModel>();

            Pseudo = "";
            Question = "";
            Answer = "";
            ID = -1;
            GMLevel = -1;
            Strcharacters = "";
            Subscription = 0;
            Strgifts = "";
        }

        public void ParseCharacters()
        {
            if (Strcharacters == "") 
                return;

            foreach (var datas in Strcharacters.Split(','))
            {
                lock (Characters)
                {
                    if (!Characters.Contains(datas))
                        Characters.Add(datas);
                }
            }
        }

        public void ParseGifts()
        {
            if (Strgifts == "") 
                return;

            var infos = Strgifts.Split('+');

            foreach (var datas in infos)
            {
                var giftDatas = datas.Split('~');
                var gift = new GiftModel();

                gift.ID = int.Parse(giftDatas[0]);
                gift.Title = giftDatas[1];
                gift.Message = giftDatas[2];
                gift.ItemID = int.Parse(giftDatas[3]);
                gift.Image = giftDatas[4];

                lock(Gifts)
                    Gifts.Add(gift);
            }
        }
    }
}
