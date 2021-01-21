using Equias.Messages;
using Equias.Services;
using Shared.Managers;
using Shared.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Equias.Managers
{
    public class EquiasManager : IEquiasManager
    {
        private readonly IEquiasAuthenticationService equiasAuthenticationService;
        private readonly IEquiasService equiasService;
        private readonly ITradeService tradeService;
        private readonly IEquiasMappingService equiasMappingService;
        private readonly ITradeSummaryService tradeSummaryService;
        private readonly ICashflowService cashflowService;
        private readonly IProfileService profileService;

        public EquiasManager(IEquiasAuthenticationService equiasAuthenticationService, IEquiasService equiasService, ITradeService tradeService, IEquiasMappingService equiasMappingService,
            ITradeSummaryService tradeSummaryService, ICashflowService cashflowService, IProfileService profileService)
        {
            this.equiasAuthenticationService = equiasAuthenticationService;
            this.equiasService = equiasService;
            this.tradeService = tradeService;
            this.equiasMappingService = equiasMappingService;
            this.tradeSummaryService = tradeSummaryService;
            this.cashflowService = cashflowService;
            this.profileService = profileService;
        }

        public async Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest)
        {
            return await equiasAuthenticationService.GetAuthenticationToken(requestTokenRequest);
        }

        public async Task<AddPhysicalTradeResponse> AddPhysicalTrade(string tradeReference, int tradeLeg, RequestTokenResponse requestTokenResponse, string apiJwtToken)
        {
            var tradeDataObject = (await tradeService.GetTradeAsync(apiJwtToken, tradeReference, tradeLeg)).Data?.First();
            var mappingManager = new MappingManager(await equiasMappingService.GetMappingsAsync(apiJwtToken));
            var mappingService = equiasMappingService.SetMappingManager(mappingManager);
            var tradeSummary = (await tradeSummaryService.TradeSummaryAsync(tradeReference, tradeLeg, apiJwtToken))?.Data?.First();
            var cashflow = (await cashflowService.CashflowAsync(tradeReference, tradeLeg, apiJwtToken))?.Data;
            var profileResponses = (await profileService.ProfileAsync(tradeReference, tradeLeg, apiJwtToken, "sparse"))?.Data;
            var physicalTrade = mappingService.MapTrade(tradeDataObject, tradeSummary, cashflow, profileResponses);

            return await equiasService.AddPhysicalTrade(physicalTrade, requestTokenResponse);
        }
    }
}
