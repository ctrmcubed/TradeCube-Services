using System.Text.Json.Serialization;
using Shared.Messages;

namespace Enegen.Messages;

public class EnegenGenstarEcvnResponse : ApiResponse
{
    public string ContractName { get; init; }
    public string ContractDescription { get; init; }
    public string Trader { get; init; }
    public string TraderProdConFlag { get; init; }

    public string Party2 { get; init; }
    public string Party2ProdConFlag { get; init; }

    public string ContractStartDate { get; init; }
    public string ContractEndDate { get; init; }

    [JsonPropertyName("ContractGroupID")] 
    public string ContractGroupId { get; init; }

    public string ContractProfile { get; init; }
    public bool Evergreen { get; init; }
    public IEnumerable<EnergyVolumeItem> EnergyVolumeItems { get; init; }
}