using System.Collections.Generic;

namespace Shared.Messages;

public class TradeDetailResponse
{
    public string TradeReference { get; init; }
    public int TradeLeg { get; init; }
    public string TimeHierarchy { get; init; }
    public string VolumeUnit { get; init; }
    public string PriceUnit { get; init; }
    public string Currency { get; init; }
    public string Status { get; init; }
    public IEnumerable<TimeNodeProfileValueBase> Profile { get; init; }
}