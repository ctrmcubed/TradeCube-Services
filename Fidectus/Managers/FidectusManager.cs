using Fidectus.Helpers;
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
using System.Collections.Generic;
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

        public FidectusManager(IFidectusAuthenticationService fidectusAuthenticationService, IFidectusService fidectusService, ITradeService tradeService,
            ITradeSummaryService tradeSummaryService, IProfileService profileService, ISettingService settingService, IVaultService vaultService,
            IFidectusMappingService fidectusMappingService, ILogger<FidectusManager> logger)
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

        public async Task<FidectusConfiguration> GetFidectusConfiguration(string apiJwtToken)
        {
            var mappingHelper = new MappingHelper(await fidectusMappingService.GetMappingsAsync(apiJwtToken));
            var settingHelper = new SettingHelper((await settingService.GetSettingsViaJwtAsync(apiJwtToken))?.Data);

            return new FidectusConfiguration(settingHelper, mappingHelper);
        }

        public async Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.SingleOrDefault();
        }

        public async Task<GetConfirmationResponse> GetConfirmationAsync(string tradeId, RequestTokenResponse requestTokenResponse, FidectusConfiguration fidectusConfiguration)
        {
            return await fidectusService.GetTradeConfirmation(fidectusConfiguration.CompanyId(), new List<string> { tradeId }, requestTokenResponse, fidectusConfiguration);
        }

        public Task<SendConfirmationResponse> SendConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            throw new NotImplementedException();
        }

        public async Task<SendConfirmationResponse> SendConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken, FidectusConfiguration fidectusConfiguration)
        {
            var tradeDataObject = await GetTradeAsync(tradeReference, tradeLeg, apiJwtToken);
            if (tradeDataObject is null || tradeDataObject.External.Confirmation.Withhold)
            {
                return new SendConfirmationResponse();
            }

            var requestTokenResponse = await CreateAuthenticationTokenAsync(apiJwtToken, fidectusConfiguration);
            var tradeId = fidectusMappingService.MapTradeReferenceToTradeId(tradeReference, tradeLeg);
            var getConfirmationResponse = await GetConfirmationAsync(tradeId, requestTokenResponse, fidectusConfiguration);

            return new SendConfirmationResponse();
        }

        public Task<SendConfirmationResponse> SendConfirmationAsync(TradeConfirmation tradeConfirmation, string apiJwtToken)
        {
            throw new NotImplementedException();
        }

        public async Task<(TradeConfirmation tradeConfirmation, ConfigurationHelper configurationHelper)> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            try
            {
                var (tradeConfirmation, configurationHelper) = await MapTradeConfirmationAsync(tradeDataObject, apiJwtToken);

                logger.LogDebug($"Trade Confirmation: {TradeCubeJsonSerializer.Serialize(tradeConfirmation)}");

                return (tradeConfirmation, configurationHelper);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<SendConfirmationResponse> SendConfirmationAsync(TradeConfirmation tradeConfirmation, string apiJwtToken, FidectusConfiguration fidectusConfiguration)
        {
            try
            {
                return await SendTradeConfirmation(tradeConfirmation, await CreateAuthenticationTokenAsync(apiJwtToken, fidectusConfiguration), fidectusConfiguration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<(TradeConfirmation tradeConfirmation, ConfigurationHelper configurationHelper)> MapTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            var mappingHelper = new MappingHelper(await fidectusMappingService.GetMappingsAsync(apiJwtToken));
            var settingHelper = new SettingHelper((await settingService.GetSettingsViaJwtAsync(apiJwtToken))?.Data);

            var tradeSummary = (await tradeSummaryService.TradeSummaryAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.FirstOrDefault();
            var profileResponses = (await profileService.ProfileAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken, ProfileTradeConstants.ProfileFormatSparse))?.Data;

            logger.LogTrace($"Trade Summary: {TradeCubeJsonSerializer.Serialize(tradeSummary)}\r\n");
            logger.LogTrace($"Trade Profile: {TradeCubeJsonSerializer.Serialize(profileResponses)}\r\n");

            var configurationHelper = new ConfigurationHelper(mappingHelper, settingHelper);

            return (await fidectusMappingService.MapConfirmation(tradeDataObject, tradeSummary, profileResponses, configurationHelper, apiJwtToken), configurationHelper);
        }

        private async Task<SendConfirmationResponse> SendTradeConfirmation(TradeConfirmation tradeConfirmation, RequestTokenResponse requestTokenResponse, FidectusConfiguration fidectusConfiguration)
        {
            var tradeConfirmationRequest = new TradeConfirmationRequest { TradeConfirmation = tradeConfirmation };

            return await fidectusService.SendTradeConfirmation(fidectusConfiguration.CompanyId(tradeConfirmation.SenderId), tradeConfirmationRequest, requestTokenResponse, fidectusConfiguration);
        }

        private async Task<RequestTokenResponse> CreateAuthenticationTokenAsync(string apiJwtToken, FidectusConfiguration fidectusConfiguration)
        {
            return await fidectusAuthenticationService.FidectusGetAuthenticationToken(await CreateAuthenticationTokenRequestAsync(apiJwtToken), fidectusConfiguration);
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
