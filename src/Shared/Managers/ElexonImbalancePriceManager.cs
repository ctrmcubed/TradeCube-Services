using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Messages;
using Shared.Services;
using Shared.Types.CubeDataBulk;
using Shared.Types.Elexon;

namespace Shared.Managers;

public class ElexonImbalancePriceManager : IElexonImbalancePriceManager
{
    private readonly IVaultService vaultService;
    private readonly ISettingService settingService;
    private readonly IElexonService elexonService;
    private readonly ICubeService cubeService;
    private readonly IDataItemService dataItemService;
    private readonly ICubeTypeService cubeTypeService;
    private readonly ILogger<ElexonImbalancePriceManager> logger;

    public ElexonImbalancePriceManager(IVaultService vaultService, ISettingService settingService,
        IElexonService elexonService, ICubeService cubeService, IDataItemService dataItemService, 
        ICubeTypeService cubeTypeService, ILogger<ElexonImbalancePriceManager> logger)
    {
        this.vaultService = vaultService;
        this.settingService = settingService;
        this.elexonService = elexonService;
        this.cubeService = cubeService;
        this.dataItemService = dataItemService;
        this.cubeTypeService = cubeTypeService;
        this.logger = logger;
    }
    
    public DerivedSystemWideDataRequest CreateElexonImbalancePriceRequest(ElexonImbalancePriceContext elexonImbalancePriceContext)
    {
        if (elexonImbalancePriceContext.StartDate.HasValue && elexonImbalancePriceContext.EndDate.HasValue)
        {
            return new DerivedSystemWideDataRequest
            {
                ApiKey = elexonImbalancePriceContext.ElexonApiKey,
                FromSettlementDate = elexonImbalancePriceContext.StartDate.Value.ToIso8601Date(),
                ToSettlementDate = elexonImbalancePriceContext.EndDate.Value.ToIso8601Date(),
                SettlementPeriod = "*",
                ServiceType = "xml"
            };            
        }

        return new DerivedSystemWideDataRequest();
    }

    public ElexonSettlementPeriodRequest CreateElexonSettlementPeriodRequest(ElexonImbalancePriceContext elexonImbalancePriceContext)
    {
        if (elexonImbalancePriceContext.StartDate.HasValue && elexonImbalancePriceContext.EndDate.HasValue)
        {
            return new ElexonSettlementPeriodRequest
            {
                StartDateTimeUtc = elexonImbalancePriceContext.StartDate.Value.AddDays(-1).ToIso8601Date(),
                EndDateTimeUtc = elexonImbalancePriceContext.EndDate.Value.AddDays(1).ToIso8601Date(),
            };    
        }

        return new ElexonSettlementPeriodRequest();
    }

    public CubeDataBulkRequest CreateCubeDataBulkRequest(ElexonImbalancePriceContext elexonImbalancePriceContext,
        ElexonImbalancePriceResponse elexonImbalancePriceResponse)
    {
        if (elexonImbalancePriceContext.Mode == ElexonImbalancePriceConstants.ModeStandalone)
        {
            return null;
        }
        
        var data = elexonImbalancePriceResponse.Data.Select(d => new CubeDataBulkData
        {
            StartDateTimeUTC = DateTimeHelper.ParseIsoDateTime(d.StartDateTimeUTC),
            Values = new List<decimal> { d.ImbalancePrice }
        });

        return new CubeDataBulkRequest
        {
            Name = ElexonImbalancePriceConstants.CubeDataBulkName,
            Description = ElexonImbalancePriceConstants.CubeDataBulkDescription,
            Reason = ElexonImbalancePriceConstants.CubeDataBulkReason,
            Cube = elexonImbalancePriceContext.Cube,
            DataItem = elexonImbalancePriceContext.DataItem,
            Layer = elexonImbalancePriceContext.Layer,
            CreateNodes = false,
            RegularDayPeriods = 48,
            Data = data
        };
    }

