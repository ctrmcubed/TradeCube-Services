using System.Collections.Generic;
using Shared.Messages;

namespace TradeCube_ServicesTests.Shared;

public class TradeDetailTestType
{
    public TradeDetailRequest Inputs { get; init; }
    public ApiResponseWrapper<IList<TradeDetailResponse>> Response { get; init; }  
}