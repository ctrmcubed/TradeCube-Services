using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Fidectus.Messages;
using Fidectus.Services;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Services;

namespace Fidectus.Managers
{
    public class FidectusManager : IFidectusManager
    {
        private readonly IFidectusAuthenticationService fidectusAuthenticationService;
        private readonly ISettingService settingService;
        private readonly IVaultService vaultService;
        private readonly ILogger<FidectusManager> logger;

        public FidectusManager(IFidectusAuthenticationService fidectusAuthenticationService, IFidectusService fidectusService,
            ISettingService settingService, IVaultService vaultService, ILogger<FidectusManager> logger)
        {
            this.fidectusAuthenticationService = fidectusAuthenticationService;
            this.settingService = settingService;
            this.vaultService = vaultService;
            this.logger = logger;
        }

        public async Task<RequestTokenResponse> CreateAuthenticationTokenAsync(RequestTokenRequest requestTokenRequest, string apiJwtToken)
        {
            return await fidectusAuthenticationService.GetAuthenticationToken(requestTokenRequest, new FidectusConfiguration(await GetFidectusDomainAsync(apiJwtToken)));
        }

        private async Task<string> GetFidectusDomainAsync(string apiJwtToken)
        {
            const string fidectusUrl = "FIDECTUS_URL";

            var apiDomain = (await settingService.GetSettingViaJwtAsync(fidectusUrl, apiJwtToken))?.Data?.SingleOrDefault()?.SettingValue;

            return string.IsNullOrEmpty(apiDomain)
                ? throw new DataException($"The {fidectusUrl} is not configured in the system settings")
                : apiDomain;
        }
    }
}
