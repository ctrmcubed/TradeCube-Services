using System.Collections.Generic;

namespace Shared.Types.CubeDataBulk;

public class CubeDataBulkResponse
{
    private readonly List<CubeDataDataObject> deletes = new();
    
    public bool DataChanged { get; init; }
    public IEnumerable<CubeDataDataObject> CubeData { get; init; }
    public IEnumerable<CubeDataDataObject> CubeDataDeletes { get; init; }
    
    public void AddDeletes(IEnumerable<CubeDataDataObject> cubeDataDataObjects) => 
        deletes.AddRange(cubeDataDataObjects);
    
    public IEnumerable<CubeDataDataObject> Deletes() => 
        deletes;
}