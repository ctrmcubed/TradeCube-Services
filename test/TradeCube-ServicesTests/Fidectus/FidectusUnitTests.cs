using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TradeCube_ServicesTests.Fidectus
{
    public class FidectusUnitTests : IClassFixture<FidectusTestFixture>
    {
        private readonly FidectusTestFixture fidectusTestFixture;

        public FidectusUnitTests(FidectusTestFixture fidectusTestFixture)
        {
            this.fidectusTestFixture = fidectusTestFixture;
        }

        [Fact]
        public async Task Test_UK_Power_Baseload_Month_March_GMT()
        {
            await RunTest("UK Power Baseload Month March GMT");
        }

        [Fact]
        public async Task Test_UK_Power_7DPeak_Month_March()
        {
            await RunTest("UK Power 7DPeak Month March");
        }

        [Fact]
        public async Task Test_UK_Power_Baseload_Day_Short_Day_Transition()
        {
            await RunTest("UK Power Baseload Day Short Day Transition");
        }

        [Fact]
        public async Task Test_UK_Power_Baseload_March_BST()
        {
            await RunTest("UK Power Baseload March BST");
        }

        [Fact]
        public async Task Test_UK_Power_Baseload_October_BST()
        {
            await RunTest("UK Power Baseload October BST");
        }

        [Fact]
        public async Task Test_UK_Power_Baseload_Day_Long_Day_Transition()
        {
            await RunTest("UK Power Baseload Day Long Day Transition");
        }

        [Fact]
        public async Task Test_UK_Power_7DPeak_Month_October()
        {
            await RunTest("UK Power 7DPeak Month October");
        }

        [Fact]
        public async Task Test_UK_Power_Baseload_Day_GMT()
        {
            await RunTest("UK Power Baseload Day GMT");
        }

        [Fact]
        public async Task Test_NBP_Gas_Month()
        {
            await RunTest("NBP Gas Month");
        }

        [Fact]
        public async Task Test_NBP_Gas_March_GMT()
        {
            await RunTest("NBP Gas March GMT");
        }

        [Fact]
        public async Task Test_NBP_Gas_Day_Short_Day_Transition()
        {
            await RunTest("NBP Gas Day Short Day Transition");
        }

        [Fact]
        public async Task Test_NBP_Gas_March_BST()
        {
            await RunTest("NBP Gas March BST");
        }

        [Fact]
        public async Task Test_NBP_Gas_October_BST()
        {
            await RunTest("NBP Gas October BST");
        }

        [Fact]
        public async Task Test_NBP_Gas_Long_Day_Transition()
        {
            await RunTest("NBP Gas Long Day Transition");
        }

        [Fact]
        public async Task Test_NBP_Gas_October_GMT()
        {
            await RunTest("NBP Gas October GMT");
        }

        private async Task RunTest(string testName)
        {
            var test = fidectusTestFixture.ExpectedResults.SingleOrDefault(t => t.Description == testName);

            Assert.NotNull(test);

            var (tradeConfirmation, _) = await fidectusTestFixture.FidectusManager.CreateTradeConfirmationAsync(test.Inputs.TradeReference, test.Inputs.TradeLeg, "apiJwtToken");

            Assert.Equal(test.ExpectedResults.TradeConfirmation.DocumentId, tradeConfirmation.DocumentId);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.DocumentUsage, tradeConfirmation.DocumentUsage);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.SenderId, tradeConfirmation.SenderId);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.ReceiverId, tradeConfirmation.ReceiverId);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.ReceiverRole, tradeConfirmation.ReceiverRole);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.DocumentVersion, tradeConfirmation.DocumentVersion);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.Market, tradeConfirmation.Market);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.Commodity, tradeConfirmation.Commodity);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.TransactionType, tradeConfirmation.TransactionType);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.DeliveryPointArea, tradeConfirmation.DeliveryPointArea);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.BuyerParty, tradeConfirmation.BuyerParty);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.SellerParty, tradeConfirmation.SellerParty);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.LoadType, tradeConfirmation.LoadType);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.Agreement, tradeConfirmation.Agreement);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.Currency.CurrencyCodeType, tradeConfirmation.Currency.CurrencyCodeType);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.Currency.UseFractionUnit, tradeConfirmation.Currency.UseFractionUnit);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.TotalVolume, tradeConfirmation.TotalVolume);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.TotalVolumeUnit, tradeConfirmation.TotalVolumeUnit);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.TradeDate, tradeConfirmation.TradeDate);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.CapacityUnit, tradeConfirmation.CapacityUnit);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.PriceUnit.Currency.CurrencyCodeType, tradeConfirmation.PriceUnit.Currency.CurrencyCodeType);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.PriceUnit.Currency.UseFractionUnit, tradeConfirmation.PriceUnit.Currency.UseFractionUnit);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.PriceUnit.CapacityUnit, tradeConfirmation.PriceUnit.CapacityUnit);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.TotalContractValue, tradeConfirmation.TotalContractValue);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.TraderName, tradeConfirmation.TraderName);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.HubCodificationInformation?.BuyerHubCode, tradeConfirmation.HubCodificationInformation?.BuyerHubCode);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.HubCodificationInformation?.SellerHubCode, tradeConfirmation.HubCodificationInformation?.SellerHubCode);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.AccountAndChargeInformation?.BuyerEnergyAccountIdentification, tradeConfirmation.AccountAndChargeInformation?.BuyerEnergyAccountIdentification);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.AccountAndChargeInformation?.SellerEnergyAccountIdentification, tradeConfirmation.AccountAndChargeInformation?.SellerEnergyAccountIdentification);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.AccountAndChargeInformation?.TransmissionChargeIdentification, tradeConfirmation.AccountAndChargeInformation?.TransmissionChargeIdentification);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.AccountAndChargeInformation?.NotificationAgent, tradeConfirmation.AccountAndChargeInformation?.NotificationAgent);

            // Time Interval Quantities
            Assert.Equal(test.ExpectedResults.TradeConfirmation.TimeIntervalQuantities.Count(), tradeConfirmation.TimeIntervalQuantities.Count());

            var zippedTimeIntervalQuantities = test.ExpectedResults.TradeConfirmation.TimeIntervalQuantities.Zip(tradeConfirmation.TimeIntervalQuantities,
                (e, a) => new
                {
                    DeliveryStartTimestamp = e.DeliveryStartTimestamp == a.DeliveryStartTimestamp
                        ? a.DeliveryStartTimestamp
                        : throw new DataException("Misaligned DeliveryStartDateAndTime"),
                    DeliveryEndTimestamp = e.DeliveryEndTimestamp == a.DeliveryEndTimestamp
                        ? a.DeliveryEndTimestamp
                        : throw new DataException("Misaligned DeliveryEndDateAndTime"),
                    e,
                    a
                });

            foreach (var timeIntervalQuantity in zippedTimeIntervalQuantities)
            {
                Assert.Equal(timeIntervalQuantity.e.DeliveryStartTimestamp, timeIntervalQuantity.a.DeliveryStartTimestamp);
                Assert.Equal(timeIntervalQuantity.e.DeliveryEndTimestamp, timeIntervalQuantity.a.DeliveryEndTimestamp);
                Assert.Equal(timeIntervalQuantity.e.ContractCapacity, timeIntervalQuantity.a.ContractCapacity);
                Assert.Equal(timeIntervalQuantity.e.Price, timeIntervalQuantity.a.Price);
            }

            // Agents
            Assert.Equal(test.ExpectedResults.TradeConfirmation.Agents is null, tradeConfirmation.Agents is null);
            Assert.Equal(test.ExpectedResults.TradeConfirmation.Agents?.Count(), tradeConfirmation.Agents?.Count());

            if (test.ExpectedResults.TradeConfirmation.Agents is not null && tradeConfirmation.Agents is not null)
            {
                var zippedAgents = test.ExpectedResults.TradeConfirmation.Agents.Zip(tradeConfirmation.Agents,
                    (e, a) => new
                    {
                        AgentName = a.AgentName == e.AgentName
                            ? a.AgentName
                            : throw new DataException("Misaligned Agent"),
                        e,
                        a
                    });

                foreach (var agent in zippedAgents)
                {
                    Assert.Equal(agent.e.AgentType, agent.a.AgentType);
                    Assert.Equal(agent.e.AgentName, agent.a.AgentName);
                    Assert.Equal(agent.e.Ecvna.BscPartyId, agent.a.Ecvna.BscPartyId);
                    Assert.Equal(agent.e.Ecvna.BuyerEnergyAccount, agent.a.Ecvna.BuyerEnergyAccount);
                    Assert.Equal(agent.e.Ecvna.SellerEnergyAccount, agent.a.Ecvna.SellerEnergyAccount);
                    Assert.Equal(agent.e.Ecvna.BuyerId, agent.a.Ecvna.BuyerId);
                    Assert.Equal(agent.e.Ecvna.SellerId, agent.a.Ecvna.SellerId);
                }
            }
        }
    }
}
