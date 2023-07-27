using System.Text;
using Enegen.Exceptions;
using Enegen.Messages;
using Enegen.Services;
using Enegen.Types;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Managers;
using Shared.Messages;
using Shared.Serialization;
using Shared.Services;

namespace Enegen.Managers;

public class EcvnManager : IEcvnManager
{
    private readonly IElexonSettlementPeriodManager elexonSettlementPeriodManager;
    private readonly IModuleService moduleService;
    private readonly ISettingService settingService;
    private readonly ITradeService tradeService;
    private readonly ITradeDetailService tradeDetailService;
    private readonly IVaultService vaultService;
    private readonly IHmacService hmacService;
    private readonly IEcvnService ecvnService;
    private readonly ILogger logger;

    public EcvnManager(IElexonSettlementPeriodManager elexonSettlementPeriodManager, IModuleService moduleService, 
        ISettingService settingService, ITradeService tradeService, ITradeDetailService tradeDetailService,
        IVaultService vaultService, IHmacService hmacService, IEcvnService ecvnService, ILogger<EcvnManager> logger)
    {
        this.elexonSettlementPeriodManager = elexonSettlementPeriodManager;
        this.moduleService = moduleService;
        this.settingService = settingService;
        this.tradeService = tradeService;
        this.tradeDetailService = tradeDetailService;
        this.vaultService = vaultService;
        this.hmacService = hmacService;
        this.ecvnService = ecvnService;
        this.logger = logger;
    }

    public async Task<EcvnContext> CreateEcvnContext(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken)
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
        var enegenEcvnAppIdSetting = (await settingService.GetSettingAsync(SettingConstants.EnegenAppIdSetting, apiJwtToken))?.Data?.SingleOrDefault()?.SettingValue;
        var enegenPskVaultValue = (await vaultService.GetVaultValueAsync(VaultConstants.EnegenPsk, apiJwtToken)).Data?.SingleOrDefault();

        return new()
        {
            TradeReference = ecvnRequest.TradeReference,
            TradeLeg = ecvnRequest.TradeLeg,
            EnegenEcvnUrlSetting = string.IsNullOrWhiteSpace(enegenEcvnUrlSetting)
                ? throw new EcvnException($"The '{SettingConstants.EnegenEcvnUrlSetting}' System Setting is not set.")
                : enegenEcvnUrlSetting,
            EnegenEcvnAppIdSetting = string.IsNullOrWhiteSpace(enegenEcvnAppIdSetting)
                ? throw new EcvnException($"The '{SettingConstants.EnegenAppIdSetting}' System Setting is not set.")
                : enegenEcvnAppIdSetting,
            EnegenPskVaultValue = string.IsNullOrWhiteSpace(enegenPskVaultValue?.VaultValue)
                ? throw new EcvnException($"The '{VaultConstants.EnegenPsk}' Vault value is not set.")
                : enegenPskVaultValue.VaultValue,
            ApiJwtToken = apiJwtToken
        };
    }

    public async Task<EnegenGenstarEcvnExternalRequest> CreateEcvnRequest(EcvnContext context, string apiJwtToken)
    {
        string TruncateDateTime(string dt) =>
            string.IsNullOrWhiteSpace(dt)
                ? null
                : dt.Split("T")[0];
        try
        {
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

            var settlementPeriodServiceResponses = elexonSettlementPeriodManager.ElexonSettlementPeriods(new ElexonSettlementPeriodRequest
            {
                UtcStartDateTime = minProfileUtcStartDateTime.ToIso8601DateTime(),
                UtcEndDateTime = maxUtcStartDateTime.ToIso8601DateTime()
            });

            var elexonSettlementPeriodResponseItems = settlementPeriodServiceResponses?.Data
                .ToList() ?? new List<ElexonSettlementPeriodResponseItem>();

            var minSettlementPeriod = elexonSettlementPeriodResponseItems.Min(r => r.UtcStartDateTime);
            var maxSettlementPeriod = elexonSettlementPeriodResponseItems.Max(r => r.UtcStartDateTime);

            var settlementPeriodVolumes = Merge(tradeDetailResponse.Profile, elexonSettlementPeriodResponseItems);
            var isTradeVoided = tradeDataObject.IsVoid();
            var contractName = $"{tradeDataObject.TradeReference}{tradeDataObject.TradeLeg:D3}";
            
            var ecvn = new EnegenGenstarEcvnExternalRequest
            {
                ContractName = contractName,
                ContractDescription = tradeDataObject.Contract?.ContractLongName ??  contractName,
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
            return new EnegenGenstarEcvnExternalRequest
            {
                ValidationStatus = ApiConstants.FailedResult,
                ValidationMessage = ex.Message
            };
        }
    }
    
    public async Task<ApiResponseWrapper<string>> NotifyEcvn(EnegenGenstarEcvnExternalRequest enegenGenstarEcvnExternalRequest, EcvnContext ecvnContext)
    {
        try
        {
            string Uuid() =>
                Guid
                    .NewGuid()
                    .ToString("N");

            var uri = ecvnContext.EnegenEcvnUrlSetting;
            var body = TradeCubeJsonSerializer.Serialize(enegenGenstarEcvnExternalRequest);
            var bodyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(body));
            var appId = ecvnContext.EnegenEcvnAppIdSetting;
            var unixTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
            var nonce = Uuid();
            var signature = hmacService.CreateSignature(uri, bodyBase64, appId, unixTimeSeconds, nonce);
            var hashedPayload = hmacService.GenerateHash(signature, ecvnContext.EnegenPskVaultValue);

            logger.LogInformation("Context: {@Context}", ecvnContext);
        
            return await ecvnService.NotifyAsync(uri, appId, hashedPayload, nonce, unixTimeSeconds, body);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            return new ApiResponseWrapper<string>
            {
                Status = ApiConstants.FailedResult,
                Message = ex.Message
            };
        }
    }

    private static string TraderProdConFlag(TradeDataObject tradeDataObject)
    {
        string Default(string defaultEnergyAccount) =>
            string.IsNullOrWhiteSpace(defaultEnergyAccount)
                ? throw new EcvnException("The InternalPartyEnergyAccountType is set to 'Use Default' but the Internal Party's DefaultEnergyAccount is not set")
                : defaultEnergyAccount[0].ToString();
        
        return tradeDataObject.Extension?.InternalPartyEnergyAccountType switch
        {
            "Production" => "P",
            "Consumption" => "C",
            "Use Default" => Default(tradeDataObject.InternalParty?.Extension?.DefaultEnergyAccount),
            _ => throw new EcvnException(
                "Cannot determine the Internal Party's Production / Consumption Flag. This Trade cannot be nominated via the Enegen interface.")
        };
    }

    private static string Party2ProdConFlag(TradeDataObject tradeDataObject)
    {
        string Default(string defaultEnergyAccount) =>
            string.IsNullOrWhiteSpace(defaultEnergyAccount)
                ? throw new EcvnException("The CounterpartyEnergyAccountType is set to 'Use Default' but the Counterparty's DefaultEnergyAccount is not set")
                : defaultEnergyAccount[0].ToString();
        
        return tradeDataObject.Extension.CounterpartyEnergyAccountType switch
        {
            "Production" => "P",
            "Consumption" => "C",
            "Use Default" => Default(tradeDataObject.Counterparty?.Extension?.DefaultEnergyAccount),
            _ => throw new EcvnException("Cannot determine the Counterparty's Production / Consumption Flag. This Trade cannot be nominated via the Enegen interface.")
        };
    }

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
                    Volume = profileValue.Volume
                };
        }
    }
}