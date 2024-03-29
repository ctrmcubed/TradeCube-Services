﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Shared.Serialization;
using Shared.Types.Elexon;

namespace Shared.Services;

public class ElexonService : IElexonService
{
    private readonly ILogger<ElexonService> logger;

    private IHttpClientFactory HttpClientFactory { get; }

    public ElexonService(IHttpClientFactory httpClientFactory, ILogger<ElexonService> logger)
    {
        HttpClientFactory = httpClientFactory;
        this.logger = logger;
    }
    
    public DerivedSystemWideData DeserializeDerivedSystemWideData(string response)
    {
        try
        {
            return string.IsNullOrWhiteSpace(response)
                ? new DerivedSystemWideData()
                : new TradeCubeXmlSerializer().Deserialize<DerivedSystemWideData>(response, "response");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }

    public async Task<DerivedSystemWideData> DerivedSystemWideData(DerivedSystemWideDataRequest derivedSystemWideDataRequest)
    {
         IEnumerable<string> QueryStrings(DerivedSystemWideDataRequest request) =>
            new List<string>
            {
                $"APIKey={request.ApiKey}",
                $"FromSettlementDate={request.FromSettlementDate}",
                $"ToSettlementDate={request.ToSettlementDate}",
                $"SettlementPeriod={request.SettlementPeriod}",
                $"ServiceType={request.ServiceType}"
            };

        try
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)),
                    (exception, timeSpan, retryCount, _) =>
                    {
                        logger.LogWarning("Retry attempt {RetryCount}: waiting {TotalSeconds} to retry ({Message})",
                            retryCount, timeSpan.TotalSeconds, exception.Message);
                    });
            
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var httpClient = HttpClientFactory.CreateClient();
                var queryString = string.Join('&', QueryStrings(derivedSystemWideDataRequest));
                var url = $"{derivedSystemWideDataRequest.Url}?{queryString}";
                var httpResponseMessage = await httpClient.GetAsync(url);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var content = httpResponseMessage.Content;
                    var derivedSystemWideData = DeserializeDerivedSystemWideData(await content.ReadAsStreamAsync());

                    logger.LogInformation("DerivedSystemWideData success response {StatusCode}, {Reason}, {MetadataHttpCode}, {MetadataDescription}",
                        httpResponseMessage.StatusCode,
                        httpResponseMessage.ReasonPhrase,
                        derivedSystemWideData.ResponseMetadata.HttpCode,
                        derivedSystemWideData.ResponseMetadata.Description);

                    if (derivedSystemWideData.ResponseMetadata.HttpCode == 200 && derivedSystemWideData.ResponseMetadata.Description == "Success")
                    {
                        return derivedSystemWideData;
                    }

                    logger.LogError("DerivedSystemWideData failure response {Description}, {HttpCode}, {StatusCode}, {Reason}",
                        derivedSystemWideData.ResponseMetadata?.Description,
                        derivedSystemWideData.ResponseMetadata?.HttpCode,
                        httpResponseMessage.StatusCode,
                        httpResponseMessage.ReasonPhrase);

                    return derivedSystemWideData;
                }

                logger.LogInformation("DerivedSystemWideData failure response {StatusCode}, {Reason}", httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);

                return null;
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }
    
    private DerivedSystemWideData DeserializeDerivedSystemWideData(Stream response)
    {
        try
        {
            return new TradeCubeXmlSerializer().Deserialize<DerivedSystemWideData>(response, "response");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }
    
}