using System.Collections.Generic;

namespace Fidectus.Messages
{
    public class ConfirmationResultResponses
    {
        public string Message { get; set; }
        public IEnumerable<ConfirmationResultResponse> Responses { get; set; }
    }
}