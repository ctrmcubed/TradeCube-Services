using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Shared.DataObjects;

[DebuggerDisplay("Cube={Cube}, CubeType={CubeType}")]
public class CubeDataObject 
{
    public string Cube { get; init; }
    public string CubeLongName { get; init; }
    public string CubeType { get; init; }
    public string DataHierarchy { get; init; }
    public string TimeHierarchy { get; init; }
    public IEnumerable<string> DataItems { get; init; }
    public string Commodity { get; init; }
    public string Formula { get; init; }
    public string Unit { get; init; }
    public bool? InvertSign { get; init; }
    public DateTime? LastUpdated { get; init; }
    public string LastUpdatedBy { get; init; }
    public string LastUpdatedReason { get; init; }
    public string CalculationError { get; init; }
    public bool? EnableHistorian { get; init; }
    public IEnumerable<CubeDataItemPair> Calculation { get; init; }
    public IEnumerable<string> Layers { get; init; }
}