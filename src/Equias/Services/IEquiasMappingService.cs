﻿using Equias.Models.BackOfficeServices;
using Shared.DataObjects;
using Shared.Managers;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasMappingService
    {
        IEquiasMappingService SetMappingManager(MappingManager mapping);
        Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken);
        Task<PhysicalTrade> MapTrade(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse,
            IEnumerable<CashflowResponse> cashflowResponses, IEnumerable<ProfileResponse> profileResponses, string apiJwtToken);
    }
}