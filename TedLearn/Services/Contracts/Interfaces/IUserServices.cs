using Data.Entities.Persons.Users;
using Services.DTOs.AdminPanel.User;

namespace Services.Contracts.Interfaces;

public interface IUserServices
{
    Task<bool> IsUserExistAsync(int userId, CancellationToken cancellationToken);
    Task<bool> IsUserExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken);
    Task<bool> IsPhoneConfirmedAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<bool> IsUserNameOrPhoneNumberExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken);
    Task SignUpUserAsync(User user, CancellationToken cancellationToken, bool configureAwait = false);
    Task<bool> IsPhoneExistAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<double> LeftTimeActivateCode(string phoneNumber, double expireTime, CancellationToken cancellationToken);
    Task<User> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken, bool withTracking = true);
    Task AddActiveCodForUserAsync(string phoneNumber, string activeCode, CancellationToken cancellationToken, bool configureAwait = false);
    Task<User> CheckUserForLoginAsync(int userId, string password, CancellationToken cancellationToken, bool withTracking = false);
    Task<User> CheckUserForLoginAsync(string userName, string passwordHash, CancellationToken cancellationToken);
    Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken, bool withTracking = true , bool? getActive = null);
    Task<string> GetUserAvatar(int userId);
    Task<string> GetUserAvatar(int userId, CancellationToken cancellationToken);
    Task<byte[]> GetVersionOfUserAsync(int userId, CancellationToken cancellationToken);
    Task<GetUsersDto> GetAllUsersAsync(CancellationToken cancellationToken , bool deletedUser , string? orderBy, int pageId = 1);
    Task<bool> IsUserNameOrPhoneNumberExist(string userName, string phoneNumber , CancellationToken cancellationToken);
    Task<EditUserDto> GetUserForEditInAdminAsync(int userId, CancellationToken cancellationToken);
    Task<GetUserInformationDto> GetUserInformationAsync(int userId, CancellationToken cancellationToken);

    Task<int> ExecuteInTransactionAsync(Services.TransactionalDelegate transactionalDelegate, CancellationToken cancellationToken, bool configureAwait = false);
    Task AddUserAsync(User user, CancellationToken cancellationToken, bool configureAwait = false);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken, bool configureAwait = false);
}
