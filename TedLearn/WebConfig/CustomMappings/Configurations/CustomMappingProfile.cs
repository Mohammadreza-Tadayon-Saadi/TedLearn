using AutoMapper;

namespace WebConfig.CustomMappings.Configurations;

public class CustomMappingProfile : Profile
{
    public CustomMappingProfile(IEnumerable<IHaveCustomMapping> haveCustomMappings)
    {
        foreach (var item in haveCustomMappings)
            item.CreateMapping(this);
    }
}
