using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Enegen.Services
{
    public class EcvnService : IEcvnService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<EcvnService> logger;

        public EcvnService(IHttpClientFactory httpClientFactory, ILogger<EcvnService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        
        public async Task<ApiResponseWrapper<string>> NotifyAsync(string uri, string appId,
            string hashedPayload, string nonce, long unixTimeSeconds, string body)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("hmacauth", $"{appId}:{hashedPayload}:{nonce}:{unixTimeSeconds}");
             
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json"); 
                var responseString = await client.PostAsync(uri, stringContent);
                
                var statusCode = (int)responseString.StatusCode;
                var resultBody = await responseString.Content.ReadAsStringAsync();

                logger.LogInformation("Response from {Uri}: '{ResultBody}', Status Code: {StatusCode}", uri, resultBody, statusCode);
                
                return new ApiResponseWrapper<string>
                {
                    StatusCode = statusCode,
                    Data = resultBody
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                throw;
            }
        }
    }
}