using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DofusOrigin.Realm.Effects
{
    class EffectAction
    {
        public static void ParseEffect(Characters.Character _client, int _type, string _args)
        {
            var datas = _args.Split(',');

            switch (_type)
            {
                case 0: //Telep
                    _client.TeleportNewMap(int.Parse(datas[0]), int.Parse(datas[1]));
                    break;

                case 1: //Life
                    _client.AddLife(int.Parse(datas[0]));
                    break;
            }
        }
    }
}
