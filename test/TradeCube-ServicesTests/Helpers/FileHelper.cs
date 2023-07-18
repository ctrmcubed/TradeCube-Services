using System.IO;
using System.Text.Json;
using MongoDB.Bson.Serialization;

namespace TradeCube_ServicesTests.Helpers
{
    public static class FileHelper
    {
        public static T ReadJsonFile<T>(string fileName) => 
            TradeCubeJsonSerializer.DeserializeFile<T>(fileName);

        public static T ReadBsonFile<T>(string fileName) => 
            ReadTextFileAsBson<T>(fileName);

        public static string ReadTextFile(string fileName)
        {
            using var file = File.OpenText(fileName);
            return file.ReadToEnd();
        }

        public static T ReadTextFileAsBson<T>(string fileName) => 
            BsonSerializer.Deserialize<T>(File.OpenText(fileName).ReadToEnd());
    
        public static void WriteJsonToFile<T>(string fileName, T data) => 
            TradeCubeJsonSerializer.SerializeToFile(data, fileName, new JsonSerializerOptions(){WriteIndented = true});
    }
}