namespace Services.Interfaces;

public interface IPermissionServices
{
    #region Roles

    Task AddNewUserRoleAsync(int userId);
    Task<List<string>> GetUserRolesName(int userId);

    #endregion
}
