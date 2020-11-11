using System.Threading.Tasks;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;

namespace TradeCube_Services.Services.ThirdParty.Enegen
{
    public interface IEnegenGenstarService
    {
        Task<ApiResponseWrapper<WebServiceResponse>> Trade(EnegenGenstarTradeParameters enegenGenstarTradeParameters);
    }
}