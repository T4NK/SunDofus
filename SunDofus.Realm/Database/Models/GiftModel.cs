using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunDofus.Database.Models
{
    class GiftModel
    {
        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private int _target;

        public int Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        private int _itemID;

        public int ItemID
        {
            get
            {
                return _itemID;
            }
            set
            {
                _itemID = value;
            }
        }

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        private string _image;

        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}~{1}~{2}~{3}~{4}", _ID, _title, _message, _itemID, _image);
        }
    }
}
