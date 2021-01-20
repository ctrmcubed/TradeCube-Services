using Equias.Models.BackOfficeServices;
using Shared.DataObjects;

namespace Equias.Services
{
    public interface IEquiasMappingService
    {
        PhysicalTrade MapTrade(TradeDataObject tradeDataObject);
    }
}