﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.8.3928.0.
// 
namespace Equias.Models.Ack {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Reason {
        
        private ReasonCodeType reasonCodeField;
        
        private string errorSourceField;
        
        private string originatorField;
        
        private string reasonTextField;
        
        /// <remarks/>
        public ReasonCodeType ReasonCode {
            get {
                return this.reasonCodeField;
            }
            set {
                this.reasonCodeField = value;
            }
        }
        
        /// <remarks/>
        public string ErrorSource {
            get {
                return this.errorSourceField;
            }
            set {
                this.errorSourceField = value;
            }
        }
        
        /// <remarks/>
        public string Originator {
            get {
                return this.originatorField;
            }
            set {
                this.originatorField = value;
            }
        }
        
        /// <remarks/>
        public string ReasonText {
            get {
                return this.reasonTextField;
            }
            set {
                this.reasonTextField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    public enum ReasonCodeType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("XML:ValidationFailure")]
        XMLValidationFailure,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:ValueNotRecognized")]
        ebxmlValueNotRecognized,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:NotSupported")]
        ebxmlNotSupported,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:Inconsistent")]
        ebxmlInconsistent,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:OtherXML")]
        ebxmlOtherXML,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:DeliveryFailure")]
        ebxmlDeliveryFailure,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:TimeToLiveExpired")]
        ebxmlTimeToLiveExpired,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:SecurityFailure")]
        ebxmlSecurityFailure,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:MimeProblem")]
        ebxmlMimeProblem,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("ebxml:Unknown")]
        ebxmlUnknown,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:InvalidData")]
        efetInvalidData,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:TimeOut")]
        efetTimeOut,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:InvalidMatchAttempt")]
        efetInvalidMatchAttempt,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:AmendmentError")]
        efetAmendmentError,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:IDNotFound")]
        efetIDNotFound,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:UniquenessViolation")]
        efetUniquenessViolation,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:NoMatch")]
        efetNoMatch,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:ReferencedDocNotExists")]
        efetReferencedDocNotExists,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:RefDocInvalidState")]
        efetRefDocInvalidState,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("efet:MinorVersionInInvalidState")]
        efetMinorVersionInInvalidState,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    [System.Xml.Serialization.XmlRootAttribute("CommonPricing", Namespace="", IsNullable=false)]
    public enum CommonPricingType {
        
        /// <remarks/>
        @true,
        
