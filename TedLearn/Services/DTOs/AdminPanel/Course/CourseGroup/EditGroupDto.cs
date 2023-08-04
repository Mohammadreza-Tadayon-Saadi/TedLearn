using Core.Utilities;

namespace Services.DTOs.AdminPanel.Course.CourseGroup;

public class EditGroupDto : BaseDto<EditGroupDto , Data.Entities.Products.Courses.CourseGroup>
{
    public int GroupId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDelete { get; set; }
    public string Base64Version { get; set; } = string.Empty;

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseGroup, EditGroupDto> mapping)
    {
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(cg => cg.Version.ToBase64String()));
    }
}
