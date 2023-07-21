using Services.DTOs.Common;

namespace Services.DTOs.AdminPanel.Role;

public class RoleDto : BaseDto<RoleDto , Data.Entities.Persons.Roles.Role>
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }
    public bool CanDeleteOrEdit { get; set; }
}
