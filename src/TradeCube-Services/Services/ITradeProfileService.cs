using System.Collections.Generic;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface ITradeProfileService
    {
        IAsyncEnumerable<ApiResponseWrapper<IEnumerable<TradeProfileResponse>>> GetProfiles(IEnumerable<TradeDataObject> trades, string apiJwtToken);
    }
}