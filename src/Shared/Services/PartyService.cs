using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class PartyService : TradeCubeApiService, IPartyService
    {
        public PartyService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<MappingService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<PartyDataObject>>> GetPartyAsync(string party, string apiJwtToken)
        {
            return await GetViaJwtAsync<PartyDataObject>($"Party/{party}", apiJwtToken);
        }
    }
}