using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public interface ICountryService
    {
        Task<ApiResponseWrapper<IEnumerable<CountryDataObject>>> CountriesAsync(string apiJwtToken);
    }
}