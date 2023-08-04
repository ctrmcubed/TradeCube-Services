using System.Linq;
using System.Threading.Tasks;
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
        var elexonDerivedSystemWideData = await elexonImbalancePriceManager.GetElexonDerivedSystemWideData(elexonImbalancePriceContext);
        var elexonSettlementPeriodResponseItems = elexonImbalancePriceManager.GetElexonSettlementPeriods(elexonImbalancePriceContext)?.Data;
                 
        var imbalancePriceResponse = elexonImbalancePriceManager.ElexonImbalancePrice(elexonImbalancePriceContext,
            elexonDerivedSystemWideData, elexonSettlementPeriodResponseItems);
        
        Assert.NotNull(expectedResults);
        Assert.NotNull(elexonDerivedSystemWideData);
        Assert.NotNull(imbalancePriceResponse);

        if (string.IsNullOrEmpty(expectedResults.ExpectedError))
        {
            Assert.NotNull(elexonSettlementPeriodResponseItems);
            Assert.NotNull(imbalancePriceResponse.Data);

            CheckData(expectedResults, imbalancePriceResponse);
        }
        else
        {
            Assert.Null(elexonSettlementPeriodResponseItems);
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