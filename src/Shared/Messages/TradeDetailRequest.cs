using System.Diagnostics;

namespace Shared.Messages;

[DebuggerDisplay("TradeReference={TradeReference}")]
public class TradeDetailRequest
{
    public string TradeReference { get; init; }
    public int TradeLeg { get; init; }
    public bool MarkToMarket { get; init; }
    public bool ProfitAndLoss { get; init; }
}