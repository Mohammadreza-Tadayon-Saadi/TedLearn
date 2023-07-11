using AutoMapper;
using Services.Services;
using System.Reflection;
using WebConfig.IoC;

namespace WebConfig.CustomMappings.Configurations;

public static class AutoMapperConfiguration
{
    public static void InitializeAutoMapper()
    {
        var configuration = new MapperConfiguration(config =>
        {
            config.AddCustomMappingProfile();
        });

        configuration.CompileMappings();
    }

    public static void AddCustomMappingProfile(this IMapperConfigurationExpression config)
    {
        config.AddCustomMappingProfile(typeof(BaseServices).Assembly);
    }

    public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
    {
        /*
         Every Assembly Have ExportedTypes That Returns Every Class Which Can Export To OutSide.
         It Means That The Class Must Be Public 
        */
        var allTypes = assemblies.SelectMany(assembly => assembly.ExportedTypes); 

        var listOfCustomMapping = allTypes
            .Where(type => type.IsClass && !type.IsAbstract && 
                    type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
            .Select(type => (IHaveCustomMapping)Activator.CreateInstance(type));

        var profile = new CustomMappingProfile(listOfCustomMapping);
        config.AddProfile(profile);
    }
}
