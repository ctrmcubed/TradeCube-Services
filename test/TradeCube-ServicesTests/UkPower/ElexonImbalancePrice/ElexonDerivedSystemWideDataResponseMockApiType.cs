using System.Text.Json.Serialization;
using Shared.Types.Elexon;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonDerivedSystemWideDataResponseMockApiType
{
    [JsonPropertyName("response")]
    public DerivedSystemWideData Response { get; init; }
}