using System.Collections.Generic;
using Shared.Messages;

namespace Shared.Types.CubeDataBulk;

public class CubeDataBulkResponse : ApiResponse
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