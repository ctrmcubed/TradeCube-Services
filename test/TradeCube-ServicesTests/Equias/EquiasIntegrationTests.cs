using Equias.Messages;
using System;
using System.Threading.Tasks;
using TradeCube_ServicesTests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace TradeCube_ServicesTests.Equias
{
    public class EquiasIntegrationTests : IClassFixture<EquiasTestFixture>
    {
        private readonly EquiasTestFixture equiasTestFixture;
        private readonly ITestOutputHelper testOutputHelper;

        public EquiasIntegrationTests(EquiasTestFixture equiasTestFixture, ITestOutputHelper testOutputHelper)
        {
            this.equiasTestFixture = equiasTestFixture;
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestGetAuthenticationToken()
        {
            Assert.False(string.IsNullOrEmpty((await RequestTokenResponse()).Token));
        }

        [Fact]
        public async Task TestAddPhysicalTrade_Gas()
        {
            var tradeDataObject = await equiasTestFixture.EquiasManager.GetTradeAsync("TEST4", 1, "apiJwtToken");
            var physicalTrade = await equiasTestFixture.EquiasManager.CreatePhysicalTrade(tradeDataObject, "apiJwtToken");

            testOutputHelper.WriteLine(TradeCubeServicesJsonSerializer.Serialize(physicalTrade));

            var addPhysicalTradeResponse = await equiasTestFixture.EquiasManager.AddPhysicalTrade(physicalTrade, await RequestTokenResponse(), "apiJwtToken");

            Assert.Equal("TEST4001", addPhysicalTradeResponse.TradeId);
            Assert.Equal(1, addPhysicalTradeResponse.TradeVersion);
        }

        private async Task<RequestTokenResponse> RequestTokenResponse()
        {
            var username = Environment.GetEnvironmentVariable("EQUIAS_USERNAME");
            var password = Environment.GetEnvironmentVariable("EQUIAS_PASSWORD");

            var authenticationService = equiasTestFixture.EquiasManager;

            return await authenticationService.CreateAuthenticationToken(new RequestTokenRequest(username, password), "apiJwtToken");
        }
    }
}
