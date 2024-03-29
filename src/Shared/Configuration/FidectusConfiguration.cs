﻿using Shared.Helpers;
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
        public string FidectusConfirmationBoxResultUrl { get; }

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
            FidectusConfirmationBoxResultUrl = Environment.GetEnvironmentVariable("FIDECTUS_CONFIRMATION_BOX_RESULT_URI");
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

            var fidectusCompanyIdMapping = string.IsNullOrWhiteSpace(senderId)
                ? null
                : mappingHelper.GetMappingTo("FIDECTUS_CompanyId", senderId);

            var companyId = string.IsNullOrWhiteSpace(fidectusCompanyIdMapping)
                ? CompanyIdSetting()
                : fidectusCompanyIdMapping;

            return companyId;
        }

        public string GetSetting(string key, string defaultValue)
        {
            return settingHelper.GetSetting(key, defaultValue);
        }

        public string GetMappingTo(string key, string mappingFrom)
        {
            return mappingHelper.GetMappingTo(key, mappingFrom);
        }
    }
}