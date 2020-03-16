using System;

namespace TradeCube_Services.Configuration
{
    public class TradeCubeConfiguration : ConfigurationBase, ITradeCubeConfiguration
    {
        public string TradeCubeApiDomain { get; set; }
        public string TradeCubeApiPort { get; set; }

        public TradeCubeConfiguration()
        {
            TradeCubeApiDomain = Environment.GetEnvironmentVariable("TRADECUBE_API_DOMAIN");
            TradeCubeApiPort = Environment.GetEnvironmentVariable("TRADECUBE_API_PORT");
        }

        public string WebApiUrl() => $"{TradeCubeApiDomain}{Port(TradeCubeApiPort)}/";
    }
}
