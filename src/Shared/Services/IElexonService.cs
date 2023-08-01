using System.Threading.Tasks;
using Shared.Types.Elexon;

namespace Shared.Services;

public interface IElexonService
{
    DerivedSystemWideData DeserializeDerivedSystemWideData(string response);
    Task<DerivedSystemWideData> DerivedSystemWideData(DerivedSystemWideDataRequest derivedSystemWideDataRequest);
}