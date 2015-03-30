using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace YnetUn
{
    public class XmlHelper
    {
        private readonly Dictionary<Type, XmlSerializer> _serializersCache = new Dictionary<Type, XmlSerializer>();

        public T Serilalize<T>(string content,string rootName="") where T : class, new()
        {

            var serializer = GetSerializer<T>(rootName);
            var result = serializer.Deserialize(new StringReader(content));
            return result as T;
        }

        private XmlSerializer GetSerializer<T>(string name)
        {
            XmlSerializer serializer;
            if (_serializersCache.ContainsKey(typeof(T)))
            {
                serializer = _serializersCache[typeof(T)] as XmlSerializer;
            }
            else
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var xRoot = new XmlRootAttribute();
                    xRoot.ElementName = name;
                    xRoot.IsNullable = true;

                    serializer = new XmlSerializer(typeof(T), xRoot);
                }
                else
                {
                    serializer = new XmlSerializer(typeof(T));
                }
                _serializersCache.Add(typeof(T), serializer);
            }
            return serializer;
        }
    }
}
