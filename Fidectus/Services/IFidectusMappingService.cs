using Fidectus.Models;
using Shared.DataObjects;
using Shared.Managers;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusMappingService
    {
        IFidectusMappingService SetMappingManager(MappingManager mapping);
        Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken);

        Task<TradeConfirmation> MapConfirmation(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse, IEnumerable<ProfileResponse> profileResponses, string apiJwtToken);
    }
}