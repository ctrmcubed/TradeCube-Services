using Shared.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Helpers
{
    public class SettingHelper
    {
        private readonly Dictionary<string, string> settings;

        public SettingHelper(IEnumerable<SettingDataObject> settings)
        {
            this.settings = settings?
                .Where(s => s.SettingName is not null)
                .ToDictionary(k => k.SettingName, v => v.SettingValue);
        }

        public string GetSetting(string key, string defaultValue = null)
        {
            if (settings is null)
            {
                return defaultValue;
            }

            return settings.ContainsKey(key)
                ? settings[key]
                : defaultValue;
        }
    }
}