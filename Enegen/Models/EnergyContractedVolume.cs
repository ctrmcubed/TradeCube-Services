using System.Text.Json.Serialization;

namespace Enegen.Models;

public class EnergyContractedVolume
{
    public string ContractName { get; init; }
    public string ContractDescription { get; init; }
    public string Trader { get; init; }
    public string TraderProdConFlag { get; init; }
    public string Party2 { get; init; }
    public string Party2ProdConFlag { get; init; }
    
    [JsonPropertyName("ContractGroupID")]
    public string ContractGroupId { get; init; }
    
    public string ContractProfile { get; init; }
    public string Evergreen { get; init; }

    public DateTime ContractStartDate { get; init; }
    public DateTime ContractEndDate { get; init; }

    public IEnumerable<EnergyContractedVolumeItem> EnergyVolumeItems { get; init; }
}