using Data.Context;
using Data.Entities.Persons.Roles;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Interfaces;

namespace Services.Contracts.Services;

public class PermissionServices : BaseServices<Role>, IPermissionServices
{
    #region ConstructorInjection

    private DbSet<UserRole> _userRole;
    public PermissionServices(TedLearnContext context) : base(context)
    {
        _userRole = _context.Set<UserRole>();
    }

    #endregion


    #region Role

    public async Task AddNewUserRoleAsync(int userId , CancellationToken cancellationToken , bool configureAwait)
    {
        var userRole = new UserRole()
        {
            UserId = userId,
            RoleId = 11001
        };
        await _userRole.AddAsync(userRole);
        await SaveChangesAsync(cancellationToken , configureAwait);
    }

    #endregion
}
