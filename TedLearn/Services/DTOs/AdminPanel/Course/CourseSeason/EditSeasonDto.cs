using Core.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.DTOs.AdminPanel.Course.CourseSeason;

public class EditSeasonDto : BaseDto<EditSeasonDto , Data.Entities.Products.Courses.CourseSeason>
{
    public int SeasonId { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Base64Version { get; set; } = string.Empty;

    public IEnumerable<SelectListItem>? CourseList { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.CourseSeason, EditSeasonDto> mapping)
    {
        mapping.ForMember(dto => dto.Title, opt => opt.MapFrom(cs => cs.SeasonTitle));
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(cs => cs.Version.ToBase64String()));
    }
}
