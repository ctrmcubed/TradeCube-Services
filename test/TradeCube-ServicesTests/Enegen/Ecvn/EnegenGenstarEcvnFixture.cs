using System.Collections.Generic;
using System.Linq;
using Enegen.Managers;
using Enegen.Services;
using Moq;
using Shared.Constants;
using Shared.DataObjects;
using TradeCube_ServicesTests.Helpers;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.Enegen.Ecvn;

public class EnegenGenstarEcvnFixture 
{
    public readonly EcvnManager EcvnManager;
    private readonly IList<EnegenGenstarEcvnTestType> expectedResults;

    public EnegenGenstarEcvnFixture()
    {
        var tradeDetailTestTypes = FileHelper.ReadJsonFile<IList<TradeDetailTestType>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/mock_api_TradeDetail.json"));
        var elexonSettlementPeriodTestTypes = FileHelper.ReadJsonFile<IList<ElexonSettlementPeriodTestType>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/mock_api_ElexonSettlementPeriod.json"));
        var tradeDataObjects = FileHelper.ReadBsonFile<IList<TradeDataObject>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/mock_trade.ejson"));
        
        expectedResults = FileHelper.ReadBsonFile<IList<EnegenGenstarEcvnTestType>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/expected_results_enegen_ecvn.json"));

        var moduleDataObjects = new List<ModuleDataObject>
        {
            new()
            {
                Module = ModuleConstants.UkPowerModule,
                Enabled = true
            }
        };

        var settingDataObjects = new List<SettingDataObject>
        {
            new()
            {
                SettingName = SettingConstants.EnegenEcvnUrlSetting,
                SettingValue = "blah"
            }
        };
        
        var vaultDataObjects = new List<VaultDataObject>
        {
            new()
            {
                VaultKey = VaultConstants.EnegenPsk,
                VaultValue = "blah"
            }
        };
        
        EcvnManager = new EcvnManager(
            MockService.CreateModuleService(moduleDataObjects),
            MockService.CreateSettingService(settingDataObjects),
            MockService.CreateTradeService(tradeDataObjects),
            MockService.CreateTradeDetailService(tradeDetailTestTypes),
            MockService.CreateElexonSettlementPeriodService(elexonSettlementPeriodTestTypes),
            MockService.CreateVaultService(vaultDataObjects),
            new Mock<IHmacService>().Object,
            new Mock<IEcvnService>().Object,
            TestHelper.CreateNullLogger<EcvnManager>());
    }
    
    public EnegenGenstarEcvnTestType GetExpectedResult(string testDescription) => 
        expectedResults.SingleOrDefault(t => t.Description == testDescription);
}