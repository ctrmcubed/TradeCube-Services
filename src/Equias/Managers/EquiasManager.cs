﻿using Equias.Constants;
using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Equias.Services;
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

namespace Equias.Managers
{
    public class EquiasManager : IEquiasManager
    {
        private readonly IEquiasAuthenticationService equiasAuthenticationService;
        private readonly IEquiasService equiasService;
        private readonly ITradeService tradeService;
        private readonly IEquiasMappingService equiasMappingService;
        private readonly IVaultService vaultService;
        private readonly ISettingService settingService;
        private readonly ILogger<EquiasManager> logger;
        private readonly ITradeSummaryService tradeSummaryService;
        private readonly ICashflowService cashflowService;
        private readonly IProfileService profileService;

        public EquiasManager(IEquiasAuthenticationService equiasAuthenticationService, IEquiasService equiasService, ITradeService tradeService, ITradeSummaryService tradeSummaryService,
            ICashflowService cashflowService, IProfileService profileService, ISettingService settingService, IVaultService vaultService, IEquiasMappingService equiasMappingService,
            ILogger<EquiasManager> logger)
        {
            this.equiasAuthenticationService = equiasAuthenticationService;
            this.equiasService = equiasService;
            this.tradeService = tradeService;
            this.equiasMappingService = equiasMappingService;
            this.vaultService = vaultService;
            this.settingService = settingService;
            this.logger = logger;
            this.tradeSummaryService = tradeSummaryService;
            this.cashflowService = cashflowService;
            this.profileService = profileService;
        }

        public async Task<EboGetTradeStatusResponse> TradeStatusAsync(IEnumerable<TradeKey> tradeKeys, string apiJwtToken)
        {
            var enumerable = tradeKeys.ToList();
            var tradeIds = enumerable.Select(t => EquiasService.MapTradeId(t.TradeReference, t.TradeLeg)).ToList();
            var equiasConfiguration = new EquiasConfiguration(await GetEquiasDomainAsync(apiJwtToken));
            var requestTokenResponse = await CreateAuthenticationTokenAsync(apiJwtToken);

            logger.LogInformation("Attempting to get trade status for trades {TradeIds}", TradeCubeJsonSerializer.Serialize(tradeIds));

            return tradeIds.Any()
                ? await equiasService.EboGetTradeStatus(tradeIds, requestTokenResponse, equiasConfiguration)
                : new EboGetTradeStatusResponse();
        }

        public async Task<EboTradeResponse> CreatePhysicalTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            async Task<EboTradeResponse> Withhold(string jwt, TradeDataObject tradeDataObject)
            {
                tradeDataObject.External.Equias.EboSubmissionStatus = EquiasConstants.StatusWithheld;

                var saveTradeWithheld = await SaveTradeAsync(tradeDataObject, jwt);

                logger.LogInformation("Withheld Trade updated (EboSubmissionStatus={StatusWithheld}), result: {Status}",
                    EquiasConstants.StatusWithheld,
                    saveTradeWithheld.Status);

                return new EboTradeResponse();
            }

            async Task<EboTradeResponse> NewTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, TradeDataObject updateTradePreSubmission)
            {
                var eboAddPhysicalTradeResponse = await AddPhysicalTradeAsync(physicalTrade, requestTokenResponse, apiJwtToken);

                if (!eboAddPhysicalTradeResponse.IsSuccess())
                {
                    throw new DataException($"Add physical Trade failed result: {eboAddPhysicalTradeResponse.Message}");
                }

                logger.LogInformation("AddPhysicalTrade success response, TradeId: {TradeId}, TradeVersion: {TradeVersion}",
                    eboAddPhysicalTradeResponse.TradeId,
                    eboAddPhysicalTradeResponse.TradeVersion);

                var addTradePostSubmission = SetTradePostSubmission(eboAddPhysicalTradeResponse, updateTradePreSubmission);
                var savePostSubmissionAdd = await SaveTradeAsync(addTradePostSubmission, apiJwtToken);

                logger.LogInformation("Add physical Trade updated (EboSubmissionStatus={EboSubmissionStatus}), result: {Status}",
                    addTradePostSubmission.External.Equias.EboSubmissionStatus,
                    savePostSubmissionAdd.Status);

                return eboAddPhysicalTradeResponse;
            }

            async Task<EboTradeResponse> ExistingTrade(PhysicalTrade physicalTrade, TradeDataObject tradeDataObject, RequestTokenResponse requestTokenResponse)
            {
                var eboModifyPhysicalTradeResponse = await ModifyPhysicalTradeAsync(physicalTrade, requestTokenResponse, apiJwtToken);
                if (!eboModifyPhysicalTradeResponse.IsSuccess())
                {
                    throw new DataException($"Modify physical Trade failed result: {eboModifyPhysicalTradeResponse.Message}");
                }

                logger.LogInformation("ModifyPhysicalTrade success response, TradeId: {TradeId}, TradeVersion: {TradeVersion}",
                    eboModifyPhysicalTradeResponse.TradeId,
                    eboModifyPhysicalTradeResponse.TradeVersion);

                var modifyTradePostSubmission = SetTradePostSubmission(eboModifyPhysicalTradeResponse, tradeDataObject);
                var savePostSubmissionModify = await SaveTradeAsync(modifyTradePostSubmission, apiJwtToken);

                logger.LogInformation("Modify physical Trade updated (EboSubmissionStatus={EboSubmissionStatus}), result: {Status}",
                    modifyTradePostSubmission.External.Equias.EboSubmissionStatus,
                    savePostSubmissionModify.Status);

                return eboModifyPhysicalTradeResponse;
            }

