using Equias.Models.BackOfficeServices;
using Shared.DataObjects;
using Shared.Helpers;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasMappingService
    {
        Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken);
        Task<PhysicalTrade> MapTrade(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse, IEnumerable<CashflowResponse> cashflowResponses,
            IEnumerable<ProfileResponse> profileResponses, MappingHelper mappingHelper, string apiJwtToken);
    }
}