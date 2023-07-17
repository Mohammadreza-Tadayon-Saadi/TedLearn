using Data.Entities.Persons.Roles;
using Services.DTOs.AdminPanel.Role;

namespace Services.Contracts.Interfaces;

public interface IPermissionServices
{
    #region Roles

    Task AddNewUserRoleAsync(int userId , CancellationToken cancellationToken , bool configureAwait = false);
    Task<GetRolesForUserDto> GetRolesForUserAsync(int userId, CancellationToken cancellationToken);
    Task<UserRole> GetUserRoleAsync(int userId, int roleId , CancellationToken cancellationToken , bool withTracking = true);


    Task AddRoleToUserAsync(UserRole userRole , CancellationToken cancellationToken, bool configureAwait = false);
    Task DeleteRoleFromUserAsync(UserRole userRole, CancellationToken cancellationToken, bool configureAwait = false);

    #endregion
}
