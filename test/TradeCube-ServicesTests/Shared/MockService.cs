using System.Collections.Generic;
using System.Linq;
using Moq;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using TradeCube_ServicesTests.Enegen.Ecvn;

namespace TradeCube_ServicesTests.Shared;

public static class MockService
{
    public static ITradeService CreateTradeService(IEnumerable<TradeDataObject> tradeDataObjects)
    {
        var service = new Mock<ITradeService>();

        service
            .Setup(c => c.GetTradeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((string _, string tradeReference, int tradeLeg) => new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Data = tradeDataObjects.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg)
                });

        return service.Object;
    }

    public static IMappingService CreateMappingService(IEnumerable<MappingDataObject> mappingDataObjects)
    {
        var service = new Mock<IMappingService>();

        service
            .Setup(c => c.GetMappingsViaJwtAsync(It.IsAny<string>()))
            .ReturnsAsync((string _) =>
                new ApiResponseWrapper<IEnumerable<MappingDataObject>> { Data = mappingDataObjects });

        return service.Object;
    }

    public static IPartyService CreatePartyService(IEnumerable<PartyDataObject> partyDataObjects)
    {
        var service = new Mock<IPartyService>();

        service
            .Setup(c => c.GetPartyAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string party, string _) => new ApiResponseWrapper<IEnumerable<PartyDataObject>> { Data = partyDataObjects.Where(p => p.Party == party) });

        return service.Object;
    }

    public static IVaultService CreateVaultService(IEnumerable<VaultDataObject> vaultDataObjects)
    {
        var service = new Mock<IVaultService>();

        service
            .Setup(c => c.GetVaultValueAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string vaultKey, string _) => new ApiResponseWrapper<IEnumerable<VaultDataObject>> { Data = vaultDataObjects.Where(v => v.VaultKey == vaultKey) });

        return service.Object;
    }

    public static IModuleService CreateModuleService(IEnumerable<ModuleDataObject> moduleDataObjects)
    {
        var service = new Mock<IModuleService>();

        service
            .Setup(c => c.ModulesAsync(It.IsAny<string>()))
            .ReturnsAsync((string _) => new ApiResponseWrapper<IEnumerable<ModuleDataObject>> { Data = moduleDataObjects });

        service
            .Setup(c => c.IsEnabled(It.IsAny<string>(), It.IsAny<IEnumerable<ModuleDataObject>>()))
            .Returns((string module, IEnumerable<ModuleDataObject> modules) => modules.Any(m => m.Module == module && m.Enabled == true));
        
        return service.Object;
    }

    public static ISettingService CreateSettingService(IEnumerable<SettingDataObject> settingDataObjects)
    {
        var service = new Mock<ISettingService>();

        service
            .Setup(c => c.GetSettingAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string _, string _) => new ApiResponseWrapper<IEnumerable<SettingDataObject>>
            {
                Data = settingDataObjects
            });

        return service.Object;
    }

    public static IProfileService CreateProfileService(IEnumerable<ProfileResponse> profileResponses)
    {
        var service = new Mock<IProfileService>();

        service
            .Setup(c => c.GetProfileAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string tradeReference, int tradeLeg, string _, string _) => new ApiResponseWrapper<IEnumerable<ProfileResponse>>
                {
                    Data = profileResponses.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg)
                });

        return service.Object;
    }

    public static ICashflowService CreateCashflowService(IEnumerable<CashflowResponse> cashflowResponses)
    {
        var service = new Mock<ICashflowService>();

        service
            .Setup(c => c.CashflowAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((string tradeReference, int tradeLeg, string _) => new ApiResponseWrapper<IEnumerable<CashflowResponse>>
                {
                    Data = cashflowResponses.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg)
                });

        return service.Object;
    }

    public static ITradeSummaryService CreateTradeSummaryService(IEnumerable<TradeSummaryResponse> tradeSummaryResponses)
    {
        var service = new Mock<ITradeSummaryService>();

        service
            .Setup(c => c.GetTradeSummaryAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((string tradeReference, int tradeLeg, string _) => new ApiResponseWrapper<IEnumerable<TradeSummaryResponse>>
            {
                Data = tradeSummaryResponses.Where(t => t.TradeReference == tradeReference && t.TradeLeg == tradeLeg)
            });
        return service.Object;
    }

    public static ITradeDetailService CreateTradeDetailService(IEnumerable<TradeDetailTestType> tradeDetailTestTypes)
    {
        var service = new Mock<ITradeDetailService>();
    
        service
            .Setup(c => c.GetTradeDetailAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((string tradeReference, int tradeLeg, string _) => new ApiResponseWrapper<IEnumerable<TradeDetailResponse>>
                {
                    Data = tradeDetailTestTypes
                        .Where(t => t.Inputs.TradeReference == tradeReference && t.Inputs.TradeLeg == tradeLeg)
                        .SelectMany(t => t.Response.Data)
                });
    
        return service.Object;
    }
    
    public static IElexonSettlementPeriodService CreateElexonSettlementPeriodService(IEnumerable<ElexonSettlementPeriodTestType> elexonSettlementPeriodTestTypes)
    {
        var service = new Mock<IElexonSettlementPeriodService>();
    
        service
            .Setup(c => c.ElexonSettlementPeriodsAsync(It.IsAny<ElexonSettlementPeriodRequest>(), It.IsAny<string>()))
            .ReturnsAsync((ElexonSettlementPeriodRequest request, string _) => new ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>>
                {
                    Data = ElexonSettlementPeriodsAsync(elexonSettlementPeriodTestTypes, request)
                });
    
        return service.Object;
    }

    private static List<ElexonSettlementPeriodResponseItem> ElexonSettlementPeriodsAsync(IEnumerable<ElexonSettlementPeriodTestType> elexonSettlementPeriodTestTypes, ElexonSettlementPeriodRequest request)
    {
        return elexonSettlementPeriodTestTypes
            .Where(e => e.Inputs.UtcStartDateTime == request.UtcStartDateTime && e.Inputs.UtcEndDateTime == request.UtcEndDateTime)
            .SelectMany(t => t.Response.Data)
            .ToList();
    }
}