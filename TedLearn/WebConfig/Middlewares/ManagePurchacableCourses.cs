using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebConfig.Middlewares;

public static class PurchacableCoursesHandlerMiddlewareExtentions
{
    public static void UsePurchacableCoursesHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}

public class ManagePurchacableCourses
{
    private readonly RequestDelegate _next;
    public ManagePurchacableCourses(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request.Path.Value.ToString().ToLower();
        if (request.StartsWith("/courses/course-episodes-video/purchacable") || request.StartsWith("/courses/course-episodes-file"))
        {
            var FromUrl = context.Request.Headers["Referer"].ToString();
            if (!String.IsNullOrEmpty(FromUrl) && (FromUrl.StartsWith("https://localhost:7256") || FromUrl.StartsWith("http://localhost:7256")))
                await _next(context);
        }
        else await _next(context);
    }
}
