namespace Shared.Types.Elexon;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class ResponseResponseMetadata
{

    private int httpCodeField;

    private string errorTypeField;

    private string descriptionField;

    private string cappingAppliedField;

    private int cappingLimitField;

    private string queryStringField;

    /// <remarks/>
    public int httpCode
    {
        get
        {
            return this.httpCodeField;
        }
        set
        {
            this.httpCodeField = value;
        }
    }

    /// <remarks/>
    public string errorType
    {
        get
        {
            return this.errorTypeField;
        }
        set
        {
            this.errorTypeField = value;
        }
    }

    /// <remarks/>
    public string description
    {
        get
        {
            return this.descriptionField;
        }
        set
        {
            this.descriptionField = value;
        }
    }

    /// <remarks/>
    public string cappingApplied
    {
        get
        {
            return this.cappingAppliedField;
        }
        set
        {
            this.cappingAppliedField = value;
        }
    }

    /// <remarks/>
    public int cappingLimit
    {
        get
        {
            return this.cappingLimitField;
        }
        set
        {
            this.cappingLimitField = value;
        }
    }

    /// <remarks/>
    public string queryString
    {
        get
        {
            return this.queryStringField;
        }
        set
        {
            this.queryStringField = value;
        }
    }
}