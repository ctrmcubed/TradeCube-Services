using Enegen.Messages;

namespace Enegen.Managers;

public interface IEcvnManager
{
    Task<EnegenGenstarEcvnResponse> CreateEcvn(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken);
}