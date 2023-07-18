using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;

namespace TradeCube_ServicesTests.Helpers;

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

    public static string SerializeIgnoreNullsUnsafeRelaxedJsonEscaping(object t, bool indented = false) =>
        JsonSerializer.Serialize(t, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = indented,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

    public static void SerializeToFile(object t, string fileName, JsonSerializerOptions jsonSerializerOptions) => 
        File.WriteAllText(fileName, JsonSerializer.Serialize(t, jsonSerializerOptions));

    public static T Deserialize<T>(string values) => 
        JsonSerializer.Deserialize<T>(values);

    public static T DeserializeFile<T>(string fileName, JsonSerializerOptions jsonSerializerOptions = null) =>
        jsonSerializerOptions is null
            ? JsonSerializer.Deserialize<T>(File.ReadAllText(fileName))
            : JsonSerializer.Deserialize<T>(File.ReadAllText(fileName), jsonSerializerOptions);

    public static async Task<TV> DeserializeFileAsync<TV>(string fileName, JsonSerializerOptions jsonSerializerOptions = null) =>
        jsonSerializerOptions is null
            ? JsonSerializer.Deserialize<TV>(await File.ReadAllTextAsync(fileName))
            : JsonSerializer.Deserialize<TV>(await File.ReadAllTextAsync(fileName), jsonSerializerOptions);

    public static async Task<TV> DeserializeStreamAsync<TV>(Stream stream, JsonSerializerOptions jsonSerializerOptions = null)
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

            var value = await new StreamReader(stream, Encoding.UTF8).ReadToEndAsync();

            Logger.Error("Raw response: {Value}", value);

            throw new FormatException(value);
        }
    }
}