    // public async Task<ElexonImbalancePriceResponse> ElexonImbalancePrice(
    //     ElexonImbalancePriceRequest elexonImbalancePriceRequest,
    //     ElexonDerivedSystemWideDataMockApiType elexonDerivedSystemWideDataMockApiType)
    // {
    //     try
    //     {
    //         var elexonImbalancePriceContext = await CreateContext(elexonImbalancePriceRequest);
    //         var derivedSystemWideData = await elexonService.DerivedSystemWideData(new DerivedSystemWideDataRequest
    //         {
    //             ApiKey = elexonImbalancePriceContext.ElexonApiKey,
    //             FromSettlementDate = elexonImbalancePriceContext.StartDate.ToString("yyyy-MM-dd"),
    //             ToSettlementDate = elexonImbalancePriceContext.EndDate.ToString("yyyy-MM-dd"),
    //             SettlementPeriod = "*",
    //             ServiceType = "xml"
    //         });
    //         
    //         logger.JsonLogDebug("Envelope", derivedSystemWideData);
    //         
    //         // var imbalancePriceSetting = await settingService.FindBySettingByNameAsync(SettingConstants.PeakGenElexonImbalancePriceCube, tenant);
    //         // var targetCube = string.IsNullOrWhiteSpace(imbalancePriceSetting.SettingValue)
    //         //     ? "Imbalance Price"
    //         //     : imbalancePriceSetting.SettingValue;
    //         //
    //         // var grouped = derivedSystemWideData?.responseBody?.responseList
    //         //     .GroupBy(d => d.settlementDate)
    //         //     .Select(x => new
    //         //     {
    //         //         Date = x.Key,
    //         //         Values = x.ToList()
    //         //     });
    //         //
    //         // var data = grouped?
    //         //     .Select(m => new CubeDataBulkData
    //         //     {
    //         //         StartDateTimeLocal = m.Date.ToIso8601Date(),
    //         //         Values = m.Values.Select(v => v.systemSellPrice)
    //         //     })
    //         //     .ToList();
    //         //
    //         // var cubeDataBulkRequest = new CubeDataBulkRequest
    //         // {
    //         //     Name = "PeakGen Imbalance Price Load",
    //         //     Cube = targetCube,
    //         //     DataItem = DataItemConstants.DataItemPrice,
    //         //     Node = "SBP",
    //         //     Unit = UnitConstants.GBPPerMWh,
    //         //     Timezone = TimezoneConstants.TimezoneEuropeLondon,
    //         //     ShortDayRule = ClockChangeConstants.ClockChangeSequential,
    //         //     LongDayRule = ClockChangeConstants.ClockChangeSequential,
    //         //     RegularDayPeriods = 48,
    //         //     CreateNodes = true,
    //         //     Reason = "Imbalance Price Automated Load",
    //         //     Data = data
    //         // };
    //         //
    //         // var responseWrapper = await scafellWebApiService
    //         //     .PostViaApiKeyAsync<CubeDataBulkRequest, ApiResponseWrapper<IEnumerable<CubeDataDataObject>>>(
    //         //         request.ApiKey, cubeDataBulkRequest, "CubeDataBulk", false);
    //         //
    //         // if (responseWrapper.IsSuccessStatusCode)
    //         // {
    //         //     logger.LogInformation("PeakGen Imbalance Price Load success");
    //         //
    //         //     // await Notifications(NotificationType.Ok, request, "PeakGen Imbalance Price Load success", tenant);
    //         //
    //         //     return new ElexonImbalancePriceResponse();
    //         // }
    //         
    //         return new ElexonImbalancePriceResponse();
    //     }
    //     catch (Exception ex)
    //     {
    //         logger.LogError(ex, "{Message}", ex.Message);
    //         return new ElexonImbalancePriceResponse
    //         {
    //             Status = ApiConstants.FailedResult,
    //             Message   = ex.Message
    //         };
    //     }
    // }

    public async Task<ElexonImbalancePriceContext> CreateContext(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        try
        {
            return await CreateContext2(elexonImbalancePriceRequest);
        }
        catch (Exception ex)
        {
            return new ElexonImbalancePriceContext
            {
                Mode = Mode(elexonImbalancePriceRequest),
                MessageResponseBag = new MessageResponseBag(ex.Message, MessageResponseType.Error)
            };
        }
    }
    
