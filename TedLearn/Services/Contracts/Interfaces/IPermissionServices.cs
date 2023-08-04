using Data.Entities.Persons.Roles;
using Services.DTOs.AdminPanel.Role;

namespace Services.Contracts.Interfaces;

public interface IPermissionServices
{
    #region Roles

    Task<GetRolesForUserDto> GetRolesForUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default, bool? isDeleted = null);
    Task<bool> IsRoleExistAsync(string roleName , CancellationToken cancellationToken = default);
    Task<Role> GetRoleAsync(int roleId , CancellationToken cancellationToken = default, bool? isDeleted = null, bool withTracking = true);


    Task AddRoleAsync(Role role , CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task UpdateRoleAsync(Role role, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);

    #endregion

    #region UserRoles

    Task<UserRole> GetUserRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default, bool withTracking = true);


    Task AddRoleToUserAsync(UserRole userRole, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task DeleteRoleFromUserAsync(UserRole userRole, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);

    #endregion UserRoles
}
