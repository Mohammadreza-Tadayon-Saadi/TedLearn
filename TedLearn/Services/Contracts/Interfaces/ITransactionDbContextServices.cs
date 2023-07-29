using Microsoft.EntityFrameworkCore;

namespace Services.Contracts.Interfaces;
public interface ITransactionDbContextServices
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken, bool configureAwait = false);
    int SaveChanges();
    Task<int> ExecuteInTransactionAsync(TransactionalDelegate transactionalDelegate, CancellationToken cancellationToken, bool configureAwait = false);
}

public delegate Task TransactionalDelegate();