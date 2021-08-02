using Shared.Helpers;
using System;
using System.Data;

namespace Shared.Configuration
{
    public class FidectusConfiguration : IFidectusConfiguration
    {
        public string FidectusUrl { get; }
        public string FidectusAuthUrl { get; }
        public string FidectusAudience { get; }
        public string FidectusConfirmationUrl { get; }

        private readonly SettingHelper settingHelper;
        private readonly MappingHelper mappingHelper;

        public FidectusConfiguration(SettingHelper settingHelper, MappingHelper mappingHelper)
        {
            this.settingHelper = settingHelper;
            this.mappingHelper = mappingHelper;

            FidectusUrl = settingHelper.GetSetting("FIDECTUS_URL");
            FidectusAuthUrl = settingHelper.GetSetting("FIDECTUS_AUTH_URL");
            FidectusAudience = settingHelper.GetSetting("FIDECTUS_AUDIENCE");

            FidectusConfirmationUrl = Environment.GetEnvironmentVariable("FIDECTUS_CONFIRMATION_URI");
        }

        public string CompanyId(string senderId = null)
        {
            string CompanyIdSetting()
            {
                var fidectusCompanyIdSetting = settingHelper.GetSetting("FIDECTUS_COMPANYID");

                return string.IsNullOrWhiteSpace(fidectusCompanyIdSetting)
                    ? throw new DataException("The FIDECTUS_COMPANYID is not configured in the system settings")
                    : fidectusCompanyIdSetting;
            }

            var fidectusCompanyIdMapping = string.IsNullOrEmpty(senderId)
                ? null
                : mappingHelper.GetMappingTo("FIDECTUS_CompanyId", senderId);

            var companyId = string.IsNullOrEmpty(fidectusCompanyIdMapping)
                ? CompanyIdSetting()
                : fidectusCompanyIdMapping;

            return companyId;
        }
    }
}