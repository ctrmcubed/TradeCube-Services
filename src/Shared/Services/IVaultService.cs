using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IVaultService
    {
        Task<ApiResponseWrapper<IEnumerable<VaultDataObject>>> GetVaultValueAsync(string vaultKey, string apiKey);
    }
}