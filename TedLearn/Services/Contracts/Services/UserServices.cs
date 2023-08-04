using Core.Securities;
using Core.Utilities;
using Data.Context;
using Data.Entities.Persons.Roles;
using Data.Entities.Persons.Users;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.AdminPanel.User;

namespace Services.Contracts.Services;

public class UserServices : BaseServices<User>, IUserServices
{
    #region ConstructorInjection

    private readonly ITransactionDbContextServices _transactions;
    public UserServices(TedLearnContext context, ITransactionDbContextServices transactions) : base(context)
    {
        _transactions = transactions;
    }

    #endregion


    public async Task<bool> IsUserExistAsync(int userId, CancellationToken cancellationToken = default)
        => await TableNoTracking
                    .Where(u => u.UserId == userId)
                    .AnyAsync(cancellationToken);

    public async Task<bool> IsUserExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken = default)
        => await TableNoTracking
                    .Where(u => u.UserName == userName && u.PhoneNumber == phoneNumber)
                    .AnyAsync(cancellationToken);

    public async Task<bool> IsPhoneConfirmedAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await TableNoTracking
                    .Where(u => u.PhoneNumber == phoneNumber && u.PhoneNumberConfirmed)
                    .AnyAsync(cancellationToken);

    public async Task<bool> IsUserNameOrPhoneNumberExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken = default)
        => await TableNoTracking
                    .Where(u => u.PhoneNumber == phoneNumber || u.UserName == userName)
                    .AnyAsync(cancellationToken);

    public async Task SignUpUserAsync(User user, CancellationToken cancellationToken = default, bool configureAwait = false)
    {
        await _transactions.ExecuteInTransactionAsync(async () =>
        {
            user.UserRole = new List<UserRole>
            {
                new UserRole
                {
                    RoleId = 11001
                }
            };
            await AddUserAsync(user, cancellationToken, withSaveChanges: false, configureAwait);
        }, cancellationToken, configureAwait);
    }

    public async Task<bool> IsPhoneExistAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await TableNoTracking
                    .Where(u => u.PhoneNumber == phoneNumber)
                    .AnyAsync(cancellationToken);

    public async Task<double> LeftTimeActivateCodeAsync(string phoneNumber, double expireTime, CancellationToken cancellationToken = default)
    {
        var registerPhones = await TableNoTracking
            .Where(u => u.PhoneNumber == phoneNumber && u.CreateActiveCode.Value.AddSeconds(expireTime) >= DateTime.Now)
            .FirstOrDefaultAsync(cancellationToken);
        if (registerPhones == null) return 0;
        return (registerPhones.CreateActiveCode.Value.AddSeconds(expireTime) - DateTime.Now).TotalSeconds;
    }

    public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default, bool withTracking = true)
    {
        if (withTracking)
            return await Table.Where(u => u.PhoneNumber == phoneNumber).SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(u => u.PhoneNumber == phoneNumber).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task AddActiveCodForUserAsync(string phoneNumber, string activeCode, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        var user = await GetUserByPhoneNumberAsync(phoneNumber, cancellationToken);
        user.ActiveCode = activeCode;
        user.CreateActiveCode = DateTime.Now;
        
        if(withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken , configureAwait);
    }

    public async Task<User> CheckUserForLoginAsync(int userId, string password, CancellationToken cancellationToken = default, bool withTracking = false)
    {
        if (withTracking)
            return await Table
                    .Where(u => u.UserId == userId && u.PasswordHash == SecurityHelper.GetSha256Hash(password))
                    .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking
                    .Where(u => u.UserId == userId && u.PasswordHash == SecurityHelper.GetSha256Hash(password))
                    .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<User> CheckUserForLoginAsync(string userName, string password, CancellationToken cancellationToken = default)
        => await TableNoTracking
                    .Where(u => u.UserName == userName && u.PasswordHash == SecurityHelper.GetSha256Hash(password))
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(u => u.UserId == userId && ((getActive.HasValue) ? u.IsDelete == !getActive : true)).SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(u => u.UserId == userId && ((getActive.HasValue) ? u.IsDelete == !getActive : true)).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<string> GetUserAvatarAsync(int userId)
        => await TableNoTracking.Where(u => u.UserId == userId)
                    .Select(u => u.UserAvatar)
                    .SingleOrDefaultAsync();

    public async Task<string> GetUserAvatarAsync(int userId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(u => u.UserId == userId)
                    .Select(u => u.UserAvatar)
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<byte[]> GetVersionOfUserAsync(int userId, CancellationToken cancellationToken = default)
        => await TableNoTracking
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Version)
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<GetUsersDto> GetAllUsersAsync(bool deletedUser, CancellationToken cancellationToken = default, string? orderBy = null, int pageId = 1)
    {
        IQueryable<User> result = TableNoTracking
                                    .Where(u => u.IsDelete == deletedUser)
                                    .Include(ur => ur.UserRole).ThenInclude(r => r.Role);

        if (orderBy.HasValue())
            result = result.Where(u => u.UserName.Contains(orderBy) || u.PhoneNumber.Contains(orderBy));

        int take = 20;
        int skip = (pageId - 1) * take;

        var model = new GetUsersDto();
        model.CurrentPage = pageId;

        var count = result.Count();
        model.PageCount = count / take;
        if (count % take != 0) model.PageCount++;

        model.Users = await UserDto.ProjectTo(result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take)).ToListAsync(cancellationToken);

        return model;
    }

    public async Task<EditUserDto> GetUserForEditInAdminAsync(int userId, CancellationToken cancellationToken = default)
        => await EditUserDto.ProjectTo(TableNoTracking.Where(u => u.UserId == userId)).SingleOrDefaultAsync(cancellationToken);

    public async Task<GetUserInformationDto> GetUserInformationAsync(int userId, CancellationToken cancellationToken = default)
        => await GetUserInformationDto.ProjectTo(TableNoTracking.Where(u => u.UserId == userId)).SingleOrDefaultAsync(cancellationToken);




    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(user, cancellationToken);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(user);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
