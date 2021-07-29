using Microsoft.Extensions.Logging;
using Shared.Services;
using System.Net.Http;

namespace Fidectus.Services
{
    public class FidectusService : ApiService, IFidectusService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ApiService> logger;

        public FidectusService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
    }
}
