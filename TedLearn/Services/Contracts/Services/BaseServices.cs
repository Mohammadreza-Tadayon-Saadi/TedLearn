using Data.Context;
using Data.Entities.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace Services.Contracts.Services;


public delegate Task TransactionalDelegate(DbContext dbContext);
public class BaseServices<TEntity> where TEntity : class, IEntity, new()
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

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken, bool configureAwait = false) => await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(configureAwait);
    public virtual int SaveChanges() => _context.SaveChanges();

    public virtual async Task<int> ExecuteInTransactionAsync(TransactionalDelegate transactionalDelegate, CancellationToken cancellationToken, bool configureAwait = false)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await transactionalDelegate(_context);

                await SaveChangesAsync(cancellationToken, configureAwait);

                await transaction.CommitAsync(cancellationToken);
                
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
