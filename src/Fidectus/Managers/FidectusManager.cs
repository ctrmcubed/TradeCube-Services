using Fidectus.Constants;
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
using Shared.Serialization;
using Shared.Services;
using System;
using System.Collections.Generic;
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

        public async Task<ConfirmationResponse> ProcessConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            var tradeDataObject = await GetTradeAsync(tradeReference, tradeLeg, apiJwtToken);
            if (tradeDataObject is null || tradeDataObject.IsConfirmationWithheld())
            {
                return new ConfirmationResponse();
            }

            var confirmationDocumentId = tradeDataObject.ConfirmationDocumentId();

            if (string.IsNullOrWhiteSpace(confirmationDocumentId))
            {
                return new ConfirmationResponse { Message = "Trade has already been confirmed" };
            }

            var tradePreSubmission = SetPreSubmissionUpdates(tradeDataObject, confirmationDocumentId);
            var savePreSubmission = await SaveTradeAsync(tradePreSubmission, apiJwtToken);

            if (!savePreSubmission.IsSuccessStatusCode)
            {
                throw new DataException("Failed to set pre-submission");
            }

            var tradeConfirmation = await CreateTradeConfirmationAsync(tradeDataObject, apiJwtToken, fidectusConfiguration);

            logger.LogInformation($"Confirmation: {TradeCubeJsonSerializer.Serialize(tradeConfirmation)}");

            var method = string.IsNullOrWhiteSpace(confirmationDocumentId)
                ? "PUT"
                : "POST";

            var confirmationResponse = await SendConfirmationAsync(method, tradeConfirmation, apiJwtToken, fidectusConfiguration);

            logger.LogInformation($"Confirmation response: {confirmationResponse.StatusCode}");

            if (!confirmationResponse.IsSuccessStatusCode)
            {
                throw new DataException("Failed to send confirmation");
            }

            var tradePostSubmission = SetPostSubmissionUpdates(tradeDataObject, confirmationResponse);
            var savePostSubmission = await SaveTradeAsync(tradePostSubmission, apiJwtToken);

            if (!savePostSubmission.IsSuccessStatusCode)
            {
                throw new DataException("Failed to set post-submission");
            }

            return confirmationResponse;
        }

        public async IAsyncEnumerable<BoxResultResponse> BoxResults(IEnumerable<TradeKey> tradeKeys, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            var requestTokenResponse = await CreateAuthenticationTokenAsync(apiJwtToken, fidectusConfiguration);
            var companyId = fidectusConfiguration.CompanyId();

            foreach (var tradeKey in tradeKeys)
            {
                var tradeDataObject = await GetTradeAsync(tradeKey.TradeReference, tradeKey.TradeLeg, apiJwtToken);
                if (tradeDataObject is null)
                {
                    continue;
                }

                yield return await fidectusService.GetBoxResult(companyId, tradeDataObject.ConfirmationDocumentId(), requestTokenResponse, fidectusConfiguration);
            }
        }

        public async Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.SingleOrDefault();
        }

        public async Task<TradeConfirmation> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                var tradeConfirmation = await MapTradeConfirmationAsync(tradeDataObject, apiJwtToken, fidectusConfiguration);

                logger.LogDebug($"Trade Confirmation: {TradeCubeJsonSerializer.Serialize(tradeConfirmation)}");

                return tradeConfirmation;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private static TradeDataObject SetPreSubmissionUpdates(TradeDataObject tradeDataObject, string documentId)
        {
            // Mutation!
            tradeDataObject.External ??= new ExternalFieldsType();
            tradeDataObject.External.Confirmation ??= new ConfirmationType();
            tradeDataObject.External.Confirmation.SubmissionTime = DateTime.UtcNow;
            tradeDataObject.External.Confirmation.SubmissionStatus = string.IsNullOrWhiteSpace(documentId)
                ? FidectusConstants.SubmissionStatusSubmitted
                : FidectusConstants.SubmissionStatusResubmitted;

            tradeDataObject.External.Confirmation.ConfirmationProvider = ConfirmationConstants.ConfirmationProviderFidectus;

            return tradeDataObject;
        }

        private static TradeDataObject SetPostSubmissionUpdates(TradeDataObject tradeDataObject, ConfirmationResponse confirmationResponse)
        {
            // Mutation!
            tradeDataObject.External ??= new ExternalFieldsType();
            tradeDataObject.External.Confirmation ??= new ConfirmationType();
            tradeDataObject.External.Confirmation.SubmissionStatus = confirmationResponse.IsSuccessStatusCode
                ? "Success"
                : "Failed";

            tradeDataObject.External.Confirmation.SubmissionMessage = confirmationResponse.Message;

            return tradeDataObject;
        }

        private async Task<ConfirmationResponse> SendConfirmationAsync(string method, TradeConfirmation tradeConfirmation, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                return await SendTradeConfirmation(method, tradeConfirmation,
                    await CreateAuthenticationTokenAsync(apiJwtToken, fidectusConfiguration), fidectusConfiguration);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<ConfirmationResponse> SendTradeConfirmation(string method, TradeConfirmation tradeConfirmation,
            RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            return await fidectusService.SendTradeConfirmation(method, fidectusConfiguration.CompanyId(tradeConfirmation.SenderId),
                    new TradeConfirmationRequest { TradeConfirmation = tradeConfirmation }, requestTokenResponse, fidectusConfiguration);
        }

        private async Task<TradeConfirmation> MapTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken, IFidectusConfiguration fidectusConfiguration)
        {
            var tradeSummary = (await tradeSummaryService.TradeSummaryAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.FirstOrDefault();
            var profileResponses = (await profileService.ProfileAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken, ProfileTradeConstants.ProfileFormatSparse))?.Data;

            logger.LogTrace($"Trade Summary: {TradeCubeJsonSerializer.Serialize(tradeSummary)}\r\n");
            logger.LogTrace($"Trade Profile: {TradeCubeJsonSerializer.Serialize(profileResponses)}\r\n");

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

        private async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> SaveTradeAsync(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            return await tradeService.PutTradesViaJwtAsync(apiJwtToken, new List<TradeDataObject> { tradeDataObject });
        }
    }
}
