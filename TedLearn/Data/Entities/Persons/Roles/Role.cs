using Data.Entities.BaseEntity;

namespace Data.Entities.Persons.Roles;

[Table("Roles", Schema = "Persons")]
public class Role : IEntity
{
    [Key]
    public int RoleId { get; set; }


    [MaxLength(70)]
    public string RoleName { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }
    public bool CanDeleteOrEdit { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }


    #region Relations

    public ICollection<UserRole> UserRole { get; set; }

    #endregion Relations
}
