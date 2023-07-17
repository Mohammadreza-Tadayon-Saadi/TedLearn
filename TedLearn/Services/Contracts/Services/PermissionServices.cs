using Data.Context;
using Data.Entities.Persons.Roles;
using Data.Entities.Persons.Users;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Interfaces;
using Services.DTOs.AdminPanel.Role;
using System.Threading;

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

    public async Task AddNewUserRoleAsync(int userId , CancellationToken cancellationToken , bool configureAwait = false)
    {
        var userRole = new UserRole()
        {
            UserId = userId,
            RoleId = 11001
        };
        await _userRole.AddAsync(userRole);
        await SaveChangesAsync(cancellationToken , configureAwait);
    }

    public async Task<GetRolesForUserDto> GetRolesForUserAsync(int userId, CancellationToken cancellationToken)
    {
        var model = new GetRolesForUserDto();

        var roles = await TableNoTracking.Select(r => new { r.RoleId, r.RoleName }).ToListAsync(cancellationToken);

        var userRoles = await _userRole.AsNoTracking()
                            .Where(ur => ur.UserId == userId)
                            .Select(ur => ur.RoleId)
                            .ToListAsync(cancellationToken);

        model.UserIsInRoles = roles
                        .Where(r => userRoles.Contains(r.RoleId))
                        .Select(r => new UserRoleDto
                        {
                            UserId = userId,
                            RoleId = r.RoleId,
                            RoleName = r.RoleName
                        });

        model.UserIsNotInRoles = roles
                        .Where(r => !userRoles.Contains(r.RoleId))
                        .Select(r => new UserRoleDto
                        {
                            UserId = userId,
                            RoleId = r.RoleId,
                            RoleName = r.RoleName
                        });

        return model;
    }

    public async Task<UserRole> GetUserRoleAsync(int userId, int roleId, CancellationToken cancellationToken, bool withTracking = true)
    => withTracking ? await _userRole
                            .Where(ur => ur.UserId == userId && ur.RoleId == roleId)
                            .SingleOrDefaultAsync(cancellationToken) 
                    : await _userRole.AsNoTracking()
                            .Where(ur => ur.UserId == userId && ur.RoleId == roleId)
                            .SingleOrDefaultAsync(cancellationToken);

    public async Task AddRoleToUserAsync(UserRole userRole, CancellationToken cancellationToken, bool configureAwait = false)
    {
        await _userRole.AddAsync(userRole, cancellationToken);
        await SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task DeleteRoleFromUserAsync(UserRole userRole, CancellationToken cancellationToken, bool configureAwait = false)
    {
        _userRole.Remove(userRole);
        await SaveChangesAsync(cancellationToken, configureAwait);
    }

    #endregion
}
