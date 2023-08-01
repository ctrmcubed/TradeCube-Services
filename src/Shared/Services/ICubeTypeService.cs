using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public interface ICubeTypeService
{
    Task<ApiResponseWrapper<IEnumerable<CubeTypeDataObject>>> GetCubeTypes(string jwtApiToken);
    Task<ApiResponseWrapper<IEnumerable<CubeTypeDataObject>>> GetCubeType(string cubeType, string jwtApiToken);
    Task<ApiResponseWrapper<IEnumerable<CubeTypeDataObject>>> GetCubeTypeViaApiKey(string cubeType, string apiKey);
}