namespace Shared.Configuration
{
    public interface IFidectusConfiguration
    {
        public string FidectusUrl { get; }
        public string FidectusAuthUrl { get; }
        public string FidectusAudience { get; }
        public string FidectusConfirmationUrl { get; }

        string CompanyId(string senderId = null);
        string GetSetting(string key, string defaultValue);
        string GetMappingTo(string key, string mappingFrom);
    }
}