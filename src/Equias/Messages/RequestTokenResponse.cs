using Shared.Messages;

namespace Equias.Messages
{
    public class RequestTokenResponse : ApiResponse
    {
        public string Token { get; set; }
    }
}