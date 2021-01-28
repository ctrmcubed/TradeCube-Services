using Shared.Messages;

namespace Equias.Messages
{
    public class EboPhysicalTradeResponse : ApiResponse
    {
        public string TradeId { get; set; }
        public int TradeVersion { get; set; }
    }
}