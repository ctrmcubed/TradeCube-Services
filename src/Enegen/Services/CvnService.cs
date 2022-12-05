using Microsoft.Extensions.Logging;
using Shared.Services;

namespace Enegen.Services;

public class CvnService : ApiService, ICvnService
{
    public CvnService(IHttpClientFactory httpClientFactory, ILogger<CvnService> logger) : base(logger)
    {
    }
}