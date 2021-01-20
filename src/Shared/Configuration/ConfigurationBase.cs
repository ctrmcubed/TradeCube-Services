namespace Shared.Configuration
{
    public class ConfigurationBase
    {
        protected static string Port(string port)
        {
            return string.IsNullOrEmpty(port)
                ? string.Empty
                : $":{port}";
        }
    }
}