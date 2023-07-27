using System.Threading.Tasks;
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
    public async Task TEST_0001_GMT_Day()
    {
        await RunTest("TEST 0001 GMT Day");
    }

    [Fact]
    public async Task TEST_0002_Short_Day()
    {
        await RunTest("TEST 0002 Short Day");
    }

    [Fact]
    public async Task TEST_0003_Short_Day()
    {
        await RunTest("TEST 0003 Short Day");
    }

    [Fact]
    public async Task TEST_0004_BST_Day()
    {
        await RunTest("TEST 0004 BST Day");
    }

    [Fact]
    public async Task TEST_0005_BST_Day()
    {
        await RunTest("TEST 0005 BST Day");
    }

    [Fact]
    public async Task TEST_0006_Long_Day()
    {
        await RunTest("TEST 0006 Long Day");
    }

    [Fact]
    public async Task TEST_0007_Long_Day()
    {
        await RunTest("TEST 0007 Long Day");
    }

    [Fact]
    public async Task TEST_0008_MISSING_START_DATE()
    {
        await RunTest("TEST 0008 MISSING START DATE");
    }

    [Fact]
    public async Task TEST_0009_MISSING_CUBE()
    {
        await RunTest("TEST 0009 MISSING CUBE");
    }

    [Fact]
    public async Task TEST_0010_MISSING_DATA_ITEM()
    {
        await RunTest("TEST 0010 MISSING DATA ITEM");
    }

    [Fact]
    public async Task TEST_0011_END_DATE_BEFORE_START_DATE()
    {
        await RunTest("TEST 0011 END DATE BEFORE START DATE");
    }

    private async Task RunTest(string testDescription)
    {
        var elexonElexonImbalancePriceTestType = elexonImbalancePriceFixture.GetExpectedResult(testDescription);
        // var expectedResults = elexonElexonImbalancePriceTestType

        var imbalancePriceResponse = elexonImbalancePriceFixture.ElexonImbalancePriceManager.ElexonImbalancePrice(elexonElexonImbalancePriceTestType.Inputs);
        
        Assert.NotNull(imbalancePriceResponse);
    }
}