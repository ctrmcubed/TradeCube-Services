using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;

namespace Shared.Serialization
{
    public static class TradeCubeJsonSerializer
    {
        private static readonly ILogger Logger = Log.ForContext(typeof(TradeCubeJsonSerializer));

        public static string Serialize(object t) => 
            JsonSerializer.Serialize(t);

        public static string SerializeIgnoreNulls(object t, bool indented = false) =>
            JsonSerializer.Serialize(t, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = indented
            });

        public static T Deserialize<T>(string values) => 
            JsonSerializer.Deserialize<T>(values);
        
        public static async Task<TV> DeserializeAsync<TV>(Stream stream,
            JsonSerializerOptions jsonSerializerOptions = null)
        {
            try
            {
                return jsonSerializerOptions is null
                    ? await JsonSerializer.DeserializeAsync<TV>(stream)
                    : await JsonSerializer.DeserializeAsync<TV>(stream, jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "{Message}", ex.Message);

                stream.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(stream, Encoding.UTF8);
                var value = await reader.ReadToEndAsync();

                Logger.Error("Raw response: {Value}", value);

                throw new FormatException(value);
            }
        }
    }
}