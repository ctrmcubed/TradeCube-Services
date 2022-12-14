using Shared.Messages;

namespace TradeCube_ServicesTests.Enegen.Ecvn;

public class ElexonSettlementPeriodTestType
{
    public int Test { get; init; }
    public string Description { get; init; }
    public ElexonSettlementPeriodRequest Inputs { get; init; }
    public ElexonSettlementPeriodResponse Response { get; init; }  
}