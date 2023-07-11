using AutoMapper;

namespace WebConfig.CustomMappings.Configurations;

public interface IHaveCustomMapping
{
    void CreateMapping(Profile profile);
}
