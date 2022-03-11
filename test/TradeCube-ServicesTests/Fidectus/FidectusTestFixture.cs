using Fidectus.Managers;
using Fidectus.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using Shared.Services.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using TradeCube_ServicesTests.Helpers;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.Fidectus
{
    public class FidectusTestFixture
    {
        public FidectusAuthenticationService FidectusAuthenticationService { get; }
        public IFidectusService FidectusService { get; }
        public IFidectusManager FidectusManager { get; }

        public IEnumerable<FidectusTestType> ExpectedResults { get; set; }

        private IEnumerable<TradeDataObject> FidectusTrades { get; }
        private IEnumerable<MappingDataObject> FidectusMappings { get; }
        private IEnumerable<TradeSummaryResponse> FidectusTradeSummaries { get; }
        private IEnumerable<ProfileResponse> FidectusProfiles { get; }
        private IEnumerable<PartyDataObject> FidectusParties { get; }

        public FidectusTestFixture()
        {
            Configuration.SetEnvironmentVariables();

            var defaultHttpClientFactory = new DefaultHttpClientFactory();

            FidectusTrades = FileHelper.ReadJsonFile<IList<TradeDataObject>>(TestHelper.GetTestDataFolder("TestData/Fidectus/mock_trade.json"));
            FidectusMappings = FileHelper.ReadJsonFile<IList<MappingDataObject>>(TestHelper.GetTestDataFolder("TestData/Fidectus/mock_mapping.json"));
            FidectusTradeSummaries = FileHelper.ReadJsonFile<IList<TradeSummaryResponse>>(TestHelper.GetTestDataFolder("TestData/Fidectus/mock_tradesummary.json"));
            FidectusProfiles = FileHelper.ReadJsonFile<IList<ProfileResponse>>(TestHelper.GetTestDataFolder("TestData/Fidectus/mock_tradeprofile.json"));
            FidectusParties = FileHelper.ReadJsonFile<IList<PartyDataObject>>(TestHelper.GetTestDataFolder("TestData/Fidectus/mock_party.json"));
            ExpectedResults = FileHelper.ReadJsonFile<IList<FidectusTestType>>(TestHelper.GetTestDataFolder("TestData/Fidectus/expected_results_fidectus_confirms.json"));

            FidectusAuthenticationService = new FidectusAuthenticationService(defaultHttpClientFactory, new Mock<IRedisService>().Object, new Logger<FidectusAuthenticationService>(LoggerFactory.Create(l => l.AddConsole())));
            FidectusService = new FidectusService(defaultHttpClientFactory, new Logger<FidectusService>(LoggerFactory.Create(l => l.AddConsole())));

            var vaultDataObjects = new List<VaultDataObject>
            {
                new()
                {
                    VaultKey = VaultConstants.FidectusClientId,
                    VaultValue = Environment.GetEnvironmentVariable("FIDECTUS_CLIENT_ID")
                },
                new()
                {
                    VaultKey = VaultConstants.FidectusClientSecret,
                    VaultValue = Environment.GetEnvironmentVariable("FIDECTUS_SECRET")
                }
            };

            var settingDataObjects = new List<SettingDataObject>
            {
                new()
                {
                    SettingName = "FIDECTUS_URL",
                    SettingValue = "https://staging.gen.fidectus.com/api/v1"
                },
                new()
                {
                    SettingName = "FIDECTUS_AUTH_URL",
                    SettingValue = "https://staging--fidectus.eu.auth0.com/oauth/token"
                },
                new()
                {
                    SettingName = "FIDECTUS_AUDIENCE",
                    SettingValue = "fidectus_open_api_staging"
                },
                new()
                {
                    SettingName = "FIDECTUS_TENANT",
                    SettingValue = "staging--fidectus"
                },
                new ()
                {
                    SettingName = "FIDECTUS_COMPANYID",
                    SettingValue =  Environment.GetEnvironmentVariable("FIDECTUS_COMPANYID")
                }
            };

            FidectusManager = new FidectusManager(
                FidectusAuthenticationService,
                FidectusService,
                CreateTradeService(FidectusTrades),
                CreateTradeSummaryService(FidectusTradeSummaries),
                CreateProfileService(FidectusProfiles),
                CreateSettingService(settingDataObjects),
                CreateVaultService(vaultDataObjects),
                new FidectusMappingService(CreateMappingService(FidectusMappings), CreatePartyService(FidectusParties)),
                new Logger<FidectusManager>(LoggerFactory.Create(l => l.AddConsole())));
        }

        private static ITradeService CreateTradeService(IEnumerable<TradeDataObject> trades)
        {
            var service = new Mock<ITradeService>();

            service
                .Setup(c => c.GetTradeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string _, string tradeReference, int tradeLeg) =>
                    new ApiResponseWrapper<IEnumerable<TradeDataObject>> { Data = trades.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg) });

            service.Setup(c => c.PutTradesViaJwtAsync(It.IsAny<string>(), It.IsAny<IEnumerable<TradeDataObject>>()))
                .ReturnsAsync((string _, IEnumerable<TradeDataObject> _) =>
                    new ApiResponseWrapper<IEnumerable<TradeDataObject>> { IsSuccessStatusCode = true });

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

        private static ISettingService CreateSettingService(IEnumerable<SettingDataObject> settings)
        {
            var service = new Mock<ISettingService>();

            service
                .Setup(c => c.GetSettingViaJwtAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string settingName, string _) =>
                    new ApiResponseWrapper<IEnumerable<SettingDataObject>>
                    {
                        Data = settings.Where(s => s.SettingName == settingName)
                    });

            service
                .Setup(c => c.GetSettingsViaJwtAsync(It.IsAny<string>()))
                .ReturnsAsync((string _) =>
                    new ApiResponseWrapper<IEnumerable<SettingDataObject>>
                    {
                        Data = settings
                    });

            return service.Object;
        }

        private static IVaultService CreateVaultService(IEnumerable<VaultDataObject> data)
        {
            var service = new Mock<IVaultService>();

            service
                .Setup(c => c.GetVaultValueAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string vaultKey, string _) =>
                    new ApiResponseWrapper<IEnumerable<VaultDataObject>> { Data = data.Where(v => v.VaultKey == vaultKey) });

            return service.Object;
        }
    }
}
