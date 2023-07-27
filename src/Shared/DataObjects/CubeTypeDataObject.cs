using System.Collections.Generic;

namespace Shared.DataObjects;

public class CubeTypeDataObject
{
    public string CubeType { get; init; }
    public IEnumerable<string> EnabledItems { get; init; }
}