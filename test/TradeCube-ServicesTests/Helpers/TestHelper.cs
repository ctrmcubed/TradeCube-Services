using System;
using System.IO;

namespace TradeCube_ServicesTests.Helpers
{
    public static class TestHelper
    {
        public static string GetTestDataFolder(string filename)
        {
            var startupPath = AppContext.BaseDirectory;
            return Path.Combine(startupPath, filename);
        }

        public static string GetTestDataFolder(string testDataFolder, string filename)
        {
            var startupPath = AppContext.BaseDirectory;
            return Path.Combine(startupPath, testDataFolder, filename);
        }
    }
}