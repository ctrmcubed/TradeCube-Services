using System.Threading.Tasks;

namespace Shared.Services.Redis
{
    public interface IRedisService
    {
        Task<T> Get<T>(params string[] keys);
        Task Set<T>(T item, params string[] keys);
        Task Set<T>(T item, int expiresInHours, params string[] keys);
        Task Clear(params string[] keys);
    }
}