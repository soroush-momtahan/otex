namespace Otex.Gateways.Web.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        
        logger.LogInformation("Incoming Request: {Method} {Path} from {IP}", 
            context.Request.Method, 
            context.Request.Path, 
            context.Connection.RemoteIpAddress);

        await next(context);

        var duration = DateTime.UtcNow - startTime;
        
        logger.LogInformation("Outgoing Response: {StatusCode} in {Duration}ms", 
            context.Response.StatusCode, 
            duration.TotalMilliseconds);
    }
}