    public DerivedSystemWideData DeserializeDerivedSystemWideData(string response)
    {
        try
        {
            return elexonService.DeserializeDerivedSystemWideData(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }

    public async Task<ElexonImbalancePriceResponse> ElexonImbalancePrice2(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        var elexonImbalancePriceContext = await CreateContext(elexonImbalancePriceRequest); 
        var derivedSystemWideDataRequest = CreateElexonImbalancePriceRequest(elexonImbalancePriceContext);
        var elexonDerivedSystemWideData = await elexonService.DerivedSystemWideData(derivedSystemWideDataRequest);

        return new ElexonImbalancePriceResponse();
    }
    
    public ElexonImbalancePriceResponse ElexonImbalancePrice(ElexonImbalancePriceContext elexonImbalancePriceContext, 
        DerivedSystemWideData derivedSystemWideData, IEnumerable<ElexonSettlementPeriodResponseItem> settlementPeriodResponseItems)
    {
        if (elexonImbalancePriceContext.MessageResponseBag.GotErrors())
        {
            return new ElexonImbalancePriceResponse();
        }
        
        var elexonImbalancePriceItems = CreateElexonImbalancePriceItems(derivedSystemWideData, settlementPeriodResponseItems);
        
        var imbalancePriceResponse = new ElexonImbalancePriceResponse
        {
            Data = elexonImbalancePriceItems
        };

        if (elexonImbalancePriceContext.IsModeStandalone())
        {
            return imbalancePriceResponse;
        }

        return new ElexonImbalancePriceResponse
        {
            CubeDataBulk = CreateCubeDataBulkRequest(elexonImbalancePriceContext, imbalancePriceResponse)
        };
    }

    private async Task<ElexonImbalancePriceContext> CreateContext2(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        ArgumentNullException.ThrowIfNull(elexonImbalancePriceRequest);

        var mode = Mode(elexonImbalancePriceRequest);

        var edt = string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.EndDate) 
            ? DateTime.Today
            : DateTimeHelper.ParseIsoDateTime(elexonImbalancePriceRequest.EndDate);
        
        var sdt = string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.StartDate) 
            ? edt.AddDays(-7)
            : DateTimeHelper.ParseIsoDateTime(elexonImbalancePriceRequest.StartDate);

        if (edt < sdt)
        {
            throw new ElexonImbalancePriceException($"The EndDate '{edt.ToIso8601Date()}' is before the StartDate '{sdt.ToIso8601Date()}'");
        }
        
        if (edt.Subtract(sdt).TotalDays > 40)
        {
            throw new ElexonImbalancePriceException($"The EndDate '{edt.ToIso8601Date()}' cannot be more than 40 days after the StartDate '{sdt.ToIso8601Date()}'");
        }
        
        var isStandaloneMode = IsMode(mode, ElexonImbalancePriceConstants.ModeStandalone);

        var elexonApiKey = isStandaloneMode
            ? string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.ElexonApiKey)
                ? (await vaultService.GetVaultValueAsync(VaultConstants.ElexonApiKey,
                    elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault()?.VaultValue
                : elexonImbalancePriceRequest.ElexonApiKey
            : null;
        
        if (isStandaloneMode && string.IsNullOrWhiteSpace(elexonApiKey))
        {
            throw new ElexonImbalancePriceException($"An Elexon API key must be provided");
        }

        var isCubeMode = IsMode(mode, ElexonImbalancePriceConstants.ModeCube);

        var cube = isCubeMode
            ? string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.Cube)
                ? (await settingService.GetSettingAsync(SettingConstants.ElexonImbalancePriceCube,
                    elexonImbalancePriceRequest.ElexonApiKey))?.Data?.SingleOrDefault()?.SettingValue
                : elexonImbalancePriceRequest.Cube
            : null;

        var dataItem = isCubeMode
            ? string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.DataItem)
                ? (await settingService.GetSettingAsync(SettingConstants.ElexonImbalancePriceDataItem,
                    elexonImbalancePriceRequest.ElexonApiKey))?.Data?.SingleOrDefault()?.SettingValue
                : elexonImbalancePriceRequest.DataItem
            : null;

        if (!isCubeMode)
        {
            return new ElexonImbalancePriceContext
            {
                ElexonApiKey = elexonImbalancePriceRequest.ElexonApiKey,
                Mode = mode,
                StartDate = sdt,
                EndDate = edt,
                MessageResponseBag = new MessageResponseBag()
            };
        }
        
