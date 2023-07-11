using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebConfig.Middlewares;

public static class NotFoundHandlerMiddlewareExtentions
{
    public static void UseNotFoundHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<RedirectTo404Page>();
    }
}

public class RedirectTo404Page
{
    private readonly RequestDelegate _next;
    public RedirectTo404Page(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        if (context.Response.StatusCode == 404 || context.Response.StatusCode == 403 || context.Response.StatusCode == 400)
            context.Response.Redirect("/Error404");
    }
}
