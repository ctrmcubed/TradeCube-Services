using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TradeCube_ServicesTests.Helpers
{
    public static class FileHelper
    {
        public static JObject ReadJsonFile(string filename)
        {
            using var file = File.OpenText(filename);
            using var reader = new JsonTextReader(file);
            return (JObject)JToken.ReadFrom(reader);
        }

        public static T ReadJsonFile<T>(string filename)
        {
            return TradeCubeServicesJsonSerializer<T>.DeserializeFile(filename);
        }

        public static string ReadTextFile(string filename)
        {
            using var file = File.OpenText(filename);
            return file.ReadToEnd();
        }
    }
}
