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

            var (tradeConfirmation, settingHelper) = await fidectusTestFixture.FidectusManager.CreateTradeConfirmationAsync(test.Inputs.TradeReference, test.Inputs.TradeLeg, "apiJwtToken");
            var confirmationResponse = await fidectusTestFixture.FidectusManager.SendTradeConfirmationAsync(tradeConfirmation, "apiJwtToken", settingHelper);

            Assert.True(confirmationResponse.IsSuccessStatusCode);
        }
    }
}
