namespace Services.DTOs.AdminPanel.Role;

public class EditRoleDto : BaseDto<EditRoleDto, Data.Entities.Persons.Roles.Role>
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool CanDeleteOrEdit { get; set; }
    public string Base64Version { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Data.Entities.Persons.Roles.Role, EditRoleDto> mapping)
    {
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(r => Convert.ToBase64String(r.Version)));
    }
}
