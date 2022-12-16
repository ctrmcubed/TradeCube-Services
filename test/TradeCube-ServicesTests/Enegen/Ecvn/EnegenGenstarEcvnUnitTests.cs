using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TradeCube_ServicesTests.Enegen.Ecvn;

public class EnegenGenstarEcvnUnitTests : IClassFixture<EnegenGenstarEcvnFixture>
{
    private readonly EnegenGenstarEcvnFixture enegenGenstarEcvnFixture;

    public EnegenGenstarEcvnUnitTests(EnegenGenstarEcvnFixture enegenGenstarEcvnFixture)
    {
        this.enegenGenstarEcvnFixture = enegenGenstarEcvnFixture;
    }

    [Fact]
    public async Task TEST_0000_FIRST_DAY_HalfHour_Spot()
    {
        await RunTest("TEST 0000 FIRST_DAY HalfHour Spot");
    }

    [Fact]
    public async Task TEST_0001_FIRST_DAY_Hour_Spot()
    {
        await RunTest("TEST 0001 FIRST_DAY Hour Spot");
    }

    [Fact]
    public async Task TEST_0002_FIRST_DAY_TwoHour_Spot()
    {
        await RunTest("TEST 0002 FIRST_DAY TwoHour Spot");
    }

    [Fact]
    public async Task TEST_0003_FIRST_DAY_Day_EFA1()
    {
        await RunTest("TEST 0003 FIRST_DAY Day EFA1");
    }

    [Fact]
    public async Task TEST_0004_FIRST_DAY_Day_EFA4()
    {
        await RunTest("TEST 0004 FIRST_DAY Day EFA4");
    }

    [Fact]
    public async Task TEST_0005_FIRST_DAY_Day_5_6A()
    {
        await RunTest("TEST 0005 FIRST_DAY Day 5+6A");
    }

    [Fact]
    public async Task TEST_0006_FIRST_DAY_Day_5B_6A()
    {
        await RunTest("TEST 0006 FIRST_DAY Day 5B+6A");
    }

    [Fact]
    public async Task TEST_0007_FIRST_DAY_Day_Peak()
    {
        await RunTest("TEST 0007 FIRST_DAY Day Peak");
    }

    [Fact]
    public async Task TEST_0008_FIRST_DAY_Day_Extended_Peak()
    {
        await RunTest("TEST 0008 FIRST_DAY Day Extended Peak");
    }

    [Fact]
    public async Task TEST_0009_FIRST_DAY_Day_Overnight()
    {
        await RunTest("TEST 0009 FIRST_DAY Day Overnight");
    }

    [Fact]
    public async Task TEST_0010_FIRST_DAY_Day_Baseload()
    {
        await RunTest("TEST 0010 FIRST_DAY Day Baseload");
    }

