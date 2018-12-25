using System.Runtime.InteropServices.ComTypes;

namespace Serialize
{
    public interface ISerializable
    {
    object Deserialize(string serStr);
    string Serialize(object inputObj);
    }
}