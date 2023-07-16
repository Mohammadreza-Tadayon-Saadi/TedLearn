using Data.Context;
using Data.Entities.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace Services.Contracts.Services;

public class BaseServices<TEntity> where TEntity : class, IEntity , new()
{
    #region ConstructorInjection

    protected readonly TedLearnContext _context;
    public DbSet<TEntity> Entity { get; }
    public virtual IQueryable<TEntity> Table => Entity;
    public virtual IQueryable<TEntity> TableNoTracking => Entity.AsNoTracking();
    public BaseServices(TedLearnContext context)
    {
        _context = context;
        Entity = _context.Set<TEntity>();
    }

    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken , bool configureAwait = false) => await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(configureAwait);
    public int SaveChanges() => _context.SaveChanges();
}
