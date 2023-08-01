using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class VaultService : TradeCubeApiService, IVaultService
    {
        public VaultService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<VaultService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<VaultDataObject>>> GetVaultValueAsync(string vaultKey, string jwtApiToken)
        {
            return await GetViaJwtAsync<VaultDataObject>($"Vault/{vaultKey}", jwtApiToken);
        }

        public async Task<ApiResponseWrapper<IEnumerable<VaultDataObject>>> GetVaultValueViaApiKeyAsync(string vaultKey, string apiKey)
        {
            return await GetViaApiKeyAsync<VaultDataObject>($"Vault/{vaultKey}", apiKey);
        }
    }
}