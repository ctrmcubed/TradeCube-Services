using System.Collections.Generic;
using Enegen.Services;
using TradeCube_ServicesTests.Helpers;

namespace TradeCube_ServicesTests.Enegen.Hmac;

public class EnegenGenstarHmacFixture
{
    public IList<EnegenGenstarHmacTestType> ExpectedResults { get; init; }
    public HmacService HmacService { get; init; }
    
    public EnegenGenstarHmacFixture()
    {
        HmacService = new HmacService();
        ExpectedResults = FileHelper.ReadJsonFile<IList<EnegenGenstarHmacTestType>>(TestHelper.GetTestDataFolder("TestData/Enegen/Hmac/expected_results_enegen_hmac.json"));
    }
}