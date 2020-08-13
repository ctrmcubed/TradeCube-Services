using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace TradeCube_Services.Parameters
{
    public class WebhookParameters : ParametersBase
    {
        public string Webhook { get; set; }
        public string Entity { get; set; }
        public string EntityType { get; set; }
        public string SubscriberId { get; set; }
        public string Body { get; set; }
        public Dictionary<string, KeyValuePair<string, StringValues>> RequestHeaders { get; set; }
    }
}