using Shared.Messages;
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

            var fidectusConfiguration = await fidectusTestFixture.FidectusManager.GetFidectusConfiguration("apiJwtToken");
            var tradeKey = new TradeKey(test.Inputs.TradeReference, test.Inputs.TradeLeg);
            var confirmationResponse = await fidectusTestFixture.FidectusManager.ConfirmAsync(tradeKey, "apiJwtToken", fidectusConfiguration);

            Assert.True(confirmationResponse.IsSuccess());
        }
    }
}
