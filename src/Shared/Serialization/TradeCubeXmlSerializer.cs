using System.IO;
using System.Xml.Serialization;

namespace Shared.Serialization;

public class TradeCubeXmlSerializer
{
    public T Deserialize<T>(Stream response, string rootElementName)
    {
        return (T)new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName)).Deserialize(response);
    }
    
    public T Deserialize<T>(string response, string rootElementName)
    {
        return (T)new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName)).Deserialize(new StringReader(response));
    }
}