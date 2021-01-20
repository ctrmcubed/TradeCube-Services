using Equias.Messages;
using Equias.Services;
using Shared.Messages;
using Shared.Services;
using System.Collections.Generic;
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

        public EquiasManager(IEquiasAuthenticationService equiasAuthenticationService, IEquiasService equiasService, ITradeService tradeService, IEquiasMappingService equiasMappingService)
        {
            this.equiasAuthenticationService = equiasAuthenticationService;
            this.equiasService = equiasService;
            this.tradeService = tradeService;
            this.equiasMappingService = equiasMappingService;
        }

        public async Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest)
        {
            return await equiasAuthenticationService.GetAuthenticationToken(requestTokenRequest);
        }

        public async Task<AddPhysicalTradeResponse> AddPhysicalTrade(RequestTokenResponse requestTokenResponse)
        {
            var trade = await tradeService.GetTradesAsync("", new TradeRequest { TradeReferences = new List<string> { "" } });
            var physicalTrade = equiasMappingService.MapTrade(trade.Data.First());

            return await equiasService.AddPhysicalTrade(physicalTrade, requestTokenResponse);
        }
    }
}
