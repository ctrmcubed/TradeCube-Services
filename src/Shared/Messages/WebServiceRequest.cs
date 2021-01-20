using System.Collections.Generic;

namespace Shared.Messages
{
    public class WebServiceRequest
    {
        public string ActionName { get; set; }
        public string EntityType { get; set; }
        public string Format { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Entities { get; set; }
    }
}