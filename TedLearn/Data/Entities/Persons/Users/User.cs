using Data.Entities.BaseEntity;
using Data.Entities.Persons.Discounts;
using Data.Entities.Persons.Roles;
using Data.Entities.Persons.Transactions;

namespace Data.Entities.Persons.Users;

[Table("Users", Schema = "Persons")]
public class User : IEntity
{
    [Key]
    public int UserId { get; set; }

    [MinLength(3)]
    [MaxLength(80)]
    public string UserName { get; set; } = string.Empty;

    [MinLength(11)]
    [MaxLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool PhoneNumberConfirmed { get; set; }

    [MaxLength(150)]
    public string UserAvatar { get; set; } = string.Empty;


    [MaxLength(70)]
    public string? FirstName { get; set; }

    [MaxLength(70)]
    public string? LastName { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(1000)]
    public string? Biography { get; set; }

    [MaxLength(100)]
    public string? WebsiteAddress { get; set; }

    [MinLength(4)]
    [MaxLength(300)]
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime RegisterDate { get; set; }
    public DateTime? LastLoginDate { get; set; }

    [MaxLength(50)]
    public string? ActiveCode { get; set; }
    public DateTime? CreateActiveCode { get; set; }
    public bool IsDelete { get; set; }

    [Timestamp]
    public byte[] Version { get; set; }


    #region Relations

    public ICollection<UserRole> UserRole { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<UserDiscount> UserDiscounts { get; set; }

    #endregion Relations
}
