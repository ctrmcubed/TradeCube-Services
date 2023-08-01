using System.Collections.Generic;
using System.Diagnostics;
using Shared.Messages;

namespace TradeCube_ServicesTests.Shared;

[DebuggerDisplay("TradeReference={Inputs.TradeReference}")]
public class TradeDetailTestType
{
    public TradeDetailRequest Inputs { get; init; }
    public ApiResponseWrapper<IList<TradeDetailResponse>> Response { get; init; }  
}