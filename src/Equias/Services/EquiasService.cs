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
        private readonly ILogger<ApiService> logger;

        public EquiasService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<EboGetTradeStatusResponse> EboGetTradeStatus(IEnumerable<string> tradeIds, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration)
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

        public async Task<EboTradeResponse> EboAddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration)
        {
            try
            {
                logger.LogInformation($"{TradeCubeJsonSerializer.Serialize(physicalTrade)}");

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await PostAsync<PhysicalTrade, EboTradeResponse>(httpClient, equiasConfiguration.AddPhysicalTradeUri, physicalTrade, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new EboTradeResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<EboTradeResponse> ModifyPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration)
        {
            try
            {
                logger.LogInformation($"{TradeCubeJsonSerializer.Serialize(physicalTrade)}");

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await PostAsync<PhysicalTrade, EboTradeResponse>(httpClient, equiasConfiguration.ModifyPhysicalTradeUri, physicalTrade, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new EboTradeResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<EboTradeResponse> CancelTrade(CancelTrade cancelTrade, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);
                httpClient.DefaultRequestHeaders.Add("token", requestTokenResponse?.Token);

                return await DeleteAsync<CancelTrade, EboTradeResponse>(httpClient, equiasConfiguration.CancelTradeUri, cancelTrade, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new EboTradeResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
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
