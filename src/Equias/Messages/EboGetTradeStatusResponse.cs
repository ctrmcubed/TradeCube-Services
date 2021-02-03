using Shared.Messages;
using System.Collections.Generic;

namespace Equias.Messages
{
    public class EboGetTradeStatusResponse : ApiResponse
    {
        public IEnumerable<EboGetTradeStatus> States { get; set; }

        public EboGetTradeStatusResponse()
        {
            States = new List<EboGetTradeStatus>();
        }
    }
}