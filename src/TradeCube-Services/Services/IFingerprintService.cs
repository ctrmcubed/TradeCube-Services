using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface IFingerprintService
    {
        Task<ApiResponseWrapper<IEnumerable<FingerprintResponse>>> FingerprintAsync(string apiKey, FingerprintRequest fingerprintRequest);
    }
}