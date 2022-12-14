namespace Shared.DataObjects;

public class ModuleDataObject
{
    public string Module { get; set; }
    public bool? Enabled { get; set; }

    public ModuleSettingsType ModuleSettings { get; set; }
}