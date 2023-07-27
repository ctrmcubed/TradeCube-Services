namespace Shared.Types.Elexon;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class ResponseResponseBodyItem
{

    private string recordTypeField;

    private System.DateTime settlementDateField;

    private int settlementPeriodField;

    private decimal systemSellPriceField;

    private decimal systemBuyPriceField;

    private string bSADDefaultField;

    private string priceDerivationCodeField;

    private string reserveScarcityPriceField;

    private decimal indicativeNetImbalanceVolumeField;

    private decimal sellPriceAdjustmentField;

    private decimal buyPriceAdjustmentField;

    private decimal replacementPriceField;

    private bool replacementPriceFieldSpecified;

    private decimal replacementPriceCalculationVolumeField;

    private bool replacementPriceCalculationVolumeFieldSpecified;

    private decimal totalSystemAcceptedOfferVolumeField;

    private decimal totalSystemAcceptedBidVolumeField;

    private decimal totalSystemTaggedAcceptedOfferVolumeField;

    private decimal totalSystemTaggedAcceptedBidVolumeField;

    private decimal totalSystemAdjustmentSellVolumeField;

    private decimal totalSystemAdjustmentBuyVolumeField;

    private decimal totalSystemTaggedAdjustmentSellVolumeField;

    private decimal totalSystemTaggedAdjustmentBuyVolumeField;

    private string activeFlagField;

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
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime settlementDate
    {
        get
        {
            return this.settlementDateField;
        }
        set
        {
            this.settlementDateField = value;
        }
    }

    /// <remarks/>
    public int settlementPeriod
    {
        get
        {
            return this.settlementPeriodField;
        }
        set
        {
            this.settlementPeriodField = value;
        }
    }

    /// <remarks/>
    public decimal systemSellPrice
    {
        get
        {
            return this.systemSellPriceField;
        }
        set
        {
            this.systemSellPriceField = value;
        }
    }

    /// <remarks/>
    public decimal systemBuyPrice
    {
        get
        {
            return this.systemBuyPriceField;
        }
        set
        {
            this.systemBuyPriceField = value;
        }
    }

    /// <remarks/>
    public string bSADDefault
    {
        get
        {
            return this.bSADDefaultField;
        }
        set
        {
            this.bSADDefaultField = value;
        }
    }

    /// <remarks/>
    public string priceDerivationCode
    {
        get
        {
            return this.priceDerivationCodeField;
        }
        set
        {
            this.priceDerivationCodeField = value;
        }
    }

    /// <remarks/>
    public string reserveScarcityPrice
    {
        get
        {
            return this.reserveScarcityPriceField;
        }
        set
        {
            this.reserveScarcityPriceField = value;
        }
    }

    /// <remarks/>
    public decimal indicativeNetImbalanceVolume
    {
        get
        {
            return this.indicativeNetImbalanceVolumeField;
        }
        set
        {
            this.indicativeNetImbalanceVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal sellPriceAdjustment
    {
        get
        {
            return this.sellPriceAdjustmentField;
        }
        set
        {
            this.sellPriceAdjustmentField = value;
        }
    }

    /// <remarks/>
    public decimal buyPriceAdjustment
    {
        get
        {
            return this.buyPriceAdjustmentField;
        }
        set
        {
            this.buyPriceAdjustmentField = value;
        }
    }

    /// <remarks/>
    public decimal replacementPrice
    {
        get
        {
            return this.replacementPriceField;
        }
        set
        {
            this.replacementPriceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool replacementPriceSpecified
    {
        get
        {
            return this.replacementPriceFieldSpecified;
        }
        set
        {
            this.replacementPriceFieldSpecified = value;
        }
    }

    /// <remarks/>
    public decimal replacementPriceCalculationVolume
    {
        get
        {
            return this.replacementPriceCalculationVolumeField;
        }
        set
        {
            this.replacementPriceCalculationVolumeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool replacementPriceCalculationVolumeSpecified
    {
        get
        {
            return this.replacementPriceCalculationVolumeFieldSpecified;
        }
        set
        {
            this.replacementPriceCalculationVolumeFieldSpecified = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemAcceptedOfferVolume
    {
        get
        {
            return this.totalSystemAcceptedOfferVolumeField;
        }
        set
        {
            this.totalSystemAcceptedOfferVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemAcceptedBidVolume
    {
        get
        {
            return this.totalSystemAcceptedBidVolumeField;
        }
        set
        {
            this.totalSystemAcceptedBidVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemTaggedAcceptedOfferVolume
    {
        get
        {
            return this.totalSystemTaggedAcceptedOfferVolumeField;
        }
        set
        {
            this.totalSystemTaggedAcceptedOfferVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemTaggedAcceptedBidVolume
    {
        get
        {
            return this.totalSystemTaggedAcceptedBidVolumeField;
        }
        set
        {
            this.totalSystemTaggedAcceptedBidVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemAdjustmentSellVolume
    {
        get
        {
            return this.totalSystemAdjustmentSellVolumeField;
        }
        set
        {
            this.totalSystemAdjustmentSellVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemAdjustmentBuyVolume
    {
        get
        {
            return this.totalSystemAdjustmentBuyVolumeField;
        }
        set
        {
            this.totalSystemAdjustmentBuyVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemTaggedAdjustmentSellVolume
    {
        get
        {
            return this.totalSystemTaggedAdjustmentSellVolumeField;
        }
        set
        {
            this.totalSystemTaggedAdjustmentSellVolumeField = value;
        }
    }

    /// <remarks/>
    public decimal totalSystemTaggedAdjustmentBuyVolume
    {
        get
        {
            return this.totalSystemTaggedAdjustmentBuyVolumeField;
        }
        set
        {
            this.totalSystemTaggedAdjustmentBuyVolumeField = value;
        }
    }

    /// <remarks/>
    public string activeFlag
    {
        get
        {
            return this.activeFlagField;
        }
        set
        {
            this.activeFlagField = value;
        }
    }
}