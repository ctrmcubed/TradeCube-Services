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
    private readonly IElexonSettlementPeriodManager elexonSettlementPeriodManager;
    private readonly IVaultService vaultService;
    private readonly ISettingService settingService;
    private readonly IElexonService elexonService;
    private readonly ICubeService cubeService;
    private readonly IDataItemService dataItemService;
    private readonly ICubeTypeService cubeTypeService;
    private readonly ICubeDataBulkService cubeDataBulkService;
    private readonly ILogger<ElexonImbalancePriceManager> logger;

    public ElexonImbalancePriceManager(IElexonSettlementPeriodManager elexonSettlementPeriodManager,
        IElexonService elexonService, ICubeService cubeService, IDataItemService dataItemService, 
        ICubeTypeService cubeTypeService, ICubeDataBulkService cubeDataBulkService, IVaultService vaultService, 
        ISettingService settingService, ILogger<ElexonImbalancePriceManager> logger)
    {
        this.elexonSettlementPeriodManager = elexonSettlementPeriodManager;
        this.vaultService = vaultService;
        this.settingService = settingService;
        this.elexonService = elexonService;
        this.cubeService = cubeService;
        this.dataItemService = dataItemService;
        this.cubeTypeService = cubeTypeService;
        this.cubeDataBulkService = cubeDataBulkService;
        this.logger = logger;
    }
    
    public DerivedSystemWideDataRequest CreateElexonImbalancePriceRequest(ElexonImbalancePriceContext elexonImbalancePriceContext)
    {
        if (elexonImbalancePriceContext.StartDate.HasValue && elexonImbalancePriceContext.EndDate.HasValue)
        {
            return new DerivedSystemWideDataRequest
            {
                ApiKey = elexonImbalancePriceContext.ElexonApiKey,
                Url = "https://api.bmreports.com/BMRS/DERSYSDATA/v1",
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

    public async Task<ElexonImbalancePriceResponse> ElexonImbalancePriceWithCdb(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        string ErrorType(string errorType) => 
            string.IsNullOrWhiteSpace(errorType) ? string.Empty : $"Error Type '{errorType}'";

        string Description (string description) => 
            string.IsNullOrWhiteSpace(description) ? string.Empty : $"Description '{description}'";
        
        var elexonImbalancePriceContext = await CreateContext(elexonImbalancePriceRequest);
        if (elexonImbalancePriceContext.MessageResponseBag.GotErrors())
        {
            return new ElexonImbalancePriceResponse
            {
                Status = ApiConstants.FailedResult,
                Message = elexonImbalancePriceContext.MessageResponseBag.ErrorsAsString()
            };
        }
        var derivedSystemWideDataRequest = CreateElexonImbalancePriceRequest(elexonImbalancePriceContext);
        var elexonDerivedSystemWideData = await elexonService.DerivedSystemWideData(derivedSystemWideDataRequest);

        if (elexonDerivedSystemWideData.ResponseMetadata.HttpCode != 200 || 
            elexonDerivedSystemWideData.ResponseMetadata.ErrorType != "Ok" ||
            elexonDerivedSystemWideData.ResponseMetadata.Description != "Success")
        {
            return new ElexonImbalancePriceResponse
            {
                Status = ApiConstants.FailedResult,
                Message = $"The query to Elexon was not successful. HTTP Code {elexonDerivedSystemWideData.ResponseMetadata.HttpCode}, {ErrorType(elexonDerivedSystemWideData.ResponseMetadata.ErrorType)}, {Description(elexonDerivedSystemWideData.ResponseMetadata.Description)}"
            };
        }

        var elexonSettlementPeriodRequest = CreateElexonSettlementPeriodRequest(elexonImbalancePriceContext);
        var elexonSettlementPeriodResponse = elexonSettlementPeriodManager.ElexonSettlementPeriods(elexonSettlementPeriodRequest);

        var elexonImbalancePriceResponse = ElexonImbalancePrice(elexonImbalancePriceContext, elexonDerivedSystemWideData, elexonSettlementPeriodResponse?.Data);
        var cubeDataBulkRequest = elexonImbalancePriceResponse?.CubeDataBulk;

        if (cubeDataBulkRequest is null)
        {
            return elexonImbalancePriceResponse;
        }
        
        var responseWrapper = await cubeDataBulkService.CubeDataBulk(cubeDataBulkRequest, elexonImbalancePriceContext.ApiKey);

        if (!responseWrapper.IsSuccess())
        {
            return new ElexonImbalancePriceResponse
            {
                Status = ApiConstants.FailedResult,
                Message = responseWrapper.Message
            };
        }
        
        logger.LogInformation("Imbalance Price Load success");

        return new ElexonImbalancePriceResponse
        {
            Status = ApiConstants.SuccessResult,
            Message = responseWrapper.Message
        };
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

    public async Task<DerivedSystemWideData> GetElexonDerivedSystemWideData(DerivedSystemWideDataRequest derivedSystemWideDataRequest) =>
        await elexonService.DerivedSystemWideData(derivedSystemWideDataRequest);

    public ElexonSettlementPeriodResponse GetElexonSettlementPeriods(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest) =>
        elexonSettlementPeriodManager.ElexonSettlementPeriods(elexonSettlementPeriodRequest);

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
            ? null
            : string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.ElexonApiKey)
                ? (await vaultService.GetVaultValueViaApiKeyAsync(VaultConstants.ElexonApiKey,
                    elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault()?.VaultValue
                : elexonImbalancePriceRequest.ElexonApiKey;
        
        if (isStandaloneMode && string.IsNullOrWhiteSpace(elexonApiKey))
        {
            throw new ElexonImbalancePriceException($"An Elexon API key must be provided");
        }

        var isCubeMode = IsMode(mode, ElexonImbalancePriceConstants.ModeCube);

        var cube = isCubeMode
            ? string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.Cube)
                ? (await settingService.GetSettingViaApiKeyAsync(SettingConstants.ElexonImbalancePriceCube,
                    elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault()?.SettingValue
                : elexonImbalancePriceRequest.Cube
            : null;

        var dataItem = isCubeMode
            ? string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.DataItem)
                ? (await settingService.GetSettingViaApiKeyAsync(SettingConstants.ElexonImbalancePriceDataItem,
                    elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault()?.SettingValue
                : elexonImbalancePriceRequest.DataItem
            : null;

        if (!isCubeMode)
        {
            return new ElexonImbalancePriceContext
            {
                ApiKey = elexonImbalancePriceRequest.ApiKey,
                ElexonApiKey = elexonApiKey,
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
            ? (await settingService.GetSettingViaApiKeyAsync(SettingConstants.ElexonImbalancePriceLayer,
                elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault()?.SettingValue
            : elexonImbalancePriceRequest.Layer;

        var cubeDataObject = (await cubeService.GetCubeViaApiKey(cube, elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault();
        if (cubeDataObject is null)
        {
            throw new ElexonImbalancePriceException($"The Cube '{cube}' is not present in the database");
        }

        var dataItemObject = (await dataItemService.GetDataItemViaApiKey(dataItem, elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault();
        if (dataItemObject is null)
        {
            throw new ElexonImbalancePriceException($"The DataItem '{dataItem}' is not present in the database");
        }

        if (!await IsCubeOfCubeTypeItemAsync(cubeDataObject, CubeConstants.CubeTypeItemTimeSeriesCube, elexonImbalancePriceRequest.ApiKey))
        {
            throw new ElexonImbalancePriceException($"The Cube '{cube}' is not a time series cube");
        }

        return new ElexonImbalancePriceContext
        {
            ApiKey = elexonImbalancePriceRequest.ApiKey,
            ElexonApiKey = elexonApiKey,
            Mode = mode,
            Cube = cube,
            DataItem = dataItem,
            Layer = layer,
            StartDate = sdt,
            EndDate = edt,
            MessageResponseBag = new MessageResponseBag()
        };
    }

    private static CubeDataBulkRequest CreateCubeDataBulkRequest(ElexonImbalancePriceContext elexonImbalancePriceContext,
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

    private async Task<bool> IsCubeOfCubeTypeItemAsync(CubeDataObject cubeDataObject, string cubeItemType, string apiKey)
    {
        var cubeTypeDataObjects = (await cubeTypeService.GetCubeTypeViaApiKey(cubeDataObject.CubeType, apiKey))?.Data?.ToList();
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