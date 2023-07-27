namespace Shared.Types.Elexon;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class ResponseResponseBody
{

    private ResponseResponseBodyItem[] responseListField;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
    public ResponseResponseBodyItem[] responseList
    {
        get
        {
            return this.responseListField;
        }
        set
        {
            this.responseListField = value;
        }
    }
}