namespace TradeCube_Services.Models.ThirdParty.ETRMServices
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class OwnTradeProduct
    {

        private string prodTypeField;

        private string dsplNameField;

        private string currencyField;

        private byte lotSizeField;

        private byte tickSizeField;

        private string exchangeIdField;

        private string executionRestrictionField;

        private string timeZoneField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string prodType
        {
            get
            {
                return this.prodTypeField;
            }
            set
            {
                this.prodTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dsplName
        {
            get
            {
                return this.dsplNameField;
            }
            set
            {
                this.dsplNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte lotSize
        {
            get
            {
                return this.lotSizeField;
            }
            set
            {
                this.lotSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte tickSize
        {
            get
            {
                return this.tickSizeField;
            }
            set
            {
                this.tickSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string exchangeId
        {
            get
            {
                return this.exchangeIdField;
            }
            set
            {
                this.exchangeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string executionRestriction
        {
            get
            {
                return this.executionRestrictionField;
            }
            set
            {
                this.executionRestrictionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string timeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }
    }
}

