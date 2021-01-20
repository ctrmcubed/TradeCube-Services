using Shared.Messages;
using System.Threading.Tasks;
using TradeCube_Services.Parameters;

namespace TradeCube_Services.Services
{
    public interface IConfirmationReportService
    {
        Task<ApiResponseWrapper<WebServiceResponse>> CreateReportAsync(ConfirmationReportParameters confirmationReportParameters);
    }
}