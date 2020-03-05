using System.Threading.Tasks;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;

namespace TradeCube_Services.Services
{
    public interface INotificationService
    {
        Task<ApiResponseWrapper<WebhookResponse>> Notify(WebhookParameters webhookParameters);
    }
}