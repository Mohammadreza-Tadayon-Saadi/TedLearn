namespace Services.Contracts.Interfaces;
public interface ITransactionDbContextServices
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool configureAwait = false);
    int SaveChanges();
    Task<int> ExecuteInTransactionAsync(TransactionalDelegate transactionalDelegate, CancellationToken cancellationToken = default, bool configureAwait = false);
}

public delegate Task TransactionalDelegate();