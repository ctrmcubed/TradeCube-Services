using Enegen.Messages;
using Enegen.Services;

namespace Enegen.Managers;

public interface IEcvnManager
{
    Task<EcvnContext> CreateEcvnContext(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken);
    Task<EnegenGenstarEcvnResponse> CreateEcvn(EcvnContext context, string apiJwtToken);
    Task<EnegenGenstarEcvnResponse> NotifyEcvn(EnegenGenstarEcvnResponse enegenGenstarEcvnResponse, EcvnContext ecvnContext);
}