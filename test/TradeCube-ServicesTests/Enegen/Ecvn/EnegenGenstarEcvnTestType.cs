using Enegen.Messages;

namespace TradeCube_ServicesTests.Enegen.Ecvn;

public class EnegenGenstarEcvnTestType
{
    public int Test { get; init; }
    public string Description { get; init; }
    public string ExpectedError { get; init; }
    public EnegenGenstarEcvnRequest Inputs { get; init; }
    public EnegenGenstarEcvnResponse ExpectedResults { get; init; }       
}
