using Microsoft.Extensions.Logging;
using Shared.Serialization;

namespace Shared.Extensions
{
    public static class LoggerExtensions
    {
        public static void JsonLogDebug<T, TV>(this ILogger<T> logger, TV body) =>
            logger.LogDebug(TradeCubeJsonSerializer.Serialize(body));

        public static void JsonLogDebug<T, TV>(this ILogger<T> logger, string message, TV body) =>
            logger.LogDebug(
                $"{message}: {TradeCubeJsonSerializer.Serialize(body)}");
    }
}