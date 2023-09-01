using Data.Entities.Persons.Transactions;
using Services.DTOs.UserPanel.Home;
using Services.DTOs.UserPanel.Transaction;

namespace Services.Contracts.Interfaces;

public interface IUserPanelServices
{
    Task<UserInformationDto> GetUserInformation(int userId, CancellationToken cancellationToken = default);
    Task<EditUserProfileDto> GetUserForEdit(int userId, CancellationToken cancellationToken = default);
    Task<decimal> GetStockForUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<int?> ChargeWalletAsync(int userId, decimal amount, string description, CancellationToken cancellationToken = default, bool isPay = false, bool withSaveChanges = true, bool configureAwait = false); // And Return TransactionId For OnlinePayment
    Task<Transaction> GetTransactionByTransactionIdAsync(int transactionId, CancellationToken cancellationToken = default, bool withTracking = true);
    Task<TransactionReportsWithPaginationDto> GetTransationsForUser(int userId, CancellationToken cancellationToken = default, int pageId = 1);
    
    
    Task<int?> AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false); // And Return TransactionId For OnlinePayment
    Task UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task RemoveTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    //bool IsExistTransaction(int transactionId);
    //int RemoveTransaction(Transaction transaction);
}
