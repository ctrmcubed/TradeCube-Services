using Fidectus.Messages;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TradeCube_ServicesTests.Fidectus
{
    public class FidectusIntegrationTests : IClassFixture<FidectusTestFixture>
    {
        private readonly FidectusTestFixture fidectusTestFixture;

        public FidectusIntegrationTests(FidectusTestFixture fidectusTestFixture)
        {
            this.fidectusTestFixture = fidectusTestFixture;
        }

        [Fact]
        public async Task TestGetAuthenticationToken()
        {
            Assert.False(string.IsNullOrEmpty((await RequestTokenResponse()).AccessToken));
        }

        private async Task<RequestTokenResponse> RequestTokenResponse()
        {
            var username = Environment.GetEnvironmentVariable("FIDECTUS_CLIENT_ID");
            var password = Environment.GetEnvironmentVariable("FIDECTUS_SECRET");

            var authenticationService = fidectusTestFixture.FidectusManager;

            return await authenticationService.CreateAuthenticationTokenAsync(new RequestTokenRequest(username, password), "apiJwtToken");
        }
    }
}
