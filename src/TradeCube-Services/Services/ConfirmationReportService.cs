using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TradeCube_Services.Parameters;

namespace TradeCube_Services.Services
{
    public class ConfirmationReportService : IConfirmationReportService
    {
        private readonly ITradeService tradeService;
        private readonly IReportTemplateService reportTemplateService;
        private readonly IReportRenderService reportRenderService;
        private readonly ILogger<ConfirmationReportService> logger;
        private readonly ICountryLookupService countryLookupService;

        public ConfirmationReportService(ITradeService tradeService, ICountryLookupService countryLookupService, IReportTemplateService reportTemplateService, IReportRenderService reportRenderService,
            ILogger<ConfirmationReportService> logger)
        {
            this.tradeService = tradeService;
            this.countryLookupService = countryLookupService;
            this.reportTemplateService = reportTemplateService;
            this.reportRenderService = reportRenderService;
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<WebServiceResponse>> CreateReportAsync(ConfirmationReportParameters confirmationReportParameters)
        {
            try
            {
                var apiJwtToken = confirmationReportParameters.ApiJwtToken;
                var request = new TradeRequest { TradeReferences = confirmationReportParameters.TradeReferences };
                var trades = await tradeService.GetTradesAsync(apiJwtToken, request);

                if (trades.Status == ApiConstants.SuccessResult)
                {
                    var enrichedTrades = await EnrichTradesWithCountries(trades.Data, confirmationReportParameters);
                    var template = await reportTemplateService.ReportTemplateAsync(confirmationReportParameters.Template);
                    var tradeDataObjects = enrichedTrades.ToList();
                    var report = await reportRenderService.RenderAsync(template?.Data?.Html,
                        confirmationReportParameters.Format, tradeDataObjects);
                    var ms = new MemoryStream();

                    report.Content.CopyTo(ms);

                    return new ApiResponseWrapper<WebServiceResponse>
                    {
                        Status = ApiConstants.SuccessResult,
                        Data = new WebServiceResponse
                        {
                            ActionName = confirmationReportParameters.ActionName,
                            Format = confirmationReportParameters.Format,
                            Data = Convert.ToBase64String(ms.ToArray())
                        }
                    };
                }

                logger.LogError("Error calling Trade API", trades.Message);
                return new ApiResponseWrapper<WebServiceResponse> { Status = ApiConstants.FailedResult, Message = trades.Message };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new ApiResponseWrapper<WebServiceResponse> { Status = ApiConstants.FailedResult, Message = ex.Message };
            }
        }

        async Task<IEnumerable<TradeDataObject>> EnrichTradesWithCountries(IEnumerable<TradeDataObject> trades, ReportParametersBase confirmationReportParametersBase)
        {
            await countryLookupService.LoadAsync(confirmationReportParametersBase.ApiJwtToken);

            return SetCountryLongName(trades);
        }

        private IEnumerable<TradeDataObject> SetCountryLongName(IEnumerable<TradeDataObject> trades)
        {
            foreach (var trade in trades)
            {
                // Mutate trades by setting the country property to the country long name 

                if (!string.IsNullOrEmpty(trade?.Buyer?.PrimaryConfirmationContact?.PrimaryAddress?.Country))
                {
                    var buyerCountry = countryLookupService.Lookup(trade.Buyer?.PrimaryConfirmationContact?.PrimaryAddress?.Country);

                    trade.Buyer.PrimaryConfirmationContact.PrimaryAddress.Country = buyerCountry is null
                        ? trade.Buyer?.PrimaryConfirmationContact?.PrimaryAddress?.Country
                        : buyerCountry.CountryLongName;
                }

                if (!string.IsNullOrEmpty(trade?.Seller?.PrimaryConfirmationContact?.PrimaryAddress?.Country))
                {
                    var sellerCountry = countryLookupService.Lookup(trade.Seller?.PrimaryConfirmationContact?.PrimaryAddress?.Country);

                    trade.Seller.PrimaryConfirmationContact.PrimaryAddress.Country = sellerCountry is null
                        ? trade.Seller.PrimaryConfirmationContact.PrimaryAddress.Country
                        : sellerCountry.CountryLongName;
                }

                yield return trade;
            }
        }
    }
}
