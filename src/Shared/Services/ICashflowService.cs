using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ICashflowService
    {
        Task<ApiResponseWrapper<IEnumerable<CashflowResponse>>> CashflowAsync(string tradeReference, int tradeLeg, string apiJwtToken);
    }
}