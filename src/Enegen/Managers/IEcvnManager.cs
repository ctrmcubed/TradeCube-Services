using Enegen.Messages;
using Enegen.Services;
using Shared.Messages;

namespace Enegen.Managers;

public interface IEcvnManager
{
    Task<EcvnContext> CreateEcvnContext(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken);
    Task<EnegenGenstarEcvnExternalRequest> CreateEcvnRequest(EcvnContext context, string apiJwtToken);
    Task<ApiResponseWrapper<string>> NotifyEcvn(EnegenGenstarEcvnExternalRequest enegenGenstarEcvnExternalRequest, EcvnContext ecvnContext);
}