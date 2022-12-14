using System.Threading.Tasks;
using Shared.Messages;

namespace Shared.Services;

public interface IElexonSettlementPeriodService
{
    Task<ElexonSettlementPeriodResponse> ElexonSettlementPeriodsAsync(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest, string apiJwtToken);
}