using Enegen.Messages;
using Shared.Messages;

namespace Enegen.Services
{
    public interface IEcvnService
    {
        Task<ApiResponseWrapper<EnegenGenstarEcvnResponse>> NotifyAsync(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken);
    }
}