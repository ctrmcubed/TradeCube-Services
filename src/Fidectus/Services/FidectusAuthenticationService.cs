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
        private readonly ILogger<ApiService> logger;

        public FidectusAuthenticationService(IHttpClientFactory httpClientFactory, IRedisService redisService, ILogger<FidectusAuthenticationService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.redisService = redisService;
            this.logger = logger;
        }

        public async Task<RequestTokenResponse> FidectusGetAuthenticationToken(RequestTokenRequest requestTokenRequest, IFidectusConfiguration fidectusConfiguration)
        {
            var requestTokenResponse = await redisService.Get<FidectusAuthenticationToken>(RedisConstants.FidectusAuthenticationTokenKey, requestTokenRequest.ClientId);

            if (!string.IsNullOrWhiteSpace(requestTokenResponse?.AccessToken))
            {
                logger.LogInformation("Reusing cached Fidectus access token...");

                return new RequestTokenResponse
                {
                    AccessToken = requestTokenResponse.AccessToken,
                    IsSuccessStatusCode = true
                };
            }

            logger.LogInformation("Getting new Fidectus access token...");

            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusAuthUrl);

            var getAuthenticationToken = await PostAsJsonAsync<RequestTokenRequest, RequestTokenResponse>(httpClient, fidectusConfiguration.FidectusAuthUrl, requestTokenRequest);
            var fidectusAuthenticationToken = new FidectusAuthenticationToken { AccessToken = getAuthenticationToken.AccessToken };

            await redisService.Set(fidectusAuthenticationToken, 24, RedisConstants.FidectusAuthenticationTokenKey, requestTokenRequest.ClientId);

            logger.LogInformation("New Fidectus access token cached");

            return getAuthenticationToken;
        }
    }
}
