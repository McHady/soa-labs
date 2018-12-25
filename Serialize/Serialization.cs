using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialize
{
    public class Serialization<T> where T : new()
    {
        private string type;

        public Serialization(string type)
        {
            this.type = type;
        }

        // сильные мувы с интерфейсами!

        private string SerSel(ISerializable serializable, object input) => serializable.Serialize(input);

        public string Serialize(object input)
        {
            switch (type)
            {
                case "Xml":
                    return SerSel(new XmlSerialization<T>(), input);

                case "Json":
                    return SerSel(new JSonSerialization<T>(), input);
                    
            }

            return "";
        }

        public object Deserialize(string serStr)
        {
            switch (type)
            {
                case "Xml":
                    return DeserSel(new XmlSerialization<T>(), serStr);

                case "Json":
                    return DeserSel(new JSonSerialization<T>(), serStr);
            }

            return null;
        }

        private object DeserSel(ISerializable serializable, string serStr) => serializable.Deserialize(serStr);
    }
}
