using Core.Securities;
using Core.Utilities;
using Data.Context;
using Data.Entities.Persons.Users;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Interfaces;
using Services.DTOs.AdminPanel.User;

namespace Services.Contracts.Services;

public class UserServices : BaseServices<User>, IUserServices
{
    #region ConstructorInjection

    private readonly IPermissionServices _permissionServices;
    public UserServices(TedLearnContext context, IPermissionServices permissionServices) : base(context)
    {
        _permissionServices = permissionServices;
    }

    #endregion


    public async Task<bool> IsUserExistAsync(int userId, CancellationToken cancellationToken)
        => await TableNoTracking
                    .Where(u => u.UserId == userId)
                    .AnyAsync(cancellationToken);

    public async Task<bool> IsUserExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken)
        => await TableNoTracking
                    .Where(u => u.UserName == userName && u.PhoneNumber == phoneNumber)
                    .AnyAsync(cancellationToken);

    public async Task<bool> IsPhoneConfirmedAsync(string phoneNumber, CancellationToken cancellationToken)
        => await TableNoTracking
                    .Where(u => u.PhoneNumber == phoneNumber && u.PhoneNumberConfirmed)
                    .AnyAsync(cancellationToken);

    public async Task<bool> IsUserNameOrPhoneNumberExistAsync(string userName, string phoneNumber, CancellationToken cancellationToken)
        => await TableNoTracking
                    .Where(u => u.PhoneNumber == phoneNumber || u.UserName == userName)
                    .AnyAsync(cancellationToken);

    public async Task SignUpUserAsync(User user , CancellationToken cancellationToken, bool configureAwait = false)
    {
        var transacions = _context.Database.BeginTransaction();
        await AddUserAsync(user , cancellationToken , configureAwait);
        await _permissionServices.AddNewUserRoleAsync(user.UserId , cancellationToken , configureAwait);
        await transacions.CommitAsync();
    }

    public async Task<bool> IsPhoneExistAsync(string phoneNumber, CancellationToken cancellationToken)
        => await TableNoTracking
                    .Where(u => u.PhoneNumber == phoneNumber)
                    .AnyAsync(cancellationToken);

    public async Task<double> LeftTimeActivateCode(string phoneNumber, double expireTime, CancellationToken cancellationToken)
    {
        var registerPhones = await TableNoTracking
            .Where(u => u.PhoneNumber == phoneNumber && u.CreateActiveCode.Value.AddSeconds(expireTime) >= DateTime.Now)
            .FirstOrDefaultAsync(cancellationToken);
        if (registerPhones == null) return 0;
        return (registerPhones.CreateActiveCode.Value.AddSeconds(expireTime) - DateTime.Now).TotalSeconds;
    }

    public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken, bool withTracking = true)
    {
        if (withTracking)
            return await Table.Where(u => u.PhoneNumber == phoneNumber).SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(u => u.PhoneNumber == phoneNumber).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task AddActiveCodForUserAsync(string phoneNumber, string activeCode, CancellationToken cancellationToken, bool configureAwait = false)
    {
        var user = await GetUserByPhoneNumberAsync(phoneNumber, cancellationToken);
        user.ActiveCode = activeCode;
        user.CreateActiveCode = DateTime.Now;
        await UpdateUserAsync(user , cancellationToken , configureAwait);
    }

    public async Task<User> CheckUserForLoginAsync(int userId, string password, CancellationToken cancellationToken, bool withTracking = false)
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

    public async Task<User> CheckUserForLoginAsync(string userName, string password, CancellationToken cancellationToken)
        => await TableNoTracking
                    .Where(u => u.UserName == userName && u.PasswordHash == SecurityHelper.GetSha256Hash(password))
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
        {
            if(getActive.HasValue)
                return await Table.Where(u => u.UserId == userId && u.IsDelete == !getActive).SingleOrDefaultAsync(cancellationToken);
            else
                return await Table.Where(u => u.UserId == userId).SingleOrDefaultAsync(cancellationToken);
        }
        else
        {
            if (getActive.HasValue)
                return await TableNoTracking.Where(u => u.UserId == userId && u.IsDelete == !getActive).SingleOrDefaultAsync(cancellationToken);
            else
                return await TableNoTracking.Where(u => u.UserId == userId).SingleOrDefaultAsync(cancellationToken);
        }
    }

    public async Task<string> GetUserAvatar(int userId)
        => await TableNoTracking.Where(u => u.UserId == userId)
                    .Select(u => u.UserAvatar)
                    .SingleOrDefaultAsync();

    public async Task<string> GetUserAvatar(int userId, CancellationToken cancellationToken)
        => await TableNoTracking.Where(u => u.UserId == userId)
                    .Select(u => u.UserAvatar)
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<byte[]> GetVersionOfUserAsync(int userId, CancellationToken cancellationToken)
        => await TableNoTracking
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Version)
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<GetUsersDto> GetAllUsersAsync(CancellationToken cancellationToken, bool deletedUser, string? orderBy, int pageId = 1)
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

    public async Task<bool> IsUserNameOrPhoneNumberExist(string userName, string phoneNumber, CancellationToken cancellationToken)
        => await TableNoTracking.Where(u => u.UserName == userName || u.PhoneNumber == phoneNumber).AnyAsync(cancellationToken);

    public async Task<EditUserDto> GetUserForEditInAdminAsync(int userId, CancellationToken cancellationToken)
        => await EditUserDto.ProjectTo(TableNoTracking.Where(u => u.UserId == userId)).SingleOrDefaultAsync(cancellationToken);

    public async Task<GetUserInformationDto> GetUserInformationAsync(int userId, CancellationToken cancellationToken)
        => await GetUserInformationDto.ProjectTo(TableNoTracking.Where(u => u.UserId == userId)).SingleOrDefaultAsync(cancellationToken);



    public async Task<int> ExecuteInTransactionAsync(Services.TransactionalDelegate transactionalDelegate, CancellationToken cancellationToken, bool configureAwait = false)
        => await base.ExecuteInTransactionAsync(transactionalDelegate, cancellationToken, configureAwait);

    public async Task AddUserAsync(User user, CancellationToken cancellationToken, bool configureAwait = false)
    {
        await Entity.AddAsync(user);
        await SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken, bool configureAwait = false)
    {
        Entity.Update(user);
        await SaveChangesAsync(cancellationToken, configureAwait);
    }
}
