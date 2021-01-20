using Equias.Services;
using Xunit;

namespace TradeCube_ServicesTests.Equias
{
    public class EquiasUnitTests
    {
        [Fact]
        public void TestMapTradeToTradeId()
        {
            Assert.Equal("0000001001", EquiasMappingService.MapTradeId("1", 1));
            Assert.Equal("000000A002", EquiasMappingService.MapTradeId("A", 2));
            Assert.Equal("0000012003", EquiasMappingService.MapTradeId("1_2", 3));
            Assert.Equal("1234567890022", EquiasMappingService.MapTradeId("1234567890", 22));
            Assert.Equal("BC1234567890001", EquiasMappingService.MapTradeId("?BC1234567890", 1));
            Assert.Equal("ABC1234567890235", EquiasMappingService.MapTradeId("ABC1234567890", 235));
            Assert.Equal("CDEFGHIJKLMNOPQRSTUVWXYZ123033", EquiasMappingService.MapTradeId("ABCDEFGHIJKLMNOPQRSTUVWXYZ123", 33));
        }
    }
}
