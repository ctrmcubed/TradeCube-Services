using System.Collections.Generic;

namespace Shared.DataObjects;

public class EquiasSettingsType
{
    public IEnumerable<string> Commodities { get; set; }
    public bool? AutoSubmitTrades { get; set; }
    public IEnumerable<string> AutoSubmitStatuses { get; set; }
    public string AutoSubmitTag { get; set; }
    public bool? AutoCheckStatus { get; set; }
    public int? CheckStatusEvery { get; set; }

    // ReSharper disable once InconsistentNaming
    public IEnumerable<string> CMCheckStatus { get; set; }

    // ReSharper disable once InconsistentNaming
    public IEnumerable<string> BFCheckStatus { get; set; }

    // ReSharper disable once InconsistentNaming
    public IEnumerable<string> RRCheckStatus { get; set; }

    public IEnumerable<string> FailureAlertUserIds { get; set; }
    public IEnumerable<string> FailureEmailUserIds { get; set; }
    public bool? WithholdNewTrades { get; set; }
}