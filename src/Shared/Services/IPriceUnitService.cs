using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IPriceUnitService
    {
        Task<ApiResponseWrapper<IEnumerable<PriceUnitDataObject>>> GetPriceUnitAsync(string priceUnit, string apiJwtToken);
    }
}