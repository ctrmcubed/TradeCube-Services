using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Equias.Services
{
    public class EquiasService : ApiService, IEquiasService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IEquiasConfiguration equiasConfiguration;
        private readonly ILogger<ApiService> logger;

        public EquiasService(IHttpClientFactory httpClientFactory, IEquiasConfiguration equiasConfiguration, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.equiasConfiguration = equiasConfiguration;
            this.logger = logger;
        }

        public async Task<AddPhysicalTradeResponse> AddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await PostAsync<PhysicalTrade, AddPhysicalTradeResponse>(httpClient, equiasConfiguration.AddPhysicalTradeUri, physicalTrade, false);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
