using Shared.Helpers;

namespace Fidectus.Helpers
{
    public class ConfigurationHelper
    {
        public SettingHelper SettingHelper { get; }
        public MappingHelper MappingHelper { get; }

        public ConfigurationHelper(MappingHelper mappingHelper, SettingHelper settingHelper)
        {
            MappingHelper = mappingHelper;
            SettingHelper = settingHelper;
        }
    }
}
