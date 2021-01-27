using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Services;
using System;
using System.Collections.Generic;
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

        public async Task<EboGetTradeStatusResponse> EboGetTradeStatus(IEnumerable<string> tradeIds, RequestTokenResponse requestTokenResponse)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await PostAsync<IEnumerable<string>, EboGetTradeStatusResponse>(httpClient, equiasConfiguration.GetTradeStatusUri, tradeIds, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<EboAddPhysicalTradeResponse> EboAddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await PostAsync<PhysicalTrade, EboAddPhysicalTradeResponse>(httpClient, equiasConfiguration.AddPhysicalTradeUri, physicalTrade, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
