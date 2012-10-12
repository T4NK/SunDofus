using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Database.Models.Spells
{
    class SpellModel
    {
        public int myId = -1, mySprite = -1;
        public string mySpriteInfos = "";
        public List<SpellLevelModel> myLevels;

        public SpellModel()
        {
            myLevels = new List<SpellLevelModel>();
        }

        public void ParseLevel(string Data)
        {
            if (Data == "-1")
                return;

            var myLevel = new SpellLevelModel();

            string[] Stats = Data.Split(',');
            var Effect = Stats[0];
            var EffectCC = Stats[1];

            if (Stats[2] != "" && Stats[2] != "-1")
                myLevel.myCost = int.Parse(Stats[2]);
            else
                myLevel.myCost = 6;

            myLevel.myMinPO = int.Parse(Stats[3]);
            myLevel.myMaxPO = int.Parse(Stats[4]);
            myLevel.myCC = int.Parse(Stats[5]);
            myLevel.myEC = int.Parse(Stats[6]);

            myLevel.myOnlyLine = (Stats[7] == "true" ? true : false);
            myLevel.myOnlyViewLine = (Stats[8] == "true" ? true : false);
            myLevel.myEmptyCell = (Stats[9] == "true" ? true : false);
            myLevel.myAlterablePO = (Stats[10] == "true" ? true : false);

            myLevel.myMaxPerTurn = int.Parse(Stats[12]);
            myLevel.myMaxPerPlayer = int.Parse(Stats[13]);
            myLevel.myTurnNumber = int.Parse(Stats[14]);
            myLevel.myType = Stats[15];
            myLevel.myECEndTurn = (Stats[19] == "true" ? true : false);

            myLevel.ParseEffect(Effect, false);
            myLevel.ParseEffect(EffectCC, true);

            myLevels.Add(myLevel);
        }
    }
}
