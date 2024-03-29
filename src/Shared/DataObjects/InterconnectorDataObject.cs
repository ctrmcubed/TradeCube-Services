﻿using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class InterconnectorDataObject
    {
        public string Interconnector { get; set; }
        public string InterconnectorLongName { get; set; }

        [JsonPropertyName("EIC")]
        public string Eic { get; set; }
    }
}