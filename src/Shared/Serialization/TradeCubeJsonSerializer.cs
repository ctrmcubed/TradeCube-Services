using NLog;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Shared.Serialization
{
    public static class TradeCubeJsonSerializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Serialize(object t)
        {
            return JsonSerializer.Serialize(t, new JsonSerializerOptions { IgnoreNullValues = true, WriteIndented = true });
        }

        public static async Task<TV> DeserializeAsync<TV>(Stream stream, JsonSerializerOptions jsonSerializerOptions = null)
        {
            try
            {
                return jsonSerializerOptions is null
                    ? await JsonSerializer.DeserializeAsync<TV>(stream)
                    : await JsonSerializer.DeserializeAsync<TV>(stream, jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);

                stream.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(stream, Encoding.UTF8);
                var value = await reader.ReadToEndAsync();

                Logger.Error($"Raw response: {value}");

                throw new FormatException(value);
            }
        }
    }
}
