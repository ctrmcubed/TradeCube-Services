using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public interface ICubeService
{
    Task<ApiResponseWrapper<IEnumerable<CubeDataObject>>> GetCube(string cubeKey, string jwtApiToken);
}