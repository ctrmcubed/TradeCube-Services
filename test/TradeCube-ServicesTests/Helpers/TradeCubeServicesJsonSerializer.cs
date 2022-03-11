using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TradeCube_ServicesTests.Helpers
{
    public static class TradeCubeServicesJsonSerializer
    {
        public static string Serialize(object t)
        {
            return JsonSerializer.Serialize(t, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
    }

    public static class TradeCubeServicesJsonSerializer<T>
    {
        public static T DeserializeFile(string filename)
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(filename));
        }
    }
}