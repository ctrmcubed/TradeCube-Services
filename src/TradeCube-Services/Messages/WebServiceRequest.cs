using System.Collections.Generic;

namespace TradeCube_Services.Messages
{
    public class WebServiceRequest
    {
        public string WebService { get; set; }
        public string ActionName { get; set; }
        public string Format { get; set; }
        public IEnumerable<string> Entities { get; set; }
    }
}