using System.Collections.Generic;
using System.Linq;
using Moq;
using Shared.Managers;
using Shared.Services;
using TradeCube_ServicesTests.Helpers;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonImbalancePriceFixture
{
    public readonly IElexonImbalancePriceManager ElexonImbalancePriceManager;
    
    private readonly IList<ElexonImbalancePriceTestType> expectedResults;

    public ElexonImbalancePriceFixture()
    {
        expectedResults = FileHelper.ReadJsonFile<IList<ElexonImbalancePriceTestType>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonElexonImbalancePrice/expected_results_elexon_settlement_period.json"));

        ElexonImbalancePriceManager = new ElexonImbalancePriceManager(
            Mock.Of<IVaultService>(),
            Mock.Of<ISettingService>(),
            Mock.Of<IElexonService>(),
            Mock.Of<ICubeService>(),
            
            TestHelper.CreateNullLogger<ElexonImbalancePriceManager>());
    }
    
    public ElexonImbalancePriceTestType GetExpectedResult(string testDescription) => 
        expectedResults.SingleOrDefault(t => t.Description == testDescription);
}
