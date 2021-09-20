namespace Shared.Configuration
{
    public interface IFidectusConfiguration
    {
        string FidectusUrl { get; }
        string FidectusAuthUrl { get; }
        string FidectusAudience { get; }
        string FidectusConfirmationUrl { get; }
        string FidectusConfirmationBoxResultUrl { get; }

        string CompanyId(string senderId = null);
        string GetSetting(string key, string defaultValue);
        string GetMappingTo(string key, string mappingFrom);
    }
}