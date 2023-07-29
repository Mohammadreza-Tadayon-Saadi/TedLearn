using Data.Context;

namespace Services.Contracts.Services;

public class TransactionDbContextServices : ITransactionDbContextServices
{
    #region ConstructorInjection

    private readonly TedLearnContext _context;
    public TransactionDbContextServices(TedLearnContext context)
    {
        _context = context;
    }

    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken, bool configureAwait = false) => await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(configureAwait);
    public int SaveChanges() => _context.SaveChanges();

    public async Task<int> ExecuteInTransactionAsync(TransactionalDelegate transactionalDelegate, CancellationToken cancellationToken, bool configureAwait = false)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await transactionalDelegate();

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
