using System;
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

    // [Fact]
    // public async Task TEST_0000_FIRST_DAY_HalfHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0000 FIRST_DAY HalfHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0001_FIRST_DAY_HalfHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0001 FIRST_DAY HalfHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0002_FIRST_DAY_Hour_Spot_Profile()
    // {
    //     await RunTest("TEST 0002 FIRST_DAY Hour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0003_FIRST_DAY_Hour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0003 FIRST_DAY Hour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0004_FIRST_DAY_TwoHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0004 FIRST_DAY TwoHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0005_FIRST_DAY_TwoHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0005 FIRST_DAY TwoHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0006_FIRST_DAY_Day_EFA1_Profile()
    // {
    //     await RunTest("TEST 0006 FIRST_DAY Day EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0007_FIRST_DAY_Day_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0007 FIRST_DAY Day EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0008_FIRST_DAY_Day_EFA4_Profile()
    // {
    //     await RunTest("TEST 0008 FIRST_DAY Day EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0009_FIRST_DAY_Day_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0009 FIRST_DAY Day EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0010_FIRST_DAY_Day_5_6A_Profile()
    // {
    //     await RunTest("TEST 0010 FIRST_DAY Day 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0011_FIRST_DAY_Day_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0011 FIRST_DAY Day 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0012_FIRST_DAY_Day_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0012 FIRST_DAY Day 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0013_FIRST_DAY_Day_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0013 FIRST_DAY Day 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0014_FIRST_DAY_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0014 FIRST_DAY Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0015_FIRST_DAY_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0015 FIRST_DAY Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0016_FIRST_DAY_Day_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0016 FIRST_DAY Day Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0017_FIRST_DAY_Day_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0017 FIRST_DAY Day Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0018_FIRST_DAY_Day_Overnight_Profile()
    // {
    //     await RunTest("TEST 0018 FIRST_DAY Day Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0019_FIRST_DAY_Day_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0019 FIRST_DAY Day Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0020_FIRST_DAY_Day_Baseload_Profile()
    // {
    //     await RunTest("TEST 0020 FIRST_DAY Day Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0021_FIRST_DAY_Day_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0021 FIRST_DAY Day Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0022_FIRST_DAY_Day_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0022 FIRST_DAY Day 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0023_FIRST_DAY_Day_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0023 FIRST_DAY Day 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0024_FIRST_DAY_Week_EFA1_Profile()
    // {
    //     await RunTest("TEST 0024 FIRST_DAY Week EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0025_FIRST_DAY_Week_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0025 FIRST_DAY Week EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0026_FIRST_DAY_Week_EFA4_Profile()
    // {
    //     await RunTest("TEST 0026 FIRST_DAY Week EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0027_FIRST_DAY_Week_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0027 FIRST_DAY Week EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0028_FIRST_DAY_Week_WD1_Profile()
    // {
    //     await RunTest("TEST 0028 FIRST_DAY Week WD1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0029_FIRST_DAY_Week_WD1_Fixed()
    // {
    //     await RunTest("TEST 0029 FIRST_DAY Week WD1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0030_FIRST_DAY_Week_WD4_Profile()
    // {
    //     await RunTest("TEST 0030 FIRST_DAY Week WD4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0031_FIRST_DAY_Week_WD4_Fixed()
    // {
    //     await RunTest("TEST 0031 FIRST_DAY Week WD4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0032_FIRST_DAY_Week_WE1_Profile()
    // {
    //     await RunTest("TEST 0032 FIRST_DAY Week WE1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0033_FIRST_DAY_Week_WE1_Fixed()
    // {
    //     await RunTest("TEST 0033 FIRST_DAY Week WE1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0034_FIRST_DAY_Week_WE4_Profile()
    // {
    //     await RunTest("TEST 0034 FIRST_DAY Week WE4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0035_FIRST_DAY_Week_WE4_Fixed()
    // {
    //     await RunTest("TEST 0035 FIRST_DAY Week WE4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0036_FIRST_DAY_Week_5_6A_Profile()
    // {
    //     await RunTest("TEST 0036 FIRST_DAY Week 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0037_FIRST_DAY_Week_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0037 FIRST_DAY Week 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0038_FIRST_DAY_Week_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0038 FIRST_DAY Week 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0039_FIRST_DAY_Week_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0039 FIRST_DAY Week 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0040_FIRST_DAY_Week_Peak_Profile()
    // {
    //     await RunTest("TEST 0040 FIRST_DAY Week Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0041_FIRST_DAY_Week_Peak_Fixed()
    // {
    //     await RunTest("TEST 0041 FIRST_DAY Week Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0042_FIRST_DAY_Week_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0042 FIRST_DAY Week Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0043_FIRST_DAY_Week_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0043 FIRST_DAY Week Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0044_FIRST_DAY_Week_Overnight_Profile()
    // {
    //     await RunTest("TEST 0044 FIRST_DAY Week Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0045_FIRST_DAY_Week_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0045 FIRST_DAY Week Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0046_FIRST_DAY_Week_Baseload_Profile()
    // {
    //     await RunTest("TEST 0046 FIRST_DAY Week Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0047_FIRST_DAY_Week_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0047 FIRST_DAY Week Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0048_FIRST_DAY_Week_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0048 FIRST_DAY Week 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0049_FIRST_DAY_Week_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0049 FIRST_DAY Week 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0050_SHORT_DAY_HalfHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0050 SHORT_DAY HalfHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0051_SHORT_DAY_HalfHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0051 SHORT_DAY HalfHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0052_SHORT_DAY_Hour_Spot_Profile()
    // {
    //     await RunTest("TEST 0052 SHORT_DAY Hour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0053_SHORT_DAY_Hour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0053 SHORT_DAY Hour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0054_SHORT_DAY_TwoHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0054 SHORT_DAY TwoHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0055_SHORT_DAY_TwoHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0055 SHORT_DAY TwoHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0056_SHORT_DAY_Day_EFA1_Profile()
    // {
    //     await RunTest("TEST 0056 SHORT_DAY Day EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0057_SHORT_DAY_Day_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0057 SHORT_DAY Day EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0058_SHORT_DAY_Day_EFA4_Profile()
    // {
    //     await RunTest("TEST 0058 SHORT_DAY Day EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0059_SHORT_DAY_Day_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0059 SHORT_DAY Day EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0060_SHORT_DAY_Day_5_6A_Profile()
    // {
    //     await RunTest("TEST 0060 SHORT_DAY Day 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0061_SHORT_DAY_Day_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0061 SHORT_DAY Day 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0062_SHORT_DAY_Day_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0062 SHORT_DAY Day 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0063_SHORT_DAY_Day_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0063 SHORT_DAY Day 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0064_SHORT_DAY_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0064 SHORT_DAY Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0065_SHORT_DAY_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0065 SHORT_DAY Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0066_SHORT_DAY_Day_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0066 SHORT_DAY Day Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0067_SHORT_DAY_Day_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0067 SHORT_DAY Day Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0068_SHORT_DAY_Day_Overnight_Profile()
    // {
    //     await RunTest("TEST 0068 SHORT_DAY Day Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0069_SHORT_DAY_Day_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0069 SHORT_DAY Day Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0070_SHORT_DAY_Day_Baseload_Profile()
    // {
    //     await RunTest("TEST 0070 SHORT_DAY Day Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0071_SHORT_DAY_Day_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0071 SHORT_DAY Day Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0072_SHORT_DAY_Day_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0072 SHORT_DAY Day 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0073_SHORT_DAY_Day_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0073 SHORT_DAY Day 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0074_SHORT_DAY_Week_EFA1_Profile()
    // {
    //     await RunTest("TEST 0074 SHORT_DAY Week EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0075_SHORT_DAY_Week_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0075 SHORT_DAY Week EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0076_SHORT_DAY_Week_EFA4_Profile()
    // {
    //     await RunTest("TEST 0076 SHORT_DAY Week EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0077_SHORT_DAY_Week_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0077 SHORT_DAY Week EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0078_SHORT_DAY_Week_WD1_Profile()
    // {
    //     await RunTest("TEST 0078 SHORT_DAY Week WD1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0079_SHORT_DAY_Week_WD1_Fixed()
    // {
    //     await RunTest("TEST 0079 SHORT_DAY Week WD1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0080_SHORT_DAY_Week_WD4_Profile()
    // {
    //     await RunTest("TEST 0080 SHORT_DAY Week WD4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0081_SHORT_DAY_Week_WD4_Fixed()
    // {
    //     await RunTest("TEST 0081 SHORT_DAY Week WD4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0082_SHORT_DAY_Week_WE1_Profile()
    // {
    //     await RunTest("TEST 0082 SHORT_DAY Week WE1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0083_SHORT_DAY_Week_WE1_Fixed()
    // {
    //     await RunTest("TEST 0083 SHORT_DAY Week WE1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0084_SHORT_DAY_Week_WE4_Profile()
    // {
    //     await RunTest("TEST 0084 SHORT_DAY Week WE4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0085_SHORT_DAY_Week_WE4_Fixed()
    // {
    //     await RunTest("TEST 0085 SHORT_DAY Week WE4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0086_SHORT_DAY_Week_5_6A_Profile()
    // {
    //     await RunTest("TEST 0086 SHORT_DAY Week 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0087_SHORT_DAY_Week_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0087 SHORT_DAY Week 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0088_SHORT_DAY_Week_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0088 SHORT_DAY Week 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0089_SHORT_DAY_Week_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0089 SHORT_DAY Week 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0090_SHORT_DAY_Week_Peak_Profile()
    // {
    //     await RunTest("TEST 0090 SHORT_DAY Week Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0091_SHORT_DAY_Week_Peak_Fixed()
    // {
    //     await RunTest("TEST 0091 SHORT_DAY Week Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0092_SHORT_DAY_Week_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0092 SHORT_DAY Week Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0093_SHORT_DAY_Week_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0093 SHORT_DAY Week Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0094_SHORT_DAY_Week_Overnight_Profile()
    // {
    //     await RunTest("TEST 0094 SHORT_DAY Week Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0095_SHORT_DAY_Week_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0095 SHORT_DAY Week Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0096_SHORT_DAY_Week_Baseload_Profile()
    // {
    //     await RunTest("TEST 0096 SHORT_DAY Week Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0097_SHORT_DAY_Week_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0097 SHORT_DAY Week Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0098_SHORT_DAY_Week_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0098 SHORT_DAY Week 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0099_SHORT_DAY_Week_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0099 SHORT_DAY Week 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0100_BST_DAY_HalfHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0100 BST_DAY HalfHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0101_BST_DAY_HalfHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0101 BST_DAY HalfHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0102_BST_DAY_Hour_Spot_Profile()
    // {
    //     await RunTest("TEST 0102 BST_DAY Hour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0103_BST_DAY_Hour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0103 BST_DAY Hour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0104_BST_DAY_TwoHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0104 BST_DAY TwoHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0105_BST_DAY_TwoHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0105 BST_DAY TwoHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0106_BST_DAY_Day_EFA1_Profile()
    // {
    //     await RunTest("TEST 0106 BST_DAY Day EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0107_BST_DAY_Day_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0107 BST_DAY Day EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0108_BST_DAY_Day_EFA4_Profile()
    // {
    //     await RunTest("TEST 0108 BST_DAY Day EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0109_BST_DAY_Day_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0109 BST_DAY Day EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0110_BST_DAY_Day_5_6A_Profile()
    // {
    //     await RunTest("TEST 0110 BST_DAY Day 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0111_BST_DAY_Day_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0111 BST_DAY Day 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0112_BST_DAY_Day_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0112 BST_DAY Day 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0113_BST_DAY_Day_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0113 BST_DAY Day 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0114_BST_DAY_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0114 BST_DAY Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0115_BST_DAY_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0115 BST_DAY Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0116_BST_DAY_Day_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0116 BST_DAY Day Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0117_BST_DAY_Day_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0117 BST_DAY Day Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0118_BST_DAY_Day_Overnight_Profile()
    // {
    //     await RunTest("TEST 0118 BST_DAY Day Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0119_BST_DAY_Day_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0119 BST_DAY Day Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0120_BST_DAY_Day_Baseload_Profile()
    // {
    //     await RunTest("TEST 0120 BST_DAY Day Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0121_BST_DAY_Day_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0121 BST_DAY Day Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0122_BST_DAY_Day_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0122 BST_DAY Day 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0123_BST_DAY_Day_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0123 BST_DAY Day 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0124_BST_DAY_Week_EFA1_Profile()
    // {
    //     await RunTest("TEST 0124 BST_DAY Week EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0125_BST_DAY_Week_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0125 BST_DAY Week EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0126_BST_DAY_Week_EFA4_Profile()
    // {
    //     await RunTest("TEST 0126 BST_DAY Week EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0127_BST_DAY_Week_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0127 BST_DAY Week EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0128_BST_DAY_Week_WD1_Profile()
    // {
    //     await RunTest("TEST 0128 BST_DAY Week WD1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0129_BST_DAY_Week_WD1_Fixed()
    // {
    //     await RunTest("TEST 0129 BST_DAY Week WD1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0130_BST_DAY_Week_WD4_Profile()
    // {
    //     await RunTest("TEST 0130 BST_DAY Week WD4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0131_BST_DAY_Week_WD4_Fixed()
    // {
    //     await RunTest("TEST 0131 BST_DAY Week WD4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0132_BST_DAY_Week_WE1_Profile()
    // {
    //     await RunTest("TEST 0132 BST_DAY Week WE1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0133_BST_DAY_Week_WE1_Fixed()
    // {
    //     await RunTest("TEST 0133 BST_DAY Week WE1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0134_BST_DAY_Week_WE4_Profile()
    // {
    //     await RunTest("TEST 0134 BST_DAY Week WE4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0135_BST_DAY_Week_WE4_Fixed()
    // {
    //     await RunTest("TEST 0135 BST_DAY Week WE4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0136_BST_DAY_Week_5_6A_Profile()
    // {
    //     await RunTest("TEST 0136 BST_DAY Week 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0137_BST_DAY_Week_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0137 BST_DAY Week 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0138_BST_DAY_Week_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0138 BST_DAY Week 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0139_BST_DAY_Week_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0139 BST_DAY Week 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0140_BST_DAY_Week_Peak_Profile()
    // {
    //     await RunTest("TEST 0140 BST_DAY Week Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0141_BST_DAY_Week_Peak_Fixed()
    // {
    //     await RunTest("TEST 0141 BST_DAY Week Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0142_BST_DAY_Week_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0142 BST_DAY Week Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0143_BST_DAY_Week_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0143 BST_DAY Week Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0144_BST_DAY_Week_Overnight_Profile()
    // {
    //     await RunTest("TEST 0144 BST_DAY Week Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0145_BST_DAY_Week_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0145 BST_DAY Week Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0146_BST_DAY_Week_Baseload_Profile()
    // {
    //     await RunTest("TEST 0146 BST_DAY Week Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0147_BST_DAY_Week_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0147 BST_DAY Week Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0148_BST_DAY_Week_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0148 BST_DAY Week 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0149_BST_DAY_Week_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0149 BST_DAY Week 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0150_LAST_DAY_HalfHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0150 LAST_DAY HalfHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0151_LAST_DAY_HalfHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0151 LAST_DAY HalfHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0152_LAST_DAY_Hour_Spot_Profile()
    // {
    //     await RunTest("TEST 0152 LAST_DAY Hour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0153_LAST_DAY_Hour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0153 LAST_DAY Hour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0154_LAST_DAY_TwoHour_Spot_Profile()
    // {
    //     await RunTest("TEST 0154 LAST_DAY TwoHour Spot Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0155_LAST_DAY_TwoHour_Spot_Fixed()
    // {
    //     await RunTest("TEST 0155 LAST_DAY TwoHour Spot Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0156_LAST_DAY_Day_EFA1_Profile()
    // {
    //     await RunTest("TEST 0156 LAST_DAY Day EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0157_LAST_DAY_Day_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0157 LAST_DAY Day EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0158_LAST_DAY_Day_EFA4_Profile()
    // {
    //     await RunTest("TEST 0158 LAST_DAY Day EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0159_LAST_DAY_Day_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0159 LAST_DAY Day EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0160_LAST_DAY_Day_5_6A_Profile()
    // {
    //     await RunTest("TEST 0160 LAST_DAY Day 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0161_LAST_DAY_Day_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0161 LAST_DAY Day 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0162_LAST_DAY_Day_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0162 LAST_DAY Day 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0163_LAST_DAY_Day_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0163 LAST_DAY Day 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0164_LAST_DAY_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0164 LAST_DAY Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0165_LAST_DAY_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0165 LAST_DAY Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0166_LAST_DAY_Day_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0166 LAST_DAY Day Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0167_LAST_DAY_Day_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0167 LAST_DAY Day Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0168_LAST_DAY_Day_Overnight_Profile()
    // {
    //     await RunTest("TEST 0168 LAST_DAY Day Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0169_LAST_DAY_Day_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0169 LAST_DAY Day Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0170_LAST_DAY_Day_Baseload_Profile()
    // {
    //     await RunTest("TEST 0170 LAST_DAY Day Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0171_LAST_DAY_Day_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0171 LAST_DAY Day Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0172_LAST_DAY_Day_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0172 LAST_DAY Day 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0173_LAST_DAY_Day_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0173 LAST_DAY Day 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0174_LAST_DAY_Week_EFA1_Profile()
    // {
    //     await RunTest("TEST 0174 LAST_DAY Week EFA1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0175_LAST_DAY_Week_EFA1_Fixed()
    // {
    //     await RunTest("TEST 0175 LAST_DAY Week EFA1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0176_LAST_DAY_Week_EFA4_Profile()
    // {
    //     await RunTest("TEST 0176 LAST_DAY Week EFA4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0177_LAST_DAY_Week_EFA4_Fixed()
    // {
    //     await RunTest("TEST 0177 LAST_DAY Week EFA4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0178_LAST_DAY_Week_WD1_Profile()
    // {
    //     await RunTest("TEST 0178 LAST_DAY Week WD1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0179_LAST_DAY_Week_WD1_Fixed()
    // {
    //     await RunTest("TEST 0179 LAST_DAY Week WD1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0180_LAST_DAY_Week_WD4_Profile()
    // {
    //     await RunTest("TEST 0180 LAST_DAY Week WD4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0181_LAST_DAY_Week_WD4_Fixed()
    // {
    //     await RunTest("TEST 0181 LAST_DAY Week WD4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0182_LAST_DAY_Week_WE1_Profile()
    // {
    //     await RunTest("TEST 0182 LAST_DAY Week WE1 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0183_LAST_DAY_Week_WE1_Fixed()
    // {
    //     await RunTest("TEST 0183 LAST_DAY Week WE1 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0184_LAST_DAY_Week_WE4_Profile()
    // {
    //     await RunTest("TEST 0184 LAST_DAY Week WE4 Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0185_LAST_DAY_Week_WE4_Fixed()
    // {
    //     await RunTest("TEST 0185 LAST_DAY Week WE4 Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0186_LAST_DAY_Week_5_6A_Profile()
    // {
    //     await RunTest("TEST 0186 LAST_DAY Week 5+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0187_LAST_DAY_Week_5_6A_Fixed()
    // {
    //     await RunTest("TEST 0187 LAST_DAY Week 5+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0188_LAST_DAY_Week_5B_6A_Profile()
    // {
    //     await RunTest("TEST 0188 LAST_DAY Week 5B+6A Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0189_LAST_DAY_Week_5B_6A_Fixed()
    // {
    //     await RunTest("TEST 0189 LAST_DAY Week 5B+6A Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0190_LAST_DAY_Week_Peak_Profile()
    // {
    //     await RunTest("TEST 0190 LAST_DAY Week Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0191_LAST_DAY_Week_Peak_Fixed()
    // {
    //     await RunTest("TEST 0191 LAST_DAY Week Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0192_LAST_DAY_Week_Extended_Peak_Profile()
    // {
    //     await RunTest("TEST 0192 LAST_DAY Week Extended Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0193_LAST_DAY_Week_Extended_Peak_Fixed()
    // {
    //     await RunTest("TEST 0193 LAST_DAY Week Extended Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0194_LAST_DAY_Week_Overnight_Profile()
    // {
    //     await RunTest("TEST 0194 LAST_DAY Week Overnight Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0195_LAST_DAY_Week_Overnight_Fixed()
    // {
    //     await RunTest("TEST 0195 LAST_DAY Week Overnight Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0196_LAST_DAY_Week_Baseload_Profile()
    // {
    //     await RunTest("TEST 0196 LAST_DAY Week Baseload Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0197_LAST_DAY_Week_Baseload_Fixed()
    // {
    //     await RunTest("TEST 0197 LAST_DAY Week Baseload Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0198_LAST_DAY_Week_7_Day_Peak_Profile()
    // {
    //     await RunTest("TEST 0198 LAST_DAY Week 7 Day Peak Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0199_LAST_DAY_Week_7_Day_Peak_Fixed()
    // {
    //     await RunTest("TEST 0199 LAST_DAY Week 7 Day Peak Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0200_FIRST_DAY_Day_UK_Gas_Profile()
    // {
    //     await RunTest("TEST 0200 FIRST_DAY Day UK Gas Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0201_FIRST_DAY_Day_UK_Gas_Fixed()
    // {
    //     await RunTest("TEST 0201 FIRST_DAY Day UK Gas Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0202_SHORT_DAY_Day_UK_Gas_Profile()
    // {
    //     await RunTest("TEST 0202 SHORT_DAY Day UK Gas Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0203_SHORT_DAY_Day_UK_Gas_Fixed()
    // {
    //     await RunTest("TEST 0203 SHORT_DAY Day UK Gas Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0204_BST_DAY_Day_UK_Gas_Profile()
    // {
    //     await RunTest("TEST 0204 BST_DAY Day UK Gas Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0205_BST_DAY_Day_UK_Gas_Fixed()
    // {
    //     await RunTest("TEST 0205 BST_DAY Day UK Gas Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0206_LAST_DAY_Day_UK_Gas_Profile()
    // {
    //     await RunTest("TEST 0206 LAST_DAY Day UK Gas Profile");
    // }
    //
    // [Fact]
    // public async Task TEST_0207_LAST_DAY_Day_UK_Gas_Fixed()
    // {
    //     await RunTest("TEST 0207 LAST_DAY Day UK Gas Fixed");
    // }
    //
    // [Fact]
    // public async Task TEST_0208_NO_TRADE_REFERENCE()
    // {
    //     await RunTest("TEST 0208 NO TRADE REFERENCE");
    // }
    //
    // [Fact]
    // public async Task TEST_0209_MISSING_TRADE_REFERENCE()
    // {
    //     await RunTest("TEST 0209 MISSING TRADE REFERENCE");
    // }

    private async Task RunTest(string testName)
    {
        var test = enegenGenstarEcvnFixture.GetExpectedResult(testName);

        Assert.NotNull(test);

        try
        {
            var ecvnContext = await enegenGenstarEcvnFixture.EcvnManager.CreateEcvnContext(test.Inputs, string.Empty);
            var ecvn = await enegenGenstarEcvnFixture.EcvnManager.CreateEcvnRequest(ecvnContext, string.Empty);

            if (!string.IsNullOrWhiteSpace(test.ExpectedError))
            {
                Assert.Equal(test.ExpectedError, ecvn.ValidationMessage);    
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
        catch (Exception ex)
        {
            Assert.Equal(test.ExpectedError, ex.Message);    
        }
    }
}