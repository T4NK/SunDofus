using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DofusOrigin.Settings
{
    public class Configuration
    {
        private Dictionary<string, string> _elements;

        public Configuration()
        {
            _elements = new Dictionary<string, string>();
        } 

        public void LoadConfiguration(string path)
        {
            var reader = new StreamReader(path);

            while (!reader.EndOfStream)
            {
                var Line = reader.ReadLine();

                if (!Line.StartsWith("$")) 
                    continue;

                var lineInfos = Line.Substring(1).Split(' ');
                _elements.Add(lineInfos[0], lineInfos[1].Substring(0, lineInfos[1].Length - 1));
            }

            reader.Close();
        }

        public void InsertElement(string _key, string _value)
        {
            if(!_elements.ContainsKey(_key))
                _elements.Add(_key, _value);
        }

        public string GetStringElement(string _element)
        {
            if (!_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return _elements[_element];
        }

        public int GetIntElement(string _element)
        {
            if (!_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return int.Parse(_elements[_element]);
        }

        public bool GetBoolElement(string _element)
        {
            if (!_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return bool.Parse(_elements[_element]);
        }

        public long GetLongElement(string _element)
        {
            if (!_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return long.Parse(_elements[_element]);
        }
    }
}
