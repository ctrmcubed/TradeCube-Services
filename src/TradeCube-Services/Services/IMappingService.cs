using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface IMappingService
    {
        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingAsync(string mappingKey, string mappingFrom, string apiKey);

        Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingsAsync(string apiKey);
    }
}