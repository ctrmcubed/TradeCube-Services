namespace TradeCube_Services.Middlleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestLoggingMiddleware> logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        logger.LogWarning("Request: {Method} {Path} from {IPAddress}",
            request.Method,
            request.Path,
            ipAddress);

        await next(context);
    }
}