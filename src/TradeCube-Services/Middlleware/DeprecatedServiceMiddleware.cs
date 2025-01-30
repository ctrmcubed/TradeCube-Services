using Shared.Serialization;

namespace TradeCube_Services.Middlleware;

public class DeprecatedServiceMiddleware
{
    // ReSharper disable once NotAccessedField.Local
    private readonly RequestDelegate next;

    public DeprecatedServiceMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var response = new
        {
            message = "This service has been deprecated. Please contact Support.",
            status = 410 
        };

        context.Response.StatusCode = StatusCodes.Status410Gone;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(TradeCubeJsonSerializer.Serialize(response));
    }
}