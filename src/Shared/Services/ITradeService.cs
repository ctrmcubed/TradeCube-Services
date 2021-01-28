using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ITradeService
    {
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> GetTradeAsync(string apiJwtToken, string tradeReference, int tradeLeg);
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> GetTradesAsync(string apiJwtToken, TradeRequest tradeRequest);
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> PostTradesViaApiKeyAsync(string apiKey, IEnumerable<TradeDataObject> trades);
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> PutTradesViaJwtAsync(string apiJwtToken, IEnumerable<TradeDataObject> trades);
    }
}