using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Extensions;
using Shared.Messages;
using Xunit;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonImbalancePriceUnitTests : IClassFixture<ElexonImbalancePriceFixture>
{
    private readonly ElexonImbalancePriceFixture elexonImbalancePriceFixture;

    public ElexonImbalancePriceUnitTests(ElexonImbalancePriceFixture elexonImbalancePriceFixture)
    {
        this.elexonImbalancePriceFixture = elexonImbalancePriceFixture;
    }
     
    [Fact]
    public async Task TEST_0000_GMT_Day()
    {
        await RunTest("TEST 0000 GMT Day");
    }

    [Fact]
    public async Task TEST_0001_Short_Day()
    {
        await RunTest("TEST 0001 Short Day");
    }

    [Fact]
    public async Task TEST_0002_BST_Day()
    {
        await RunTest("TEST 0002 BST Day");
    }

    [Fact]
    public async Task TEST_0003_Long_Day()
    {
        await RunTest("TEST 0003 Long Day");
    }

    [Fact]
    public async Task TEST_0004_MISSING_START_DATE()
    {
        await RunTest("TEST 0004 MISSING START DATE");
    }

    [Fact]
    public async Task TEST_0005_END_DATE_BEFORE_START_DATE()
    {
        await RunTest("TEST 0005 END DATE BEFORE START DATE");
    }

    [Fact]
    public async Task TEST_0006_MORE_THAN_40_DAYS()
    {
        await RunTest("TEST 0006 MORE THAN 40 DAYS");
    } 

    private async Task RunTest(string testDescription)
    {
        var expectedResults = elexonImbalancePriceFixture.GetExpectedResult(testDescription);
        var elexonImbalancePriceManager = elexonImbalancePriceFixture.ElexonImbalancePriceManager;
        
        var elexonImbalancePriceContext = elexonImbalancePriceManager.CreateContext(expectedResults.Inputs); 

        var derivedSystemWideDataRequest = elexonImbalancePriceManager.CreateElexonImbalancePriceRequest(elexonImbalancePriceContext);
        var elexonDerivedSystemWideData = await elexonImbalancePriceManager.GetElexonDerivedSystemWideData(derivedSystemWideDataRequest);
        
        Assert.NotNull(elexonDerivedSystemWideData);

        var elexonSettlementPeriodRequest = elexonImbalancePriceManager.CreateElexonSettlementPeriodRequest(elexonImbalancePriceContext);
        
        var elexonSettlementPeriodResponseItems = elexonImbalancePriceManager
            .GetElexonSettlementPeriods(elexonSettlementPeriodRequest)?.Data
            ?.EmptyIfNull().ToList() ?? new List<ElexonSettlementPeriodResponseItem>();
        
        var elexonSettlementPeriods = elexonSettlementPeriodResponseItems.Any()
            ? elexonSettlementPeriodResponseItems 
            : null;
            
        var imbalancePriceResponse = elexonImbalancePriceManager.ElexonImbalancePrice(elexonImbalancePriceContext,
            elexonDerivedSystemWideData, elexonSettlementPeriods);
        
        Assert.NotNull(expectedResults);
        Assert.NotNull(imbalancePriceResponse);

        if (string.IsNullOrEmpty(expectedResults.ExpectedError))
        {
            Assert.NotNull(elexonSettlementPeriods);
            Assert.NotNull(imbalancePriceResponse.Data);

            if (string.IsNullOrEmpty(expectedResults.ExpectedError))
            {
                CheckData(expectedResults, imbalancePriceResponse);
            }
            else
            {
                Assert.Null(imbalancePriceResponse.Data);
                Assert.Equal(expectedResults.ExpectedError, elexonImbalancePriceContext.MessageResponseBag.ErrorsAsString());
            }
        }
        else
        {
            Assert.Null(elexonSettlementPeriods);
            Assert.Null(imbalancePriceResponse.Data);
            Assert.Equal(expectedResults.ExpectedError, elexonImbalancePriceContext.MessageResponseBag.ErrorsAsString(true));
        }
    }

    private static void CheckData(ElexonImbalancePriceTestType expectedResults, ElexonImbalancePriceResponse imbalancePriceResponse)
    {
        Assert.Equal(expectedResults.ExpectedResults.Data.Count(), imbalancePriceResponse.Data.Count());

        var zipped = expectedResults.ExpectedResults.Data.Zip(imbalancePriceResponse.Data, (e, a) => new
        {
            Expected = e,
            Actual = a
        }).ToList();

        foreach (var result in zipped)
        {
            Assert.Equal(result.Expected.SettlementDate, result.Actual.SettlementDate);
            Assert.Equal(result.Expected.SettlementPeriod, result.Actual.SettlementPeriod);
            Assert.Equal(result.Expected.StartDateTimeUtc, result.Actual.StartDateTimeUtc);
            Assert.Equal(result.Expected.ImbalancePrice, result.Actual.ImbalancePrice);
        }
    }
}