public class ResponseMessage
{
    public string Message { get; }
    public MessageResponseType MessageResponseType { get; }
    
    public ResponseMessage(string message, MessageResponseType messageResponseType)
    {
        Message = message;
        MessageResponseType = messageResponseType;
    }
}