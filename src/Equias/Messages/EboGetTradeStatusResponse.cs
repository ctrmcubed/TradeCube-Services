using System.Collections.Generic;

namespace Equias.Messages
{
    public class EboGetTradeStatusResponse
    {
        public IEnumerable<EboGetTradeStatus> States { get; set; }
    }
}