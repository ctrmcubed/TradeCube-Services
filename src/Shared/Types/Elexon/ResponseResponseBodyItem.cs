using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Shared.Types.Elexon;

public class ResponseResponseBodyItem
{
    [XmlElement("recordType")]
    [JsonPropertyName("recordType")]
    public string RecordType { get; init; }

    [XmlElement("settlementDate")]
    [JsonPropertyName("settlementDate")]
    public System.DateTime SettlementDate { get; init; }

    [XmlElement("settlementPeriod")]
    [JsonPropertyName("settlementPeriod")]
    public int SettlementPeriod { get; init; }

    [XmlElement("systemSellPrice")]
    [JsonPropertyName("systemSellPrice")]
    public decimal SystemSellPrice { get; init; }

    [XmlElement("systemBuyPrice")]
    [JsonPropertyName("systemBuyPrice")]
    public decimal SystemBuyPrice { get; init; }

    [XmlElement("bSADDefault")]
    [JsonPropertyName("bSADDefault")]
    public string BSadDefault { get; init; }

    [XmlElement("priceDerivationCode")]
    [JsonPropertyName("priceDerivationCode")]
    public string PriceDerivationCode { get; init; }

    [XmlElement("reserveScarcityPrice")]
    [JsonPropertyName("reserveScarcityPrice")]
    public string ReserveScarcityPrice { get; init; }

    [XmlElement("indicativeNetImbalanceVolume")]
    [JsonPropertyName("indicativeNetImbalanceVolume")]
    public decimal IndicativeNetImbalanceVolume { get; init; }

    [XmlElement("sellPriceAdjustment")]
    [JsonPropertyName("sellPriceAdjustment")]
    public decimal SellPriceAdjustment { get; init; }

    [XmlElement("buyPriceAdjustment")]
    [JsonPropertyName("buyPriceAdjustment")]
    public decimal BuyPriceAdjustment { get; init; }

    [XmlElement("replacementPrice")]
    [JsonPropertyName("replacementPrice")]
    public decimal ReplacementPrice { get; init; }

    [XmlElement("replacementPriceSpecified")]
    [JsonPropertyName("replacementPriceSpecified")]
    public bool ReplacementPriceSpecified { get; init; }

    [XmlElement("replacementPriceCalculationVolume")]
    [JsonPropertyName("replacementPriceCalculationVolume")]
    public decimal ReplacementPriceCalculationVolume { get; init; }

    [XmlElement("replacementPriceCalculationVolumeSpecified")]
    [JsonPropertyName("replacementPriceCalculationVolumeSpecified")]
    public bool ReplacementPriceCalculationVolumeSpecified { get; init; }

    [XmlElement("totalSystemAcceptedOfferVolume")]
    [JsonPropertyName("totalSystemAcceptedOfferVolume")]
    public decimal TotalSystemAcceptedOfferVolume { get; init; }

    [XmlElement("totalSystemAcceptedBidVolume")]
    [JsonPropertyName("totalSystemAcceptedBidVolume")]
    public decimal TotalSystemAcceptedBidVolume { get; init; }

    [XmlElement("totalSystemTaggedAcceptedOfferVolume")]
    [JsonPropertyName("totalSystemTaggedAcceptedOfferVolume")]
    public decimal TotalSystemTaggedAcceptedOfferVolume { get; init; }

    [XmlElement("totalSystemTaggedAcceptedBidVolume")]
    [JsonPropertyName("totalSystemTaggedAcceptedBidVolume")]
    public decimal TotalSystemTaggedAcceptedBidVolume { get; init; }

    [XmlElement("totalSystemAdjustmentSellVolume")]
    [JsonPropertyName("totalSystemAdjustmentSellVolume")]
    public decimal TotalSystemAdjustmentSellVolume { get; init; }

    [XmlElement("totalSystemAdjustmentBuyVolume")]
    [JsonPropertyName("totalSystemAdjustmentBuyVolume")]
    public decimal TotalSystemAdjustmentBuyVolume { get; init; }

    [XmlElement("totalSystemTaggedAdjustmentSellVolume")]
    [JsonPropertyName("totalSystemTaggedAdjustmentSellVolume")]
    public decimal TotalSystemTaggedAdjustmentSellVolume { get; init; }

    [XmlElement("totalSystemTaggedAdjustmentBuyVolume")]
    [JsonPropertyName("totalSystemTaggedAdjustmentBuyVolume")]
    public decimal TotalSystemTaggedAdjustmentBuyVolume { get; init; }

    [XmlElement("activeFlag")]
    [JsonPropertyName("activeFlag")]
    public string ActiveFlag { get; init; }
}