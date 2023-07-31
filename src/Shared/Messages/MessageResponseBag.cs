using System.Collections.Generic;
using System.Linq;
using Serilog;
using Shared.Extensions;

public class MessageResponseBag
{
    private static readonly ILogger Logger = Log.ForContext(typeof(MessageResponseBag));

    private readonly List<ResponseMessage> messages = new();

    public MessageResponseBag()
    {
    }
    
    public MessageResponseBag(string message, MessageResponseType type)
    {
        AddMessage(message, type);
    }
    
    public void AddMessage(string message, MessageResponseType type)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        Logger.Information("Message: {Message}", message);
        messages.Add(new ResponseMessage(message, type));
    }
    
    public bool GotErrors() =>
        messages.Any(m => m.MessageResponseType == MessageResponseType.Error);

    public string ErrorsAsString(bool? addFullStop = false)
    {
        var errorsAsString = string.Join(", ", messages
            .Where(m => m.MessageResponseType == MessageResponseType.Error)
            .Select(m => m.Message));

        return string.IsNullOrWhiteSpace(errorsAsString) 
            ? null 
            : addFullStop ?? false 
                ? $"{errorsAsString}." 
                : errorsAsString;
    }
    
    public IEnumerable<string> ErrorsAsList() =>
        messages
            .EmptyIfNull()
            .Where(m => m.MessageResponseType == MessageResponseType.Error)
            .Select(m => m.Message);
    
    public IEnumerable<string> MessagesAsList(MessageResponseType type) =>
        messages
            .EmptyIfNull()
            .Where(m => m.MessageResponseType == type)
            .Select(m => m.Message);
}