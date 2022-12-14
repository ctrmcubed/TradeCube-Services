using Enegen.Exceptions;
using Enegen.Messages;
using Enegen.Services;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Extensions;
using Shared.Messages;
using Shared.Services;

namespace Enegen.Managers;

public class EcvnManager : IEcvnManager
{
    private readonly IModuleService moduleService;
    private readonly ISettingService settingService;
    private readonly ITradeService tradeService;
    private readonly ITradeDetailService tradeDetailService;
    private readonly IElexonSettlementPeriodService elexonSettlementPeriodService;

    public EcvnManager(IModuleService moduleService, ISettingService settingService, ITradeService tradeService,
        ITradeDetailService tradeDetailService, IElexonSettlementPeriodService elexonSettlementPeriodService)
    {
        this.moduleService = moduleService;
        this.settingService = settingService;
        this.tradeService = tradeService;
        this.tradeDetailService = tradeDetailService;
        this.elexonSettlementPeriodService = elexonSettlementPeriodService;
    }

    public async Task<EnegenGenstarEcvnResponse> NotifyAsync(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken)
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

        if (string.IsNullOrWhiteSpace(tradeDataObject.InternalParty.Extension.BscParty?.BscPartyId))
        {
            throw new EcvnException("Cannot determine the internal party's BSC Party ID. This trade cannot be nominated via the Enegen interface.");
        }
        
        if (string.IsNullOrWhiteSpace(tradeDataObject.Counterparty.Extension.BscParty?.BscPartyId))
        {
            throw new EcvnException("Cannot determine the counterparty's BSC Party ID. This trade cannot be nominated via the Enegen interface.");
        }

        var traderProdConFlag = TraderProdConFlag(tradeDataObject);
        if (string.IsNullOrWhiteSpace(traderProdConFlag))
        {
            throw new EcvnException("Cannot determine the internal party's Production / Consumption Flag. This trade cannot be nominated via the Enegen interface.");
        }
        
        var party2ProdConFlag = Party2ProdConFlag(tradeDataObject);
        if (string.IsNullOrWhiteSpace(party2ProdConFlag))
        {
            throw new EcvnException("Cannot determine the counterparty's Production / Consumption Flag. This trade cannot be nominated via the Enegen interface.");
        }

        var tradeSummaryResponses = (await tradeDetailService.GetTradeDetailAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.SingleOrDefault();
        if (tradeSummaryResponses is null)
        {
            throw new EcvnException("No Trade Details returned.");            
        }
        
        var min = tradeSummaryResponses.Profile.Min(p => p.UtcStartDateTime);
        var max = tradeSummaryResponses.Profile.Max(p => p.UtcStartDateTime);

        var settlementPeriodServiceResponses = (await elexonSettlementPeriodService.ElexonSettlementPeriodsAsync(new ElexonSettlementPeriodRequest
        {
            UtcStartDateTime = min.ToIso8601DateTime(),
            UtcEndDateTime = max.ToIso8601DateTime()
        }, apiJwtToken))?.Data;
        
        throw new NotImplementedException();
    }
    
    private async Task<EnegenGenstarEcvnRequest> ValidateRequest(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken)
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

    private static EcvnContext CreateContext(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken)
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