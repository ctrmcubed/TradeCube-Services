namespace Shared.Configuration
{
    public interface IFidectusConfiguration
    {
        public string FidectusUrl { get; }
        public string FidectusAuthUrl { get; }
        public string FidectusAudience { get; }
        public string FidectusConfirmationUrl { get; }
    }
}