namespace Shared.DataAccess;

public class Tenant
{
    public string TradeCubeServicesConnectionString { get; }
    
    public Tenant()
    {
        TradeCubeServicesConnectionString = System.Environment.GetEnvironmentVariable("TRADECUBE_SERVICES_CONNECTION_STRING");
    }
}