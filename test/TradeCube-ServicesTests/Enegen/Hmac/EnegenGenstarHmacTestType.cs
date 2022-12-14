using Shared.Messages;

namespace TradeCube_ServicesTests.Enegen.Hmac;

public class EnegenGenstarHmacTestType
{
    public int Test { get; init; }
    public string Description { get; init; }
    public EnegenGenstarHmacRequest Inputs { get; init; }
    public EnegenGenstarHmacResponse ExpectedResults { get; init; }    
}