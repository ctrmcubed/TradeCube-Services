using Shared.Messages;
using System.Threading.Tasks;
using TradeCube_Services.Parameters;

namespace TradeCube_Services.Services
{
    public interface INotificationService
    {
        Task<ApiResponseWrapper<WebhookResponse>> NotifyAsync(WebhookParameters webhookParameters);
    }
}