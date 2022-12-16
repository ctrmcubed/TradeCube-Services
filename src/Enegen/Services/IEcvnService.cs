using Shared.Messages;

namespace Enegen.Services
{
    public interface IEcvnService
    {
        Task<ApiResponseWrapper<string>> NotifyAsync(string uri, string appId, string hashedPayload,
            string nonce, long unixTimeSeconds, string body);
    }
}