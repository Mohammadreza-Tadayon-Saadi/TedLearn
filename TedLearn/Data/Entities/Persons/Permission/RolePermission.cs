using Data.Entities.BaseEntity;
using Data.Entities.Persons.Roles;

namespace Data.Entities.Persons.Permission;

[Table("RolePermissions" , Schema = "Persons")]
public class RolePermission : IEntity
{
    [Key]
    public int RolePermissionId { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }


    #region Relations

    [ForeignKey(nameof(PermissionId))]
    public Permission Permission { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }

    #endregion
}
