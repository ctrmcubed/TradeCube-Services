using Shared.Messages;

namespace Fidectus.Messages
{
    public class ConfirmationResponse : ApiResponse
    {
        public string DocumentId { get; set; }
    }
}
