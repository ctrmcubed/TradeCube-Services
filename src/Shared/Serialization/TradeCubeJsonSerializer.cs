using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Serialization
{
    public static class TradeCubeJsonSerializer
    {
        public static string Serialize(object t)
        {
            return JsonSerializer.Serialize(t);
        }

        public static async Task<TV> DeserializeAsync<TV>(Stream stream)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<TV>(stream);
            }
            catch (Exception)
            {
                stream.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(stream, Encoding.UTF8);
                var value = await reader.ReadToEndAsync();

                throw new FormatException(value);
            }
        }
    }
}
