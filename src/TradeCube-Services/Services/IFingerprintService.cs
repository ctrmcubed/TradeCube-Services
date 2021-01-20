using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public interface IFingerprintService
    {
        Task<ApiResponseWrapper<IEnumerable<FingerprintResponse>>> FingerprintAsync(string apiKey, FingerprintRequest fingerprintRequest);
    }
}