using System;
using System.Collections.Generic;
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
    private readonly IElexonSettlementPeriodManager elexonSettlementPeriodManager;
    private readonly IElexonService elexonService;
    private readonly ILogger<ElexonImbalancePriceManager> logger;

    public ElexonImbalancePriceManager(IElexonSettlementPeriodManager elexonSettlementPeriodManager,
        IElexonService elexonService, ILogger<ElexonImbalancePriceManager> logger)
    {
        this.elexonSettlementPeriodManager = elexonSettlementPeriodManager;
        this.elexonService = elexonService;
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
    
    public ElexonImbalancePriceContext CreateContext(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        try
        {
            return CreateContext2(elexonImbalancePriceRequest);
        }
        catch (Exception ex)
        {
            return new ElexonImbalancePriceContext
            {
                MessageResponseBag = new MessageResponseBag(ex.Message, MessageResponseType.Error)
            };
        }
    }
    
    public async Task<ElexonImbalancePriceResponse> ElexonImbalancePrice(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        string ErrorType(string errorType) => 
            string.IsNullOrWhiteSpace(errorType) ? string.Empty : $"Error Type '{errorType}'";

        string Description (string description) => 
            string.IsNullOrWhiteSpace(description) ? string.Empty : $"Description '{description}'";
        
        var elexonImbalancePriceContext = CreateContext(elexonImbalancePriceRequest);
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

        if (elexonImbalancePriceResponse.IsSuccess())
        {
            logger.LogInformation("Imbalance Price success");

            return new ElexonImbalancePriceResponse
            {
                Status = ApiConstants.SuccessResult,
                Message = elexonImbalancePriceResponse.Message
            };
        }

        logger.LogInformation("Imbalance Price failure");
        
        return new ElexonImbalancePriceResponse
        {
            Status = ApiConstants.FailedResult,
            Message = elexonImbalancePriceResponse.Message
        };
    }
    
    public ElexonImbalancePriceResponse ElexonImbalancePrice(ElexonImbalancePriceContext elexonImbalancePriceContext, 
        DerivedSystemWideData derivedSystemWideData, IEnumerable<ElexonSettlementPeriodResponseItem> settlementPeriodResponseItems)
    {
        if (elexonImbalancePriceContext.MessageResponseBag.GotErrors())
        {
            return new ElexonImbalancePriceResponse
            {
                Status = ApiConstants.FailedResult,
                Message = elexonImbalancePriceContext.MessageResponseBag.ErrorsAsString()
            };
        }

        return new ElexonImbalancePriceResponse
        {
            Status = ApiConstants.SuccessResult,
            Data = CreateElexonImbalancePriceItems(derivedSystemWideData, settlementPeriodResponseItems)
        };
    }

    public async Task<DerivedSystemWideData> GetElexonDerivedSystemWideData(DerivedSystemWideDataRequest derivedSystemWideDataRequest) =>
        await elexonService.DerivedSystemWideData(derivedSystemWideDataRequest);

    public ElexonSettlementPeriodResponse GetElexonSettlementPeriods(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest) =>
        elexonSettlementPeriodManager.ElexonSettlementPeriods(elexonSettlementPeriodRequest);

    private static ElexonImbalancePriceContext CreateContext2(ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        ArgumentNullException.ThrowIfNull(elexonImbalancePriceRequest);

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
        
        if (string.IsNullOrWhiteSpace(elexonImbalancePriceRequest.ElexonApiKey))
        {
            throw new ElexonImbalancePriceException($"An Elexon API key must be provided");
        }

        return new ElexonImbalancePriceContext
        {
            ApiKey = elexonImbalancePriceRequest.ApiKey,
            ElexonApiKey = elexonImbalancePriceRequest.ElexonApiKey,
            StartDate = sdt,
            EndDate = edt,
            MessageResponseBag = new MessageResponseBag()
        };
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
                StartDateTimeUtc = utcStartDateTime
            };
        }
    }
}