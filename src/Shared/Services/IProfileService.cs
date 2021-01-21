using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IProfileService
    {
        Task<ApiResponseWrapper<IEnumerable<ProfileResponse>>> ProfileAsync(string tradeReference, int tradeLeg, string apiJwtToken, string format);
    }
}