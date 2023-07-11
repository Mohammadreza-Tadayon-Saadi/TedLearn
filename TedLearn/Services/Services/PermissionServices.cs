using Data.Context;
using Data.Entities.Persons.Roles;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Services
{
    public class PermissionServices : BaseServices, IPermissionServices
    {
        #region ConstructorInjection

        private DbSet<UserRole> _userRole;
        private DbSet<Role> _role;
        public PermissionServices(TedLearnContext context) : base(context)
        {
            _userRole = context.Set<UserRole>();
            _role = context.Set<Role>();
        }

        #endregion


        #region Role

        public async Task AddNewUserRoleAsync(int userId)
        {
            var userRole = new UserRole()
            {
                UserId = userId,
                RoleId = 11001
            };
            await _userRole.AddAsync(userRole);
            await SaveChangesAsync();
        }

        public async Task<List<string>> GetUserRolesName(int userId)
            => await _userRole.Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.RoleName)
                .ToListAsync();

        #endregion
    }
}
