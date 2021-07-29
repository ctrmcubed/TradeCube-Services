using Fidectus.Messages;
using Fidectus.Models;
using Shared.Helpers;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public interface IFidectusManager
    {
        Task<(TradeConfirmation tradeConfirmation, SettingHelper settingHelper)> CreateTradeConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken);

        Task<TradeConfirmationResponse> SendTradeConfirmationAsync(TradeConfirmation tradeConfirmation, string apiJwtToken, SettingHelper settingHelper);
    }
}