using Microsoft.Extensions.DependencyInjection;
using Services.Contracts.Interfaces;
using Services.Contracts.Services;

namespace WebConfig.IoC;

public static class DependencyContainer
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ITransactionDbContextServices , TransactionDbContextServices>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IPermissionServices, PermissionServices>();
        services.AddScoped<IUserPanelServices, UserPanelServices>();
        services.AddScoped<ICourseGroupServices, CourseGroupServices>();
        services.AddScoped<ICourseServices, CourseServices>();
        services.AddScoped<ICourseSeasonServices, CourseSeasonServices>();
        services.AddScoped<ICourseEpisodeServices, CourseEpisodeServices>();
        services.AddScoped<ICourseCommentServices, CourseCommentServices>();
        services.AddScoped<IDiscountServices, DiscountServices>();
        services.AddScoped<IOrderServices, OrderServices>();
    }
}