        if (string.IsNullOrWhiteSpace(cube))
        {
            throw new ElexonImbalancePriceException("The mandatory field 'Cube' has not been supplied");
        }
            
        if (string.IsNullOrWhiteSpace(dataItem))
        {
            throw new ElexonImbalancePriceException("The mandatory field 'DataItem' has not been supplied");
        }

        var layer = string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.Layer)
            ? (await settingService.GetSettingAsync(SettingConstants.ElexonImbalancePriceLayer,
                elexonImbalancePriceRequest.ElexonApiKey))?.Data?.SingleOrDefault()?.SettingValue
            : elexonImbalancePriceRequest.Layer;

        var cubeDataObject = (await cubeService.GetCube(cube, elexonImbalancePriceRequest.ElexonApiKey))?.Data?.SingleOrDefault();
        if (cubeDataObject is null)
        {
            throw new ElexonImbalancePriceException($"The Cube '{cube}' is not present in the database");
        }

        var dataItemObject = (await dataItemService.GetDataItem(dataItem, elexonImbalancePriceRequest.ElexonApiKey))?.Data?.SingleOrDefault();
        if (dataItemObject is null)
        {
            throw new ElexonImbalancePriceException($"The DataItem '{dataItem}' is not present in the database");
        }

        if (!await IsCubeOfCubeTypeItemAsync(cubeDataObject, CubeConstants.CubeTypeItemTimeSeriesCube, elexonImbalancePriceRequest.ElexonApiKey))
        {
            throw new ElexonImbalancePriceException($"The Cube '{cube}' is not a time series cube");
        }

        return new ElexonImbalancePriceContext
        {
            ElexonApiKey = elexonImbalancePriceRequest.ElexonApiKey,
            Mode = mode,
            Cube = cube,
            DataItem = dataItem,
            Layer = layer,
            StartDate = sdt,
            EndDate = edt,
            MessageResponseBag = new MessageResponseBag()
        };
    }

    private async Task<bool> IsCubeOfCubeTypeItemAsync(CubeDataObject cubeDataObject, string cubeItemType, string apiJwt)
    {
        var cubeTypeDataObjects = (await cubeTypeService.GetCubeType(cubeDataObject.CubeType, apiJwt))?.Data?.ToList();
        var cubeTypes = cubeTypeDataObjects?.Where(c => c.EnabledItems is not null && c.EnabledItems.Contains(cubeItemType)).Any() ?? false;

        return cubeTypes;
    }
      
    private static IEnumerable<ElexonImbalancePriceItem> CreateElexonImbalancePriceItems(DerivedSystemWideData derivedSystemWideData, 
        IEnumerable<ElexonSettlementPeriodResponseItem> elexonSettlementPeriodResponseItems)
    {
        var lookup = elexonSettlementPeriodResponseItems.ToLookup(k => (k.SettlementPeriod, k.SettlementDate), v => v);
        
        foreach (var responseResponseBodyItem in derivedSystemWideData?.ResponseBody?.ResponseList?.Item ?? Array.Empty<ResponseResponseBodyItem>())
        {
            var startDateTime = responseResponseBodyItem.SettlementDate.ToIso8601Date();
            var lookupKey = (responseResponseBodyItem.SettlementPeriod, startDateTime);
            var utcStartDateTime = lookup[lookupKey].SingleOrDefault()?.StartDateTimeUtc;
            
            yield return new ElexonImbalancePriceItem
            {
                SettlementDate = startDateTime,
                SettlementPeriod = responseResponseBodyItem.SettlementPeriod,
                ImbalancePrice = responseResponseBodyItem.SystemSellPrice,
                StartDateTimeUTC = utcStartDateTime
            };
        }
    }
    
    private static string Mode(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        if (string.IsNullOrWhiteSpace(elexonImbalancePriceRequest?.Mode))
        {
            return ElexonImbalancePriceConstants.ModeStandalone;
        }

        return string.Equals(elexonImbalancePriceRequest.Mode, ElexonImbalancePriceConstants.ModeCube, StringComparison.CurrentCultureIgnoreCase) 
            ? ElexonImbalancePriceConstants.ModeCube 
            : ElexonImbalancePriceConstants.ModeStandalone;
    }
    
    private static bool IsMode(string mode1, string mode2) => 
        mode1 == mode2;
}