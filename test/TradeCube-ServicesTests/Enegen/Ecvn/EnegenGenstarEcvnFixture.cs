using System.Collections.Generic;
using Enegen.Managers;
using Shared.Constants;
using Shared.DataObjects;
using TradeCube_ServicesTests.Helpers;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.Enegen.Ecvn;

public class EnegenGenstarEcvnFixture 
{
    public readonly EcvnManager EcvnManager;
    public IList<EnegenGenstarEcvnTestType> ExpectedResults { get; init; }

    public EnegenGenstarEcvnFixture()
    {
        var tradeDataObjects = FileHelper.ReadJsonFile<IList<TradeDataObject>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/mock_trade.json"));
        var tradeDetailTestTypes = FileHelper.ReadJsonFile<IList<TradeDetailTestType>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/mock_api_TradeDetail.json"));
        var elexonSettlementPeriodResponses = FileHelper.ReadJsonFile<IList<ElexonSettlementPeriodTestType>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/mock_api_ElexonSettlementPeriod.json"));
        
        ExpectedResults = FileHelper.ReadJsonFile<IList<EnegenGenstarEcvnTestType>>(TestHelper.GetTestDataFolder("TestData/Enegen/Ecvn/expected_results_enegen_ecvn.json"));

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
        
        EcvnManager = new EcvnManager(
            MockService.CreateModuleService(moduleDataObjects),
            MockService.CreateSettingService(settingDataObjects),
            MockService.CreateTradeService(tradeDataObjects),
            MockService.CreateTradeDetailService(tradeDetailTestTypes),
            MockService.CreateElexonSettlementPeriodService(elexonSettlementPeriodResponses));
    }
}