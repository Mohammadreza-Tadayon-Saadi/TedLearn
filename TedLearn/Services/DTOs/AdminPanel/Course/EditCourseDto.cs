using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.DTOs.AdminPanel.Course;

public class EditCourseDto : BaseDto<EditCourseDto , Data.Entities.Products.Courses.Course>
{
    public int CourseId { get; set; }
    public string PreImage { get; set; } = string.Empty;
    public string PreDemo { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public int? SubGroupId { get; set; }
    public int TeacherId { get; set; }
    public int StatusId { get; set; }
    public string CourseLevel { get; set; } = string.Empty;
    public decimal? CoursePrice { get; set; }
    public string? Tags { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Requirment { get; set; }
    public string Base64Version { get; set; } = string.Empty;


    public IEnumerable<SelectListItem>? GroupList { get; set; }
    public IEnumerable<SelectListItem>? SubGroupList { get; set; }
    public IEnumerable<SelectListItem>? TeacherList { get; set; }
    public IEnumerable<SelectListItem>? StatusList { get; set; }
    public IEnumerable<SelectListItem>? LevelList { get; set; }


    public IFormFile? DemoFile { get; set; }
    public IFormFile? ImageFile { get; set; }

    public override void CustomMappings(IMappingExpression<Data.Entities.Products.Courses.Course, EditCourseDto> mapping)
    {
        mapping.ForMember(dto => dto.Description, opt => opt.MapFrom(c => c.CourseDescription));
        mapping.ForMember(dto => dto.Requirment, opt => opt.MapFrom(c => c.CourseRequirement));
        mapping.ForMember(dto => dto.Tags, opt => opt.MapFrom(c => c.CourseTags));
        mapping.ForMember(dto => dto.Base64Version, opt => opt.MapFrom(c => Convert.ToBase64String(c.Version)));
        mapping.ForMember(dto => dto.PreDemo, opt => opt.MapFrom(c => c.CourseDemoFile));
        mapping.ForMember(dto => dto.PreImage, opt => opt.MapFrom(c => c.CourseImage));
    }
}