    [Fact]
    public async Task TEST_0011_FIRST_DAY_Day_7_Day_Peak()
    {
        await RunTest("TEST 0011 FIRST_DAY Day 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0012_FIRST_DAY_Week_EFA1()
    {
        await RunTest("TEST 0012 FIRST_DAY Week EFA1");
    }

    [Fact]
    public async Task TEST_0013_FIRST_DAY_Week_EFA4()
    {
        await RunTest("TEST 0013 FIRST_DAY Week EFA4");
    }

    [Fact]
    public async Task TEST_0014_FIRST_DAY_Week_WD1()
    {
        await RunTest("TEST 0014 FIRST_DAY Week WD1");
    }

    [Fact]
    public async Task TEST_0015_FIRST_DAY_Week_WD4()
    {
        await RunTest("TEST 0015 FIRST_DAY Week WD4");
    }

    [Fact]
    public async Task TEST_0016_FIRST_DAY_Week_WE1()
    {
        await RunTest("TEST 0016 FIRST_DAY Week WE1");
    }

    [Fact]
    public async Task TEST_0017_FIRST_DAY_Week_WE4()
    {
        await RunTest("TEST 0017 FIRST_DAY Week WE4");
    }

    [Fact]
    public async Task TEST_0018_FIRST_DAY_Week_5_6A()
    {
        await RunTest("TEST 0018 FIRST_DAY Week 5+6A");
    }

    [Fact]
    public async Task TEST_0019_FIRST_DAY_Week_5B_6A()
    {
        await RunTest("TEST 0019 FIRST_DAY Week 5B+6A");
    }

    [Fact]
    public async Task TEST_0020_FIRST_DAY_Week_Peak()
    {
        await RunTest("TEST 0020 FIRST_DAY Week Peak");
    }

    [Fact]
    public async Task TEST_0021_FIRST_DAY_Week_Extended_Peak()
    {
        await RunTest("TEST 0021 FIRST_DAY Week Extended Peak");
    }

    [Fact]
    public async Task TEST_0022_FIRST_DAY_Week_Overnight()
    {
        await RunTest("TEST 0022 FIRST_DAY Week Overnight");
    }

    [Fact]
    public async Task TEST_0023_FIRST_DAY_Week_Baseload()
    {
        await RunTest("TEST 0023 FIRST_DAY Week Baseload");
    }

    [Fact]
    public async Task TEST_0024_FIRST_DAY_Week_7_Day_Peak()
    {
        await RunTest("TEST 0024 FIRST_DAY Week 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0025_FIRST_DAY_Month_EFA1()
    {
        await RunTest("TEST 0025 FIRST_DAY Month EFA1");
    }

    [Fact]
    public async Task TEST_0026_FIRST_DAY_Month_EFA4()
    {
        await RunTest("TEST 0026 FIRST_DAY Month EFA4");
    }

    [Fact]
    public async Task TEST_0027_FIRST_DAY_Month_WD1()
    {
        await RunTest("TEST 0027 FIRST_DAY Month WD1");
    }

    [Fact]
    public async Task TEST_0028_FIRST_DAY_Month_WD4()
    {
        await RunTest("TEST 0028 FIRST_DAY Month WD4");
    }

    [Fact]
    public async Task TEST_0029_FIRST_DAY_Month_WE1()
    {
        await RunTest("TEST 0029 FIRST_DAY Month WE1");
    }

    [Fact]
    public async Task TEST_0030_FIRST_DAY_Month_WE4()
    {
        await RunTest("TEST 0030 FIRST_DAY Month WE4");
    }

    [Fact]
    public async Task TEST_0031_FIRST_DAY_Month_5_6A()
    {
        await RunTest("TEST 0031 FIRST_DAY Month 5+6A");
    }

    [Fact]
    public async Task TEST_0032_FIRST_DAY_Month_5B_6A()
    {
        await RunTest("TEST 0032 FIRST_DAY Month 5B+6A");
    }

    [Fact]
    public async Task TEST_0033_FIRST_DAY_Month_Peak()
    {
        await RunTest("TEST 0033 FIRST_DAY Month Peak");
    }

    [Fact]
    public async Task TEST_0034_FIRST_DAY_Month_Extended_Peak()
    {
        await RunTest("TEST 0034 FIRST_DAY Month Extended Peak");
    }

    [Fact]
    public async Task TEST_0035_FIRST_DAY_Month_Overnight()
    {
        await RunTest("TEST 0035 FIRST_DAY Month Overnight");
    }

    [Fact]
    public async Task TEST_0036_FIRST_DAY_Month_Baseload()
    {
        await RunTest("TEST 0036 FIRST_DAY Month Baseload");
    }

    [Fact]
    public async Task TEST_0037_FIRST_DAY_Month_7_Day_Peak()
    {
        await RunTest("TEST 0037 FIRST_DAY Month 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0038_FIRST_DAY_Quarter_Peak()
    {
        await RunTest("TEST 0038 FIRST_DAY Quarter Peak");
    }

    [Fact]
    public async Task TEST_0039_FIRST_DAY_Quarter_Baseload()
    {
        await RunTest("TEST 0039 FIRST_DAY Quarter Baseload");
    }

    [Fact]
    public async Task TEST_0040_FIRST_DAY_HalfYear_Peak()
    {
        await RunTest("TEST 0040 FIRST_DAY HalfYear Peak");
    }

    [Fact]
    public async Task TEST_0041_FIRST_DAY_HalfYear_Baseload()
    {
        await RunTest("TEST 0041 FIRST_DAY HalfYear Baseload");
    }

    [Fact]
    public async Task TEST_0042_FIRST_DAY_Season_Summer_Peak()
    {
        await RunTest("TEST 0042 FIRST_DAY Season Summer Peak");
    }

    [Fact]
    public async Task TEST_0043_FIRST_DAY_Season_Summer_Baseload()
    {
        await RunTest("TEST 0043 FIRST_DAY Season Summer Baseload");
    }

    [Fact]
    public async Task TEST_0044_FIRST_DAY_Season_Winter_Peak()
    {
        await RunTest("TEST 0044 FIRST_DAY Season Winter Peak");
    }

    [Fact]
    public async Task TEST_0045_FIRST_DAY_Season_Winter_Baseload()
    {
        await RunTest("TEST 0045 FIRST_DAY Season Winter Baseload");
    }

    [Fact]
    public async Task TEST_0046_FIRST_DAY_Year_Peak()
    {
        await RunTest("TEST 0046 FIRST_DAY Year Peak");
    }

    [Fact]
    public async Task TEST_0047_FIRST_DAY_Year_Baseload()
    {
        await RunTest("TEST 0047 FIRST_DAY Year Baseload");
    }

    [Fact]
    public async Task TEST_0048_SHORT_DAY_HalfHour_Spot()
    {
        await RunTest("TEST 0048 SHORT_DAY HalfHour Spot");
    }

    [Fact]
    public async Task TEST_0049_SHORT_DAY_Hour_Spot()
    {
        await RunTest("TEST 0049 SHORT_DAY Hour Spot");
    }

    [Fact]
    public async Task TEST_0050_SHORT_DAY_TwoHour_Spot()
    {
        await RunTest("TEST 0050 SHORT_DAY TwoHour Spot");
    }

    [Fact]
    public async Task TEST_0051_SHORT_DAY_Day_EFA1()
    {
        await RunTest("TEST 0051 SHORT_DAY Day EFA1");
    }

    [Fact]
    public async Task TEST_0052_SHORT_DAY_Day_EFA4()
    {
        await RunTest("TEST 0052 SHORT_DAY Day EFA4");
    }

    [Fact]
    public async Task TEST_0053_SHORT_DAY_Day_5_6A()
    {
        await RunTest("TEST 0053 SHORT_DAY Day 5+6A");
    }

    [Fact]
    public async Task TEST_0054_SHORT_DAY_Day_5B_6A()
    {
        await RunTest("TEST 0054 SHORT_DAY Day 5B+6A");
    }

    [Fact]
    public async Task TEST_0055_SHORT_DAY_Day_Peak()
    {
        await RunTest("TEST 0055 SHORT_DAY Day Peak");
    }

    [Fact]
    public async Task TEST_0056_SHORT_DAY_Day_Extended_Peak()
    {
        await RunTest("TEST 0056 SHORT_DAY Day Extended Peak");
    }

    [Fact]
    public async Task TEST_0057_SHORT_DAY_Day_Overnight()
    {
        await RunTest("TEST 0057 SHORT_DAY Day Overnight");
    }

    [Fact]
    public async Task TEST_0058_SHORT_DAY_Day_Baseload()
    {
        await RunTest("TEST 0058 SHORT_DAY Day Baseload");
    }

    [Fact]
    public async Task TEST_0059_SHORT_DAY_Day_7_Day_Peak()
    {
        await RunTest("TEST 0059 SHORT_DAY Day 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0060_SHORT_DAY_Week_EFA1()
    {
        await RunTest("TEST 0060 SHORT_DAY Week EFA1");
    }

    [Fact]
    public async Task TEST_0061_SHORT_DAY_Week_EFA4()
    {
        await RunTest("TEST 0061 SHORT_DAY Week EFA4");
    }

    [Fact]
    public async Task TEST_0062_SHORT_DAY_Week_WD1()
    {
        await RunTest("TEST 0062 SHORT_DAY Week WD1");
    }

    [Fact]
    public async Task TEST_0063_SHORT_DAY_Week_WD4()
    {
        await RunTest("TEST 0063 SHORT_DAY Week WD4");
    }

    [Fact]
    public async Task TEST_0064_SHORT_DAY_Week_WE1()
    {
        await RunTest("TEST 0064 SHORT_DAY Week WE1");
    }

    [Fact]
    public async Task TEST_0065_SHORT_DAY_Week_WE4()
    {
        await RunTest("TEST 0065 SHORT_DAY Week WE4");
    }

    [Fact]
    public async Task TEST_0066_SHORT_DAY_Week_5_6A()
    {
        await RunTest("TEST 0066 SHORT_DAY Week 5+6A");
    }

    [Fact]
    public async Task TEST_0067_SHORT_DAY_Week_5B_6A()
    {
        await RunTest("TEST 0067 SHORT_DAY Week 5B+6A");
    }

    [Fact]
    public async Task TEST_0068_SHORT_DAY_Week_Peak()
    {
        await RunTest("TEST 0068 SHORT_DAY Week Peak");
    }

    [Fact]
    public async Task TEST_0069_SHORT_DAY_Week_Extended_Peak()
    {
        await RunTest("TEST 0069 SHORT_DAY Week Extended Peak");
    }

    [Fact]
    public async Task TEST_0070_SHORT_DAY_Week_Overnight()
    {
        await RunTest("TEST 0070 SHORT_DAY Week Overnight");
    }

    [Fact]
    public async Task TEST_0071_SHORT_DAY_Week_Baseload()
    {
        await RunTest("TEST 0071 SHORT_DAY Week Baseload");
    }

    [Fact]
    public async Task TEST_0072_SHORT_DAY_Week_7_Day_Peak()
    {
        await RunTest("TEST 0072 SHORT_DAY Week 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0073_SHORT_DAY_Month_EFA1()
    {
        await RunTest("TEST 0073 SHORT_DAY Month EFA1");
    }

    [Fact]
    public async Task TEST_0074_SHORT_DAY_Month_EFA4()
    {
        await RunTest("TEST 0074 SHORT_DAY Month EFA4");
    }

    [Fact]
    public async Task TEST_0075_SHORT_DAY_Month_WD1()
    {
        await RunTest("TEST 0075 SHORT_DAY Month WD1");
    }

    [Fact]
    public async Task TEST_0076_SHORT_DAY_Month_WD4()
    {
        await RunTest("TEST 0076 SHORT_DAY Month WD4");
    }

    [Fact]
    public async Task TEST_0077_SHORT_DAY_Month_WE1()
    {
        await RunTest("TEST 0077 SHORT_DAY Month WE1");
    }

    [Fact]
    public async Task TEST_0078_SHORT_DAY_Month_WE4()
    {
        await RunTest("TEST 0078 SHORT_DAY Month WE4");
    }

    [Fact]
    public async Task TEST_0079_SHORT_DAY_Month_5_6A()
    {
        await RunTest("TEST 0079 SHORT_DAY Month 5+6A");
    }

    [Fact]
    public async Task TEST_0080_SHORT_DAY_Month_5B_6A()
    {
        await RunTest("TEST 0080 SHORT_DAY Month 5B+6A");
    }

    [Fact]
    public async Task TEST_0081_SHORT_DAY_Month_Peak()
    {
        await RunTest("TEST 0081 SHORT_DAY Month Peak");
    }

    [Fact]
    public async Task TEST_0082_SHORT_DAY_Month_Extended_Peak()
    {
        await RunTest("TEST 0082 SHORT_DAY Month Extended Peak");
    }

    [Fact]
    public async Task TEST_0083_SHORT_DAY_Month_Overnight()
    {
        await RunTest("TEST 0083 SHORT_DAY Month Overnight");
    }

    [Fact]
    public async Task TEST_0084_SHORT_DAY_Month_Baseload()
    {
        await RunTest("TEST 0084 SHORT_DAY Month Baseload");
    }

    [Fact]
    public async Task TEST_0085_SHORT_DAY_Month_7_Day_Peak()
    {
        await RunTest("TEST 0085 SHORT_DAY Month 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0086_BST_DAY_HalfHour_Spot()
    {
        await RunTest("TEST 0086 BST_DAY HalfHour Spot");
    }

    [Fact]
    public async Task TEST_0087_BST_DAY_Hour_Spot()
    {
        await RunTest("TEST 0087 BST_DAY Hour Spot");
    }

    [Fact]
    public async Task TEST_0088_BST_DAY_TwoHour_Spot()
    {
        await RunTest("TEST 0088 BST_DAY TwoHour Spot");
    }

    [Fact]
    public async Task TEST_0089_BST_DAY_Day_EFA1()
    {
        await RunTest("TEST 0089 BST_DAY Day EFA1");
    }

    [Fact]
    public async Task TEST_0090_BST_DAY_Day_EFA4()
    {
        await RunTest("TEST 0090 BST_DAY Day EFA4");
    }

    [Fact]
    public async Task TEST_0091_BST_DAY_Day_5_6A()
    {
        await RunTest("TEST 0091 BST_DAY Day 5+6A");
    }

    [Fact]
    public async Task TEST_0092_BST_DAY_Day_5B_6A()
    {
        await RunTest("TEST 0092 BST_DAY Day 5B+6A");
    }

    [Fact]
    public async Task TEST_0093_BST_DAY_Day_Peak()
    {
        await RunTest("TEST 0093 BST_DAY Day Peak");
    }

    [Fact]
    public async Task TEST_0094_BST_DAY_Day_Extended_Peak()
    {
        await RunTest("TEST 0094 BST_DAY Day Extended Peak");
    }

    [Fact]
    public async Task TEST_0095_BST_DAY_Day_Overnight()
    {
        await RunTest("TEST 0095 BST_DAY Day Overnight");
    }

    [Fact]
    public async Task TEST_0096_BST_DAY_Day_Baseload()
    {
        await RunTest("TEST 0096 BST_DAY Day Baseload");
    }

    [Fact]
    public async Task TEST_0097_BST_DAY_Day_7_Day_Peak()
    {
        await RunTest("TEST 0097 BST_DAY Day 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0098_BST_DAY_Week_EFA1()
    {
        await RunTest("TEST 0098 BST_DAY Week EFA1");
    }

    [Fact]
    public async Task TEST_0099_BST_DAY_Week_EFA4()
    {
        await RunTest("TEST 0099 BST_DAY Week EFA4");
    }

    [Fact]
    public async Task TEST_0100_BST_DAY_Week_WD1()
    {
        await RunTest("TEST 0100 BST_DAY Week WD1");
    }

    [Fact]
    public async Task TEST_0101_BST_DAY_Week_WD4()
    {
        await RunTest("TEST 0101 BST_DAY Week WD4");
    }

    [Fact]
    public async Task TEST_0102_BST_DAY_Week_WE1()
    {
        await RunTest("TEST 0102 BST_DAY Week WE1");
    }

    [Fact]
    public async Task TEST_0103_BST_DAY_Week_WE4()
    {
        await RunTest("TEST 0103 BST_DAY Week WE4");
    }

    [Fact]
    public async Task TEST_0104_BST_DAY_Week_5_6A()
    {
        await RunTest("TEST 0104 BST_DAY Week 5+6A");
    }

    [Fact]
    public async Task TEST_0105_BST_DAY_Week_5B_6A()
    {
        await RunTest("TEST 0105 BST_DAY Week 5B+6A");
    }

    [Fact]
    public async Task TEST_0106_BST_DAY_Week_Peak()
    {
        await RunTest("TEST 0106 BST_DAY Week Peak");
    }

    [Fact]
    public async Task TEST_0107_BST_DAY_Week_Extended_Peak()
    {
        await RunTest("TEST 0107 BST_DAY Week Extended Peak");
    }

    [Fact]
    public async Task TEST_0108_BST_DAY_Week_Overnight()
    {
        await RunTest("TEST 0108 BST_DAY Week Overnight");
    }

    [Fact]
    public async Task TEST_0109_BST_DAY_Week_Baseload()
    {
        await RunTest("TEST 0109 BST_DAY Week Baseload");
    }

    [Fact]
    public async Task TEST_0110_BST_DAY_Week_7_Day_Peak()
    {
        await RunTest("TEST 0110 BST_DAY Week 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0111_BST_DAY_Month_EFA1()
    {
        await RunTest("TEST 0111 BST_DAY Month EFA1");
    }

    [Fact]
    public async Task TEST_0112_BST_DAY_Month_EFA4()
    {
        await RunTest("TEST 0112 BST_DAY Month EFA4");
    }

    [Fact]
    public async Task TEST_0113_BST_DAY_Month_WD1()
    {
        await RunTest("TEST 0113 BST_DAY Month WD1");
    }

    [Fact]
    public async Task TEST_0114_BST_DAY_Month_WD4()
    {
        await RunTest("TEST 0114 BST_DAY Month WD4");
    }

    [Fact]
    public async Task TEST_0115_BST_DAY_Month_WE1()
    {
        await RunTest("TEST 0115 BST_DAY Month WE1");
    }

    [Fact]
    public async Task TEST_0116_BST_DAY_Month_WE4()
    {
        await RunTest("TEST 0116 BST_DAY Month WE4");
    }

    [Fact]
    public async Task TEST_0117_BST_DAY_Month_5_6A()
    {
        await RunTest("TEST 0117 BST_DAY Month 5+6A");
    }

    [Fact]
    public async Task TEST_0118_BST_DAY_Month_5B_6A()
    {
        await RunTest("TEST 0118 BST_DAY Month 5B+6A");
    }

    [Fact]
    public async Task TEST_0119_BST_DAY_Month_Peak()
    {
        await RunTest("TEST 0119 BST_DAY Month Peak");
    }

    [Fact]
    public async Task TEST_0120_BST_DAY_Month_Extended_Peak()
    {
        await RunTest("TEST 0120 BST_DAY Month Extended Peak");
    }

    [Fact]
    public async Task TEST_0121_BST_DAY_Month_Overnight()
    {
        await RunTest("TEST 0121 BST_DAY Month Overnight");
    }

    [Fact]
    public async Task TEST_0122_BST_DAY_Month_Baseload()
    {
        await RunTest("TEST 0122 BST_DAY Month Baseload");
    }

    [Fact]
    public async Task TEST_0123_BST_DAY_Month_7_Day_Peak()
    {
        await RunTest("TEST 0123 BST_DAY Month 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0124_BST_DAY_Quarter_Peak()
    {
        await RunTest("TEST 0124 BST_DAY Quarter Peak");
    }

    [Fact]
    public async Task TEST_0125_BST_DAY_Quarter_Baseload()
    {
        await RunTest("TEST 0125 BST_DAY Quarter Baseload");
    }

    [Fact]
    public async Task TEST_0126_BST_DAY_HalfYear_Peak()
    {
        await RunTest("TEST 0126 BST_DAY HalfYear Peak");
    }

    [Fact]
    public async Task TEST_0127_BST_DAY_HalfYear_Baseload()
    {
        await RunTest("TEST 0127 BST_DAY HalfYear Baseload");
    }

    [Fact]
    public async Task TEST_0128_BST_DAY_Season_Winter_Peak()
    {
        await RunTest("TEST 0128 BST_DAY Season Winter Peak");
    }

    [Fact]
    public async Task TEST_0129_BST_DAY_Season_Winter_Baseload()
    {
        await RunTest("TEST 0129 BST_DAY Season Winter Baseload");
    }

    [Fact]
    public async Task TEST_0130_LAST_DAY_HalfHour_Spot()
    {
        await RunTest("TEST 0130 LAST_DAY HalfHour Spot");
    }

    [Fact]
    public async Task TEST_0131_LAST_DAY_Hour_Spot()
    {
        await RunTest("TEST 0131 LAST_DAY Hour Spot");
    }

    [Fact]
    public async Task TEST_0132_LAST_DAY_TwoHour_Spot()
    {
        await RunTest("TEST 0132 LAST_DAY TwoHour Spot");
    }

    [Fact]
    public async Task TEST_0133_LAST_DAY_Day_EFA1()
    {
        await RunTest("TEST 0133 LAST_DAY Day EFA1");
    }

    [Fact]
    public async Task TEST_0134_LAST_DAY_Day_EFA4()
    {
        await RunTest("TEST 0134 LAST_DAY Day EFA4");
    }

    [Fact]
    public async Task TEST_0135_LAST_DAY_Day_5_6A()
    {
        await RunTest("TEST 0135 LAST_DAY Day 5+6A");
    }

    [Fact]
    public async Task TEST_0136_LAST_DAY_Day_5B_6A()
    {
        await RunTest("TEST 0136 LAST_DAY Day 5B+6A");
    }

    [Fact]
    public async Task TEST_0137_LAST_DAY_Day_Peak()
    {
        await RunTest("TEST 0137 LAST_DAY Day Peak");
    }

    [Fact]
    public async Task TEST_0138_LAST_DAY_Day_Extended_Peak()
    {
        await RunTest("TEST 0138 LAST_DAY Day Extended Peak");
    }

    [Fact]
    public async Task TEST_0139_LAST_DAY_Day_Overnight()
    {
        await RunTest("TEST 0139 LAST_DAY Day Overnight");
    }

    [Fact]
    public async Task TEST_0140_LAST_DAY_Day_Baseload()
    {
        await RunTest("TEST 0140 LAST_DAY Day Baseload");
    }

    [Fact]
    public async Task TEST_0141_LAST_DAY_Day_7_Day_Peak()
    {
        await RunTest("TEST 0141 LAST_DAY Day 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0142_LAST_DAY_Week_EFA1()
    {
        await RunTest("TEST 0142 LAST_DAY Week EFA1");
    }

    [Fact]
    public async Task TEST_0143_LAST_DAY_Week_EFA4()
    {
        await RunTest("TEST 0143 LAST_DAY Week EFA4");
    }

    [Fact]
    public async Task TEST_0144_LAST_DAY_Week_WD1()
    {
        await RunTest("TEST 0144 LAST_DAY Week WD1");
    }

    [Fact]
    public async Task TEST_0145_LAST_DAY_Week_WD4()
    {
        await RunTest("TEST 0145 LAST_DAY Week WD4");
    }

    [Fact]
    public async Task TEST_0146_LAST_DAY_Week_WE1()
    {
        await RunTest("TEST 0146 LAST_DAY Week WE1");
    }

    [Fact]
    public async Task TEST_0147_LAST_DAY_Week_WE4()
    {
        await RunTest("TEST 0147 LAST_DAY Week WE4");
    }

    [Fact]
    public async Task TEST_0148_LAST_DAY_Week_5_6A()
    {
        await RunTest("TEST 0148 LAST_DAY Week 5+6A");
    }

    [Fact]
    public async Task TEST_0149_LAST_DAY_Week_5B_6A()
    {
        await RunTest("TEST 0149 LAST_DAY Week 5B+6A");
    }

    [Fact]
    public async Task TEST_0150_LAST_DAY_Week_Peak()
    {
        await RunTest("TEST 0150 LAST_DAY Week Peak");
    }

    [Fact]
    public async Task TEST_0151_LAST_DAY_Week_Extended_Peak()
    {
        await RunTest("TEST 0151 LAST_DAY Week Extended Peak");
    }

    [Fact]
    public async Task TEST_0152_LAST_DAY_Week_Overnight()
    {
        await RunTest("TEST 0152 LAST_DAY Week Overnight");
    }

    [Fact]
    public async Task TEST_0153_LAST_DAY_Week_Baseload()
    {
        await RunTest("TEST 0153 LAST_DAY Week Baseload");
    }

    [Fact]
    public async Task TEST_0154_LAST_DAY_Week_7_Day_Peak()
    {
        await RunTest("TEST 0154 LAST_DAY Week 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0155_LAST_DAY_Month_EFA1()
    {
        await RunTest("TEST 0155 LAST_DAY Month EFA1");
    }

    [Fact]
    public async Task TEST_0156_LAST_DAY_Month_EFA4()
    {
        await RunTest("TEST 0156 LAST_DAY Month EFA4");
    }

    [Fact]
    public async Task TEST_0157_LAST_DAY_Month_WD1()
    {
        await RunTest("TEST 0157 LAST_DAY Month WD1");
    }

    [Fact]
    public async Task TEST_0158_LAST_DAY_Month_WD4()
    {
        await RunTest("TEST 0158 LAST_DAY Month WD4");
    }

    [Fact]
    public async Task TEST_0159_LAST_DAY_Month_WE1()
    {
        await RunTest("TEST 0159 LAST_DAY Month WE1");
    }

    [Fact]
    public async Task TEST_0160_LAST_DAY_Month_WE4()
    {
        await RunTest("TEST 0160 LAST_DAY Month WE4");
    }

    [Fact]
    public async Task TEST_0161_LAST_DAY_Month_5_6A()
    {
        await RunTest("TEST 0161 LAST_DAY Month 5+6A");
    }

    [Fact]
    public async Task TEST_0162_LAST_DAY_Month_5B_6A()
    {
        await RunTest("TEST 0162 LAST_DAY Month 5B+6A");
    }

    [Fact]
    public async Task TEST_0163_LAST_DAY_Month_Peak()
    {
        await RunTest("TEST 0163 LAST_DAY Month Peak");
    }

    [Fact]
    public async Task TEST_0164_LAST_DAY_Month_Extended_Peak()
    {
        await RunTest("TEST 0164 LAST_DAY Month Extended Peak");
    }

    [Fact]
    public async Task TEST_0165_LAST_DAY_Month_Overnight()
    {
        await RunTest("TEST 0165 LAST_DAY Month Overnight");
    }

    [Fact]
    public async Task TEST_0166_LAST_DAY_Month_Baseload()
    {
        await RunTest("TEST 0166 LAST_DAY Month Baseload");
    }

    [Fact]
    public async Task TEST_0167_LAST_DAY_Month_7_Day_Peak()
    {
        await RunTest("TEST 0167 LAST_DAY Month 7 Day Peak");
    }

    [Fact]
    public async Task TEST_0168_LAST_DAY_Quarter_Peak()
    {
        await RunTest("TEST 0168 LAST_DAY Quarter Peak");
    }

    [Fact]
    public async Task TEST_0169_LAST_DAY_Quarter_Baseload()
    {
        await RunTest("TEST 0169 LAST_DAY Quarter Baseload");
    }

    [Fact]
    public async Task TEST_0170_LAST_DAY_Season_Summer_Peak()
    {
        await RunTest("TEST 0170 LAST_DAY Season Summer Peak");
    }

    [Fact]
    public async Task TEST_0171_LAST_DAY_Season_Summer_Baseload()
    {
        await RunTest("TEST 0171 LAST_DAY Season Summer Baseload");
    }

    [Fact]
    public async Task TEST_0172_FIRST_DAY_Day_UK_Gas()
    {
        await RunTest("TEST 0172 FIRST_DAY Day UK Gas");
    }

    [Fact]
    public async Task TEST_0173_FIRST_DAY_Month_UK_Gas()
    {
        await RunTest("TEST 0173 FIRST_DAY Month UK Gas");
    }

    [Fact]
    public async Task TEST_0174_FIRST_DAY_Quarter_UK_Gas()
    {
        await RunTest("TEST 0174 FIRST_DAY Quarter UK Gas");
    }

    [Fact]
    public async Task TEST_0175_FIRST_DAY_Season_Summer_UK_Gas()
    {
        await RunTest("TEST 0175 FIRST_DAY Season Summer UK Gas");
    }

    [Fact]
    public async Task TEST_0176_FIRST_DAY_Season_Winter_UK_Gas()
    {
        await RunTest("TEST 0176 FIRST_DAY Season Winter UK Gas");
    }

    [Fact]
    public async Task TEST_0177_FIRST_DAY_Year_UK_Gas()
    {
        await RunTest("TEST 0177 FIRST_DAY Year UK Gas");
    }

    [Fact]
    public async Task TEST_0178_SHORT_DAY_Day_UK_Gas()
    {
        await RunTest("TEST 0178 SHORT_DAY Day UK Gas");
    }

    [Fact]
    public async Task TEST_0179_SHORT_DAY_Month_UK_Gas()
    {
        await RunTest("TEST 0179 SHORT_DAY Month UK Gas");
    }

    [Fact]
    public async Task TEST_0180_SHORT_DAY_Quarter_UK_Gas()
    {
        await RunTest("TEST 0180 SHORT_DAY Quarter UK Gas");
    }

    [Fact]
    public async Task TEST_0181_BST_DAY_Day_UK_Gas()
    {
        await RunTest("TEST 0181 BST_DAY Day UK Gas");
    }

    [Fact]
    public async Task TEST_0182_BST_DAY_Month_UK_Gas()
    {
        await RunTest("TEST 0182 BST_DAY Month UK Gas");
    }

    [Fact]
    public async Task TEST_0183_BST_DAY_Quarter_UK_Gas()
    {
        await RunTest("TEST 0183 BST_DAY Quarter UK Gas");
    }

    [Fact]
    public async Task TEST_0184_BST_DAY_Season_Winter_UK_Gas()
    {
        await RunTest("TEST 0184 BST_DAY Season Winter UK Gas");
    }

    [Fact]
    public async Task TEST_0185_LAST_DAY_Day_UK_Gas()
    {
        await RunTest("TEST 0185 LAST_DAY Day UK Gas");
    }

    [Fact]
    public async Task TEST_0186_LAST_DAY_Month_UK_Gas()
    {
        await RunTest("TEST 0186 LAST_DAY Month UK Gas");
    }

    [Fact]
    public async Task TEST_0187_LAST_DAY_Quarter_UK_Gas()
    {
        await RunTest("TEST 0187 LAST_DAY Quarter UK Gas");
    }

    [Fact]
    public async Task TEST_0188_LAST_DAY_Season_Summer_UK_Gas()
    {
        await RunTest("TEST 0188 LAST_DAY Season Summer UK Gas");
    }

    [Fact]
    public async Task TEST_0189_LAST_DAY_Year_UK_Gas()
    {
        await RunTest("TEST 0189 LAST_DAY Year UK Gas");
    }

    [Fact]
    public async Task TEST_0190_NO_TRADE_REFERENCE()
    {
        await RunTest("TEST 0190 NO TRADE REFERENCE");
    }

    [Fact]
    public async Task TEST_0191_MISSING_TRADE_REFERENCE()
    {
        await RunTest("TEST 0191 MISSING TRADE REFERENCE");
    }

    private async Task RunTest(string testName)
    {
        var test = enegenGenstarEcvnFixture.ExpectedResults.SingleOrDefault(t => t.Description == testName);

        Assert.NotNull(test);

        var ecvn = await enegenGenstarEcvnFixture.EcvnManager.CreateEcvn(test.Inputs, string.Empty);

        if (!string.IsNullOrWhiteSpace(test.ExpectedError))
        {
            Assert.Equal(test.ExpectedError, ecvn.Message);    
            return;
        }
        
        Assert.Equal(test.ExpectedResults.ContractName, ecvn.ContractName);
        Assert.Equal(test.ExpectedResults.ContractDescription, ecvn.ContractDescription);
        Assert.Equal(test.ExpectedResults.Trader, ecvn.Trader);
        Assert.Equal(test.ExpectedResults.TraderProdConFlag, ecvn.TraderProdConFlag);
        Assert.Equal(test.ExpectedResults.Party2, ecvn.Party2);
        Assert.Equal(test.ExpectedResults.Party2ProdConFlag, ecvn.Party2ProdConFlag);
        Assert.Equal(test.ExpectedResults.ContractStartDate, ecvn.ContractStartDate);
        Assert.Equal(test.ExpectedResults.ContractEndDate, ecvn.ContractEndDate);
        Assert.Equal(test.ExpectedResults.ContractGroupId, ecvn.ContractGroupId);
        Assert.Equal(test.ExpectedResults.ContractProfile , ecvn.ContractProfile);
        Assert.Equal(test.ExpectedResults.Evergreen , ecvn.Evergreen);
        
        var zipped = test.ExpectedResults.EnergyVolumeItems.Zip(ecvn.EnergyVolumeItems, (e, a) => new
        {
            Expected = e,
            Actual = a
        }).ToList();

        foreach (var result in zipped)
        {
            Assert.Equal(result.Expected.EcvDate, result.Actual.EcvDate);   
            Assert.Equal(result.Expected.EcvPeriod, result.Actual.EcvPeriod);
            Assert.Equal(result.Expected.EcvVolume, result.Actual.EcvVolume);
        }
    }
}