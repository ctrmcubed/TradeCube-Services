using Fidectus.Models;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusMappingService
    {
        string MapTradeReferenceToTradeId(string tradeReference, int tradeLeg);
        Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken);

        Task<TradeConfirmation> MapConfirmation(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse, IEnumerable<ProfileResponse> profileResponses,
            string apiJwtToken, IFidectusConfiguration fidectusConfiguration);
    }
}