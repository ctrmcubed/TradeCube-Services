using System.Linq;
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
        public async Task TestPostConfirmation()
        {
            var test = fidectusTestFixture.ExpectedResults.SingleOrDefault(t => t.Description == "UK Power Baseload Month March GMT");

            Assert.NotNull(test);

            var tradeDataObject = await fidectusTestFixture.FidectusManager.GetTradeAsync(test.Inputs.TradeReference, test.Inputs.TradeLeg, "apiJwtToken");
            var (tradeConfirmation, configurationHelper) = await fidectusTestFixture.FidectusManager.CreateTradeConfirmationAsync(tradeDataObject, "apiJwtToken");
            var confirmationResponse = await fidectusTestFixture.FidectusManager.SendConfirmationAsync(tradeConfirmation, "apiJwtToken");

            Assert.True(confirmationResponse.IsSuccessStatusCode);
        }
    }
}
