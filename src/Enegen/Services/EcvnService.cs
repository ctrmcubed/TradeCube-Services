using Enegen.Messages;
using Microsoft.Extensions.Logging;
using Shared.Messages;

namespace Enegen.Services
{
    public class EcvnService : IEcvnService
    {
        private readonly ILogger<EcvnService> logger;

        public EcvnService(ILogger<EcvnService> logger)
        {
            this.logger = logger;
        }

        public Task<ApiResponseWrapper<EnegenGenstarEcvnResponse>> NotifyAsync(EnegenGenstarEcvnRequest ecvnRequest, string apiJwtToken)
        {
            throw new NotImplementedException();
        }
    }
}