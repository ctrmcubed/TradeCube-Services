using System.Threading.Tasks;
using System.Xml.Linq;
using TradeCube_Services.DataObjects;

namespace TradeCube_Services.Services.ThirdParty.ETRMServices
{
    public interface IM7TradeService
    {
        Task<TradeDataObject> ConvertTradeAsync(XElement m7Trade, string apiKey);
    }
}