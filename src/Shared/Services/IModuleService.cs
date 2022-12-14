using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public interface IModuleService
{
    Task<ApiResponseWrapper<IEnumerable<ModuleDataObject>>> ModulesAsync(string apiJwtToken);
    bool IsEnabled(string module, IEnumerable<ModuleDataObject> moduleDataObjects);
}