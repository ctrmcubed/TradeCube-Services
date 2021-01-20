using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ITradeService
    {
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> GetTradesAsync(string apiJwtToken, TradeRequest tradeRequest);
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> SaveTradesAsync(string apiKey, IEnumerable<TradeDataObject> trades);
    }
}