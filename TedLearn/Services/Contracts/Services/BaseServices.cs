using Data.Context;
using Data.Entities.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace Services.Contracts.Services;

public class BaseServices<TEntity> where TEntity : class, IEntity, new()
{
    #region ConstructorInjection

    protected readonly TedLearnContext _context;
    protected DbSet<TEntity> Entity { get; }
    protected virtual IQueryable<TEntity> Table => Entity;
    protected virtual IQueryable<TEntity> TableNoTracking => Entity.AsNoTracking();
    public BaseServices(TedLearnContext context)
    {
        _context = context;
        Entity = _context.Set<TEntity>();
    }

    #endregion
}
