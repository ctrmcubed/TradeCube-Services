using Shared.Messages;

namespace Shared.Managers;

public interface IElexonSettlementPeriodManager
{
    ElexonSettlementPeriodResponse ElexonSettlementPeriods(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest);
}