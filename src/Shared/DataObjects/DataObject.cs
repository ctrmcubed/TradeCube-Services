namespace Shared.DataObjects
{
    public class DataObject
    {
        public VisibilityType Visibility { get; set; }

        public InternalFieldsType InternalFields { get; set; }

        public VersionFieldsType Version { get; set; }
    }
}