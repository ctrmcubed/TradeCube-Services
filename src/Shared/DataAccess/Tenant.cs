namespace Shared.DataAccess;

public class Tenant
{
    public string ScafellConnectionString { get; }
    
    public Tenant()
    {
        ScafellConnectionString = System.Environment.GetEnvironmentVariable("SCAFELL_SCAFELL_CONNECTION_STRING");
    }
}