using System;

namespace Shared.Configuration
{
    public class EquiasConfiguration : IEquiasConfiguration
    {
        public string ApiDomain { get; private set; }
        public string RequestTokenUri { get; }
        public string GetTradeStatusUri { get; }
        public string AddPhysicalTradeUri { get; }
        public string ModifyPhysicalTradeUri { get; }
        public string CancelTradeUri { get; }

        public EquiasConfiguration()
        {
            RequestTokenUri = Environment.GetEnvironmentVariable("EQUIAS_REQUEST_TOKEN_URI");
            GetTradeStatusUri = Environment.GetEnvironmentVariable("EQUIAS_GET_TRADE_STATUS_URI");
            AddPhysicalTradeUri = Environment.GetEnvironmentVariable("EQUIAS_ADD_PHYSICAL_TRADE_URI");
            ModifyPhysicalTradeUri = Environment.GetEnvironmentVariable("EQUIAS_MODIFY_PHYSICAL_TRADE_URI");
            CancelTradeUri = Environment.GetEnvironmentVariable("EQUIAS_CANCEL_TRADE_URI");
        }

        public EquiasConfiguration(string apiDomain)
        {
            ApiDomain = apiDomain;
            RequestTokenUri = Environment.GetEnvironmentVariable("EQUIAS_REQUEST_TOKEN_URI");
            GetTradeStatusUri = Environment.GetEnvironmentVariable("EQUIAS_GET_TRADE_STATUS_URI");
            AddPhysicalTradeUri = Environment.GetEnvironmentVariable("EQUIAS_ADD_PHYSICAL_TRADE_URI");
            ModifyPhysicalTradeUri = Environment.GetEnvironmentVariable("EQUIAS_MODIFY_PHYSICAL_TRADE_URI");
            CancelTradeUri = Environment.GetEnvironmentVariable("EQUIAS_CANCEL_TRADE_URI");
        }
    }
}
