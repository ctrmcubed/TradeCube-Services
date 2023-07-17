using System.Collections.Generic;
using System.Linq;
using Shared.Managers;
using TradeCube_ServicesTests.Helpers;

namespace TradeCube_ServicesTests.UkPower;

public class ElexonSettlementPeriodFixture
{
    public readonly UkPowerManager UkPowerManager;
    
    private readonly IList<ElexonSettlementPeriodTestType> expectedResults;

    public ElexonSettlementPeriodFixture()
    {
        expectedResults = FileHelper.ReadJsonFile<IList<ElexonSettlementPeriodTestType>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonSettlementPeriod/expected_results_elexon_settlement_period.json"));

        UkPowerManager = new UkPowerManager(TestHelper.CreateNullLogger<UkPowerManager>());
    }

    public ElexonSettlementPeriodTestType GetExpectedResult(string testDescription) => 
        expectedResults.SingleOrDefault(t => t.Description == testDescription);
}