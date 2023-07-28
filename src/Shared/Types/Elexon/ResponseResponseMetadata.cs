using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Shared.Types.Elexon;

public class ResponseResponseMetadata
{
    [XmlElement("httpCode")]
    [JsonPropertyName("httpCode")]
    public int HttpCode { get; init; }

    [XmlElement("errorType")]
    [JsonPropertyName("errorType")]
    public string ErrorType { get; init; }

    [XmlElement("description")]
    [JsonPropertyName("description")]
    public string Description { get; init; }

    [XmlElement("cappingApplied")]
    [JsonPropertyName("cappingApplied")]
    public string CappingApplied { get; init; }

    [XmlElement("cappingLimit")]
    [JsonPropertyName("cappingLimit")]
    public int CappingLimit { get; init; }

    [XmlElement("queryString")]
    [JsonPropertyName("queryString")]
    public string QueryString { get; init; }
}