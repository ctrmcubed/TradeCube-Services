using System;

namespace Shared.DataObjects
{
    public class FileUpload
    {
        public string Hash { get; set; }
        public string DisplayName { get; set; }
        public string FileUri { get; set; }
        public DateTime UploadDateTime { get; set; }
        public int Version { get; set; }
        public bool ProfileAttachment { get; set; }
    }
}