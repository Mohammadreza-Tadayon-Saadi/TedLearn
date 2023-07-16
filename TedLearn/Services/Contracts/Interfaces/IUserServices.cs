using Data.Entities.Persons.Users;

namespace Services.Contracts.Interfaces;

public interface IUserServices
{
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
    Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken, bool withTracking = true);
    Task<string> GetUserAvatar(int userId);
    Task<string> GetUserAvatar(int userId, CancellationToken cancellationToken);
    Task<byte[]> GetVersionOfUserAsync(int userId, CancellationToken cancellationToken);

    Task AddUserAsync(User user, CancellationToken cancellationToken, bool configureAwait = false);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken, bool configureAwait = false);
}
