using System;

namespace TradeCube_Services.Helpers
{
    public static class X509Helper
    {
        public static (string name, string password) CertificateInfo(string certNameKey, string certPasswordKey)
        {
            var name = Environment.GetEnvironmentVariable(certNameKey);
            var password = Environment.GetEnvironmentVariable(certPasswordKey);

            return (name, password);
        }

        public static bool IsValidHttpsConfig(int? port, (string name, string password) certificateInfo)
        {
            var (name, password) = certificateInfo;

            return port.HasValue && port > 0 && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(password);
        }
    }
}