        /// <remarks/>
        @false,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class FixedPriceInformation {
        
        private string fixedPricePayerField;
        
        private string fPCurrencyUnitField;
        
        private UnitOfMeasureType fPCapacityUnitField;
        
        private bool fPCapacityUnitFieldSpecified;
        
        private decimal fPCapacityConversionRateField;
        
        private bool fPCapacityConversionRateFieldSpecified;
        
        private FXInformation fXInformationField;
        
        /// <remarks/>
        public string FixedPricePayer {
            get {
                return this.fixedPricePayerField;
            }
            set {
                this.fixedPricePayerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="NMTOKEN")]
        public string FPCurrencyUnit {
            get {
                return this.fPCurrencyUnitField;
            }
            set {
                this.fPCurrencyUnitField = value;
            }
        }
        
        /// <remarks/>
        public UnitOfMeasureType FPCapacityUnit {
            get {
                return this.fPCapacityUnitField;
            }
            set {
                this.fPCapacityUnitField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FPCapacityUnitSpecified {
            get {
                return this.fPCapacityUnitFieldSpecified;
            }
            set {
                this.fPCapacityUnitFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal FPCapacityConversionRate {
            get {
                return this.fPCapacityConversionRateField;
            }
            set {
                this.fPCapacityConversionRateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FPCapacityConversionRateSpecified {
            get {
                return this.fPCapacityConversionRateFieldSpecified;
            }
            set {
                this.fPCapacityConversionRateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public FXInformation FXInformation {
            get {
                return this.fXInformationField;
            }
            set {
                this.fXInformationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    public enum UnitOfMeasureType {
        
        /// <remarks/>
        Therm,
        
        /// <remarks/>
        KWh,
        
        /// <remarks/>
        MWh,
        
        /// <remarks/>
        GWh,
        
        /// <remarks/>
        MJ,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("100MJ")]
        Item100MJ,
        
        /// <remarks/>
        MMJ,
        
        /// <remarks/>
        GJ,
        
        /// <remarks/>
        BBL,
        
        /// <remarks/>
        MT,
        
        /// <remarks/>
        GAL,
        
        /// <remarks/>
        ThermPerDay,
        
        /// <remarks/>
        KWhPerDay,
        
        /// <remarks/>
        GWhPerDay,
        
        /// <remarks/>
        MJPerDay,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("100MJPerDay")]
        Item100MJPerDay,
        
        /// <remarks/>
        MMJPerDay,
        
        /// <remarks/>
        MW,
        
        /// <remarks/>
        KW,
        
        /// <remarks/>
        GW,
        
        /// <remarks/>
        GJPerDay,
        
        /// <remarks/>
        Day,
        
        /// <remarks/>
        EUA,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class FXInformation {
        
        private object[] itemsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FXMethod", typeof(FXConversionMethodType))]
        [System.Xml.Serialization.XmlElementAttribute("FXRate", typeof(decimal))]
        [System.Xml.Serialization.XmlElementAttribute("FXReference", typeof(string))]
        public object[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    public enum FXConversionMethodType {
        
        /// <remarks/>
        Daily,
        
        /// <remarks/>
        Monthly,
        
        /// <remarks/>
        Mixed,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    [System.Xml.Serialization.XmlRootAttribute("Rounding", Namespace="", IsNullable=false)]
    public enum RoundingType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Item0,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,
        
        /// <remarks/>
        N_A,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Acknowledgement {
        
        private string documentIDField;
        
        private UsageType documentUsageField;
        
        private string senderIDField;
        
        private string receiverIDField;
        
        private RoleType receiverRoleField;
        
        private DocumentType referencedDocumentTypeField;
        
        private string referencedDocumentIDField;
        
        private string referencedDocumentVersionField;
        
        private string schemaVersionField;
        
        private string schemaReleaseField;
        
        /// <remarks/>
        public string DocumentID {
            get {
                return this.documentIDField;
            }
            set {
                this.documentIDField = value;
            }
        }
        
        /// <remarks/>
        public UsageType DocumentUsage {
            get {
                return this.documentUsageField;
            }
            set {
                this.documentUsageField = value;
            }
        }
        
        /// <remarks/>
        public string SenderID {
            get {
                return this.senderIDField;
            }
            set {
                this.senderIDField = value;
            }
        }
        
        /// <remarks/>
        public string ReceiverID {
            get {
                return this.receiverIDField;
            }
            set {
                this.receiverIDField = value;
            }
        }
        
        /// <remarks/>
        public RoleType ReceiverRole {
            get {
                return this.receiverRoleField;
            }
            set {
                this.receiverRoleField = value;
            }
        }
        
        /// <remarks/>
        public DocumentType ReferencedDocumentType {
            get {
                return this.referencedDocumentTypeField;
            }
            set {
                this.referencedDocumentTypeField = value;
            }
        }
        
        /// <remarks/>
        public string ReferencedDocumentID {
            get {
                return this.referencedDocumentIDField;
            }
            set {
                this.referencedDocumentIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer")]
        public string ReferencedDocumentVersion {
            get {
                return this.referencedDocumentVersionField;
            }
            set {
                this.referencedDocumentVersionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SchemaVersion {
            get {
                return this.schemaVersionField;
            }
            set {
                this.schemaVersionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SchemaRelease {
            get {
                return this.schemaReleaseField;
            }
            set {
                this.schemaReleaseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    public enum UsageType {
        
        /// <remarks/>
        Test,
        
        /// <remarks/>
        Live,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    public enum RoleType {
        
        /// <remarks/>
        Trader,
        
        /// <remarks/>
        Broker,
        
        /// <remarks/>
        ClearingHouse,
        
        /// <remarks/>
        ECVNA,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.efet.org/schemas/eCM/V4R0/EFET-CoreCmpts-V4R0.xsd")]
    public enum DocumentType {
        
        /// <remarks/>
        ACK,
        
        /// <remarks/>
        BCN,
        
        /// <remarks/>
        BFI,
        
        /// <remarks/>
        BMN,
        
        /// <remarks/>
        BRS,
        
        /// <remarks/>
        CAN,
        
        /// <remarks/>
        CNF,
        
        /// <remarks/>
        MSA,
        
        /// <remarks/>
        MSR,
        
        /// <remarks/>
        MSU,
        
        /// <remarks/>
        REJ,
        
        /// <remarks/>
        TUR,
    }
}
