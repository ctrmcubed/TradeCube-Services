using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface ITradeService
    {
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> GetTradesAsync(string apiJwtToken, TradeRequest tradeRequest);
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> SaveTradesAsync(string apiKey, IEnumerable<TradeDataObject> trades);
    }
}