using Equias.Constants;
using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Equias.Services;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Managers;
using Shared.Messages;
using Shared.Services;
using System;
using System.Collections.Generic;
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
        private readonly ILogger<EquiasManager> logger;
        private readonly ITradeSummaryService tradeSummaryService;
        private readonly ICashflowService cashflowService;
        private readonly IProfileService profileService;

        public EquiasManager(IEquiasAuthenticationService equiasAuthenticationService, IEquiasService equiasService, ITradeService tradeService, ITradeSummaryService tradeSummaryService,
            ICashflowService cashflowService, IProfileService profileService, IEquiasMappingService equiasMappingService, IVaultService vaultService, ILogger<EquiasManager> logger)
        {
            this.equiasAuthenticationService = equiasAuthenticationService;
            this.equiasService = equiasService;
            this.tradeService = tradeService;
            this.equiasMappingService = equiasMappingService;
            this.vaultService = vaultService;
            this.logger = logger;
            this.tradeSummaryService = tradeSummaryService;
            this.cashflowService = cashflowService;
            this.profileService = profileService;
        }

        public async Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest)
        {
            return await equiasAuthenticationService.GetAuthenticationToken(requestTokenRequest);
        }

        public async Task<EboGetTradeStatusResponse> EboGetTradeStatus(IEnumerable<string> tradeIds, RequestTokenResponse requestTokenResponse)
        {
            return await equiasService.EboGetTradeStatus(tradeIds, requestTokenResponse);
        }

        public async Task<RequestTokenRequest> GetAuthenticationToken(string apiJwtToken)
        {
            var eboUsername = (await vaultService.GetVaultValueAsync(VaultConstants.EquiasEboUsernameKey, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;
            var eboPassword = (await vaultService.GetVaultValueAsync(VaultConstants.EquiasEboPasswordKey, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;

            if (string.IsNullOrEmpty(eboUsername) || string.IsNullOrEmpty(eboPassword))
            {
                logger.LogError("Equias eBO username/password not found in the vault");
                throw new SecurityException("Equias eBO username/password not found in the vault");
            }

            return new RequestTokenRequest(eboUsername, eboPassword);
        }

        public async Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.SingleOrDefault();
        }

        public async Task<PhysicalTrade> CreatePhysicalTrade(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            var mappingManager = new MappingManager(await equiasMappingService.GetMappingsAsync(apiJwtToken));
            var mappingService = equiasMappingService.SetMappingManager(mappingManager);
            var tradeSummary = (await tradeSummaryService.TradeSummaryAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.First();
            var cashflows = (await cashflowService.CashflowAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data;
            var profileResponses = (await profileService.ProfileAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken, "sparse"))?.Data;

            return await mappingService.MapTrade(tradeDataObject, tradeSummary, cashflows, profileResponses, apiJwtToken);
        }

        public async Task<EboPhysicalTradeResponse> AddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse)
        {
            return await equiasService.EboAddPhysicalTrade(physicalTrade, requestTokenResponse);
        }

        public async Task<EboPhysicalTradeResponse> ModifyPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse)
        {
            return await equiasService.ModifyPhysicalTrade(physicalTrade, requestTokenResponse);
        }

        public TradeDataObject SetTradePreSubmission(EboGetTradeStatusResponse eboGetTradeStatusResponse, TradeDataObject tradeDataObject)
        {
            if (eboGetTradeStatusResponse == null)
            {
                throw new ArgumentNullException(nameof(eboGetTradeStatusResponse));
            }

            var getTradeStatus = eboGetTradeStatusResponse.States.SingleOrDefault();

            tradeDataObject.External.Equias.EboSubmissionTime = DateTime.UtcNow;
            tradeDataObject.External.Equias.EboSubmissionStatus = getTradeStatus?.TradeVersion == null
                ? EquiasConstants.StatusSubmitted
                : EquiasConstants.StatusResubmitted;

            return tradeDataObject;
        }

        public TradeDataObject SetTradePostSubmission(EboPhysicalTradeResponse eboAddPhysicalTradeResponse, TradeDataObject tradeDataObject)
        {
            if (eboAddPhysicalTradeResponse == null)
            {
                throw new ArgumentNullException(nameof(eboAddPhysicalTradeResponse));
            }

            // Mutation!
            tradeDataObject.External.Equias.EboTradeId = eboAddPhysicalTradeResponse.TradeId;
            tradeDataObject.External.Equias.EboTradeVersion = eboAddPhysicalTradeResponse.TradeVersion;
            tradeDataObject.External.Equias.EboSubmissionStatus = eboAddPhysicalTradeResponse.IsSuccessStatusCode
                ? ApiConstants.SuccessResult
                : ApiConstants.FailedResult;

            tradeDataObject.External.Equias.EboSubmissionMessage = eboAddPhysicalTradeResponse.IsSuccessStatusCode
                ? null
                : eboAddPhysicalTradeResponse.Message;

            tradeDataObject.External.Equias.EboStatusLastCheckedTime = DateTime.UtcNow;

            return tradeDataObject;
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> SaveTrade(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            return await tradeService.PutTradesViaJwtAsync(apiJwtToken, new List<TradeDataObject> {tradeDataObject});
        }
    }
}
