using Microsoft.Extensions.DependencyInjection;
using Services.Contracts.Interfaces;
using Services.Contracts.Services;

namespace WebConfig.IoC;

public static class DependencyContainer
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IPermissionServices, PermissionServices>();
        services.AddScoped<IUserPanelServices, UserPanelServices>();
        services.AddScoped<ICourseGroupServices, CourseGroupServices>();
        services.AddScoped<ICourseServices, CourseServices>();
    }
}
