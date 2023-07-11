using AutoMapper;
using WebConfig.CustomMappings.Configurations;

namespace WebConfig.DTOs.Common;

public abstract class BaseDto<TDto, TEntity> : IHaveCustomMapping
    where TDto : class , new() 
    where TEntity : class, new()
{
    private static readonly IMapper _mapper;
    private static IMappingExpression<TDto, TEntity> _mappingExpression;
    static BaseDto()
    {
        var dtoType = typeof(TDto);
        var entityType = typeof(TEntity);
        var config = new MapperConfiguration(cfg => {
            _mappingExpression = cfg.CreateMap<TDto, TEntity>();
            cfg.CreateMap<TEntity, TDto>();
        });

        config.CompileMappings();
        _mapper = config.CreateMapper();
    }

    public TEntity ToEntity() =>
        _mapper.Map<TEntity>(CastToDerivedClass(this));

    public TEntity ToEntity(TEntity entity) =>
        _mapper.Map(CastToDerivedClass(this) , entity);

    public static TDto FromEntity(TEntity entity) =>
        _mapper.Map<TDto>(entity);

    protected TDto CastToDerivedClass(BaseDto<TDto, TEntity> baseInstance) =>
        _mapper.Map<TDto>(baseInstance);

    public void CreateMapping(Profile profile)
    {
        var dtoType = typeof(TDto);
        var entityType = typeof(TEntity);

        //Ignore any property of source that dose not contains in destination 
        foreach (var property in entityType.GetProperties())
        {
            if (dtoType.GetProperty(property.Name) == null)
                _mappingExpression.ForMember(property.Name, opt => opt.Ignore());
        }

        CustomMappings(_mappingExpression.ReverseMap());
    }

    public virtual void CustomMappings(IMappingExpression<TEntity, TDto> mapping)
    {
    }
}
