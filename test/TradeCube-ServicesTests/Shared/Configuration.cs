using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace TradeCube_ServicesTests.Shared
{
    public class Configuration
    {
        public static void SetEnvironmentVariables()
        {
            const string launchSettings = "Properties\\launchSettings.json";

            if (!File.Exists(launchSettings))
            {
                return;
            }

            using var file = File.OpenText(launchSettings);
            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);

            var variables = jObject
                .GetValue("profiles")?
                .SelectMany(profiles => profiles.Children())
                .SelectMany(profile => profile.Children<JProperty>())
                .Where(prop => prop.Name == "environmentVariables")
                .SelectMany(prop => prop.Value.Children<JProperty>())
                .ToList();

            if (variables == null)
            {
                return;
            }

            foreach (var variable in variables)
            {
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
            }
        }

        public static IConfigurationRoot AppSettings()
        {
            return new ConfigurationBuilder().AddJsonFile("appconfig.json").Build();
        }

    }
}