            try
            {
                var requestTokenResponse = await CreateAuthenticationTokenAsync(apiJwtToken);
                var tradeDataObject = await GetTradeAsync(tradeReference, tradeLeg, apiJwtToken);

                if (tradeDataObject.WithholdEquiasSubmission())
                {
                    return await Withhold(apiJwtToken, tradeDataObject);
                }

                var tradeIds = new List<TradeKey> { new() { TradeReference = tradeReference, TradeLeg = tradeLeg } };
                var eboGetTradeStatusResponse = await TradeStatusAsync(tradeIds, apiJwtToken);

                if (string.IsNullOrWhiteSpace(tradeDataObject.EboActionType()))
                {
                    if (tradeDataObject.EboStatus() == EquiasConstants.StatusMatched || eboGetTradeStatusResponse.Status == EquiasConstants.StatusMatched)
                    {
                        return new EboTradeResponse
                        {
                            Status = ApiConstants.FailedWithWarningResult,
                            Message = "Confirmation already matched. To resubmit, amend the Action Type"
                        };
                    }
                }

                var updateTradePreSubmission = SetTradePreSubmission(eboGetTradeStatusResponse, tradeDataObject);
                var savePreSubmission = await SaveTradeAsync(updateTradePreSubmission, apiJwtToken);

                logger.LogInformation("Pre-submission Trade updated (EboSubmissionStatus={EboSubmissionStatus}), result: {Status}",
                    updateTradePreSubmission.External.Equias.EboSubmissionStatus,
                    savePreSubmission.Status);

                var physicalTrade = await CreatePhysicalTradeAsync(updateTradePreSubmission, apiJwtToken);

                logger.LogDebug("Physical Trade: {PhysicalTrade)}",TradeCubeJsonSerializer.Serialize(physicalTrade));

                return eboGetTradeStatusResponse.States.SingleOrDefault()?.TradeVersion is null
                    ? await NewTrade(physicalTrade, requestTokenResponse, updateTradePreSubmission)
                    : await ExistingTrade(physicalTrade, tradeDataObject, requestTokenResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                throw;
            }
        }

        public async Task<PhysicalTrade> CreatePhysicalTradeAsync(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            var mappingHelper = new MappingHelper(await equiasMappingService.GetMappingsAsync(apiJwtToken));
            var tradeSummary = (await tradeSummaryService.GetTradeSummaryAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.FirstOrDefault();
            var cashflows = (await cashflowService.CashflowAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data;
            var profileResponses = (await profileService.GetProfileAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken, ProfileTradeConstants.ProfileFormatSparse))?.Data;

            logger.LogTrace("Trade Summary: {TradeSummary}\r\n", TradeCubeJsonSerializer.Serialize(tradeSummary));
            logger.LogTrace("Trade Cashflows: {TradeCashflows}\r\n", TradeCubeJsonSerializer.Serialize(cashflows));
            logger.LogTrace("Trade Profile: {TradeProfile}\r\n", TradeCubeJsonSerializer.Serialize(profileResponses));

            return await equiasMappingService.MapTrade(tradeDataObject, tradeSummary, cashflows, profileResponses, mappingHelper, apiJwtToken);
        }

        public async Task<EboTradeResponse> AddPhysicalTradeAsync(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, string apiJwtToken)
        {
            return await equiasService.EboAddPhysicalTrade(physicalTrade, requestTokenResponse, new EquiasConfiguration(await GetEquiasDomainAsync(apiJwtToken)));
        }

        public async Task<EboTradeResponse> CancelTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            var equiasConfiguration = new EquiasConfiguration(await GetEquiasDomainAsync(apiJwtToken));
            var requestTokenResponse = await CreateAuthenticationTokenAsync(apiJwtToken);
            var cancelTrade = new CancelTrade { TradeId = EquiasService.MapTradeId(tradeReference, tradeLeg) };
            var eboTradeResponse = await equiasService.CancelTrade(cancelTrade, requestTokenResponse, equiasConfiguration);
            var tradeDataObject = await GetTradeAsync(tradeReference, tradeLeg, apiJwtToken);
            var addTradePostSubmission = SetTradePostSubmission(eboTradeResponse, tradeDataObject);
            var savePostSubmissionAdd = await SaveTradeAsync(addTradePostSubmission, apiJwtToken);

            logger.LogInformation("Cancel Trade updated (EboSubmissionStatus={EboSubmissionStatus}), result: {Status}",
                tradeDataObject.External.Equias.EboSubmissionStatus,
                savePostSubmissionAdd.Status);

            return eboTradeResponse;
        }

