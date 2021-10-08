using Fidectus.Messages;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Constants;
using Shared.Services;
using Shared.Services.Redis;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public class FidectusAuthenticationService : ApiService, IFidectusAuthenticationService
    {
        private class FidectusAuthenticationToken
        {
            public string AccessToken { get; set; }
        }

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRedisService redisService;

        public FidectusAuthenticationService(IHttpClientFactory httpClientFactory, IRedisService redisService, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.redisService = redisService;
        }

        public async Task<RequestTokenResponse> FidectusGetAuthenticationToken(RequestTokenRequest requestTokenRequest, IFidectusConfiguration fidectusConfiguration)
        {
            var requestTokenResponse = await redisService.Get<FidectusAuthenticationToken>(RedisConstants.FidectusAuthenticationTokenKey, requestTokenRequest.ClientId);

            if (!string.IsNullOrWhiteSpace(requestTokenResponse?.AccessToken))
            {
                return new RequestTokenResponse
                {
                    AccessToken = requestTokenResponse.AccessToken,
                    IsSuccessStatusCode = true
                };
            }

            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusAuthUrl);

            var getAuthenticationToken = await PostAsync<RequestTokenRequest, RequestTokenResponse>(httpClient, fidectusConfiguration.FidectusAuthUrl, requestTokenRequest);
            var fidectusAuthenticationToken = new FidectusAuthenticationToken { AccessToken = getAuthenticationToken.AccessToken };

            await redisService.Set(fidectusAuthenticationToken, 24, RedisConstants.FidectusAuthenticationTokenKey, requestTokenRequest.ClientId);

            return getAuthenticationToken;
        }
    }
}
