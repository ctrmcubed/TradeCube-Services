namespace Shared.Messages
{
    public class WebhookRequest
    {
        public string Webhook { get; set; }
        public string Entity { get; set; }
        public string EntityType { get; set; }
        public string Body { get; set; }
    }
}