using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DofusOrigin.Settings
{
    public class Configuration
    {
        private Dictionary<string, string> m_elements;

        public Configuration()
        {
            m_elements = new Dictionary<string, string>();
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
                m_elements.Add(lineInfos[0], lineInfos[1].Substring(0, lineInfos[1].Length - 1));
            }

            reader.Close();
        }

        public void InsertElement(string _key, string _value)
        {
            if(!m_elements.ContainsKey(_key))
                m_elements.Add(_key, _value);
        }

        public string GetStringElement(string _element)
        {
            if (!m_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return m_elements[_element];
        }

        public int GetIntElement(string _element)
        {
            if (!m_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return int.Parse(m_elements[_element]);
        }

        public bool GetBoolElement(string _element)
        {
            if (!m_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return bool.Parse(m_elements[_element]);
        }

        public long GetLongElement(string _element)
        {
            if (!m_elements.ContainsKey(_element))
                throw new Exception("Invalid key");

            return long.Parse(m_elements[_element]);
        }
    }
}
