using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TradeCube_ServicesTests.Helpers
{
    public static class TestHelper
    {
        public static string GetTestDataFolder(string filename) =>
            Path.Combine(AppContext.BaseDirectory, filename);

        public static string GetTestDataFolder(string testDataFolder, string filename) => 
            Path.Combine(AppContext.BaseDirectory, testDataFolder, filename);

        public static Logger<T> CreateNullLogger<T>() => 
            new(new NullLoggerFactory());
    }
}