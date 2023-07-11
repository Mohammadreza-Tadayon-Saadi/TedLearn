using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Services;

namespace WebConfig.IoC;

public static class DependencyContainer
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IPermissionServices, PermissionServices>();
    }
}
