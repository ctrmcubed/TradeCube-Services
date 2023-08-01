using System.Threading.Tasks;
using Shared.Types.CubeDataBulk;

namespace Shared.Services;

public interface ICubeDataBulkService
{
    Task<CubeDataBulkResponse> CubeDataBulk(CubeDataBulkRequest cubeDataBulkRequest, string apiKey);
}