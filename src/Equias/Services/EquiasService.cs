using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Serialization;
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

        public async Task<EboPhysicalTradeResponse> EboAddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse)
        {
            try
            {
                logger.LogInformation($"{TradeCubeJsonSerializer.Serialize(physicalTrade)}");

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await PostAsync<PhysicalTrade, EboPhysicalTradeResponse>(httpClient, equiasConfiguration.AddPhysicalTradeUri, physicalTrade, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<EboPhysicalTradeResponse> ModifyPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse)
        {
            try
            {
                logger.LogInformation($"{TradeCubeJsonSerializer.Serialize(physicalTrade)}");

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await PostAsync<PhysicalTrade, EboPhysicalTradeResponse>(httpClient, equiasConfiguration.ModifyPhysicalTradeUri, physicalTrade, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public static string MapTradeId(string tradeReference, int tradeLeg)
        {
            var reference = tradeReference
                .ToAlphaNumericOnly()
                .GetLast(27)
                .Pad(7, '0');

            var leg = tradeLeg
                .ToString()
                .Pad(3, '0');

            return $"{reference}{leg}";
        }
    }
}
