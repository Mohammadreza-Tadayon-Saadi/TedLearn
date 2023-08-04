using Data.Entities.Persons.Users;
using Services.DTOs.AdminPanel.User;

namespace Services.Contracts.Interfaces;

public interface IUserServices
{
    Task<bool> IsUserExistAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneConfirmedAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> IsUserNameOrPhoneNumberExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken = default);
    Task SignUpUserAsync(User user, CancellationToken cancellationToken = default, bool configureAwait = false);
    Task<bool> IsPhoneExistAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<double> LeftTimeActivateCodeAsync(string phoneNumber, double expireTime, CancellationToken cancellationToken = default);
    Task<User> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default, bool withTracking = true);
    Task AddActiveCodForUserAsync(string phoneNumber, string activeCode, CancellationToken cancellationToken = default , bool withSaveChanges = true, bool configureAwait = false);
    Task<User> CheckUserForLoginAsync(int userId, string password, CancellationToken cancellationToken = default, bool withTracking = true);
    Task<User> CheckUserForLoginAsync(string userName, string passwordHash, CancellationToken cancellationToken = default);
    Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default, bool withTracking = true , bool? getActive = null);
    Task<string> GetUserAvatarAsync(int userId);
    Task<string> GetUserAvatarAsync(int userId, CancellationToken cancellationToken = default);
    Task<byte[]> GetVersionOfUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<GetUsersDto> GetAllUsersAsync(bool deletedUser ,CancellationToken cancellationToken = default , string? orderBy = null, int pageId = 1);
    Task<EditUserDto> GetUserForEditInAdminAsync(int userId, CancellationToken cancellationToken = default);
    Task<GetUserInformationDto> GetUserInformationAsync(int userId, CancellationToken cancellationToken = default);

    Task AddUserAsync(User user, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}
