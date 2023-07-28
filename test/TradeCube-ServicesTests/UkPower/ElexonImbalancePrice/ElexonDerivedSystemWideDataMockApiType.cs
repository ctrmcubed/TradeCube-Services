using Shared.Types.Elexon;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonDerivedSystemWideDataMockApiType
{
    public DerivedSystemWideDataRequest Inputs { get; init; }
    public string Response { get; init; }
}