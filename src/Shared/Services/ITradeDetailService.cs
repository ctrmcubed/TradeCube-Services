using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Messages;

public interface ITradeDetailService
{
    Task<ApiResponseWrapper<IEnumerable<TradeDetailResponse>>> GetTradeDetailAsync(string tradeReference, int tradeLeg, string apiJwtToken);
}