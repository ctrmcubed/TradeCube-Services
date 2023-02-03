using Enegen.Messages;
using Enegen.Services;
using Shared.Messages;

namespace Enegen.Managers;

public interface IEcvnManager
{
    Task<EcvnContext> CreateEcvnContext(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken);
    Task<EnegenGenstarEcvnResponse> CreateEcvn(EcvnContext context, string apiJwtToken);
    Task<ApiResponseWrapper<string>> NotifyEcvn(EnegenGenstarEcvnResponse enegenGenstarEcvnResponse, EcvnContext ecvnContext);
}