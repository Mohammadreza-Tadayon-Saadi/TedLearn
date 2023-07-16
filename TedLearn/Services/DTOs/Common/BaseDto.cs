using AutoMapper.QueryableExtensions;
using Services.CustomMappings.Configurations;

namespace Services.DTOs.Common;

public abstract class BaseDto<TDto, TEntity> : IHaveCustomMapping
    where TDto : class , new() 
    where TEntity : class, new()
{
    public TEntity ToEntity() =>
        AutoMapperConfiguration.Mapper.Map<TEntity>(CastToDerivedClass(this));

    public TEntity ToEntity(TEntity entity) =>
        AutoMapperConfiguration.Mapper.Map(CastToDerivedClass(this) , entity);

    public static TDto FromEntity(TEntity entity) =>
        AutoMapperConfiguration.Mapper.Map<TDto>(entity);

    protected TDto CastToDerivedClass(BaseDto<TDto, TEntity> baseInstance) =>
        AutoMapperConfiguration.Mapper.Map<TDto>(baseInstance);

    public static IQueryable<TDto> ProjectTo(IQueryable<TEntity> queryable)
        => queryable.ProjectTo<TDto>(AutoMapperConfiguration.Mapper.ConfigurationProvider);

    public void CreateMapping(Profile profile)
    {
        var mappingExpression = profile.CreateMap<TDto, TEntity>();
        var dtoType = typeof(TDto);
        var entityType = typeof(TEntity);

        //Ignore any property of source that dose not contains in destination 
        foreach (var property in entityType.GetProperties())
        {
            if (dtoType.GetProperty(property.Name) == null)
                mappingExpression.ForMember(property.Name, opt => opt.Ignore());
        }

        CustomMappings(mappingExpression.ReverseMap());
    }

    public virtual void CustomMappings(IMappingExpression<TEntity, TDto> mapping)
    {
    }
}
