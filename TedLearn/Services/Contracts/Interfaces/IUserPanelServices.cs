using Data.Entities.Persons.Transactions;
using Services.DTOs.UserPanel;

namespace Services.Contracts.Interfaces;

public interface IUserPanelServices
{
    Task<UserInformationDto> GetUserInformation(int userId, CancellationToken cancellationToken);
    Task<EditUserProfileDto> GetUserForEdit(int userId, CancellationToken cancellationToken);
    Task<decimal> GetStockForUserAsync(int userId, CancellationToken cancellationToken);
    Task<int> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool configureAwait = false); // And Return TransactionId For OnlinePayment
    Task<int> ChargeWalletAsync(int userId, decimal amount, string description, CancellationToken cancellationToken, bool isPay = false, bool configureAwait = false); // And Return TransactionId For OnlinePayment
    Task<Transaction> GetTransactionByTransactionIdAsync(int transactionId, CancellationToken cancellationToken, bool withTracking = true);
    Task UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool configureAwait = false);
    Task RemoveTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool configureAwait = false);
    Task<TransactionReportsWithPaginationDto> GetTransationsForUser(int userId, CancellationToken cancellationToken, int pageId = 1);
    Task<int> ExecuteInTransactionAsync(Services.TransactionalDelegate transactionalDelegate, CancellationToken cancellationToken, bool configureAwait = false);
    //bool IsExistTransaction(int transactionId);
    //int RemoveTransaction(Transaction transaction);
}
