﻿using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ITradeSummaryService
    {
        Task<ApiResponseWrapper<IEnumerable<TradeSummaryResponse>>> TradeSummaryAsync(string tradeReference, int tradeLeg, string apiJwtToken);
    }
}