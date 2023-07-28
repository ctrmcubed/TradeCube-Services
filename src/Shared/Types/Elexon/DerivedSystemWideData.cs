using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Shared.Types.Elexon;

public class DerivedSystemWideData
{
    [XmlElement("responseMetadata")]
    [JsonPropertyName("responseMetadata")]
    public ResponseResponseMetadata ResponseMetadata { get; init; }

    [XmlElement("responseHeader")]
    [JsonPropertyName("responseHeader")]
    public ResponseResponseHeader ResponseHeader { get; init; }

    [XmlElement("responseBody")]
    [JsonPropertyName("responseBody")]
    public ResponseResponseBody ResponseBody { get; init; }
}