namespace TradeCube_Services.Configuration
{
    public interface ITradeCubeConfiguration
    {
        string TradeCubeApiDomain { get; set; }
        string TradeCubeApiPort { get; set; }
        public string WebApiUrl();
    }
}