using Enegen.Messages;
using Shared.Messages;

namespace Enegen.Services
{
    public interface IEcvnService
    {
        Task<ApiResponseWrapper<EcvnResponse>> NotifyAsync(EcvnRequest ecvnRequest, string apiJwtToken);
    }
}