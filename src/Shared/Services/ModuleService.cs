using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public class ModuleService : TradeCubeApiService, IModuleService
{
    protected ModuleService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<ModuleService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
    }

    public async Task<ApiResponseWrapper<IEnumerable<ModuleDataObject>>> ModulesAsync(string apiJwtToken)
    {
        return await GetViaJwtAsync<ModuleDataObject>("Module", apiJwtToken);
    }

    public bool IsEnabled(string module, IEnumerable<ModuleDataObject> moduleDataObjects)
    {
        return moduleDataObjects?.Any(m => m.Module == module && m.Enabled is true) ?? false;
    }
}