using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebConfig.Middlewares;

public static class PurchacableCoursesHandlerMiddlewareExtentions
{
    public static void UsePurchacableCoursesHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ManagePurchacableCourses>();
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
        
        var securedFolders = new string[]{ "/courses/course-episodes-video/purchacable", "/courses/course-episodes-file",
           "/DownloadSeriLog" , "/SeriLog"  };
        bool isValid = true;

        foreach (var item in securedFolders)
            if (request.StartsWith(item.ToLower()))
            {
                var FromUrl = context.Request.Headers["Referer"].ToString();
                if (!String.IsNullOrEmpty(FromUrl) && (FromUrl.StartsWith("https://localhost:7199") || FromUrl.StartsWith("http://localhost:7199")))
                    await _next.Invoke(context);
                else
                {
                    isValid = false;
                    context.Response.Redirect("/Error404");
                }
            }

        if(isValid)
            await _next.Invoke(context);
    }
}
