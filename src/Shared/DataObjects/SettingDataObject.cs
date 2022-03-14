namespace Shared.DataObjects
{
    public class SettingDataObject
    {
        public string SettingName { get; set; }
        public string SettingCategory { get; set; }
        public string SettingType { get; set; }
        public string SettingValue { get; set; }
        public InternalFieldsType InternalFields { get; init; }
    }
}