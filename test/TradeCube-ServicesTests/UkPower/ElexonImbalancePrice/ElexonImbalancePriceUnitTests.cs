using System;
using System.Linq;
using System.Threading.Tasks;
using Shared.Messages;
using Shared.Types.CubeDataBulk;
using Xunit;
using Xunit.Abstractions;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonImbalancePriceUnitTests : IClassFixture<ElexonImbalancePriceFixture>
{
    private readonly ElexonImbalancePriceFixture elexonImbalancePriceFixture;
    private readonly ITestOutputHelper testOutputHelper;

    public ElexonImbalancePriceUnitTests(ElexonImbalancePriceFixture elexonImbalancePriceFixture, ITestOutputHelper testOutputHelper)
    {
        this.elexonImbalancePriceFixture = elexonImbalancePriceFixture;
        this.testOutputHelper = testOutputHelper;
    }
     
    [Fact]
    public async Task TEST_0000_GMT_Day_STANDALONE()
    {
        await RunTest("TEST 0000 GMT Day STANDALONE");
    }

    [Fact]
    public async Task TEST_0001_GMT_Day_CUBE()
    {
        await RunTest("TEST 0001 GMT Day CUBE");
    }

    [Fact]
    public async Task TEST_0002_Short_Day_STANDALONE()
    {
        await RunTest("TEST 0002 Short Day STANDALONE");
    }

    [Fact]
    public async Task TEST_0003_Short_Day_CUBE()
    {
        await RunTest("TEST 0003 Short Day CUBE");
    }

    [Fact]
    public async Task TEST_0004_BST_Day_STANDALONE()
    {
        await RunTest("TEST 0004 BST Day STANDALONE");
    }

    [Fact]
    public async Task TEST_0005_BST_Day_CUBE()
    {
        await RunTest("TEST 0005 BST Day CUBE");
    }

    [Fact]
    public async Task TEST_0006_Long_Day_STANDALONE()
    {
        await RunTest("TEST 0006 Long Day STANDALONE");
    }

    [Fact]
    public async Task TEST_0007_Long_Day_CUBE()
    {
        await RunTest("TEST 0007 Long Day CUBE");
    }

    [Fact]
    public async Task TEST_0008_MISSING_START_DATE_STANDALONE()
    {
        await RunTest("TEST 0008 MISSING START DATE STANDALONE");
    }

    [Fact]
    public async Task TEST_0009_MISSING_START_DATE_CUBE()
    {
        await RunTest("TEST 0009 MISSING START DATE CUBE");
    }

    [Fact]
    public async Task TEST_0010_MISSING_CUBE_STANDALONE()
    {
        await RunTest("TEST 0010 MISSING CUBE STANDALONE");
    }

    [Fact]
    public async Task TEST_0011_MISSING_CUBE_CUBE()
    {
        await RunTest("TEST 0011 MISSING CUBE CUBE");
    }

    [Fact]
    public async Task TEST_0012_MISSING_DATA_ITEM_STANDALONE()
    {
        await RunTest("TEST 0012 MISSING DATA ITEM STANDALONE");
    }

    [Fact]
    public async Task TEST_0013_MISSING_DATA_ITEM_CUBE()
    {
        await RunTest("TEST 0013 MISSING DATA ITEM CUBE");
    }

    [Fact]
    public async Task TEST_0014_WRONG_CUBE_STANDALONE()
    {
        await RunTest("TEST 0014 WRONG CUBE STANDALONE");
    }

    [Fact]
    public async Task TEST_0015_WRONG_CUBE_CUBE()
    {
        await RunTest("TEST 0015 WRONG CUBE CUBE");
    }

    [Fact]
    public async Task TEST_0016_WRONG_DATA_ITEM_STANDALONE()
    {
        await RunTest("TEST 0016 WRONG DATA ITEM STANDALONE");
    }

    [Fact]
    public async Task TEST_0017_WRONG_DATA_ITEM_CUBE()
    {
        await RunTest("TEST 0017 WRONG DATA ITEM CUBE");
    }

    [Fact]
    public async Task TEST_0018_WRONG_CUBE_TYPE_STANDALONE()
    {
        await RunTest("TEST 0018 WRONG CUBE TYPE STANDALONE");
    }

    [Fact]
    public async Task TEST_0019_WRONG_CUBE_TYPE_CUBE()
    {
        await RunTest("TEST 0019 WRONG CUBE TYPE CUBE");
    }

    [Fact]
    public async Task TEST_0020_END_DATE_BEFORE_START_DATE_STANDALONE()
    {
        await RunTest("TEST 0020 END DATE BEFORE START DATE STANDALONE");
    }

    [Fact]
    public async Task TEST_0021_END_DATE_BEFORE_START_DATE_CUBE()
    {
        await RunTest("TEST 0021 END DATE BEFORE START DATE CUBE");
    }

    [Fact]
    public async Task TEST_0022_MORE_THAN_40_DAYS_STANDALONE()
    {
        await RunTest("TEST 0022 MORE THAN 40 DAYS STANDALONE");
    }

    [Fact]
    public async Task TEST_0023_MORE_THAN_40_DAYS_CUBE()
    {
        await RunTest("TEST 0023 MORE THAN 40 DAYS CUBE");
    }

    private async Task RunTest(string testDescription)
    {
        var expectedResults = elexonImbalancePriceFixture.GetExpectedResult(testDescription);
        var elexonImbalancePriceContext = await elexonImbalancePriceFixture.ElexonImbalancePriceManager.CreateContext(expectedResults.Inputs); 

        Assert.Equal(expectedResults.Inputs.Mode, elexonImbalancePriceContext.Mode);
        
        var derivedSystemWideDataRequest = elexonImbalancePriceFixture.ElexonImbalancePriceManager.CreateElexonImbalancePriceRequest(elexonImbalancePriceContext);
        var elexonDerivedSystemWideData = await elexonImbalancePriceFixture.ElexonImbalancePriceManager.GetElexonDerivedSystemWideData(derivedSystemWideDataRequest);
        
        Assert.NotNull(elexonDerivedSystemWideData);
        
        var elexonSettlementPeriodRequest = elexonImbalancePriceFixture.ElexonImbalancePriceManager.CreateElexonSettlementPeriodRequest(elexonImbalancePriceContext);
        var elexonSettlementPeriods = elexonImbalancePriceFixture.GetElexonSettlementPeriods(elexonSettlementPeriodRequest);
        
        var imbalancePriceResponse = elexonImbalancePriceFixture.ElexonImbalancePriceManager.ElexonImbalancePrice(
            elexonImbalancePriceContext, elexonDerivedSystemWideData, elexonSettlementPeriods?.Response?.Data);
        
        Assert.NotNull(expectedResults);
        Assert.NotNull(imbalancePriceResponse);

        if (string.IsNullOrEmpty(expectedResults.ExpectedError))
        {
            Assert.NotNull(elexonSettlementPeriods);
            
            if (elexonImbalancePriceContext.IsModeCube())
            {
                Assert.Null(imbalancePriceResponse.Data);
                Assert.NotNull(imbalancePriceResponse.CubeDataBulk);

                CheckCubeDataBulk(imbalancePriceResponse, expectedResults, imbalancePriceResponse.CubeDataBulk);
            }
            else if (elexonImbalancePriceContext.IsModeStandalone())
            {
                Assert.Null(imbalancePriceResponse.CubeDataBulk);
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
                Assert.False(true);
            }
        }
        else
        {
            Assert.Null(elexonSettlementPeriods);
            Assert.Null(imbalancePriceResponse.Data);
            Assert.Null(imbalancePriceResponse.CubeDataBulk);
            Assert.Equal(expectedResults.ExpectedError, elexonImbalancePriceContext.MessageResponseBag.ErrorsAsString(true));
        }
    }

    private void CheckData(ElexonImbalancePriceTestType expectedResults, ElexonImbalancePriceResponse imbalancePriceResponse)
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

            testOutputHelper.WriteLine($"E: {result.Expected.StartDateTimeUTC} => A: {result.Actual.StartDateTimeUTC}");

            Assert.Equal(result.Expected.StartDateTimeUTC, result.Actual.StartDateTimeUTC);
            Assert.Equal(result.Expected.ImbalancePrice, result.Actual.ImbalancePrice);
        }
    }
    
   private static void CheckCubeDataBulk(ElexonImbalancePriceResponse imbalancePriceResponse, ElexonImbalancePriceTestType expectedResults,
       CubeDataBulkRequest cubeDataBulkRequest)
    {
        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.Name, cubeDataBulkRequest?.Name);
        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.Description, cubeDataBulkRequest?.Description);
        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.Reason, cubeDataBulkRequest?.Reason);

        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.Cube, cubeDataBulkRequest?.Cube);
        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.DataItem, cubeDataBulkRequest?.DataItem);
        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.Layer, cubeDataBulkRequest?.Layer);

        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.CreateNodes, cubeDataBulkRequest?.CreateNodes);
        Assert.Equal(expectedResults.ExpectedResults.CubeDataBulk?.RegularDayPeriods,
            cubeDataBulkRequest?.RegularDayPeriods);

        Assert.Equal(expectedResults.ExpectedResults?.CubeDataBulk?.Data.Count(),
            imbalancePriceResponse.CubeDataBulk?.Data.Count());

        var zippedCubeDataBulk = expectedResults.ExpectedResults?.CubeDataBulk?.Data.Zip(
            imbalancePriceResponse.CubeDataBulk?.Data ?? Array.Empty<CubeDataBulkData>(), (e, a) => new
            {
                Expected = e,
                Actual = a
            }).ToList();

        Assert.NotNull(zippedCubeDataBulk);

        foreach (var cubeDataBulk in zippedCubeDataBulk)
        {
            Assert.Equal(cubeDataBulk.Expected.StartDateTimeUTC, cubeDataBulk.Actual.StartDateTimeUTC);

            var zippedCubeDataBulkValues = cubeDataBulk.Expected.Values.Zip(cubeDataBulk.Actual.Values, (e, a) => new
            {
                Expected = e,
                Actual = a
            }).ToList();

            foreach (var zippedCubeDataBulkValue in zippedCubeDataBulkValues)
            {
                Assert.Equal(zippedCubeDataBulkValue.Expected, zippedCubeDataBulkValue.Actual);
            }
        }
    }    
}