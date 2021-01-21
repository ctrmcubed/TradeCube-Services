using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ICashflowService
    {
        Task<ApiResponseWrapper<IEnumerable<CashflowType>>> CashflowAsync(string tradeReference, int tradeLeg, string apiJwtToken);
    }
}