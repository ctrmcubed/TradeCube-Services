using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Messages;
using Shared.Services;
using Shared.Types.Elexon;

namespace Shared.Managers;

public class ElexonImbalancePriceManager : IElexonImbalancePriceManager
{
    private readonly IVaultService vaultService;
    private readonly ISettingService settingService;
    private readonly IElexonService elexonService;
    private readonly ICubeService cubeService;
    private readonly ILogger<ElexonImbalancePriceManager> logger;

    public ElexonImbalancePriceManager(IVaultService vaultService, ISettingService settingService,
        IElexonService elexonService, ICubeService cubeService, ILogger<ElexonImbalancePriceManager> logger)
    {
        this.vaultService = vaultService;
        this.settingService = settingService;
        this.elexonService = elexonService;
        this.cubeService = cubeService;
        this.logger = logger;
    }
    
    public async Task<ElexonImbalancePriceResponse> ElexonImbalancePrice(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        try
        {
            var elexonImbalancePriceContext = await CreateContext(elexonImbalancePriceRequest);
            var derivedSystemWideData = await elexonService.DerivedSystemWideData(new DerivedSystemWideDataRequest
            {
                ElexonApiKey = elexonImbalancePriceContext.ElexonApiKey,
                FromSettlementDate = elexonImbalancePriceContext.StartDate.ToString("yyyy-MM-dd"),
                ToSettlementDate = elexonImbalancePriceContext.EndDate.ToString("yyyy-MM-dd"),
                SettlementPeriod = "*",
                ServiceType = "xml"
            });
            
            logger.JsonLogDebug("Envelope", derivedSystemWideData);
            
            // var imbalancePriceSetting = await settingService.FindBySettingByNameAsync(SettingConstants.PeakGenElexonImbalancePriceCube, tenant);
            // var targetCube = string.IsNullOrWhiteSpace(imbalancePriceSetting.SettingValue)
            //     ? "Imbalance Price"
            //     : imbalancePriceSetting.SettingValue;
            //
            // var grouped = derivedSystemWideData?.responseBody?.responseList
            //     .GroupBy(d => d.settlementDate)
            //     .Select(x => new
            //     {
            //         Date = x.Key,
            //         Values = x.ToList()
            //     });
            //
            // var data = grouped?
            //     .Select(m => new CubeDataBulkData
            //     {
            //         StartDateTimeLocal = m.Date.ToIso8601Date(),
            //         Values = m.Values.Select(v => v.systemSellPrice)
            //     })
            //     .ToList();
            //
            // var cubeDataBulkRequest = new CubeDataBulkRequest
            // {
            //     Name = "PeakGen Imbalance Price Load",
            //     Cube = targetCube,
            //     DataItem = DataItemConstants.DataItemPrice,
            //     Node = "SBP",
            //     Unit = UnitConstants.GBPPerMWh,
            //     Timezone = TimezoneConstants.TimezoneEuropeLondon,
            //     ShortDayRule = ClockChangeConstants.ClockChangeSequential,
            //     LongDayRule = ClockChangeConstants.ClockChangeSequential,
            //     RegularDayPeriods = 48,
            //     CreateNodes = true,
            //     Reason = "Imbalance Price Automated Load",
            //     Data = data
            // };
            //
            // var responseWrapper = await scafellWebApiService
            //     .PostViaApiKeyAsync<CubeDataBulkRequest, ApiResponseWrapper<IEnumerable<CubeDataDataObject>>>(
            //         request.ApiKey, cubeDataBulkRequest, "CubeDataBulk", false);
            //
            // if (responseWrapper.IsSuccessStatusCode)
            // {
            //     logger.LogInformation("PeakGen Imbalance Price Load success");
            //
            //     // await Notifications(NotificationType.Ok, request, "PeakGen Imbalance Price Load success", tenant);
            //
            //     return new ElexonImbalancePriceResponse();
            // }
            
            return new ElexonImbalancePriceResponse();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            return new ElexonImbalancePriceResponse
            {
                Status = ApiConstants.FailedResult,
                Message   = ex.Message
            };
        }
    }

    private async Task<ElexonImbalancePriceContext> CreateContext(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
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
            throw new ElexonImbalancePriceException($"The EndDate '{edt.ToIso8601Date()}' is before the StartDate '{sdt.ToIso8601Date()}'.");
        }
        
        var isStandaloneMode = IsMode(mode, ElexonImbalancePriceConstants.ModeStandalone);
        
        var elexonApiKey = isStandaloneMode && string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.ElexonApiKey)
            ? throw new ElexonImbalancePriceException("An Elexon API key must be provided")
            : (await vaultService.GetVaultValueAsync(VaultConstants.ElexonApiKey, elexonImbalancePriceRequest.ApiKey))?.Data?.SingleOrDefault()?.VaultValue;

        if (string.IsNullOrWhiteSpace(elexonApiKey))
        {
            throw new ElexonImbalancePriceException($"Missing Vault Value ({VaultConstants.ElexonApiKey})");
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
                EndDate = edt
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

        if (cubeDataObject.CubeType != CubeConstants.CubeTypeTimeSeries)
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
            EndDate = edt         
        };
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