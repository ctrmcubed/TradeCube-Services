using Shared.Messages;

namespace Shared.Managers;

public interface IUkPowerManager
{
    ElexonSettlementPeriodResponse ComputeElexonSettlementPeriods(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest);
}