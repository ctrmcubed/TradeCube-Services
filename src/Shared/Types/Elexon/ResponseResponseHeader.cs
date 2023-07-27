namespace Shared.Types.Elexon;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class ResponseResponseHeader
{

    private string recordTypeField;

    private string fileTypeField;

    /// <remarks/>
    public string recordType
    {
        get
        {
            return this.recordTypeField;
        }
        set
        {
            this.recordTypeField = value;
        }
    }

    /// <remarks/>
    public string fileType
    {
        get
        {
            return this.fileTypeField;
        }
        set
        {
            this.fileTypeField = value;
        }
    }
}