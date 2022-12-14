using Enegen.Messages;

namespace Enegen.Managers;

public interface IEcvnManager
{
    Task<EnegenGenstarEcvnResponse> NotifyAsync(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken);
}