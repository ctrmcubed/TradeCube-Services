using Equias.Messages;
using System;
using System.Threading.Tasks;
using TradeCube_ServicesTests.Shared;
using Xunit;

namespace TradeCube_ServicesTests.Equias
{
    public class EquiasIntegrationTests : IClassFixture<EquiasTestFixture>
    {
        private readonly EquiasTestFixture equiasTestFixture;

        public EquiasIntegrationTests(EquiasTestFixture equiasTestFixture)
        {
            this.equiasTestFixture = equiasTestFixture;
            Configuration.SetEnvironmentVariables();
        }

        [Fact]
        public async Task TestGetAuthenticationToken()
        {
            var authenticationToken = await RequestTokenResponse();

            Assert.False(string.IsNullOrEmpty(authenticationToken.Token));
        }

        [Fact]
        public async Task TestAddPhysicalTrade()
        {
            var authenticationToken = await RequestTokenResponse();
            var addPhysicalTradeResponse = await equiasTestFixture.EquiasManager.AddPhysicalTrade(authenticationToken);

            Assert.Equal("", addPhysicalTradeResponse.TradeId);
            Assert.Equal(1, addPhysicalTradeResponse.TradeVersion);
        }

        private async Task<RequestTokenResponse> RequestTokenResponse()
        {
            var username = Environment.GetEnvironmentVariable("EQUIAS_USERNAME");
            var password = Environment.GetEnvironmentVariable("EQUIAS_PASSWORD");

            var authenticationService = equiasTestFixture.EquiasAuthenticationService;

            return await authenticationService.GetAuthenticationToken(new RequestTokenRequest(username, password));
        }
    }
}
