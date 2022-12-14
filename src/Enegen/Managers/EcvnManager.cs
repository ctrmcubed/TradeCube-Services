using Enegen.Exceptions;
using Enegen.Messages;
using Enegen.Services;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Services;

namespace Enegen.Managers;

public class EcvnManager : IEcvnManager
{
    private readonly IModuleService moduleService;
    private readonly ISettingService settingService;
    private readonly ITradeService tradeService;

    public EcvnManager(IModuleService moduleService, ISettingService settingService, ITradeService tradeService)
    {
        this.moduleService = moduleService;
        this.settingService = settingService;
        this.tradeService = tradeService;
    }

    public async Task<EcvnResponse> NotifyAsync(EcvnRequest ecvnRequest, string apiJwtToken)
    {
        var context = CreateContext(await ValidateRequest(ecvnRequest, apiJwtToken), apiJwtToken);

        var tradeDataObject = (await tradeService.GetTradeAsync(context.ApiJwtToken, context.TradeReference, context.TradeLeg))?.Data?.SingleOrDefault();
        if (tradeDataObject is null)
        {
            throw new EcvnException($"The trade with reference '{context.TradeReference}' and Leg '{context.TradeLeg}' is not in the database.");
        }

        if (tradeDataObject.Extension is null)
        {
            throw new EcvnException($"The trade with reference '{context.TradeReference}' and Leg '{context.TradeLeg}' does not have any ECVN details and cannot be nominated via Enegen.");
        }
        
        throw new NotImplementedException();
    }
    
    private async Task<EcvnRequest> ValidateRequest(EcvnRequest ecvnRequest, string apiJwtToken)
    {
        ArgumentNullException.ThrowIfNull(ecvnRequest);

        if (string.IsNullOrWhiteSpace(ecvnRequest.TradeReference))
        {
            throw new EcvnException("The mandatory field 'TradeReference' has not been supplied.");
        }

        var moduleResponse = await moduleService.ModulesAsync(apiJwtToken);
        var isUkPowerEnabled = moduleService.IsEnabled(ModuleConstants.UkPowerModule, moduleResponse?.Data);

        if (!isUkPowerEnabled)
        {
            throw new EcvnException("The 'UK Power' Module is not enabled.");
        }

        var enegenEcvnUrlSetting = (await settingService.GetSettingAsync(SettingConstants.EnegenEcvnUrlSetting, apiJwtToken))?.Data?.SingleOrDefault()?.SettingValue;

        if (string.IsNullOrWhiteSpace(enegenEcvnUrlSetting))
        {
            throw new EcvnException("The 'ENEGEN_ECVN_URL' System setting is not set.");
        }
        
        return ecvnRequest;
    }

    private static EcvnContext CreateContext(EcvnRequest ecvnRequest, string apiJwtToken)
    {
        return new EcvnContext
        {
            TradeReference = ecvnRequest.TradeReference,
            TradeLeg = ecvnRequest.TradeLeg,
            ApiJwtToken = apiJwtToken
        };
    }

    private static string TraderProdConFlag(TradeDataObject tradeDataObject) =>
        tradeDataObject.Extension.InternalPartyEnergyAccountType switch
        {
            "Production" => "P",
            "Consumption" => "C",
            "Use Default" => tradeDataObject.InternalParty?.Extension?.DefaultEnergyAccount[0].ToString(),
            _ => throw new ArgumentOutOfRangeException(tradeDataObject.Extension.InternalPartyEnergyAccountType)
        };
    
    private static string Party2ProdConFlag(TradeDataObject tradeDataObject) =>
        tradeDataObject.Extension.CounterpartyEnergyAccountType switch
        {
            "Production" => "P",
            "Consumption" => "C",
            "Use Default" => tradeDataObject.InternalParty?.Extension?.DefaultEnergyAccount[0].ToString(),
            _ => throw new ArgumentOutOfRangeException(tradeDataObject.Extension.InternalPartyEnergyAccountType)
        };    
}