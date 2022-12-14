using Shared.Messages;

namespace TradeCube_ServicesTests.Enegen.Ecvn;

public class TradeDetailTestType
{
    public int Test { get; init; }
    public string Description { get; init; }
    public TradeDetailRequest Inputs { get; init; }
    public TradeDetailResponse Response { get; init; }  
}