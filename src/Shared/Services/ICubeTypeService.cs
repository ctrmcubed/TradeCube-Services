using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public interface ICubeTypeService
{
    Task<ApiResponseWrapper<IEnumerable<CubeTypeDataObject>>> GetCube(string cubeType, string jwtApiToken);
}