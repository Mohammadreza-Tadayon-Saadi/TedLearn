﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace WebConfig.Configurations;

public static class CustomConfigurationsExtention
{
    public static void AddCustomConfigurations(this IServiceCollection services)
    {
        services.Configure<FormOptions>(option =>
        {
            option.MultipartBodyLengthLimit = 2147483647; // Maximum 2GB
        });
    }
}
