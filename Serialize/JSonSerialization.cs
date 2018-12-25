using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serialize
{
    public class JSonSerialization<T> : ISerializable
    {
        public object Deserialize( string serStr)
        {
            return JsonConvert.DeserializeObject<T>(serStr);
        }

        public string Serialize(object inputObj)
        {
            return JsonConvert.SerializeObject(inputObj);
        }
    }
}