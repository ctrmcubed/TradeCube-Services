using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IMappingService
    {
        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingViaApiKeyAsync(string mappingKey, string mappingFrom, string apiJwtToken);
        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingsViaApiKeyAsync(string apiKey);
        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingViaJwtAsync(string mappingKey, string mappingFrom, string apiJwtToken);
        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingsViaJwtAsync(string apiJwtToken);
    }
}