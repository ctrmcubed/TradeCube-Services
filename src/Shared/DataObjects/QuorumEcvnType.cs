using System.Collections.Generic;
using System.Text.Json.Serialization;

public class QuorumEcvnType
{
    public string ValidationMessage { get; set; }
    public string Filename { get; set; }
    public string Status { get; set; }
        
    [JsonPropertyName("ECVN")]
    public IEnumerable<TradeQuorumGetStatusEcvn> Ecvn { get; set; }
}