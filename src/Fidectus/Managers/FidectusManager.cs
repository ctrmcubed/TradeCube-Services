using Fidectus.Messages;
using Fidectus.Models;
using Fidectus.Services;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Messages;
using Shared.Services;
using System;
using System.Data;
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

        public async Task<ConfirmationResponse> ConfirmAsync(TradeKey tradeKey, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            var tradeDataObject = await GetTradeAsync(tradeKey.TradeReference, tradeKey.TradeLeg, apiJwtToken);
            if (tradeDataObject is null || tradeDataObject.IsConfirmationWithheld())
            {
                return new ConfirmationResponse { IsSuccessStatusCode = true };
            }

            var tradeConfirmation = await CreateTradeConfirmationAsync(tradeDataObject, apiJwtToken, fidectusConfiguration);

            var method = string.IsNullOrWhiteSpace(tradeDataObject.ConfirmationDocumentId())
                ? "POST"
                : "PUT";

            var confirmationResponse = await SendConfirmationAsync(method, tradeConfirmation, apiJwtToken, fidectusConfiguration);

            if (confirmationResponse.IsSuccessStatusCode)
            {
                return confirmationResponse;
            }

            if (confirmationResponse.StatusCode == 409)
            {
                logger.LogInformation($"Already sent ({confirmationResponse.StatusCode}), trying a PUT...");

                return await SendConfirmationAsync("PUT", tradeConfirmation, apiJwtToken, fidectusConfiguration);
            }

            throw new DataException(string.IsNullOrWhiteSpace(confirmationResponse.Message)
                ? "Failed to send confirmation"
                : $"Failed to send confirmation ({confirmationResponse.Message})");
        }

        public async Task<ConfirmationResponse> CancelAsync(TradeKey tradeKey, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            var tradeDataObject = await GetTradeAsync(tradeKey.TradeReference, tradeKey.TradeLeg, apiJwtToken);
            if (tradeDataObject is null || tradeDataObject.IsConfirmationWithheld())
            {
                return new ConfirmationResponse();
            }

            if (string.IsNullOrEmpty(tradeDataObject.ConfirmationDocumentId()))
            {
                return new ConfirmationResponse
                {
                    IsSuccessStatusCode = false,
                    Message = "Cannot cancel confirmation, confirmation has yet to be submitted"
                };
            }

            return await DeleteConfirmationAsync(tradeDataObject, await CreateTradeConfirmationAsync(tradeDataObject, apiJwtToken, fidectusConfiguration), apiJwtToken, fidectusConfiguration);
        }

        public async Task<ConfirmationResultResponse> BoxResult(TradeKey tradeKey, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            var tradeDataObject = await GetTradeAsync(tradeKey.TradeReference, tradeKey.TradeLeg, apiJwtToken);
            if (tradeDataObject is null)
            {
                return new ConfirmationResultResponse();
            }

            var requestTokenResponse = await CreateAuthenticationTokenAsync(apiJwtToken, fidectusConfiguration);
            var companyId = fidectusConfiguration.CompanyId();
            var boxResultResponse = await fidectusService.GetBoxResult(companyId, tradeDataObject.ConfirmationDocumentId(), requestTokenResponse, fidectusConfiguration);

            return new ConfirmationResultResponse
            {
                Id = boxResultResponse.Id,
                DocumentId = boxResultResponse.BoxResult?.DocumentId,
                DocumentVersion = boxResultResponse.BoxResult?.DocumentVersion,
                DocumentType = boxResultResponse.BoxResult?.DocumentType,
                State = boxResultResponse.BoxResult?.State,
                Timestamp = boxResultResponse.BoxResult?.Timestamp,
                CounterpartyDocumentId = boxResultResponse.BoxResult?.Counterparty?.DocumentId,
                CounterpartyDocumentVersion = boxResultResponse.BoxResult?.Counterparty?.DocumentVersion
            };
        }

        public async Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.SingleOrDefault();
        }

        public async Task<TradeConfirmation> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                return await MapTradeConfirmationAsync(tradeDataObject, apiJwtToken, fidectusConfiguration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<ConfirmationResponse> SendConfirmationAsync(string method, TradeConfirmation tradeConfirmation, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                return await fidectusService.SendConfirmation(method, fidectusConfiguration.CompanyId(tradeConfirmation.SenderId),
                    new ConfirmationRequest { TradeConfirmation = tradeConfirmation },
                    await CreateAuthenticationTokenAsync(apiJwtToken, fidectusConfiguration), fidectusConfiguration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<ConfirmationResponse> DeleteConfirmationAsync(TradeDataObject tradeDataObject, TradeConfirmation tradeConfirmation, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                return await fidectusService.DeleteConfirmation(fidectusConfiguration.CompanyId(tradeConfirmation.SenderId),
                    tradeDataObject.ConfirmationDocumentId(), await CreateAuthenticationTokenAsync(apiJwtToken, fidectusConfiguration), fidectusConfiguration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<TradeConfirmation> MapTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            var tradeSummary = (await tradeSummaryService.TradeSummaryAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.FirstOrDefault();
            var profileResponses = (await profileService.ProfileAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken, ProfileTradeConstants.ProfileFormatSparse))?.Data;

            return await fidectusMappingService.MapConfirmation(tradeDataObject, tradeSummary, profileResponses, apiJwtToken, fidectusConfiguration);
        }

        private async Task<RequestTokenResponse> CreateAuthenticationTokenAsync(string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            return await fidectusAuthenticationService.FidectusGetAuthenticationToken(await CreateAuthenticationTokenRequestAsync(apiJwtToken), fidectusConfiguration);
        }

        private async Task<RequestTokenRequest> CreateAuthenticationTokenRequestAsync(string apiJwtToken)
        {
            var fidectusClientId = (await vaultService.GetVaultValueAsync(VaultConstants.FidectusClientId, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;
            var fidectusClientSecret = (await vaultService.GetVaultValueAsync(VaultConstants.FidectusClientSecret, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;

            if (string.IsNullOrWhiteSpace(fidectusClientId))
            {
                logger.LogError($"The {VaultConstants.FidectusClientId} is not configured in the vault");
                throw new SecurityException($"The {VaultConstants.FidectusClientId} is not configured in the vault");
            }

            if (string.IsNullOrEmpty(fidectusClientSecret))
            {
                logger.LogError($"The {VaultConstants.FidectusClientSecret} is not configured in the vault");
                throw new SecurityException($"The {VaultConstants.FidectusClientSecret} is not configured in the vault");
            }

            var fidectusAudience = (await settingService.GetSettingViaJwtAsync("FIDECTUS_AUDIENCE", apiJwtToken))?.Data?.SingleOrDefault()?.SettingValue;

            return new RequestTokenRequest(fidectusClientId, fidectusClientSecret, fidectusAudience);
        }
    }
}
