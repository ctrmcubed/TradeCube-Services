using System.Linq;
using Xunit;

namespace TradeCube_ServicesTests.Enegen.Hmac;

public class EnegenGenstarHmacUnitTests : IClassFixture<EnegenGenstarHmacFixture>
{
    private readonly EnegenGenstarHmacFixture enegenGenstarHmacFixture;

    public EnegenGenstarHmacUnitTests(EnegenGenstarHmacFixture enegenGenstarHmacFixture)
    {
        this.enegenGenstarHmacFixture = enegenGenstarHmacFixture;
    }

    [Fact]
    public void TEST_0000_HMAC_Test()
    {
        RunTest("TEST 0000 HMAC Test");
    }

    private void RunTest(string testName)
    {
        var test = enegenGenstarHmacFixture.ExpectedResults.SingleOrDefault(t => t.Description == testName);
        
        Assert.NotNull(test);

        var signature = enegenGenstarHmacFixture.HmacService.CreateSignature(test.Inputs.Uri, test.Inputs.Body, test.Inputs.AppId, test.Inputs.TimeStamp, test.Inputs.Nonce);
        var hash = enegenGenstarHmacFixture.HmacService.GenerateHash(signature, test.Inputs.PrivateSharedKey);
        
        Assert.Equal(test.ExpectedResults.Hmac, hash);
    }
}