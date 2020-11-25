namespace TradeCube_Services.Models.ThirdParty.ETRMServices
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class OwnTrade
    {

        private OwnTradeContract contractField;

        private OwnTradeProduct productField;

        private uint tradeIdField;

        private string stateField;

        private uint contractIdField;

        private System.DateTime execTimeUTCField;

        private System.DateTime execTimeLocalField;

        private byte revisionNoField;

        private bool prearrangedField;

        private string contractPhaseField;

        private bool decomposedField;

        private string sideField;

        private string clearingAcctTypeField;

        private string dlvryAreaIdField;

        private string acctIdField;

        private uint ordrIdField;

        private string txtField;

        private string aggressorIndicatorField;

        private string usrCodeField;

        private string clOrdrIdField;

        private string mbrIdField;

        private decimal qtyField;

        private decimal pxField;

        /// <remarks/>
        public OwnTradeContract Contract
        {
            get
            {
                return this.contractField;
            }
            set
            {
                this.contractField = value;
            }
        }

        /// <remarks/>
        public OwnTradeProduct Product
        {
            get
            {
                return this.productField;
            }
            set
            {
                this.productField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint tradeId
        {
            get
            {
                return this.tradeIdField;
            }
            set
            {
                this.tradeIdField = value;
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
        public uint contractId
        {
            get
            {
                return this.contractIdField;
            }
            set
            {
                this.contractIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime execTimeUTC
        {
            get
            {
                return this.execTimeUTCField;
            }
            set
            {
                this.execTimeUTCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime execTimeLocal
        {
            get
            {
                return this.execTimeLocalField;
            }
            set
            {
                this.execTimeLocalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte revisionNo
        {
            get
            {
                return this.revisionNoField;
            }
            set
            {
                this.revisionNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool prearranged
        {
            get
            {
                return this.prearrangedField;
            }
            set
            {
                this.prearrangedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string contractPhase
        {
            get
            {
                return this.contractPhaseField;
            }
            set
            {
                this.contractPhaseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool decomposed
        {
            get
            {
                return this.decomposedField;
            }
            set
            {
                this.decomposedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string side
        {
            get
            {
                return this.sideField;
            }
            set
            {
                this.sideField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clearingAcctType
        {
            get
            {
                return this.clearingAcctTypeField;
            }
            set
            {
                this.clearingAcctTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dlvryAreaId
        {
            get
            {
                return this.dlvryAreaIdField;
            }
            set
            {
                this.dlvryAreaIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string acctId
        {
            get
            {
                return this.acctIdField;
            }
            set
            {
                this.acctIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint ordrId
        {
            get
            {
                return this.ordrIdField;
            }
            set
            {
                this.ordrIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string txt
        {
            get
            {
                return this.txtField;
            }
            set
            {
                this.txtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string aggressorIndicator
        {
            get
            {
                return this.aggressorIndicatorField;
            }
            set
            {
                this.aggressorIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string usrCode
        {
            get
            {
                return this.usrCodeField;
            }
            set
            {
                this.usrCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clOrdrId
        {
            get
            {
                return this.clOrdrIdField;
            }
            set
            {
                this.clOrdrIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mbrId
        {
            get
            {
                return this.mbrIdField;
            }
            set
            {
                this.mbrIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal qty
        {
            get
            {
                return this.qtyField;
            }
            set
            {
                this.qtyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal px
        {
            get
            {
                return this.pxField;
            }
            set
            {
                this.pxField = value;
            }
        }
    }
}