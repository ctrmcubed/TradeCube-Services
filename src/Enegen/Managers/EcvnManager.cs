using Enegen.Exceptions;
using Enegen.Messages;
using Enegen.Services;
using Enegen.Types;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Extensions;
using Shared.Helpers;
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
    private readonly ILogger logger;

    public EcvnManager(IModuleService moduleService, ISettingService settingService, ITradeService tradeService,
        ITradeDetailService tradeDetailService, IElexonSettlementPeriodService elexonSettlementPeriodService, 
        ILogger<EcvnManager> logger)
    {
        this.moduleService = moduleService;
        this.settingService = settingService;
        this.tradeService = tradeService;
        this.tradeDetailService = tradeDetailService;
        this.elexonSettlementPeriodService = elexonSettlementPeriodService;
        this.logger = logger;
    }

    public async Task<EnegenGenstarEcvnResponse> CreateEcvn(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken)
    {
        string TruncateDateTime(string dt) =>
            string.IsNullOrWhiteSpace(dt)
                ? null
                : dt.Split("T")[0];
        try
        {
            var context = CreateContext(await ValidateRequest(ecvnRequest, apiJwtToken), apiJwtToken);

            var tradeDataObject = (await tradeService.GetTradeAsync(context.ApiJwtToken, context.TradeReference, context.TradeLeg))?.Data?.SingleOrDefault();
            if (tradeDataObject is null)
            {
                throw new EcvnException($"The Trade with reference '{context.TradeReference}' and leg '{context.TradeLeg}' is not in the database.");
            }

            if (tradeDataObject.Extension is null)
            {
                throw new EcvnException($"The Trade with reference '{context.TradeReference}' and leg '{context.TradeLeg}' does not have any ECVN details and cannot be nominated via Enegen.");
            }

            if (string.IsNullOrWhiteSpace(tradeDataObject.InternalParty.Extension.BscParty?.BscPartyId))
            {
                throw new EcvnException("Cannot determine the Internal Party's BSC Party ID. This Trade cannot be nominated via the Enegen interface.");
            }
        
            if (string.IsNullOrWhiteSpace(tradeDataObject.Counterparty.Extension.BscParty?.BscPartyId))
            {
                throw new EcvnException("Cannot determine the Counterparty's BSC Party ID. This Trade cannot be nominated via the Enegen interface.");
            }

            var traderProdConFlag = TraderProdConFlag(tradeDataObject);
            if (string.IsNullOrWhiteSpace(traderProdConFlag))
            {
                throw new EcvnException("Cannot determine the Internal Party's Production / Consumption Flag. This Trade cannot be nominated via the Enegen interface.");
            }
        
            var party2ProdConFlag = Party2ProdConFlag(tradeDataObject);
            if (string.IsNullOrWhiteSpace(party2ProdConFlag))
            {
                throw new EcvnException("Cannot determine the Counterparty's Production / Consumption Flag. This Trade cannot be nominated via the Enegen interface.");
            }

            var tradeDetailResponse = (await tradeDetailService.GetTradeDetailAsync(tradeDataObject.TradeReference, tradeDataObject.TradeLeg, apiJwtToken))?.Data?.SingleOrDefault();
            if (tradeDetailResponse is null)
            {
                throw new EcvnException("No Trade Details returned.");            
            }
        
            var minProfileUtcStartDateTime = tradeDetailResponse.Profile.Min(p => p.UtcStartDateTime);
            var maxUtcStartDateTime = tradeDetailResponse.Profile.Max(p => p.UtcStartDateTime);

            var settlementPeriodServiceResponses = await elexonSettlementPeriodService.ElexonSettlementPeriodsAsync(new ElexonSettlementPeriodRequest
            {
                UtcStartDateTime = minProfileUtcStartDateTime.ToIso8601DateTime(),
                UtcEndDateTime = maxUtcStartDateTime.ToIso8601DateTime()
            }, apiJwtToken);

            var elexonSettlementPeriodResponseItems = settlementPeriodServiceResponses?.Data
                .ToList() ?? new List<ElexonSettlementPeriodResponseItem>();

            var minSettlementPeriod = elexonSettlementPeriodResponseItems.Min(r => r.UtcStartDateTime);
            var maxSettlementPeriod = elexonSettlementPeriodResponseItems.Max(r => r.UtcStartDateTime);

            var settlementPeriodVolumes = Merge(tradeDetailResponse.Profile, elexonSettlementPeriodResponseItems);
            var isTradeVoided = tradeDataObject.IsVoid();
        
            var ecvn = new EnegenGenstarEcvnResponse
            {
                ContractName = $"{tradeDataObject.TradeReference}{tradeDataObject.TradeLeg:D3}",
                ContractDescription = tradeDataObject.Contract?.ContractLongName ?? string.Empty,
                Trader = tradeDataObject.InternalParty.Extension?.BscParty?.BscPartyId,
                TraderProdConFlag = traderProdConFlag,
                Party2 = tradeDataObject.Counterparty?.Extension?.BscParty?.BscPartyId,
                Party2ProdConFlag = party2ProdConFlag,
                ContractStartDate = TruncateDateTime(minSettlementPeriod),
                ContractEndDate = TruncateDateTime(maxSettlementPeriod),
                ContractGroupId = tradeDataObject.Counterparty?.Extension?.BscParty?.BscPartyId,
                ContractProfile = "C",
                Evergreen = "F",
                EnergyVolumeItems = settlementPeriodVolumes.Select(v=> new EnergyVolumeItem
                {
                    EcvDate = v.ElexonSettlementDate,
                    EcvPeriod = v.ElexonSettlementPeriod,
                    EcvVolume = isTradeVoided 
                        ? 0  
                        : MathsHelper.Round(v.Volume, 3)
                })
            };

            return ecvn;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            return new EnegenGenstarEcvnResponse
            {
                Message = ex.Message
            };
        }
    }

    private async Task<EnegenGenstarEcvnRequest> ValidateRequest(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken)
    {
        ArgumentNullException.ThrowIfNull(ecvnRequest);

        if (string.IsNullOrWhiteSpace(ecvnRequest.TradeReference))
        {
            throw new EcvnException("The mandatory field 'TradeReference' has not been supplied.");
        }

        var moduleResponse = await moduleService.ModulesAsync(apiJwtToken);
        var isUkPowerEnabled = moduleService.IsEnabled(ModuleConstants.UkPowerModule, moduleResponse?.Data.ToList());

        if (!isUkPowerEnabled)
        {
            throw new EcvnException("The 'UK Power' Module is not enabled.");
        }

        var enegenEcvnUrlSetting = (await settingService.GetSettingAsync(SettingConstants.EnegenEcvnUrlSetting, apiJwtToken))?.Data?.SingleOrDefault()?.SettingValue;

        return string.IsNullOrWhiteSpace(enegenEcvnUrlSetting)
            ? throw new EcvnException("The 'ENEGEN_ECVN_URL' System setting is not set.")
            : ecvnRequest;
    }

    private static EcvnContext CreateContext(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken) =>
        new()
        {
            TradeReference = ecvnRequest.TradeReference,
            TradeLeg = ecvnRequest.TradeLeg,
            ApiJwtToken = apiJwtToken
        };

    private static string TraderProdConFlag(TradeDataObject tradeDataObject) =>
        tradeDataObject.Extension.InternalPartyEnergyAccountType switch
        {
            "Production" => "P",
            "Consumption" => "C",
            "Use Default" => tradeDataObject.InternalParty?.Extension?.DefaultEnergyAccount[0].ToString(),
            _ => throw new EcvnException("Cannot determine the Internal Party's Production / Consumption Flag. This Trade cannot be nominated via the Enegen interface.")
        };
    
    private static string Party2ProdConFlag(TradeDataObject tradeDataObject) =>
        tradeDataObject.Extension.CounterpartyEnergyAccountType switch
        {
            "Production" => "P",
            "Consumption" => "C",
            "Use Default" => tradeDataObject.Counterparty?.Extension?.DefaultEnergyAccount[0].ToString(),
            _ => throw new EcvnException("Cannot determine the Counterparty's Production / Consumption Flag. This Trade cannot be nominated via the Enegen interface.")
        };

    private static IEnumerable<SettlementPeriodVolume> Merge(IEnumerable<TimeNodeProfileValueBase> profile, IEnumerable<ElexonSettlementPeriodResponseItem> elexonSettlementPeriodResponseItems)
    {
        var settlementPeriodDict = elexonSettlementPeriodResponseItems.ToLookup(e => e.UtcStartDateTime, v => v);

        foreach (var profileValue in profile)
        {
            var utcStartDateTime = profileValue.UtcStartDateTime.ToIso8601DateTime();
            var value = settlementPeriodDict[utcStartDateTime].FirstOrDefault();

            yield return value is null
                ? throw new EcvnException($"TradeDetailProfile UtcStartDateTime '{utcStartDateTime}' not found")
                : new SettlementPeriodVolume
                {
                    ElexonSettlementPeriod = value.SettlementPeriod,
                    ElexonSettlementDate = value.SettlementDate,
                    Volume = profileValue.Value
                };
        }
    }
}