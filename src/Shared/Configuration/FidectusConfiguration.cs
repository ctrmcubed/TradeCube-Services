using System;

namespace Shared.Configuration
{
    public class FidectusConfiguration : IFidectusConfiguration
    {
        public string FidectusUrl { get; }
        public string FidectusAuthUrl { get; }
        public string FidectusAudience { get; }
        public string FidectusConfirmationUrl { get; }

        public FidectusConfiguration(string fidectusUrl, string fidectusAuthUrl, string fidectusAudience)
        {
            FidectusUrl = fidectusUrl;
            FidectusAuthUrl = fidectusAuthUrl;
            FidectusAudience = fidectusAudience;

            FidectusConfirmationUrl = Environment.GetEnvironmentVariable("FIDECTUS_CONFIRMATION_URI");
        }
    }
}