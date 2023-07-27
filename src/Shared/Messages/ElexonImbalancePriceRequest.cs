using System.Text.Json.Serialization;

namespace Shared.Messages;

public class ElexonImbalancePriceRequest
{
    public string ApiKey { get; init; }
    
    [JsonPropertyName("ElexonAPIKey")]
    public string ElexonApiKey { get; init; }
    
    public string StartDate { get; init; }
    public string EndDate { get; init; }
    public string Cube { get; init; }
    public string DataItem { get; init; }
    public string Layer { get; init; }
    public string Mode { get; init; }
}