using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TradeCube_Services.Serialization
{
    public static class TradeCubeJsonSerializer
    {
        public static string Serialize(object t)
        {
            return JsonSerializer.Serialize(t);
        }

        public static async Task<TV> DeserializeAsync<TV>(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<TV>(stream);
        }
    }
}