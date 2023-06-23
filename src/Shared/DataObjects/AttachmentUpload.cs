using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects;

public class AttachmentUpload
{
    private const char Separator = '_';

    public string Hash { get; set; }
    public string DisplayName { get; set; }
    public string FileUri { get; set; }
    public DateTime UploadDateTime { get; set; }
    public int Version { get; set; }
    public bool ProfileAttachment { get; set; }
        
    [BsonIgnore]
    public string SharedAccessLink { get; set; }

    [BsonIgnore]
    public string ContentType { get; set; }

    [BsonIgnore]
    public string Icon { get; set; }
}