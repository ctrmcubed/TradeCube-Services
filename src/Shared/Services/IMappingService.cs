using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IMappingService
    {
        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingAsync(string mappingKey, string mappingFrom, string apiKey);

        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingsAsync(string apiKey);
    }
}