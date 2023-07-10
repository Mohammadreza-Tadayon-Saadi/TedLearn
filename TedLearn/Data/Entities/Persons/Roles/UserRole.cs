using Data.Entities.BaseEntity;
using Data.Entities.Persons.Users;

namespace Data.Entities.Persons.Roles;

[Table("UserRoles", Schema = "Persons")]
public class UserRole : IEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    #region Relations

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; }

    #endregion
}
