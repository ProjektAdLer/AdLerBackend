using Serilog.Context;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace AdLerBackend.API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.TraceIdentifier))
        {
            using (LogContext.PushProperty("TraceIdentifier", $"TraceIdentifier: {context.TraceIdentifier} "))
            {
                await _next(context);
            }
        }
        else
        {
            await _next(context);
        }
    }
}