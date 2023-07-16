using AutoMapper;

namespace Services.CustomMappings.Configurations;

public class CustomMappingProfile : Profile
{
    public CustomMappingProfile(IEnumerable<IHaveCustomMapping> haveCustomMappings)
    {
        foreach (var item in haveCustomMappings)
            item.CreateMapping(this);
    }
}
