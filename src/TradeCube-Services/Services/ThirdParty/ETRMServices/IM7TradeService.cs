using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Models.ThirdParty.ETRMServices;

namespace TradeCube_Services.Services.ThirdParty.ETRMServices
{
    public interface IM7TradeService
    {
        Task<TradeDataObject> ConvertTradeAsync(OwnTrade m7Trade, string apiKey);
    }
}