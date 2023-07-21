using Data.Entities.Persons.Roles;
using Services.DTOs.AdminPanel.Role;

namespace Services.Contracts.Interfaces;

public interface IPermissionServices
{
    #region Roles

    Task AddNewUserRoleAsync(int userId , CancellationToken cancellationToken , bool withSaveChanges = true , bool configureAwait = false);
    Task<GetRolesForUserDto> GetRolesForUserAsync(int userId, CancellationToken cancellationToken);
    Task<UserRole> GetUserRoleAsync(int userId, int roleId , CancellationToken cancellationToken , bool withTracking = true);
    Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken cancellationToken, bool? isDeleted = null);
    Task<bool> IsRoleExistAsync(string roleName , CancellationToken cancellationToken);
    Task<Role> GetRoleAsync(int roleId , CancellationToken cancellationToken, bool? isDeleted = null, bool withTracking = true);


    Task AddRoleAsync(Role role , CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateRoleAsync(Role role, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
    Task AddRoleToUserAsync(UserRole userRole , CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);
    Task DeleteRoleFromUserAsync(UserRole userRole, CancellationToken cancellationToken, bool withSaveChanges = true, bool configureAwait = false);

    #endregion
}
