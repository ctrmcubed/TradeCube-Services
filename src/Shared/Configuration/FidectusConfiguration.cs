using System;

namespace Shared.Configuration
{
    public class FidectusConfiguration : IFidectusConfiguration
    {
        public string ApiDomain { get; }
        public string RequestTokenUri { get; }

        public FidectusConfiguration()
        {
            RequestTokenUri = Environment.GetEnvironmentVariable("FIDECTUS_REQUEST_TOKEN_URI");
        }

        public FidectusConfiguration(string apiDomain)
        {
            ApiDomain = apiDomain;
            RequestTokenUri = Environment.GetEnvironmentVariable("FIDECTUS_REQUEST_TOKEN_URI");
        }
    }
}