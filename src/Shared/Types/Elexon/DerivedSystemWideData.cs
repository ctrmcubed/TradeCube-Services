namespace Shared.Types.Elexon;

public class DerivedSystemWideData
{
    private ResponseResponseMetadata responseMetadataField;

    private ResponseResponseHeader responseHeaderField;

    private ResponseResponseBody responseBodyField;

    /// <remarks/>
    public ResponseResponseMetadata responseMetadata
    {
        get
        {
            return this.responseMetadataField;
        }
        set
        {
            this.responseMetadataField = value;
        }
    }

    /// <remarks/>
    public ResponseResponseHeader ResponseHeader
    {
        get
        {
            return this.responseHeaderField;
        }
        set
        {
            this.responseHeaderField = value;
        }
    }

    /// <remarks/>
    public ResponseResponseBody responseBody
    {
        get
        {
            return this.responseBodyField;
        }
        set
        {
            this.responseBodyField = value;
        }
    }
}