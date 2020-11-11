using Newtonsoft.Json;

namespace TradeCube_Services.Serialization
{
    public static class TradeCubeJsonSerializer
    {
        public static string Serialize(object t)
        {
            return JsonConvert.SerializeObject(t);
        }

        public static string Serialize(object t, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(t, settings);
        }
    }
}