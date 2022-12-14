using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

        var encodedUri = WebUtility.UrlEncode(test.Inputs.Uri.ToLower());
        var bodyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(test.Inputs.Body));
        var payload = $"{test.Inputs.AppId}{HttpMethod.Post}{encodedUri}{test.Inputs.TimeStamp}{test.Inputs.Nonce}{bodyBase64}";
        var hash = enegenGenstarHmacFixture.HmacService.GenerateHash(payload, test.Inputs.PrivateSharedKey);
        
        Assert.Equal(test.ExpectedResults.Hmac, hash);
    }
}