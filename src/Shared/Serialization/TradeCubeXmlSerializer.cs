using System.IO;
using System.Xml.Serialization;

namespace Shared.Serialization;

public class TradeCubeXmlSerializer
{
    public T Deserialize<T>(Stream stream, string rootElementName)
    {
        return (T)new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName)).Deserialize(stream);
    }
}