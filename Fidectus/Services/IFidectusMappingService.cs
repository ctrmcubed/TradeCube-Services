using Fidectus.Models;
using Shared.DataObjects;
using Shared.Helpers;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusMappingService
    {
        Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken);

        Task<TradeConfirmation> MapConfirmation(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse, IEnumerable<ProfileResponse> profileResponses,
            MappingHelper mappingHelper, SettingHelper settingsHelper, string apiJwtToken);
    }
}