using System.Linq;
using Xunit;

namespace TradeCube_ServicesTests.UkPower.ElexonSettlementPeriod;

public class ElexonSettlementPeriodUnitTests : IClassFixture<ElexonSettlementPeriodFixture>
{
    private readonly ElexonSettlementPeriodFixture settlementPeriodFixture;

    public ElexonSettlementPeriodUnitTests(ElexonSettlementPeriodFixture settlementPeriodFixture)
    {
        this.settlementPeriodFixture = settlementPeriodFixture;
    }

    [Fact]
    public void TEST_0000_GMT_Day()
    {
        RunTest("TEST 0000 GMT Day");
    }

    [Fact]
    public void TEST_0001_Short_Day()
    {
        RunTest("TEST 0001 Short Day");
    }

    [Fact]
    public void TEST_0002_BST_Day()
    {
        RunTest("TEST 0002 BST Day");
    }

    [Fact]
    public void TEST_0003_Long_Day()
    {
        RunTest("TEST 0003 Long Day");
    }

    [Fact]
    public void TEST_0004_NO_END_DATE()
    {
        RunTest("TEST 0004 NO END DATE");
    }

    [Fact]
    public void TEST_0005_MISSING_START_DATE()
    {
        RunTest("TEST 0005 MISSING START DATE");
    }

    [Fact]
    public void TEST_0006_END_DATE_BEFORE_START_DATE()
    {
        RunTest("TEST 0006 END DATE BEFORE START DATE");
    }

    private void RunTest(string testDescription)
    {
        var elexonSettlementPeriodType = settlementPeriodFixture.GetExpectedResult(testDescription);
        var expectedResults = elexonSettlementPeriodType.ExpectedResults?.Data?.ToList();
        var settlementPeriodResponse = settlementPeriodFixture.ElexonSettlementPeriodManager.ElexonSettlementPeriods(elexonSettlementPeriodType.Inputs);

        if (!string.IsNullOrWhiteSpace(elexonSettlementPeriodType.ExpectedError))
        {
            Assert.Equal(elexonSettlementPeriodType.ExpectedError, settlementPeriodResponse.Message);
            Assert.Null(expectedResults);

            return;
        }

        Assert.NotNull(expectedResults);
        Assert.Equal(expectedResults.Count, settlementPeriodResponse.Data.Count());

        var zipped = expectedResults.Zip(settlementPeriodResponse.Data, (e, a) => new
        {
            Expected = e,
            Actual = a
        }).ToList();

        foreach (var result in zipped)
        {
            Assert.Equal(result.Expected.UtcStartDateTime, result.Actual.UtcStartDateTime);
            Assert.Equal(result.Expected.SettlementDate, result.Actual.SettlementDate);
            Assert.Equal(result.Expected.SettlementPeriod, result.Actual.SettlementPeriod);
        }
    }
}