using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Shared.Types.Elexon;

public class ResponseResponseHeader
{
    [XmlElement("recordType")]
    [JsonPropertyName("recordType")]
    public string RecordType { get; init; }

    [XmlElement("fileType")]
    [JsonPropertyName("fileType")]
    public string FileType { get; init; }
}