using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.DTOs.AdminPanel.Course.CourseGroup;

public class EditSubGroupDto : BaseDto<EditSubGroupDto , Data.Entities.Products.Courses.CourseGroup>
{
    public int GroupId { get; set; }
    public int ParentId { get; set; }
    public string Title { get; set; } = string.Empty;

    public bool IsDelete { get; set; }
    public string Base64Version { get; set; } = string.Empty;

    public IEnumerable<SelectListItem>? GroupList { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseGroup, EditSubGroupDto> mapping)
    {
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(cg => Convert.ToBase64String(cg.Version)));
        mapping.ForMember(dto => dto.ParentId, opt => opt.MapFrom(cg => cg.SubGroupId));
    }
}
