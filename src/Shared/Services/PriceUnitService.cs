using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class PriceUnitService : TradeCubeApiService, IPriceUnitService
    {
        public PriceUnitService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<MappingService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<PriceUnitDataObject>>> GetPriceUnitAsync(string priceUnit, string apiJwtToken)
        {
            return await GetViaJwtAsync<PriceUnitDataObject>($"PriceUnit/{priceUnit}", apiJwtToken);
        }
    }
}