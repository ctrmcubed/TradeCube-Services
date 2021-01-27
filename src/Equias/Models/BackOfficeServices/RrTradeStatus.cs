using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Equias.Models.BackOfficeServices
{
    public class RrTradeStatus
    {
        [JsonPropertyName("RRTradeStageStatuses")]
        public IEnumerable<RrTradeStageStatuses> RrTradeStageStatuses { get; set; }
    }
}