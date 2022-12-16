using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Messages;

namespace Shared.Services;

public interface IElexonSettlementPeriodService
{
    Task<ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>>> ElexonSettlementPeriodsAsync(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest, string apiJwtToken);
}