        public async Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.SingleOrDefault();
        }

        public async Task<RequestTokenResponse> CreateAuthenticationTokenAsync(RequestTokenRequest requestTokenRequest, string apiJwtToken)
        {
            return await equiasAuthenticationService.EboGetAuthenticationToken(requestTokenRequest, new EquiasConfiguration(await GetEquiasDomainAsync(apiJwtToken)));
        }

        private async Task<RequestTokenResponse> CreateAuthenticationTokenAsync(string apiJwtToken)
        {
            var requestTokenRequest = await CreateAuthenticationTokenRequestAsync(apiJwtToken);
            var equiasConfiguration = new EquiasConfiguration(await GetEquiasDomainAsync(apiJwtToken));

            return await equiasAuthenticationService.EboGetAuthenticationToken(requestTokenRequest, equiasConfiguration);
        }

        private async Task<RequestTokenRequest> CreateAuthenticationTokenRequestAsync(string apiJwtToken)
        {
            var eboUsername = (await vaultService.GetVaultValueAsync(VaultConstants.EquiasEboUsernameKey, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;
            var eboPassword = (await vaultService.GetVaultValueAsync(VaultConstants.EquiasEboPasswordKey, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;

            if (string.IsNullOrWhiteSpace(eboUsername))
            {
                logger.LogError("The {EquiasEboUsernameKey} is not configured in the vault", VaultConstants.EquiasEboUsernameKey);
                throw new SecurityException($"The {VaultConstants.EquiasEboUsernameKey} is not configured in the vault");
            }

            if (string.IsNullOrWhiteSpace(eboPassword))
            {
                logger.LogError("The {EquiasEboPasswordKey} is not configured in the vault", VaultConstants.EquiasEboPasswordKey);
                throw new SecurityException($"The {VaultConstants.EquiasEboPasswordKey} is not configured in the vault");
            }

            return new RequestTokenRequest(eboUsername, eboPassword);
        }

        private async Task<EboTradeResponse> ModifyPhysicalTradeAsync(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, string apiJwtToken)
        {
            return await equiasService.ModifyPhysicalTrade(physicalTrade, requestTokenResponse, new EquiasConfiguration(await GetEquiasDomainAsync(apiJwtToken)));
        }

        private async Task<string> GetEquiasDomainAsync(string apiJwtToken)
        {
            var apiDomain = (await settingService.GetSettingAsync(SettingConstants.EboUrlSetting, apiJwtToken))?.Data?.SingleOrDefault()?.SettingValue;

            return string.IsNullOrWhiteSpace(apiDomain)
                ? throw new DataException("EBO_URL is not configured in the system settings")
                : apiDomain;
        }

        private static TradeDataObject SetTradePreSubmission(EboGetTradeStatusResponse eboGetTradeStatusResponse, TradeDataObject tradeDataObject)
        {
            if (eboGetTradeStatusResponse is null)
            {
                throw new ArgumentNullException(nameof(eboGetTradeStatusResponse));
            }

            if (tradeDataObject is null)
            {
                throw new ArgumentNullException(nameof(tradeDataObject));
            }

            var getTradeStatus = eboGetTradeStatusResponse.States.SingleOrDefault();

            // Mutation!
            tradeDataObject.External ??= new ExternalFieldsType();
            tradeDataObject.External.Equias ??= new EquiasType();

            tradeDataObject.External.Equias.EboTradeId = eboGetTradeStatusResponse.States.SingleOrDefault()?.TradeId;
            tradeDataObject.External.Equias.EboSubmissionTime = DateTime.UtcNow;
            tradeDataObject.External.Equias.EboSubmissionStatus = getTradeStatus?.TradeVersion is null
                ? EquiasConstants.StatusSubmitted
                : EquiasConstants.StatusResubmitted;

            return tradeDataObject;
        }

        private static TradeDataObject SetTradePostSubmission(EboTradeResponse eboAddTradeResponse, TradeDataObject tradeDataObject)
        {
            if (eboAddTradeResponse is null)
            {
                throw new ArgumentNullException(nameof(eboAddTradeResponse));
            }

            if (tradeDataObject is null)
            {
                throw new ArgumentNullException(nameof(tradeDataObject));
            }

            // Mutation!
            tradeDataObject.External ??= new ExternalFieldsType();
            tradeDataObject.External.Equias ??= new EquiasType();

            tradeDataObject.External.Equias.EboTradeId = eboAddTradeResponse.TradeId;
            tradeDataObject.External.Equias.EboTradeVersion = eboAddTradeResponse.TradeVersion;
            tradeDataObject.External.Equias.EboSubmissionStatus = eboAddTradeResponse.IsSuccess()
                ? ApiConstants.SuccessResult
                : ApiConstants.FailedResult;

            tradeDataObject.External.Equias.EboSubmissionMessage = eboAddTradeResponse.Message;
            tradeDataObject.External.Equias.EboStatusLastCheckedTime = DateTime.UtcNow;

            return tradeDataObject;
        }

        private async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> SaveTradeAsync(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            return await tradeService.PutTradesViaJwtAsync(apiJwtToken, new List<TradeDataObject> { tradeDataObject });
        }
    }
}
