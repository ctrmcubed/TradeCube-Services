namespace Shared.DataObjects;

public class DataItemDataObject
{
    public string DataItem { get; init; }
    public string DataItemLongName { get; init; }
    public string AggregationMethod { get; init; }
    public string FixedData { get; init; }
    public string DataItemType { get; init; }

    public QuantityUnitDataObject QuantityUnit { get; init; }
    public EnergyUnitDataObject EnergyUnit { get; init; }
    public PriceUnitDataObject PriceUnit { get; init; }
    public string Currency { get; init; }
    public InternalFieldsType InternalFields { get; init; }
}