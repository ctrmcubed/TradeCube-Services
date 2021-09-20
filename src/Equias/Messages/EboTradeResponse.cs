using Shared.Messages;

namespace Equias.Messages
{
    public class EboTradeResponse : ApiResponse
    {
        public string TradeId { get; set; }
        public int TradeVersion { get; set; }
    }
}