using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface ITradeProfileService
    {
        Task<ApiResponseWrapper<IEnumerable<TradeProfileResponse>>> TradeProfiles(string apiJwtToken, TradeProfileRequest tradeProfileRequest);
    }
}