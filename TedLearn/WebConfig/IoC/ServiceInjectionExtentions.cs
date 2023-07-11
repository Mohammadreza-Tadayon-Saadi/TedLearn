using Data.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMarkupMin.AspNetCore6;
using Serilog;

namespace WebConfig.IoC;

public static class ServiceInjectionExtentions
{
    public static void AddTedLearnContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TedLearnContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("TedLearnConnection"), sqlOptions =>
            {
                //sqlOptions.CommandTimeout(120);
            });
        });
    }

    public static void AddMarkupMin(this IServiceCollection services)
    {
        services.AddWebMarkupMin(options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = true;
            options.AllowCompressionInDevelopmentEnvironment = true;
        }).AddHtmlMinification()
          .AddXmlMinification()
          .AddHttpCompression();
    }

    public static void AddWebAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.Cookie.Name = "AuthCookie";
            options.LoginPath = "/Login";
            options.LogoutPath = "/LogOut";
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        });
    }

    public static void AddSerilog(this ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
        });
    }
}
