using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

    // https://api.bmreports.com/BMRS/DERSYSDATA/v1?APIKey=xxsk4wq2o1mc2l5&FromSettlementDate=2021-11-09&ToSettlementDate=2021-11-25&SettlementPeriod=*&ServiceType=xml

    public async Task<DerivedSystemWideData> DerivedSystemWideData(DerivedSystemWideDataRequest derivedSystemWideDataRequest)
    {
        IEnumerable<string> QueryStrings(DerivedSystemWideDataRequest request)
        {
            return new List<string>
            {
                $"APIKey={request.ElexonApiKey}",
                $"FromSettlementDate={request.FromSettlementDate}",
                $"ToSettlementDate={request.ToSettlementDate}",
                $"SettlementPeriod={request.SettlementPeriod}",
                $"ServiceType={request.ServiceType}"
            };
        }

        try
        {
            var httpClient = HttpClientFactory.CreateClient();
            var queryString = string.Join('&', QueryStrings(derivedSystemWideDataRequest));
            var url = $"{derivedSystemWideDataRequest.Url}?{queryString}";
            var httpResponseMessage = await httpClient.GetAsync(url);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var content = httpResponseMessage.Content;
                var derivedSystemWideData = new TradeCubeXmlSerializer().Deserialize<DerivedSystemWideData>(await content.ReadAsStreamAsync(), "response");

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

                return null;
            }

            logger.LogInformation("DerivedSystemWideData failure response {StatusCode}, {Reason}", httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);

            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            throw;
        }
    }
}