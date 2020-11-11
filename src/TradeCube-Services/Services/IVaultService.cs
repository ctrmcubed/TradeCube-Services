using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface IVaultService
    {
        Task<ApiResponseWrapper<IEnumerable<VaultDataObject>>> Vault(string apiJwtToken, string key);
    }
}