using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebConfig.Middlewares;

public static class CustomExceptionHandlerMiddlewareExtentions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(DbUpdateConcurrencyException ex)
        {
            context.Response.Redirect("/ConcurrencyException");
        }
        catch(DbUpdateException ex)
        {
            if(ex.Message.Contains("Concurrency") || ex.Message.Contains("concurrency"))
                context.Response.Redirect("/ConcurrencyException");
            else
                context.Response.Redirect("/DbUpdateException");
        }
        catch (SqlException ex)
        {
            if(ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                context.Response.Redirect("/TimeoutException");
            else
                context.Response.Redirect("/SqlException");
        }
        catch(TimeoutException ex)
        {
            context.Response.Redirect("/TimeoutException");
        }
        catch
        {
            context.Response.Redirect("/InternalException");
        }
    }
}