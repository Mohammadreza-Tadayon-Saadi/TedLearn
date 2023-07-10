using Data.Entities.BaseEntity;

namespace Data.Entities.Persons.Permission;

[Table("Permissions", Schema = "Persons")]
public class Permission : IEntity
{
    [Key]
    public int PermissionId { get; set; }

    [MaxLength(70)]
    public string PermissionTitle { get; set; } = string.Empty;
    public int? ParentId { get; set; }


    #region Relations

    [ForeignKey(nameof(ParentId))]
    public ICollection<Permission> SubPermissions { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }

    #endregion
}
