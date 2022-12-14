using Enegen.Messages;

namespace Enegen.Managers;

public interface IEcvnManager
{
    Task<EcvnResponse> NotifyAsync(EcvnRequest ecvnRequest, string apiJwtToken);
}