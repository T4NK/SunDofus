using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using realm.Database.Models.Items;

namespace realm.Realm.Characters.Items
{
    class ItemsHandler
    {
        public static int m_lastID = 0;

        public static int GetNewID()
        {
            return ++m_lastID;
        }

        public static bool PositionAvaliable(int _itemType, bool _usable, int _position)
        {

            return true;

        }

        public static bool ConditionsAvaliable(ItemModel _item, Character _character)
        {
            var condi = _item.m_condistr;
            var avaliable = false;

            if (condi == "")
                return true;

            foreach(var cond in condi.Split('&'))
            {
                var spliter = cond.Substring(2, 1);
                long value = -1;
                var toCompare = int.Parse(cond.Substring(3));

                switch(cond.Substring(0,1))
                {
                    case "C":

                        switch(cond.Substring(1,1))
                        {
                            case "a":
                                value = _character.m_stats.agility.m_bases;
                                break;

                            case "i":
                                value = _character.m_stats.intelligence.m_bases;
                                break;

                            case "c":
                                value = _character.m_stats.luck.m_bases;
                                break;

                            case "s":
                                value = _character.m_stats.strenght.m_bases;
                                break;

                            case "v":
                                value = _character.m_stats.life.m_bases;
                                break;

                            case "w":
                                value = _character.m_stats.wisdom.m_bases;
                                break;

                            case "A":
                                value = _character.m_stats.agility.Total();
                                break;

                            case "I":
                                value = _character.m_stats.intelligence.Total();
                                break;

                            case "C":
                                value = _character.m_stats.luck.Total();
                                break;

                            case "S":
                                value = _character.m_stats.strenght.Total();
                                break;

                            case "V":
                                value = _character.m_stats.life.Total();
                                break;

                            case "W":
                                value = _character.m_stats.wisdom.Total();
                                break;

                            default:
                                avaliable = true;
                                break;
                        }

                        break;

                    case "P":

                        switch(cond.Substring(1,1))
                        {
                            case "G":
                                value = _character.m_class;
                                break;

                            case "L":
                                value = _character.m_level;
                                break;

                            case "K":
                                value = _character.m_kamas;
                                break;

                            default:
                                avaliable = true;
                                break;
                        }

                        break;

                    default:
                        avaliable = true;
                        break;
                }

                if (avaliable == true) 
                    return true;

                if (spliter != "")
                {
                    switch (spliter)
                    {
                        case "<":
                            avaliable = (value < toCompare ? true : false);
                            break;

                        case ">":
                            avaliable = (value > toCompare ? true : false);
                            break;

                        case "=":
                            avaliable = (value == toCompare ? true : false);
                            break;

                        case "~":
                            avaliable = (value == toCompare ? true : false);
                            break;

                        case "!":
                            avaliable = (value != toCompare ? true : false);
                            break;

                        default:
                            avaliable = true;
                            break;
                    }

                    if (avaliable == false)
                        return false;
                }
            }

            return avaliable;
        }
    }
}
