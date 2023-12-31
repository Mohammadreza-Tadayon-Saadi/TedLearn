﻿using Data.Context;
using Data.Entities.Persons.Roles;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.AdminPanel.Role;

namespace Services.Contracts.Services;

public class PermissionServices : BaseServices<Role>, IPermissionServices
{
    #region ConstructorInjection

    private DbSet<UserRole> _userRole;
    private readonly ITransactionDbContextServices _transactions;
    public PermissionServices(TedLearnContext context, ITransactionDbContextServices transactions) : base(context)
    {
        _userRole = _context.Set<UserRole>();
        _transactions = transactions;
    }

    #endregion


    #region Role

    public async Task<GetRolesForUserDto> GetRolesForUserAsync(int userId, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default,bool? isDeleted = null)
        => await RoleDto.ProjectTo( TableNoTracking.Where(r => ( (isDeleted.HasValue) ? r.IsDelete == isDeleted : true) ) ).ToListAsync(cancellationToken);

    public async Task<bool> IsRoleExistAsync(string roleName, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(r => r.RoleName == roleName).AnyAsync();

    public async Task<Role> GetRoleAsync(int roleId, CancellationToken cancellationToken = default, bool? isDeleted = null, bool withTracking = true)
    {
        if (withTracking)
            return await Table.Where(r => r.RoleId == roleId && (isDeleted.HasValue ? r.IsDelete == isDeleted : true)).SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(r => r.RoleId == roleId && (isDeleted.HasValue ? r.IsDelete == isDeleted : true)).SingleOrDefaultAsync(cancellationToken);
    }



    public async Task AddRoleAsync(Role role , CancellationToken cancellationToken = default , bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(role, cancellationToken);

        if (withSaveChanges)
           await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task UpdateRoleAsync(Role role, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        Entity.Update(role);
        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    #endregion Role


    #region UserRole

    public async Task<UserRole> GetUserRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default, bool withTracking = true)
    => withTracking ? await _userRole
                            .Where(ur => ur.UserId == userId && ur.RoleId == roleId)
                            .SingleOrDefaultAsync(cancellationToken)
                    : await _userRole.AsNoTracking()
                            .Where(ur => ur.UserId == userId && ur.RoleId == roleId)
                            .SingleOrDefaultAsync(cancellationToken);

    public async Task AddRoleToUserAsync(UserRole userRole, CancellationToken cancellationToken = default, bool withSaveChange = true, bool configureAwait = false)
    {
        await _userRole.AddAsync(userRole, cancellationToken);

        if (withSaveChange)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task DeleteRoleFromUserAsync(UserRole userRole, CancellationToken cancellationToken = default, bool withSaveChange = true, bool configureAwait = false)
    {
        _userRole.Remove(userRole);

        if (withSaveChange)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    #endregion UserRole
}
