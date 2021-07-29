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
using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public class FidectusManager : IFidectusManager
    {
        private readonly IFidectusAuthenticationService fidectusAuthenticationService;
        private readonly IFidectusService fidectusService;
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
            this.fidectusService = fidectusService;
            this.tradeService = tradeService;
            this.tradeSummaryService = tradeSummaryService;
            this.profileService = profileService;
            this.settingService = settingService;
            this.vaultService = vaultService;
            this.fidectusMappingService = fidectusMappingService;
            this.logger = logger;
        }

        public async Task<(TradeConfirmation tradeConfirmation, SettingHelper settingHelper)> CreateTradeConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            try
            {
                var tradeDataObject = await GetTradeAsync(tradeReference, tradeLeg, apiJwtToken);
                var tradeConfirmation = await MapTradeConfirmationAsync(tradeDataObject, apiJwtToken);

                logger.LogDebug($"Trade Confirmation: {TradeCubeJsonSerializer.Serialize(tradeConfirmation)}");

                return tradeConfirmation;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<TradeConfirmationResponse> SendTradeConfirmationAsync(TradeConfirmation tradeConfirmation, string apiJwtToken, SettingHelper settingHelper)
        {
            try
            {
                var requestTokenResponse = await CreateAuthenticationTokenAsync(apiJwtToken, settingHelper);

                return await SendTradeConfirmation(tradeConfirmation, requestTokenResponse, settingHelper);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<(TradeConfirmation tradeConfirmation, SettingHelper settingHelper)> MapTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            var mappingHelper = new MappingHelper(await fidectusMappingService.GetMappingsAsync(apiJwtToken));
            var settingsHelper = new SettingHelper((await settingService.GetSettingsViaJwtAsync(apiJwtToken))?.Data);

            var tradeSummary = (await tradeSummaryService.TradeSummaryAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.FirstOrDefault();
            var profileResponses = (await profileService.ProfileAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken, ProfileTradeConstants.ProfileFormatSparse))?.Data;

            logger.LogTrace($"Trade Summary: {TradeCubeJsonSerializer.Serialize(tradeSummary)}\r\n");
            logger.LogTrace($"Trade Profile: {TradeCubeJsonSerializer.Serialize(profileResponses)}\r\n");

            return (await fidectusMappingService.MapConfirmation(tradeDataObject, tradeSummary, profileResponses, mappingHelper, settingsHelper, apiJwtToken), settingsHelper);
        }

        private async Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.SingleOrDefault();
        }

        private async Task<TradeConfirmationResponse> SendTradeConfirmation(TradeConfirmation tradeConfirmation, RequestTokenResponse requestTokenResponse, SettingHelper settingHelper)
        {
            var fidectusConfiguration = new FidectusConfiguration(settingHelper.GetSetting("FIDECTUS_URL"), settingHelper.GetSetting("FIDECTUS_AUTH_URL"), settingHelper.GetSetting("FIDECTUS_AUDIENCE"));
            var tradeConfirmationRequest = new TradeConfirmationRequest {TradeConfirmation = tradeConfirmation};

            return await fidectusService.SendTradeConfirmation(tradeConfirmationRequest, requestTokenResponse, fidectusConfiguration);
        }

        private async Task<RequestTokenResponse> CreateAuthenticationTokenAsync(string apiJwtToken, SettingHelper settingHelper)
        {
            var requestTokenRequest = await CreateAuthenticationTokenRequestAsync(apiJwtToken);
            var fidectusConfiguration = new FidectusConfiguration(settingHelper.GetSetting("FIDECTUS_URL"), settingHelper.GetSetting("FIDECTUS_AUTH_URL"), settingHelper.GetSetting("FIDECTUS_AUDIENCE"));

            return await fidectusAuthenticationService.GetAuthenticationToken(requestTokenRequest, fidectusConfiguration);
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

            var fidectusAudience = (await settingService.GetSettingViaJwtAsync("FIDECTUS_AUDIENCE", apiJwtToken))?.Data?.SingleOrDefault()?.SettingValue;

            return new RequestTokenRequest(fidectusClientId, fidectusClientSecret, fidectusAudience);
        }
    }
}
