using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Shared.Types.Elexon;

public class ResponseListItems
{
    [XmlElement("item")]
    [JsonPropertyName("item")]
    public ResponseResponseBodyItem[] Item { get; init; }
}