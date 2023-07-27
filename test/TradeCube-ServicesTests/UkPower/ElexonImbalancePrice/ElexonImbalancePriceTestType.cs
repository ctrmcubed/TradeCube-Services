using Shared.Messages;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonImbalancePriceTestType
{
    public int Test { get; init; }
    public string Description { get; init; }
    public string ExpectedError { get; init; }
    public ElexonImbalancePriceRequest Inputs { get; init; }
    public ElexonImbalancePriceResponse ExpectedResults { get; init; }
}