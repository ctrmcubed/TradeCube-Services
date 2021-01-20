using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public class CountryService : TradeCubeApiService, ICountryService
    {
        public CountryService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<CountryDataObject>>> CountriesAsync(string apiJwtToken)
        {
            return await GetViaJwtAsync<CountryDataObject>("Country", apiJwtToken);
        }
    }
}