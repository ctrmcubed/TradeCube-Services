using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services
{
    public interface IVaultService
    {
        Task<ApiResponseWrapper<IEnumerable<VaultDataObject>>> GetVaultValueAsync(string vaultKey, string jwtApiToken);
    }
}