using System;
using System.Diagnostics;

namespace Shared.DataObjects;

[DebuggerDisplay("UTCStartDateTime={UTCStartDateTime}, Value={Value}")]
public class ProfileType
{
    public string LocalStartDateTime { get; set; }
    public string LocalEndDateTime { get; set; }

    public DateTime? UTCStartDateTime { get; set; }
    public DateTime? UTCEndDateTime { get; set; }

    public decimal? Value { get; set; }
}