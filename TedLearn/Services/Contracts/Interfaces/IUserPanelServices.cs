using Data.Entities.Persons.Transactions;
using Services.DTOs.UserPanel;

namespace Services.Contracts.Interfaces;

public interface IUserPanelServices
{
    Task<UserInformationDto> GetUserInformation(int userId, CancellationToken cancellationToken);
    Task<EditUserProfileDto> GetUserForEdit(int userId, CancellationToken cancellationToken);
    Task<decimal> GetStockForUserAsync(int userId, CancellationToken cancellationToken);
    Task<int?> ChargeWalletAsync(int userId, decimal amount, string description, CancellationToken cancellationToken, bool isPay = false, bool withSaveChanges = true, bool configureAwait = false); // And Return TransactionId For OnlinePayment
    Task<Transaction> GetTransactionByTransactionIdAsync(int transactionId, CancellationToken cancellationToken, bool withTracking = true);
    Task<TransactionReportsWithPaginationDto> GetTransationsForUser(int userId, CancellationToken cancellationToken, int pageId = 1);
    
    
    Task<int?> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false); // And Return TransactionId For OnlinePayment
    Task UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
    Task RemoveTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
    //bool IsExistTransaction(int transactionId);
    //int RemoveTransaction(Transaction transaction);
}
