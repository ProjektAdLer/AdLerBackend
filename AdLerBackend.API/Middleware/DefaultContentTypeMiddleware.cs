namespace AdLerBackend.API.Middleware;

public class DefaultContentTypeMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (string.IsNullOrEmpty(context.Request.ContentType) &&
            context.Request.Method is "POST" or "PUT" or "PATCH")
            context.Request.ContentType = "application/json";

        await next(context);
    }
}