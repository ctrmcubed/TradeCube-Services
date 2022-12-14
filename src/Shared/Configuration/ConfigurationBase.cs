namespace Shared.Configuration
{
    public class ConfigurationBase
    {
        protected static string Port(string port)
        {
            return string.IsNullOrWhiteSpace(port)
                ? string.Empty
                : $":{port}";
        }
    }
}