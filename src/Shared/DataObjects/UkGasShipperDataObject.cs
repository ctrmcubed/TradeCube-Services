using System;
using System.Text.Json.Serialization;

namespace Shared.DataObjects;

public class UkGasShipperDataObject
{
    public string ShipperCode { get; init; }
    public string ShipperLongName { get; init; }

    [JsonPropertyName("EIC")]
    public EnergyIdentificationCodeDataObject Eic { get; init; }

    public string State { get; init; }
    public DateTime? LastUpdatedDate { get; init; }
}