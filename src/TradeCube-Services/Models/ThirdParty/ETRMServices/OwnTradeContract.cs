namespace TradeCube_Services.Models.ThirdParty.ETRMServices
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class OwnTradeContract
    {

        private string prodField;

        private string nameField;

        private string longNameField;

        private System.DateTime dlvryStartUTCField;

        private System.DateTime dlvryStartLocalField;

        private System.DateTime dlvryEndUTCField;

        private System.DateTime dlvryEndLocalField;

        private bool predefinedField;

        private string stateField;

        private string tradingPhaseField;

        private decimal durationField;

        private System.DateTime actPointUTCField;

        private System.DateTime actPointLocalField;

        private System.DateTime expPointUTCField;

        private System.DateTime expPointLocalField;

        private decimal delUnitsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string prod
        {
            get
            {
                return this.prodField;
            }
            set
            {
                this.prodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string longName
        {
            get
            {
                return this.longNameField;
            }
            set
            {
                this.longNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime dlvryStartUTC
        {
            get
            {
                return this.dlvryStartUTCField;
            }
            set
            {
                this.dlvryStartUTCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime dlvryStartLocal
        {
            get
            {
                return this.dlvryStartLocalField;
            }
            set
            {
                this.dlvryStartLocalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime dlvryEndUTC
        {
            get
            {
                return this.dlvryEndUTCField;
            }
            set
            {
                this.dlvryEndUTCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime dlvryEndLocal
        {
            get
            {
                return this.dlvryEndLocalField;
            }
            set
            {
                this.dlvryEndLocalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool predefined
        {
            get
            {
                return this.predefinedField;
            }
            set
            {
                this.predefinedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tradingPhase
        {
            get
            {
                return this.tradingPhaseField;
            }
            set
            {
                this.tradingPhaseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime actPointUTC
        {
            get
            {
                return this.actPointUTCField;
            }
            set
            {
                this.actPointUTCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime actPointLocal
        {
            get
            {
                return this.actPointLocalField;
            }
            set
            {
                this.actPointLocalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime expPointUTC
        {
            get
            {
                return this.expPointUTCField;
            }
            set
            {
                this.expPointUTCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime expPointLocal
        {
            get
            {
                return this.expPointLocalField;
            }
            set
            {
                this.expPointLocalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal delUnits
        {
            get
            {
                return this.delUnitsField;
            }
            set
            {
                this.delUnitsField = value;
            }
        }
    }
}