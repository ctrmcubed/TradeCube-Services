using Fidectus.Messages;
using Fidectus.Models;
using Fidectus.Services;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Helpers;
using Shared.Serialization;
using Shared.Services;
using System.Data;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public class FidectusManager : IFidectusManager
    {
        private readonly IFidectusAuthenticationService fidectusAuthenticationService;
        private readonly ITradeService tradeService;
        private readonly ITradeSummaryService tradeSummaryService;
        private readonly IProfileService profileService;
        private readonly ISettingService settingService;
        private readonly IVaultService vaultService;
        private readonly IFidectusMappingService fidectusMappingService;
        private readonly ILogger<FidectusManager> logger;

        public FidectusManager(IFidectusAuthenticationService fidectusAuthenticationService, IFidectusService fidectusService, ITradeService tradeService, ITradeSummaryService tradeSummaryService,
            IProfileService profileService, ISettingService settingService, IVaultService vaultService, IFidectusMappingService fidectusMappingService,
            ILogger<FidectusManager> logger)
        {
            this.fidectusAuthenticationService = fidectusAuthenticationService;
            this.tradeService = tradeService;
            this.tradeSummaryService = tradeSummaryService;
            this.profileService = profileService;
            this.settingService = settingService;
            this.vaultService = vaultService;
            this.fidectusMappingService = fidectusMappingService;
            this.logger = logger;
        }

        public async Task<TradeConfirmation> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            var mappingHelper = new MappingHelper(await fidectusMappingService.GetMappingsAsync(apiJwtToken));
            var settingsHelper = new SettingHelper((await settingService.GetSettingsViaJwtAsync(apiJwtToken))?.Data);
            var tradeSummary = (await tradeSummaryService.TradeSummaryAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.FirstOrDefault();
            var profileResponses = (await profileService.ProfileAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken, ProfileTradeConstants.ProfileFormatSparse))?.Data;

            logger.LogTrace($"Trade Summary: {TradeCubeJsonSerializer.Serialize(tradeSummary)}\r\n");
            logger.LogTrace($"Trade Profile: {TradeCubeJsonSerializer.Serialize(profileResponses)}\r\n");

            return await fidectusMappingService.MapConfirmation(tradeDataObject, tradeSummary, profileResponses, mappingHelper, settingsHelper, apiJwtToken);
        }

        public async Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.SingleOrDefault();
        }

        public async Task<RequestTokenResponse> CreateAuthenticationTokenAsync(RequestTokenRequest requestTokenRequest, string apiJwtToken)
        {
            return await fidectusAuthenticationService.GetAuthenticationToken(requestTokenRequest, new FidectusConfiguration(await GetFidectusDomainAsync(apiJwtToken)));
        }

        private async Task<RequestTokenRequest> CreateAuthenticationTokenRequestAsync(string apiJwtToken)
        {
            var fidectusClientId = (await vaultService.GetVaultValueAsync(VaultConstants.FidectusClientId, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;
            var fidectusClientSecret = (await vaultService.GetVaultValueAsync(VaultConstants.FidectusClientSecret, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;

            if (string.IsNullOrEmpty(fidectusClientId))
            {
                logger.LogError($"The {VaultConstants.FidectusClientId} is not configured in the vault");
                throw new SecurityException($"The {VaultConstants.FidectusClientSecret} is not configured in the vault");
            }

            if (string.IsNullOrEmpty(fidectusClientSecret))
            {
                logger.LogError($"The {VaultConstants.FidectusClientId} is not configured in the vault");
                throw new SecurityException($"The {VaultConstants.FidectusClientSecret} is not configured in the vault");
            }

            return new RequestTokenRequest(fidectusClientId, fidectusClientSecret);
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
