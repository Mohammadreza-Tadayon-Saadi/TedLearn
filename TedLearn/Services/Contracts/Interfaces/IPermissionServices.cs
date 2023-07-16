namespace Services.Contracts.Interfaces;

public interface IPermissionServices
{
    #region Roles

    Task AddNewUserRoleAsync(int userId , CancellationToken cancellationToken , bool configureAwait);

    #endregion
}
