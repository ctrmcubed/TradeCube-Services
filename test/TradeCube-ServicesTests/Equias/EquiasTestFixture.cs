using Equias.Managers;
using Equias.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using System.Collections.Generic;
using System.Linq;
using TradeCube_ServicesTests.Helpers;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.Equias
{
    public class EquiasTestFixture
    {
        public EquiasAuthenticationService EquiasAuthenticationService { get; }
        public IEquiasService EquiasService { get; }
        public IEquiasManager EquiasManager { get; }
        public IEnumerable<TradeDataObject> EquiasTrades { get; }
        public IEnumerable<MappingDataObject> EquiasMappings { get; }
        public IEnumerable<TradeSummaryResponse> EquiasTradeSummaries { get; }
        public IEnumerable<CashflowResponse> EquiasCashflows { get; }
        public IEnumerable<ProfileResponse> EquiasProfiles { get; }
        public IEnumerable<PartyDataObject> EquiasParties { get; }

        public EquiasTestFixture()
        {
            Configuration.SetEnvironmentVariables();

            var defaultHttpClientFactory = new DefaultHttpClientFactory();

            EquiasTrades = FileHelper.ReadJsonFile<IList<TradeDataObject>>(TestHelper.GetTestDataFolder("TestData/Equias/EquiasTrades.json"));
            EquiasMappings = FileHelper.ReadJsonFile<IList<MappingDataObject>>(TestHelper.GetTestDataFolder("TestData/Equias/EquiasMappings.json"));
            EquiasTradeSummaries = FileHelper.ReadJsonFile<IList<TradeSummaryResponse>>(TestHelper.GetTestDataFolder("TestData/Equias/EquiasTradeSummaries.json"));
            EquiasCashflows = FileHelper.ReadJsonFile<IList<CashflowResponse>>(TestHelper.GetTestDataFolder("TestData/Equias/EquiasCashflows.json"));
            EquiasProfiles = FileHelper.ReadJsonFile<IList<ProfileResponse>>(TestHelper.GetTestDataFolder("TestData/Equias/EquiasProfiles.json"));
            EquiasParties = FileHelper.ReadJsonFile<IList<PartyDataObject>>(TestHelper.GetTestDataFolder("TestData/Equias/EquiasParties.json"));

            EquiasAuthenticationService = new EquiasAuthenticationService(defaultHttpClientFactory, new EquiasConfiguration(), new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));
            EquiasService = new EquiasService(defaultHttpClientFactory, new EquiasConfiguration(), new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));

            EquiasManager = new EquiasManager(
                EquiasAuthenticationService,
                EquiasService,
                CreateTradeService(EquiasTrades),
                CreateTradeSummaryService(EquiasTradeSummaries),
                CreateCashflowService(EquiasCashflows),
                CreateProfileService(EquiasProfiles),
                new EquiasMappingService(CreateMappingService(EquiasMappings), CreatePartyService(EquiasParties)),
                new VaultService(defaultHttpClientFactory, new TradeCubeConfiguration(), new Logger<VaultService>(LoggerFactory.Create(l => l.AddConsole()))),
                new Logger<EquiasManager>(LoggerFactory.Create(l => l.AddConsole())));
        }

        private static ITradeService CreateTradeService(IEnumerable<TradeDataObject> trades)
        {
            var service = new Mock<ITradeService>();

            service
                .Setup(c => c.GetTradeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string _, string tradeReference, int tradeLeg) =>
                    new ApiResponseWrapper<IEnumerable<TradeDataObject>> { Data = trades.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg) });

            return service.Object;
        }

        private static IMappingService CreateMappingService(IEnumerable<MappingDataObject> mappings)
        {
            var service = new Mock<IMappingService>();

            service
                .Setup(c => c.GetMappingsViaJwtAsync(It.IsAny<string>()))
                .ReturnsAsync((string _) =>
                    new ApiResponseWrapper<IEnumerable<MappingDataObject>> { Data = mappings });

            return service.Object;
        }

        private static ITradeSummaryService CreateTradeSummaryService(IEnumerable<TradeSummaryResponse> tradeSummaryResponses)
        {
            var service = new Mock<ITradeSummaryService>();

            service
                .Setup(c => c.TradeSummaryAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((string tradeReference, int tradeLeg, string _) =>
                    new ApiResponseWrapper<IEnumerable<TradeSummaryResponse>> { Data = tradeSummaryResponses.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg) });

            return service.Object;
        }

        private static ICashflowService CreateCashflowService(IEnumerable<CashflowResponse> cashflows)
        {
            var service = new Mock<ICashflowService>();

            service
                .Setup(c => c.CashflowAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((string tradeReference, int tradeLeg, string _) =>
                    new ApiResponseWrapper<IEnumerable<CashflowResponse>> { Data = cashflows.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg) });

            return service.Object;
        }

        private static IProfileService CreateProfileService(IEnumerable<ProfileResponse> profiles)
        {
            var service = new Mock<IProfileService>();

            service
                .Setup(c => c.ProfileAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string tradeReference, int tradeLeg, string _, string _) =>
                    new ApiResponseWrapper<IEnumerable<ProfileResponse>> { Data = profiles.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg) });

            return service.Object;
        }

        private static IPartyService CreatePartyService(IEnumerable<PartyDataObject> parties)
        {
            var service = new Mock<IPartyService>();

            service
                .Setup(c => c.GetPartyAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string party, string _) =>
                    new ApiResponseWrapper<IEnumerable<PartyDataObject>> { Data = parties.Where(p => p.Party == party) });

            return service.Object;
        }
    }
}
