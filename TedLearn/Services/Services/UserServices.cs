using Data.Context;
using Data.Entities.Persons.Users;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Services;

public class UserServices : BaseServices , IUserServices
{
    #region ConstructorInjection

    private DbSet<User> _users;
    private readonly IPermissionServices _permissionServices;
    public UserServices(TedLearnContext context , IPermissionServices permissionServices) : base(context)
    {
        _users = _context.Set<User>();
        _permissionServices = permissionServices;
    }

    #endregion

    public async Task<bool> IsUserExistAsync(string userName, string phoneNumber)
        => await _users.AsNoTracking()
                    .Where(u => u.UserName == userName && u.PhoneNumber == phoneNumber)
                    .AnyAsync();
    
    public async Task<bool> IsPhoneConfirmedAsync(string phoneNumber)
        => await _users.AsNoTracking()
                    .Where(u => u.PhoneNumber == phoneNumber && u.PhoneNumberConfirmed)
                    .AnyAsync();

    public async Task<bool> IsUserNameOrPhoneNumberExistAsync(string userName, string phoneNumber)
        => await _users.AsNoTracking()
                    .Where(u => u.PhoneNumber == phoneNumber || u.UserName == userName)
                    .AnyAsync();

    public async Task SignUpUserAsync(User user)
    {
        var transacions = _context.Database.BeginTransaction();
        await AddUserAsync(user);
        await _permissionServices.AddNewUserRoleAsync(user.UserId);
        await transacions.CommitAsync();
    }

    public async Task<bool> IsPhoneExistAsync(string phoneNumber)
        => await _users.AsNoTracking()
                    .Where(u => u.PhoneNumber == phoneNumber)
                    .AnyAsync();

    public double LeftTimeActivateCode(string phoneNumber , double expireTime)
    {
        var registerPhones = _users
            .Where(u => u.PhoneNumber == phoneNumber && u.CreateActiveCode.Value.AddSeconds(expireTime) >= DateTime.Now)
            .FirstOrDefault();
        if (registerPhones == null) return 0;
        return (registerPhones.CreateActiveCode.Value.AddSeconds(expireTime) - DateTime.Now).TotalSeconds;
    }

    public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        => await _users.Where(u => u.PhoneNumber == phoneNumber).SingleOrDefaultAsync();

    public async Task AddActiveCodForUserAsync(string phoneNumber, string activeCode)
    {
        var user = await GetUserByPhoneNumberAsync(phoneNumber);
        user.ActiveCode = activeCode;
        user.CreateActiveCode = DateTime.Now;
        await UpdateUserAsync(user);
    }

    public async Task<User> CheckUserForLoginAsync(string userName, string passwordHash)
        => await _users.Where(u => u.UserName == userName && u.PasswordHash == passwordHash).SingleOrDefaultAsync();

    public async Task<User> GetUserByIdAsync(int userId)
        => await _users.Where(u => u.UserId == userId).SingleOrDefaultAsync();

    public async Task AddUserAsync(User user)
    {
        await _users.AddAsync(user);
        await SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _users.Update(user);
        await SaveChangesAsync();
    }
}
