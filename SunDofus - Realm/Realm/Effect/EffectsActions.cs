using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace realm.Realm.Effect
{
    class EffectsActions
    {
        public static void ParseEffect(Character.Character Client, int Type, string Args)
        {
            string[] Data = Args.Split(',');

            switch (Type)
            {
                case 0: //Telep
                    Client.TeleportNewMap(int.Parse(Data[0]), int.Parse(Data[1]));
                    break;

                case 1: //Life

                    break;
            }
        }
    }
}
