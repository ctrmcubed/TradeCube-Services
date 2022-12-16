using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Messages;

namespace Shared.Services;

public interface ITradeDetailService
{
    Task<ApiResponseWrapper<IEnumerable<TradeDetailResponse>>> GetTradeDetailAsync(string tradeReference, int tradeLeg, string apiJwtToken);
}