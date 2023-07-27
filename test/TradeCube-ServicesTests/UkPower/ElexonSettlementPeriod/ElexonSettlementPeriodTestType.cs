using Shared.Messages;

namespace TradeCube_ServicesTests.UkPower.ElexonSettlementPeriod;

public class ElexonSettlementPeriodTestType
{
    public int Test { get; init; }
    public string Description { get; init; }
    public string ExpectedError { get; init; }
    public ElexonSettlementPeriodRequest Inputs { get; init; }
    public ElexonSettlementPeriodResponse ExpectedResults { get; init; }
}