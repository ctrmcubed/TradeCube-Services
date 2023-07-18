using System;
using System.Collections.Generic;
using Equias.Managers;
using Equias.Services;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Messages;
using TradeCube_ServicesTests.Helpers;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.Equias
{
    public class EquiasTestFixture
    {
        public EquiasAuthenticationService EquiasAuthenticationService { get; }
        public IEquiasService EquiasService { get; }
        public IEquiasManager EquiasManager { get; }

        private IEnumerable<TradeDataObject> EquiasTrades { get; }
        private IEnumerable<MappingDataObject> EquiasMappings { get; }
        private IEnumerable<TradeSummaryResponse> EquiasTradeSummaries { get; }
        private IEnumerable<CashflowResponse> EquiasCashflows { get; }
        private IEnumerable<ProfileResponse> EquiasProfiles { get; }
        private IEnumerable<PartyDataObject> EquiasParties { get; }

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

            EquiasAuthenticationService = new EquiasAuthenticationService(defaultHttpClientFactory, new Logger<EquiasAuthenticationService>(LoggerFactory.Create(l => l.AddConsole())));
            EquiasService = new EquiasService(defaultHttpClientFactory, TestHelper.CreateNullLogger<EquiasService>());

            var vaultDataObjects = new List<VaultDataObject>
            {
                new()
                {
                    VaultKey = VaultConstants.EquiasEboUsernameKey,
                    VaultValue = Environment.GetEnvironmentVariable("EQUIAS_USERNAME")
                },
                new()
                {
                    VaultKey = VaultConstants.EquiasEboPasswordKey,
                    VaultValue = Environment.GetEnvironmentVariable("EQUIAS_PASSWORD")
                }
            };

            var settingDataObjects = new List<SettingDataObject>
            {
                new()
                {
                    SettingName = SettingConstants.EboUrlSetting,
                    SettingValue = "https://ebo-test.api.equias.org"
                }
            };
            
            EquiasManager = new EquiasManager(
                EquiasAuthenticationService,
                EquiasService,
                MockService.CreateTradeService(EquiasTrades),
                MockService.CreateTradeSummaryService(EquiasTradeSummaries),
                MockService.CreateCashflowService(EquiasCashflows),
                MockService.CreateProfileService(EquiasProfiles),
                MockService.CreateSettingService(settingDataObjects),
                MockService.CreateVaultService(vaultDataObjects),
                new EquiasMappingService(MockService.CreateMappingService(EquiasMappings), MockService.CreatePartyService(EquiasParties)), 
                TestHelper.CreateNullLogger<EquiasManager>());
        }

    
    }
}
