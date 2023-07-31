using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Shared.Types.Elexon;

public class ResponseResponseBody
{
    [XmlElement("responseList")]
    [JsonPropertyName("responseList")]
    public ResponseListItems ResponseList { get; init; }
}