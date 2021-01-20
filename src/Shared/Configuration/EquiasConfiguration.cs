using System;

namespace Shared.Configuration
{
    public class EquiasConfiguration : IEquiasConfiguration
    {
        public string ApiDomain { get; }
        public string RequestTokenUri { get; }
        public string AddPhysicalTradeUri { get; }

        public EquiasConfiguration()
        {
            ApiDomain = Environment.GetEnvironmentVariable("EQUIAS_API_DOMAIN");
            RequestTokenUri = Environment.GetEnvironmentVariable("EQUIAS_REQUEST_TOKEN_URI");
            AddPhysicalTradeUri = Environment.GetEnvironmentVariable("EQUIAS_ADD_PHYSICAL_TRADE_URI");
        }
    }
}
