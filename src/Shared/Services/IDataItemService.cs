using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public interface IDataItemService
{
    Task<ApiResponseWrapper<IEnumerable<DataItemDataObject>>> GetDataItem(string dataItem, string jwtApiToken);
    Task<ApiResponseWrapper<IEnumerable<DataItemDataObject>>> GetDataItemViaApiKey(string dataItem, string apiKey);
}