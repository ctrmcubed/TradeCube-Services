using System.Threading.Tasks;
using Shared.Types.Elexon;

namespace Shared.Services;

public interface IElexonService
{
    Task<DerivedSystemWideData> DerivedSystemWideData(DerivedSystemWideDataRequest derivedSystemWideDataRequest);
}