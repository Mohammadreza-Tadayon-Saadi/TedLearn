using Data.Entities.Persons.Users;
using System.Security.Claims;

namespace Services.Interfaces;

public interface IUserServices
{
    Task<bool> IsUserExistAsync(string userName, string phoneNumber);
    Task<bool> IsPhoneConfirmedAsync(string phoneNumber);
    Task<bool> IsUserNameOrPhoneNumberExistAsync(string userName, string phoneNumber);
    Task SignUpUserAsync(User user);
    Task<bool> IsPhoneExistAsync(string phoneNumber);
    double LeftTimeActivateCode(string phoneNumber, double expireTime);
    Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
    Task AddActiveCodForUserAsync(string phoneNumber, string activeCode);
    Task<User> CheckUserForLoginAsync(string userName, string passwordHash);
    Task<User> GetUserByIdAsync(int userId);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
}
